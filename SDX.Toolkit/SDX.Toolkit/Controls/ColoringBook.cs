using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Helpers;
//using SDX.Toolkit.Views;
//using SDX.Toolkit.Services;

namespace SDX.Toolkit.Controls
{
    public class ColoringBookColor
    {
        public string URI_SelectedImage = "";
        public string URI_NotSelectedImage = "";
        public Color Color = Color.FromArgb(0, 0, 0, 0);
    }

    public sealed class ColoringBook : Control
    {

        #region Private Constants

        private const string URI_PENICON = @"ms-appx:///Assets/ColoringBook/ink-pen-icon.png";

        private static readonly Size WINDOW_BOUNDS = new Size(5000, 5000);//= PageHelper.GetViewSizeInfo();
        private static readonly double CANVAS_X = WINDOW_BOUNDS.Width;
        private static readonly double CANVAS_Y = WINDOW_BOUNDS.Height;

        private readonly double GRID_X = CANVAS_X * 0.7;
        private readonly double GRID_Y = CANVAS_Y * 0.4;

        private const double GRID_ROWSPACING = 30d;
        private const double GRID_COLUMNSPACING = 0;

        private const double WIDTH_PENICON = 151d;

        private const int Z_ORDER_CONTROLS = 99;
        private const int Z_ORDER_SHAPES = 0;


        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private InkCanvas _inkCanvas = null;
        private Canvas _touchHereCanvas = null;
        private Grid _penTouchPointGrid = null;
        private Image _touchHereImage = null;

        private bool _touchHereWasHidden = false;
        private int _currentColor = -1;
        private List<Color> _inkColors = new List<Color>()
            {
                Color.FromArgb(255, 85, 53, 55),
                Color.FromArgb(255, 53, 73, 85),
                Color.FromArgb(255, 192, 192, 192),
                Color.FromArgb(255, 33, 33, 33)
            };
        private List<AppSelectorData> _URIs;
        private Color _SelectedColor = Color.FromArgb(255, 0, 0, 0);
        private AppSelector _AppSelector = new AppSelector();


        #endregion

        #region Public Members
        public string ImageURI;
        public string ImageSVGURI;
        public int ImageWidth;
        public int ImageHeight;
        public List<ColoringBookColor> Colors;
        #endregion

        #region Constructor

        public ColoringBook()
        {
            this.DefaultStyleKey = typeof(ColoringBook);
            this.Loaded += OnLoaded;
            // inherited dependency property
            new PropertyChangeEventSource<double>(
                this, "Opacity", Windows.UI.Xaml.Data.BindingMode.OneWay).ValueChanged +=
                OnOpacityChanged;
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

        #endregion

        #region Dependency Properties

        // Caption
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(ColoringBook), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // ControlStyle
        public static readonly DependencyProperty ControlStyleProperty =
            DependencyProperty.Register("ControlStyle", typeof(ControlStyles), typeof(ColoringBook), new PropertyMetadata(null));

        public ControlStyles ControlStyle
        {
            get { return (ControlStyles)GetValue(ControlStyleProperty); }
            set { SetValue(ControlStyleProperty, value); }
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

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartAnimation();
            }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnOpacityChanged(object sender, double e)
        {
            double opacity = e;

            if (null != _layoutRoot)
            {
                // correct opacity range
                opacity = Math.Max(0.0, opacity);
                opacity = Math.Min(1.0, opacity);

                // set opacity
                _layoutRoot.Opacity = opacity;
            }
            //_AppSelector needs to be handled here as well
            if (null != _AppSelector)
            {
                // correct opacity range
                opacity = Math.Max(0.0, opacity);
                opacity = Math.Min(1.0, opacity);

                // set opacity
                _AppSelector.Opacity = opacity;
            }
        }
        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            _layoutRoot.Opacity = 0;

            if (null == _layoutRoot) { return; }

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

            };

            Grid.SetRow(_inkCanvas, 0);
            Grid.SetColumn(_inkCanvas, 0);
            _layoutRoot.Children.Add(_inkCanvas);

            // add a nohitvisible png onto this page
            // please dont not have a URI or an SVGURI or the image below will error
            Image ColoringImage = new Image()
            {                
                Width = this.ImageWidth,
                Height = this.ImageHeight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0,
                IsHitTestVisible = false
            };
            
            if (!string.IsNullOrEmpty(ImageURI)) {
                ColoringImage.Source = new BitmapImage() { UriSource = new Uri(ImageURI), DecodePixelWidth = this.ImageWidth };                
            }
            else if(!string.IsNullOrEmpty(ImageSVGURI))
            {
                ColoringImage.Source = new SvgImageSource(new Uri(ImageSVGURI)) {};
            }

            Canvas.SetZIndex(ColoringImage, 100);
            Grid.SetRow(ColoringImage, 0);
            Grid.SetRowSpan(ColoringImage, 4);
            Grid.SetColumn(ColoringImage, 0);
            Grid.SetColumnSpan(ColoringImage, 3);
            _layoutRoot.Children.Add(ColoringImage);
            this._URIs.Add(new AppSelectorData
            {
                SourceSVG_NotSelectedImage = "ms-appx:///Assets/Universal/inkingReset.svg",
                SourceSVG_SelectedImage = "ms-appx:///Assets/Universal/inkingReset.svg",
                IsClearButton = true
            });

            for (int i = 0; i < this.Colors.Count; i++)
            {
                this._URIs.Add(new AppSelectorData
                {
                    Source_NotSelectedImage = this.Colors[i].URI_NotSelectedImage,
                    Source_SelectedImage = this.Colors[i].URI_SelectedImage
                });
            }
            AppSelector _AppSelector = new AppSelector()
            {//
                //TelemetryId = TelemetryService.TELEMETRY_KEYBOARDVIEWCOLOR,
                AppSelectorMode = SelectorMode.Color,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                DurationInMilliseconds = 200d,
                StaggerDelayInMilliseconds = 200d,
                AutoStart = false,
                Orientation = Orientation.Vertical,
                ButtonHeight = 74,
                Opacity = 1,
                URIs = this._URIs
            };// bind event to catch and change color from this.colors
            // add the test selector here so it's after the color selector image
            _AppSelector.SelectedIDChanged += _AppSelector_SelectedIDChanged;
            _AppSelector.OnClearClicked += _AppSelector_ClearClickedChanged;
            Canvas.SetZIndex(_AppSelector, 101);
            Grid.SetRow(_AppSelector, 5);
            Grid.SetColumn(_AppSelector, 1);
            this._layoutRoot.Children.Add(_AppSelector);
            this._SelectedColor = this.Colors[1].Color;
            SetupBrush();
            // set up events

        }

        private void _AppSelector_SelectedIDChanged(object sender, EventArgs e)
        {
            // need to change the mouse page too
            if ((null != _AppSelector) && (null != _AppSelector) && (null != _AppSelector))
            {
                AppSelector appSelector = (AppSelector)sender;
                if (appSelector.SelectedID > 0)
                {// this is the only case there this needs to manage since there is a clear button so account for it
                    this._SelectedColor = this.Colors[appSelector.SelectedID - 1].Color;
                }
                //else
                //{// should it disable the color? or leave the selection on the previous one?
                //    _inkCanvas.InkPresenter.StrokeContainer.Clear();
                //}
                

                SetupBrush();
            }
        }

        private void _AppSelector_ClearClickedChanged(object sender, EventArgs e)
        {
            // need to change the mouse page too
            if ((null != _AppSelector) && (null != _AppSelector) && (null != _AppSelector))
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
            //TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_STARTPEN, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
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
            //TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_ENDPEN, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
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

        #endregion
    }

    // A StylusPlugin that renders ink with a linear gradient brush effect.
    //class CustomDynamicRenderer : DynamicRenderer
    //{
    //    [ThreadStatic]
    //    static private Brush brush = null;

    //    [ThreadStatic]
    //    static private Pen pen = null;

    //    private Point prevPoint;

    //    protected override void OnStylusDown(RawStylusInput rawStylusInput)
    //    {
    //        // Allocate memory to store the previous point to draw from.
    //        prevPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
    //        base.OnStylusDown(rawStylusInput);
    //    }

    //    protected override void OnDraw(DrawingContext drawingContext,
    //                                   StylusPointCollection stylusPoints,
    //                                   Geometry geometry, Brush fillBrush)
    //    {
    //        // Create a new Brush, if necessary.
    //        if (brush == null)
    //        {
    //            brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 20d);
    //        }

    //        // Create a new Pen, if necessary.
    //        if (pen == null)
    //        {
    //            pen = new Pen(brush, 2d);
    //        }

    //        // Draw linear gradient ellipses between 
    //        // all the StylusPoints that have come in.
    //        for (int i = 0; i < stylusPoints.Count; i++)
    //        {
    //            Point pt = (Point)stylusPoints[i];
    //            Vector v = Point.Subtract(prevPoint, pt);

    //            // Only draw if we are at least 4 units away 
    //            // from the end of the last ellipse. Otherwise, 
    //            // we're just redrawing and wasting cycles.
    //            if (v.Length > 4)
    //            {
    //                // Set the thickness of the stroke based 
    //                // on how hard the user pressed.
    //                double radius = stylusPoints[i].PressureFactor * 10d;
    //                drawingContext.DrawEllipse(brush, pen, pt, radius, radius);
    //                prevPoint = pt;
    //            }
    //        }
    //    }
    //}

}
