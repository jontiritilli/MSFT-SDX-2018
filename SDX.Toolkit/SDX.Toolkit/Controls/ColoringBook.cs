using System;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Helpers;
using SDX.Telemetry.Services;


namespace SDX.Toolkit.Controls
{
    public class ColoringBookColor
    {
        public string URI_SelectedImage = "";
        public string URI_NotSelectedImage = "";
        public Color Color = Color.FromArgb(0, 0, 0, 0);
    }

    public sealed class ColoringBook : Control, IAnimate
    {

        #region Private Constants

        private const string URI_PENICON = @"ms-appx:///Assets/ColoringBook/ink-pen-icon.png";

        private readonly BitmapImage BMIMAGE_CLEARBUTTON = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookReset);

        private readonly BitmapImage BMIMAGE_COLORING_BOOK = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookImage);

        private readonly BitmapImage BMIMAGE_COLOR_RED = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorRed);

        private readonly BitmapImage BMIMAGE_COLOR_BLUE = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorBlue);
        private readonly BitmapImage BMIMAGE_COLOR_TEAL = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorTeal);
        private readonly BitmapImage BMIMAGE_COLOR_ORANGE = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorOrange);
        private readonly BitmapImage BMIMAGE_COLOR_PURPLE = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorPurple);

        private readonly BitmapImage BMIMAGE_COLOR_RED_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorRedActive);
        private readonly BitmapImage BMIMAGE_COLOR_BLUE_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorBlueActive);
        private readonly BitmapImage BMIMAGE_COLOR_TEAL_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorTealActive);
        private readonly BitmapImage BMIMAGE_COLOR_ORANGE_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorOrangeActive);
        private readonly BitmapImage BMIMAGE_COLOR_PURPLE_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.ColoringBookColorPurpleActive);

        private readonly Color COLOR_COLORING_BOOK_RED = StyleHelper.GetApplicationColor(ColoringBookColors.Red);
        private readonly Color COLOR_COLORING_BOOK_BLUE = StyleHelper.GetApplicationColor(ColoringBookColors.Blue);
        private readonly Color COLOR_COLORING_BOOK_TEAL = StyleHelper.GetApplicationColor(ColoringBookColors.Teal);
        private readonly Color COLOR_COLORING_BOOK_ORANGE = StyleHelper.GetApplicationColor(ColoringBookColors.Orange);
        private readonly Color COLOR_COLORING_BOOK_PURPLE = StyleHelper.GetApplicationColor(ColoringBookColors.Purple);

        private static readonly Size WINDOW_BOUNDS = WindowHelper.GetViewSizeInfo();
        private static readonly double CANVAS_X = WINDOW_BOUNDS.Width;
        private static readonly double CANVAS_Y = WINDOW_BOUNDS.Height;

        private readonly double GRID_X = CANVAS_X * 0.7;
        private readonly double GRID_Y = CANVAS_Y * 0.4;

        private const double GRID_ROWSPACING = 30d;
        private const double GRID_COLUMNSPACING = 0;

        private const double WIDTH_PENICON = 151d;

        private const int Z_ORDER_CONTROLS = 99;
        private const int Z_ORDER_SHAPES = 0;

        private readonly double DOUBLE_COLORING_BOOK_BUTTON_WIDTH = StyleHelper.GetApplicationDouble(LayoutSizes.ColoringBookButtonWidth);
        private readonly double DOUBLE_COLORING_BOOK_BUTTON_HEIGHT = StyleHelper.GetApplicationDouble(LayoutSizes.ColoringBookButtonHeight);

        private readonly double DOUBLE_COLORING_BOOK_IMAGE_WIDTH = StyleHelper.GetApplicationDouble(LayoutSizes.ColoringBookImageWidth);

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private InkCanvas _inkCanvas = null;
        private Canvas _touchHereCanvas = null;
        private Grid _penTouchPointGrid = null;
        private ImageEx _ColoringImage = null;
        private Image _touchHereImage = null;
        private bool _touchHereWasHidden = false;        
        private List<AppSelectorData> _URIs;
        private Dictionary<int, ImagePair> _ImagePairs;
        private Color _SelectedColor = Color.FromArgb(255, 0, 0, 0);
        private AppSelector _AppSelector = new AppSelector();


        #endregion

        #region Constructor

        public ColoringBook()
        {
            this.DefaultStyleKey = typeof(ColoringBook);
            this.Loaded += OnLoaded;
            this._ImagePairs = new Dictionary<int, ImagePair>();
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            // inherited dependency property
            //new PropertyChangeEventSource<double>(
            //    this, "Opacity", Windows.UI.Xaml.Data.BindingMode.OneWay).ValueChanged +=
            //    OnOpacityChanged;
            this._URIs = new List<AppSelectorData>();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Methods

        public void StartAnimation()
        {
            // start the fade ins
            if (null != _penTouchPointGrid)
            {
                AnimationHelper.PerformFadeIn(_penTouchPointGrid, 100d, 0);
            }
        }

        public void ResetAnimation()
        {
            // reset the animations
            if (null != _penTouchPointGrid)
            {
                AnimationHelper.PerformFadeOut(_penTouchPointGrid, 100d, 0);
            }
        }

        public void ClearPath()
        {
            if (null != _inkCanvas)
            {
                _inkCanvas.InkPresenter.StrokeContainer.Clear();
            }

        }

        public void ChangeColorId(int Id)
        {
            ColorID = Id;
        }

        public void FadeInColoringImage()
        {
            if (null != this._ColoringImage && this._ColoringImage.Opacity != 1)
            {
                AnimationHelper.PerformFadeIn(this._ColoringImage, 500d);
            }
        }

        #endregion

        #region Dependency Properties

        // color id
        public static readonly DependencyProperty ColorIDProperty =
            DependencyProperty.Register("ColorID", typeof(int), typeof(AppSelector), new PropertyMetadata(0, OnColorIDChanged));

        public int ColorID
        {
            get { return (int)GetValue(ColorIDProperty); }
            set { SetValue(ColorIDProperty, value); }
        }

        // Caption
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(ColoringBook), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // TextStyle
        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register("TextStyle", typeof(TextStyles), typeof(ColoringBook), new PropertyMetadata(null));

        public TextStyles TextStyle
        {
            get { return (TextStyles)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(ColoringBook), new PropertyMetadata(100d));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // FadeInCompletedHandler
        public static readonly DependencyProperty FadeInCompletedHandlerProperty =
            DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(ColoringBook), new PropertyMetadata(null));

        public EventHandler<object> FadeInCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
            set { SetValue(FadeInCompletedHandlerProperty, value); }
        }

        // FadeOutCompletedHandler
        public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
            DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(ColoringBook), new PropertyMetadata(null));

        public EventHandler<object> FadeOutCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
            set { SetValue(FadeOutCompletedHandlerProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(ColoringBook), new PropertyMetadata(0d));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(ColoringBook), new PropertyMetadata(false));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        // ImageURI
        public static readonly DependencyProperty ImageURIProperty =
        DependencyProperty.Register("ImageURI", typeof(string), typeof(ColoringBook), new PropertyMetadata(""));

        public string ImageURI
        {
            get { return (string)GetValue(ImageURIProperty); }
            set { SetValue(ImageURIProperty, value); }
        }

        // ImageSVGURI
        public static readonly DependencyProperty ImageSVGURIProperty =
        DependencyProperty.Register("ImageSVGURI", typeof(string), typeof(ColoringBook), new PropertyMetadata(""));

        public string ImageSVGURI
        {
            get { return (string)GetValue(ImageSVGURIProperty); }
            set { SetValue(ImageSVGURIProperty, value); }
        }

        // ImageWidth
        public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register("ImageWidth", typeof(int), typeof(ColoringBook), new PropertyMetadata(0));

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // ImageHeight
        public static readonly DependencyProperty ImageHeightProperty =
        DependencyProperty.Register("ImageHeight", typeof(int), typeof(ColoringBook), new PropertyMetadata(0));

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Colors
        public static readonly DependencyProperty ColorsProperty =
        DependencyProperty.Register("Colors", typeof(List<Color>), typeof(ColoringBook), new PropertyMetadata(new List<Color>()));

        public List<Color> Colors
        {
            get { return (List<Color>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        // ImageWidth
        public static readonly DependencyProperty ButtonWidthProperty =
        DependencyProperty.Register("ButtonWidth", typeof(int), typeof(ColoringBook), new PropertyMetadata(0));

        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        // ImageHeight
        public static readonly DependencyProperty ButtonHeightProperty =
        DependencyProperty.Register("ButtonHeight", typeof(int), typeof(ColoringBook), new PropertyMetadata(0));

        public int ButtonHeight
        {
            get { return (int)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        // ImageHeight
        public static readonly DependencyProperty ClearButtonURIProperty =
        DependencyProperty.Register("ClearButtonURI", typeof(string), typeof(ColoringBook), new PropertyMetadata(""));

        public string ClearButtonURI
        {
            get { return (string)GetValue(ClearButtonURIProperty); }
            set { SetValue(ClearButtonURIProperty, value); }
        }

        // ImageColumnSpan
        public static readonly DependencyProperty ImageColumnSpanProperty =
        DependencyProperty.Register("ImageColumnSpan", typeof(int), typeof(ColoringBook), new PropertyMetadata(1));

        public int ImageColumnSpan
        {
            get { return (int)GetValue(ImageColumnSpanProperty); }
            set { SetValue(ImageColumnSpanProperty, value); }
        }

        // AnimationDirection
        public static readonly DependencyProperty PageEntranceDirectionProperty =
        DependencyProperty.Register("PageEntranceDirection", typeof(AnimationDirection), typeof(ColoringBook), new PropertyMetadata(AnimationDirection.Left));

        public AnimationDirection PageEntranceDirection
        {
            get { return (AnimationDirection)GetValue(PageEntranceDirectionProperty); }
            set { SetValue(PageEntranceDirectionProperty, value); }
        }

        public void ForceColorChange(int ID)
        {
            this._AppSelector.SelectedID = ID;
        }
        #endregion

        #region Custom Events

        public delegate void OnColorIDChangedEvent(object sender, EventArgs e);

        public event OnColorIDChangedEvent ColorIDChanged;

        private void RaiseColorIDChangedEvent(ColoringBook Book, EventArgs e)
        {
            ColorIDChanged?.Invoke(Book, e);
        }

        private void RaiseColorIDChangedEvent(ColoringBook Book)
        {
            this.RaiseColorIDChangedEvent(Book, new EventArgs());
        }

        public delegate void OnPenScreenContactStartedEvent(object sender, EventArgs e);

        public event OnPenScreenContactStartedEvent OnPenScreenContacted;

        private void RaisePenScreenContactStartedEvent(ColoringBook Book, EventArgs e)
        {
            OnPenScreenContacted?.Invoke(Book, e);
        }

        private void RaisePenScreenContactStartedEvent(ColoringBook Book)
        {
            this.RaisePenScreenContactStartedEvent(Book, new EventArgs());
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartAnimation();
            }
        }

        public static void OnColorIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ColoringBook book)
            {
                book.RaiseColorIDChangedEvent(book);
                book._AppSelector.SelectedID = book.ColorID;
            }
        }

        private void OnPenScreenContactStarted(InkStrokeInput input, PointerEventArgs e)
        {
            RaisePenScreenContactStartedEvent(this);
            FadeInColoringImage();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        //private void OnOpacityChanged(object sender, double e)
        //{
        //    double opacity = e;

        //    if (null != _layoutRoot)
        //    {
        //        // correct opacity range
        //        opacity = Math.Max(0.0, opacity);
        //        opacity = Math.Min(1.0, opacity);

        //        // set opacity
        //        _layoutRoot.Opacity = opacity;
        //    }
        //    //_AppSelector needs to be handled here as well
        //    if (null != _AppSelector)
        //    {
        //        // correct opacity range
        //        opacity = Math.Max(0.0, opacity);
        //        opacity = Math.Min(1.0, opacity);

        //        // set opacity
        //        _AppSelector.Opacity = opacity;                
        //    }
        //}
        #endregion
        
        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            //_layoutRoot.Opacity = 0;

            if (null == _layoutRoot) { return; }

            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.95, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.05, GridUnitType.Star) });
            //_layoutRoot.PointerPressed += OnPenScreenContactStarted;

            // create touch here canvas
            _touchHereCanvas = new Canvas()
            {
                Name = "ColoringBookCanvasControls",
                Margin = new Thickness(0),
                ManipulationMode = ManipulationModes.None
            };
            Grid.SetRow(_touchHereCanvas, 0);
            Grid.SetColumn(_touchHereCanvas, 0);
            _layoutRoot.Children.Add(_touchHereCanvas);

            // create the button grid
            _penTouchPointGrid = new Grid()
            {
                Name = "AccessoriesPenButtonGrid",
                Margin = new Thickness(0),
                Padding = new Thickness(0),
                Opacity = 0.0
            };
            _penTouchPointGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_ROWSPACING) });
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            Canvas.SetLeft(_penTouchPointGrid, GRID_X);
            Canvas.SetTop(_penTouchPointGrid, GRID_Y);
            Canvas.SetZIndex(_penTouchPointGrid, Z_ORDER_CONTROLS);
            _touchHereCanvas.Children.Add(_penTouchPointGrid);

            // create touch here image
            _touchHereImage = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_PENICON), DecodePixelWidth = (int)WIDTH_PENICON },
                Width = WIDTH_PENICON,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(_touchHereImage, 0);            
            Grid.SetColumn(_touchHereImage, 0);
            _penTouchPointGrid.Children.Add(_touchHereImage);

            // create the ink canvas
            _inkCanvas = new InkCanvas()
            {
                Name = "InkCanvas",
                Margin = new Thickness(0),
                HorizontalAlignment= HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch

            };

            _inkCanvas.InkPresenter.StrokeInput.StrokeStarted += OnPenScreenContactStarted;
            Grid.SetRow(_inkCanvas, 0);
            Grid.SetColumnSpan(_inkCanvas, this.ImageColumnSpan);
            Grid.SetColumn(_inkCanvas, 0);
            _layoutRoot.Children.Add(_inkCanvas);

            // add a nohitvisible png onto this page
            // please dont not have a URI or an SVGURI or the image below will error
            _ColoringImage = new ImageEx()
            {
                Name = "ColoringImage",
                BitmapImage = BMIMAGE_COLORING_BOOK,   
                ImageWidth = DOUBLE_COLORING_BOOK_IMAGE_WIDTH,
                Width = DOUBLE_COLORING_BOOK_IMAGE_WIDTH,                
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                
                Opacity = .1,
                IsHitTestVisible = false
            };            

            Canvas.SetZIndex(_ColoringImage, 101);
            Grid.SetColumnSpan(_ColoringImage, this.ImageColumnSpan);
            Grid.SetRow(_ColoringImage, 0);            
            Grid.SetColumn(_ColoringImage, 0);            
            _layoutRoot.Children.Add(_ColoringImage);

            this.Colors.Add(COLOR_COLORING_BOOK_RED);
            this.Colors.Add(COLOR_COLORING_BOOK_BLUE);
            this.Colors.Add(COLOR_COLORING_BOOK_TEAL);
            this.Colors.Add(COLOR_COLORING_BOOK_ORANGE);
            this.Colors.Add(COLOR_COLORING_BOOK_PURPLE);

            this._ImagePairs.Add(0,new ImagePair() {
                NotSelected = new Image()
                {
                    Source = BMIMAGE_COLOR_RED,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BMIMAGE_COLOR_RED_SELECTED,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });           

            this._ImagePairs.Add(1, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BMIMAGE_COLOR_BLUE,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BMIMAGE_COLOR_BLUE_SELECTED,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this._ImagePairs.Add(2, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BMIMAGE_COLOR_TEAL,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BMIMAGE_COLOR_TEAL_SELECTED,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this._ImagePairs.Add(3, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BMIMAGE_COLOR_ORANGE,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BMIMAGE_COLOR_ORANGE_SELECTED,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this._ImagePairs.Add(4, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BMIMAGE_COLOR_PURPLE,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BMIMAGE_COLOR_PURPLE_SELECTED,
                    Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this._AppSelector = new AppSelector()
            {//
                //TelemetryId = TelemetryService.TELEMETRY_KEYBOARDVIEWCOLOR,
                AppSelectorMode = SelectorMode.Color,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                MainOrientation = Orientation.Vertical,
                ButtonHeight = DOUBLE_COLORING_BOOK_BUTTON_HEIGHT,
                ButtonWidth = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                Opacity = 1,
                ImagePairs = this._ImagePairs,
                ClearButtonImagePair = new ImagePair()
                {
                    NotSelected = new Image()
                    {
                        Source = BMIMAGE_CLEARBUTTON,
                        Width = DOUBLE_COLORING_BOOK_BUTTON_WIDTH,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = 1.0
                    },
                    IsClearButton = true
                },
                SelectedID = 0

            };
            _AppSelector.SelectedIDChanged += _AppSelector_SelectedIDChanged;
            _AppSelector.OnClearClicked += _AppSelector_ClearClickedChanged;
            Canvas.SetZIndex(_AppSelector, 101);
            Grid.SetRow(_AppSelector, 0);
            Grid.SetColumn(_AppSelector, 1);
            this._layoutRoot.Children.Add(_AppSelector);
            this._SelectedColor = this.Colors[0];
            SetupBrush();
        }

        private void _AppSelector_SelectedIDChanged(object sender, EventArgs e)
        {
            // need to change the mouse page too
            if (null != _AppSelector)
            {
                this._SelectedColor = this.Colors[_AppSelector.SelectedID];
                this.ColorID = _AppSelector.SelectedID;
                SetupBrush();
            }
        }

        private void _AppSelector_ClearClickedChanged(object sender, EventArgs e)
        {
            // need to change the mouse page too
            if (null != _AppSelector)
            {
                AppSelector appSelector = (AppSelector)sender;
                _inkCanvas.InkPresenter.StrokeContainer.Clear();                               
            }
        }

        private void InkPresenter_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
        {
            // hide the pen icon
            if (!_touchHereWasHidden)
            {
                if (null != _penTouchPointGrid)
                {
                    AnimationHelper.PerformFadeOut(_penTouchPointGrid, 100d, 0d);
                }

                _touchHereWasHidden = true;
            }

            // telemetry
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.StartPen);
        }

        private void InkPresenter_StrokeEnded(InkStrokeInput sender, PointerEventArgs args)
        {
            //// show the pen icon
            //if (null != _touchHereImage)
            //{
            //    AnimationHelper.PerformFadeIn(_touchHereImage, 100d, 0d);
            //}

            // change the ink color
            if (null != _inkCanvas)
            {
                InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes()
                {
                    Color = GetNextColor(),
                    //FitToCurve = true,
                    PenTip = PenTipShape.Circle,
                    Size = new Size(WINDOW_BOUNDS.Width * 0.02, WINDOW_BOUNDS.Height * 0.02)
                };
                _inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
            }

            // telemetry
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.EndPen);
        }

        private void SetupBrush()
        {
            if (null != _inkCanvas)
            {
                InkDrawingAttributes inkDrawingAttributes = InkDrawingAttributes.CreateForPencil();
                inkDrawingAttributes.Color = GetNextColor();

                //inkDrawingAttributes.PenTip = PenTipShape.Circle;
                inkDrawingAttributes.Size = new Size(WINDOW_BOUNDS.Width * 0.01, WINDOW_BOUNDS.Height * 0.01);
                //inkDrawingAttributes.PenTipTransform = System.Numerics.Matrix3x2.CreateRotation((float)(70 * Math.PI / 180));// System.Numerics.Matrix3x2.CreateRotation(.785f);//
                //InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes()
                //{
                //    Color = GetNextColor(),
                //    //FitToCurve = true,
                //    DrawAsHighlighter = true,
                //    PenTip = PenTipShape.Circle,
                //    PenTipTransform = System.Numerics.Matrix3x2.CreateRotation((float)(70 * Math.PI / 180)),
                ////FitToCurve = true,
                //    Size = new Size(WINDOW_BOUNDS.Width * 0.01, WINDOW_BOUNDS.Height * 0.01)
                //};

                _inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
            }
        }

        #endregion

        #region UI Helpers

        private Color GetNextColor()
        {

            return _SelectedColor;
        }

        public bool HasAnimateChildren()
        {
            return true;
        }

        public bool HasPageEntranceAnimation()
        {
            return true;
        }

        public bool HasPageEntranceTranslation()
        {
            return false;
        }

        public AnimationDirection Direction()
        {
            return PageEntranceDirection;
        }

        public List<UIElement> AnimatableChildren()
        {
            List<UIElement> uIElements = new List<UIElement>();
            uIElements.Add(_AppSelector);
            return uIElements;
        }

        #endregion
    }
    

}
