using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using System.Globalization;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum PopupPositions
    {
        Above,
        Below,
        Left,
        Right
    }

    public sealed class RadiatingButton : Control
    {

        #region Constants
        
        private const double RADIATE_SIZE_DEFAULT = 0d;
        private const double RADIATE_OPACITY_DEFAULT = 0.0;
        private const double RADIATE_OPACITY_START = 0.6;
        private const double RADIATE_OPACITY_END = 0.4;

        private const double GRID_SIZE = 150d;
        private const double ROW_HEIGHT = 90d;
        private const double BUTTON_SIZE = 150d;

        private const double POPUP_MARGIN = 30d;

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";
        private const string URI_TRY_IT_IMAGE = @"ms-appx:///Assets/Universal/tryit-icon.png";
        private const string URI_PINCH_ZOOM_IMAGE = @"ms-appx:///Assets/Universal/tryit-icon.png";

        #endregion

        #region Private Members

        // ui elements to track
        Border _layoutRoot = null;
        Button _hostButton = null;
        Grid _grid = null;
        Ellipse _entranceEllipse = null;
        Ellipse _radiateEllipse = null;
        Path _tryItBoxPath = null;
        Rectangle _tryItBoxRectangle = null;
        ImageEx _imageX = null;

        Storyboard _entranceStoryboard = null;
        Storyboard _radiatingStoryboardX = null;
        Storyboard _radiatingStoryboardY = null;
        Storyboard _radiatingStoryboardOpacity = null;
        Storyboard _tryItIndicatorPathStoryboard = null;
        Storyboard _tryItIndicatorRectangleStoryboard = null;
        Storyboard _tryItTextStoryboard = null;

        private Popup _popupChild = null;

        private DispatcherTimer _timerRadiate = null;
        private int _dispatchCountRadiate = 0;
        private DispatcherTimer _timerEntrance = null;
        private int _dispatchCountEntrance = 0;
        private double TRY_IT_DELAY = 2000;
        //private double popup_spacer = popup_margin + (radiate_size_end - radiate_size_start) * 1.1;

        #endregion

        #region Construction/Destruction

        public RadiatingButton()
        {
            this.DefaultStyleKey = typeof(RadiatingButton);

            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Dependency Properties

        // IsPenOnly
        public static readonly DependencyProperty IsPenOnlyProperty =
            DependencyProperty.Register("IsPenOnly", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool IsPenOnly
        {
            get => (bool)GetValue(IsPenOnlyProperty);
            set => SetValue(IsPenOnlyProperty, value);
        }
        // IsTouchOnly
        public static readonly DependencyProperty IsTouchOnlyProperty =
            DependencyProperty.Register("IsTouchOnly", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool IsTouchOnly
        {
            get => (bool)GetValue(IsTouchOnlyProperty);
            set => SetValue(IsTouchOnlyProperty, value);
        }
        // IsMouseOnly
        public static readonly DependencyProperty IsMouseOnlyProperty =
            DependencyProperty.Register("IsMouseOnly", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool IsMouseOnly
        {
            get => (bool)GetValue(IsMouseOnlyProperty);
            set => SetValue(IsMouseOnlyProperty, value);
        }

        // TryItEnabled
        public static readonly DependencyProperty TryItEnabledProperty =
            DependencyProperty.Register("TryItEnabled", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool TryItEnabled
        {
            get => (bool)GetValue(TryItEnabledProperty);
            set => SetValue(TryItEnabledProperty, value);
        }

        // PopupPosition
        public static readonly DependencyProperty PopupPositionProperty =
            DependencyProperty.Register("PopupPosition", typeof(PopupPositions), typeof(RadiatingButton), new PropertyMetadata(PopupPositions.Right, OnPopupPositionChanged));

        public PopupPositions PopupPosition
        {
            get => (PopupPositions)GetValue(PopupPositionProperty);
            set => SetValue(PopupPositionProperty, value);
        }

        // AnimationEnabled
        public static readonly DependencyProperty AnimationEnabledProperty =
            DependencyProperty.Register("AnimationEnabled", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true, OnAnimationEnabledChanged));

        public bool AnimationEnabled
        {
            get => (bool)GetValue(AnimationEnabledProperty);
            set => SetValue(AnimationEnabledProperty, value);
        }

        // AnimationOrder
        public static readonly DependencyProperty AnimationOrderProperty =
            DependencyProperty.Register("AnimationOrder", typeof(int), typeof(RadiatingButton), new PropertyMetadata(0, OnAnimationOrderChanged));

        public int AnimationOrder
        {
            get => (int)GetValue(AnimationOrderProperty);
            set => SetValue(AnimationOrderProperty, (value < 0) ? 0 : value);
        }

        // AnimationRepeat
        public static readonly DependencyProperty AnimationRepeatProperty =
            DependencyProperty.Register("AnimationRepeat", typeof(RepeatBehavior), typeof(RadiatingButton), new PropertyMetadata(new RepeatBehavior(1d), OnAnimationOrderChanged));

        public RepeatBehavior AnimationRepeat
        {
            get => (RepeatBehavior)GetValue(AnimationRepeatProperty);
            set => SetValue(AnimationRepeatProperty, value);
        }

        // EntranceDurationInMilliseconds
        public static readonly DependencyProperty EntranceDurationInMillisecondsProperty =
            DependencyProperty.Register("EntranceDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(400d, OnEntranceDurationInMillisecondsChanged));

        public double EntranceDurationInMilliseconds
        {
            get { return (double)GetValue(EntranceDurationInMillisecondsProperty); }
            set { SetValue(EntranceDurationInMillisecondsProperty, value); }
        }

        // EntranceStaggerDelayInMilliseconds
        public static readonly DependencyProperty EntranceStaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("EntranceStaggerDelayInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(0d, OnEntranceStaggerDelayInMillisecondsChanged));

        public double EntranceStaggerDelayInMilliseconds
        {
            get { return (double)GetValue(EntranceStaggerDelayInMillisecondsProperty); }
            set { SetValue(EntranceStaggerDelayInMillisecondsProperty, value); }
        }

        // RadiateDurationInMilliseconds
        public static readonly DependencyProperty RadiateDurationInMillisecondsProperty =
            DependencyProperty.Register("RadiateDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(800d, OnRadiateDurationInMillisecondsChanged));

        public double RadiateDurationInMilliseconds
        {
            get { return (double)GetValue(RadiateDurationInMillisecondsProperty); }
            set { SetValue(RadiateDurationInMillisecondsProperty, value); }
        }

        // RadiateStaggerDelayInMilliseconds
        public static readonly DependencyProperty RadiateStaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("RadiateStaggerDelayInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(0d, OnRadiateStaggerDelayInMillisecondsChanged));

        public double RadiateStaggerDelayInMilliseconds
        {
            get { return (double)GetValue(RadiateStaggerDelayInMillisecondsProperty); }
            set { SetValue(RadiateStaggerDelayInMillisecondsProperty, value); }
        }

        #endregion

        #region Public Properties

        public Popup PopupChild
        {
            get => _popupChild;
            set
            {
                // save it
                _popupChild = value;

                if (null != value)
                {
                    // catch the closed event for the popup
                    _popupChild.Closed += this.Popup_Closed;

                    // catch the image gallery Closed event
                    object contentChild = _popupChild.Child;

                    //if (contentChild is PopupContentImageGallery pcig)
                    //{
                    //    pcig.Closed += this.PopupChild_Closed;
                    //}
                    //else if (contentChild is PopupContentCompareGallery cg)
                    //{
                    //    cg.ClosedEvent += this.PopupChild_Closed;
                    //}
                }
            }
        }
        
        //public double RadiateOffset { get => -1 * (BUTTON_SIZE - ENTRANCE_SIZE); }
        public double RadiateOffset { get => -16; }

        public string TelemetryId { get; set; }

        #endregion

        #region Public Methods

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }

        public void ClosePopup()
        {
            if (null != this.PopupChild)
            {
                this.PopupChild.IsOpen = false;
            }
        }

        public void StartRadiateAnimation()
        {
            // if any of the things we need are null, we can't start
            if ((null == _radiatingStoryboardX) || (null == _radiatingStoryboardY) || (null == _radiatingStoryboardOpacity))
            {
                // inc the counter
                _dispatchCountRadiate++;

                // limit the number of times we do this
                if (_dispatchCountRadiate < 10)
                {
                    // create a timer
                    if (null == _timerRadiate)
                    {
                        _timerRadiate = new DispatcherTimer()
                        {
                            Interval = TimeSpan.FromMilliseconds(500d)
                        };
                        _timerRadiate.Tick += DispatcherTimerRadiate_Tick;
                    }

                    // start it
                    _timerRadiate.Start();
                }

                // return
                return;
            }

            // set opacity
            if (null != _radiateEllipse)
            {
                _radiateEllipse.Opacity = RADIATE_OPACITY_START;
            }

            // launch the storyboards
            if (null != _radiatingStoryboardX)
            {
                _radiatingStoryboardX.Begin();
            }

            if (null != _radiatingStoryboardY)
            {
                _radiatingStoryboardY.Begin();
            }
            if (null != _radiatingStoryboardOpacity)
            {
                _radiatingStoryboardOpacity.Begin();
            }
        }

        public void ResetRadiateAnimation()
        {
            if (null != _radiatingStoryboardX)
            {
                _radiatingStoryboardX.Stop();
            }

            if (null != _radiatingStoryboardY)
            {
                _radiatingStoryboardY.Stop();
            }
            if (null != _radiatingStoryboardOpacity)
            {
                _radiatingStoryboardOpacity.Stop();
            }

            // reset opacity
            if (null != _radiateEllipse)
            {
                _radiateEllipse.Opacity = RADIATE_OPACITY_END;
            }
        }

        public void StartEntranceAnimation()
        {
            // if the storyboard is null, we can't start
            if (null == _entranceStoryboard)
            {
                // inc the counter
                _dispatchCountEntrance++;

                // limit the number of times we do this
                if (_dispatchCountEntrance < 10)
                {
                    // create a timer
                    if (null == _timerEntrance)
                    {
                        _timerEntrance = new DispatcherTimer()
                        {
                            Interval = TimeSpan.FromMilliseconds(500d)
                        };
                        _timerEntrance.Tick += DispatcherTimerEntrance_Tick;
                    }

                    // start it
                    _timerEntrance.Start();
                }

                // return
                return;
            }

            if (null != _entranceStoryboard)
            {
                _entranceStoryboard.Begin();
            }
            if (null != _tryItIndicatorPathStoryboard)
            {
                _tryItIndicatorPathStoryboard.Begin();
            }
            if (null != _tryItIndicatorRectangleStoryboard)
            {
                _tryItIndicatorRectangleStoryboard.Begin();
            }
        }

        public void ResetEntranceAnimation()
        {
            if (null != _entranceStoryboard)
            {
                _entranceStoryboard.Stop();
                _entranceEllipse.Opacity = 0d;
                _radiateEllipse.Opacity = 0d;
                _tryItBoxRectangle.Opacity = 0d;
                _tryItBoxPath.Opacity = 0d;
                _radiateEllipse.Opacity = 0d;
            }
        }

        #endregion

        #region Private Methods

        private void DispatcherTimerRadiate_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timerRadiate) { _timerRadiate.Stop(); }

            // call the method that set up the timer
            this.StartRadiateAnimation();
        }

        private void DispatcherTimerEntrance_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timerEntrance) { _timerEntrance.Stop(); }

            // call the method that set up the timer
            this.StartEntranceAnimation();
        }

        #endregion

        #region Custom Events

        public delegate void ClickedEvent(object sender, EventArgs e);

        public event ClickedEvent Clicked;

        private void RaiseClickedEvent(RadiatingButton radiatingButton, EventArgs e)
        {
            Clicked?.Invoke(radiatingButton, e);
        }

        #endregion

        #region Event Handlers
        
        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {

        }

        private static void OnPopupPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPopupTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChildPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAnimationOrderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAnimationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAnimationRepeatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnEntranceDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnEntranceStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnRadiateDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnRadiateStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            HandleClick();
            Debug.WriteLine("Grid Clicked or touched...");
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            HandleClick();
            Debug.WriteLine("Grid Clicked or touched...");
        }

        private void HandleClick()
        {
            if (null != this.PopupChild)
            {
                if (this.PopupChild.IsOpen)
                {
                    // close it
                    this.PopupChild.IsOpen = false;
                    _imageX.Opacity = 0.0;
                }
                else
                {
                    // if the horizontal offset is -1, calculate it
                    if (-1 == this.PopupChild.HorizontalOffset)
                    {
                        this.PopupChild.HorizontalOffset = GetPopupHorizontalOffset();
                    }

                    // if the vertical offset is -1, calculate it
                    if (-1 == this.PopupChild.VerticalOffset)
                    {
                        this.PopupChild.VerticalOffset = GetPopupVerticalOffset();
                    }

                    // open it
                    this.PopupChild.IsOpen = true;
                    _imageX.Opacity = 1.0;

                    // telemetry
                    //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                }
            }

            RaiseClickedEvent(this, new EventArgs());
        }

        private void PopupChild_Closed(object sender, EventArgs e)
        {
            if (null != this.PopupChild)
            {
                this.PopupChild.IsOpen = false;
            }

            HandleClose();
        }

        private void Popup_Closed(object sender, object e)
        {
            HandleClose();
        }

        private void HandleClose()
        {
            if (null != _imageX)
            {
                // hide the X
                _imageX.Opacity = 0.0;
            }
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Border)this.GetTemplateChild("LayoutRoot");

            // we can't work without it, so return if that failed
            if (null == _layoutRoot) { return; }

            // if the grid doesn't exist
            if (null == _grid)
            {
                double GridWidth = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonGridWidth);
                double RadiatingButtonHeight = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);

                // create the grid
                _grid = new Grid()
                {
                    Name = this.Name + "Grid",
                    Width = GridWidth,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                    RowSpacing = 0d,
                    ColumnSpacing = 0d
                };

                // add pointer pressed event
                _grid.PointerPressed += Grid_PointerPressed;

                // add to the button
                _layoutRoot.Child = _grid;

                // if try it enabled, get all appropriate sizes and add elements, otherwise, skip it
                if (this.TryItEnabled)
                {
                    // get color for rectangle and path
                    SolidColorBrush TryItColor = GetSolidColorBrush("#FF0078D4");

                    // get sizes
                    double TryItBoxHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItBoxHeight);
                    double TryItPathHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathHeight);
                    double TryItPathWidth = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathWidth);
                    double RadiatingButtonTopSpacerHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathHeight);
                    double ButtonDialogueTopSpacerHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathHeight);
                    double ButtonDialogueHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathHeight);

                    // define rows and columns
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TryItBoxHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TryItPathHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonTopSpacerHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ButtonDialogueTopSpacerHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ButtonDialogueTopSpacerHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ButtonDialogueHeight) });

                    _tryItBoxRectangle = new Rectangle()
                    {
                        Height = TryItBoxHeight,
                        Fill = GetSolidColorBrush("#FF0078D4"),
                        Stroke = new SolidColorBrush(Colors.White),
                        StrokeThickness = 2
                    };

                    Grid.SetRow(_tryItBoxRectangle, 0);
                    Grid.SetColumn(_tryItBoxRectangle, 0);

                    _grid.Children.Add(_tryItBoxRectangle);

                    // create the tryit triangle
                    LineSegment Topright = new LineSegment()
                    {
                        Point = new Point(TryItPathWidth, 0)
                    };

                    LineSegment Peak = new LineSegment()
                    {
                        Point = new Point(TryItPathHeight, TryItPathHeight),
                    };

                    LineSegment TopLeft = new LineSegment()
                    {
                        Point = new Point(0, 0)
                    };

                    PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                    myPathSegmentCollection.Add(Topright);
                    myPathSegmentCollection.Add(Peak);
                    myPathSegmentCollection.Add(TopLeft);

                    PathFigure _indicator = new PathFigure()
                    {
                        StartPoint = new Point(0, 0),
                        Segments = myPathSegmentCollection,
                    };

                    PathFigureCollection myPathFigureCollection = new PathFigureCollection();
                    myPathFigureCollection.Add(_indicator);

                    PathGeometry myPathGeometry = new PathGeometry();
                    myPathGeometry.Figures = myPathFigureCollection;


                    _tryItBoxPath = new Path()
                    {
                        Stroke = new SolidColorBrush(Colors.White),
                        Fill = TryItColor,
                        StrokeThickness = 2,
                        Data = myPathGeometry,
                    };

                    Grid.SetRow(_tryItBoxPath, 1);
                    Grid.SetColumn(_tryItBoxPath, 0);
                    _tryItBoxPath.HorizontalAlignment = HorizontalAlignment.Center;

                    _grid.Children.Add(_tryItBoxPath);

                    _tryItIndicatorRectangleStoryboard = AnimationHelper.CreateEasingAnimation(_tryItBoxRectangle, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds + TRY_IT_DELAY, false, false, new RepeatBehavior(1));
                    _tryItIndicatorPathStoryboard = AnimationHelper.CreateEasingAnimation(_tryItBoxPath, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds + TRY_IT_DELAY, false, false, new RepeatBehavior(1));
                    //_tryItTextStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1));

                }
                else
                {
                    _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GridWidth) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonHeight) });
                }

                // create the radiating ellipse
                _radiateEllipse = new Ellipse()
                {
                    Name = this.Name + "radiateEllipse",
                    Width = RadiatingButtonHeight,
                    Height = RadiatingButtonHeight,
                    Fill = new SolidColorBrush(Colors.White),
                    Opacity = RADIATE_OPACITY_DEFAULT,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_radiateEllipse, 2);
                Grid.SetColumn(_radiateEllipse, 0);


                // add it to the canvas
                _grid.Children.Add(_radiateEllipse);

                // calculate beginning and end of animation
                double RADIATE_SIZE_START = RadiatingButtonHeight;
                double RADIATE_SIZE_END = RadiatingButtonHeight * 1.2;

                // create storyboards
                _radiatingStoryboardX = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Width", RADIATE_SIZE_DEFAULT, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _radiatingStoryboardY = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Height", RADIATE_SIZE_DEFAULT, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Opacity", 0.0, RADIATE_OPACITY_START, RADIATE_OPACITY_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                // create the entrance ellipse
                _entranceEllipse = new Ellipse()
                {
                    Name = this.Name + "entranceEllipse",
                    Width = RadiatingButtonHeight,
                    Height = RadiatingButtonHeight,
                    Fill = new SolidColorBrush(Colors.White),
                    Opacity = 0d,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_entranceEllipse, 2);
                Grid.SetColumn(_entranceEllipse, 0);

                // add to the grid
                _grid.Children.Add(_entranceEllipse);

                // get the size for icons
                double IconWidth = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight);

                // create the X image
                _imageX = new ImageEx()
                {
                    Name = this.Name + "ImageX",
                    ImageSource = URI_X_IMAGE,
                    Width = IconWidth,
                    Opacity = 0.0d,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_imageX, 2);
                Grid.SetColumn(_imageX, 0);

                // add it to the grid
                _grid.Children.Add(_imageX);

                // create storyboard
                _entranceStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1));

                // create the radiating ellipse
                _radiateEllipse = new Ellipse()
                {
                    Name = this.Name + "radiateEllipse",
                    Width = RadiatingButtonHeight,
                    Height = RadiatingButtonHeight,
                    Fill = new SolidColorBrush(Colors.White),
                    Opacity = RADIATE_OPACITY_DEFAULT,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_radiateEllipse, 2);
                Grid.SetColumn(_radiateEllipse, 0);

            }
        }

        private Storyboard SetupEntranceAnimation(Ellipse ellipse, double duration, double staggerDelay)
        {
            double totalDuration = duration + staggerDelay;
            double expandDuration = (duration * 0.65);
            double contractDuration = duration - expandDuration;

            // create the storyboard
            Storyboard storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                AutoReverse = false,
                RepeatBehavior = new RepeatBehavior(1d)
            };

            // create the key frames holder
            DoubleAnimationUsingKeyFrames _frames = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(totalDuration),
                EnableDependentAnimation = true,
                AutoReverse = false,
                RepeatBehavior = new RepeatBehavior(1d)
            };

            // need sine easing
            CubicEase cubicEaseIn = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn
            };

            // create frame 0
            EasingDoubleKeyFrame _frame0 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0d)),
                Value = 0d,
                EasingFunction = cubicEaseIn
            };

            // create delay frame
            EasingDoubleKeyFrame _frameStagger = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(staggerDelay)),
                Value = 0d,
                EasingFunction = cubicEaseIn
            };

            // create frame 1
            EasingDoubleKeyFrame _frame1 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration)),
                Value = 1d,
                EasingFunction = cubicEaseIn
            };

            // add frames to the collection
            _frames.KeyFrames.Add(_frame0);
            if (staggerDelay > 0)
            {
                _frames.KeyFrames.Add(_frameStagger);
            }
            _frames.KeyFrames.Add(_frame1);

            // add frame collection to the storyboard
            storyboard.Children.Add(_frames);

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, ellipse);
            Storyboard.SetTargetProperty(storyboard, "Opacity");

            return storyboard;
        }

        #endregion

        #region Code Helpers
        
        #endregion

        #region UI Helpers

        private double GetPopupHorizontalOffset()
        {
            double offset = 0d;

            if ((null != _hostButton) && (null != this.PopupChild) && (null != this.PopupChild.Child))
            {
                // get the position of our host button on the window
                var ttv = _hostButton.TransformToVisual(Window.Current.Content);
                Point point = ttv.TransformPoint(new Point(0, 0));

                // get the width of the child of the popup
                double popupWidth = 0;

                double POPUP_SPACER = StyleHelper.GetApplicationDouble("PopupSpacer");

                object popupContent = this.PopupChild.Child;

                //if (popupContent is PopupContentText pct)
                //{
                //    popupWidth = pct.Width;
                //}
                //else if (popupContent is PopupContentFastCharge pcfc)
                //{
                //    popupWidth = pcfc.Width;
                //}
                //else if (popupContent is PopupContentCompareGallery pccg)
                //{
                //    popupWidth = pccg.Width;
                //}

                // which position?
                switch (this.PopupPosition)
                {
                    case PopupPositions.Above:
                    default:
                        offset = point.X + (_hostButton.ActualWidth / 2) - (popupWidth / 2);
                        break;

                    case PopupPositions.Below:
                        offset = point.X + (_hostButton.ActualWidth / 2) - (popupWidth / 2);
                        break;

                    case PopupPositions.Left:
                        offset = point.X - popupWidth - POPUP_SPACER;
                        break;

                    case PopupPositions.Right:
                        offset = point.X + _hostButton.ActualWidth + POPUP_SPACER;
                        break;
                }
            }

            return offset;
        }

        private double GetPopupVerticalOffset()
        {
            double offset = 0d;

            double POPUP_SPACER = StyleHelper.GetApplicationDouble("PopupSpacer");

            if ((null != _hostButton) && (null != this.PopupChild) && (null != this.PopupChild.Child))
            {
                // get the position of our host button on the window
                var ttv = _hostButton.TransformToVisual(Window.Current.Content);
                Point point = ttv.TransformPoint(new Point(0, 0));

                // get the height of the child of the popup
                double popupHeight = 0;
                object popupContent = this.PopupChild.Child;

                //if (popupContent is PopupContentText pct)
                //{
                //    popupHeight = pct.ActualHeight;

                //    // hack: we can't get the height here because this isn't rendered yet
                //    if (0 == popupHeight) { popupHeight = pct.Width * 2 / 3; }
                //}
                //else if (popupContent is PopupContentFastCharge pcfc)
                //{
                //    popupHeight = pcfc.ActualHeight;

                //    // hack: we can't get the height here because this isn't rendered yet
                //    if (0 == popupHeight) { popupHeight = pcfc.Width * 2; }
                //}
                //else if (popupContent is PopupContentCompareGallery pccg)
                //{
                //    popupHeight = pccg.ActualHeight;

                //    // hack: we can't get the height here because this isn't rendered yet
                //    if (0 == popupHeight) { popupHeight = pccg.Width * 2 / 3; }
                //}

                // which position?
                switch (this.PopupPosition)
                {
                    case PopupPositions.Above:
                    default:
                        offset = point.Y - popupHeight - POPUP_SPACER;
                        break;

                    case PopupPositions.Below:
                        offset = point.Y + _hostButton.ActualHeight + POPUP_SPACER;
                        break;

                    case PopupPositions.Left:
                        offset = point.Y + (_hostButton.ActualHeight / 2) - (popupHeight / 2);
                        break;

                    case PopupPositions.Right:
                        offset = point.Y + (_hostButton.ActualHeight / 2) - (popupHeight / 2);
                        break;
                }
            }

            return offset;
        }

        #endregion
    }
}
