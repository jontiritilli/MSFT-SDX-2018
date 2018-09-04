using System;
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

        private const double ENTRANCE_SIZE = 40d;

        private const double RADIATE_SIZE_DEFAULT = 0d;
        private const double RADIATE_SIZE_START = 40d;
        private const double RADIATE_SIZE_END = 68d;
        private const double RADIATE_OPACITY_DEFAULT = 0.0;
        private const double RADIATE_OPACITY_START = 0.6;
        private const double RADIATE_OPACITY_END = 0.4;

        private const double GRID_SIZE = 150d;
        private const double TRY_IT_HEIGHT = 50d;
        private const double TRY_IT_INDICATOR_HEIGHT = 10d;
        private const double ROW_HEIGHT = 90d;
        private const double BUTTON_SIZE = 150d;
        private const double X_SIZE = 8d;

        private const double POPUP_MARGIN = 30d;
        private double POPUP_SPACER = POPUP_MARGIN + (RADIATE_SIZE_END - RADIATE_SIZE_START) * 1.1;

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        #endregion

        #region Private Members

        // ui elements to track
        Border _layoutRoot = null;
        Button _hostButton = null;
        Grid _grid = null;
        Ellipse _entranceEllipse = null;
        Ellipse _radiateEllipse = null;
        Path _indicatorPath = null;
        Rectangle _indicatorRectangle = null;
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

            this.PopupChild = CreatePopup(PopupType, null, 60d);
        }

        #endregion

        #region Dependency Properties

        // IsPenOnly
        public static readonly DependencyProperty IsPenOnlyProperty =
            DependencyProperty.Register("IsPenOnly", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false, OnTryItClicked));

        public bool IsPenOnly
        {
            get => (bool)GetValue(IsPenOnlyProperty);
            set => SetValue(IsPenOnlyProperty, value);
        }

        // TryItEnabled
        public static readonly DependencyProperty TryItEnabledProperty =
            DependencyProperty.Register("TryItEnabled", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false, OnTryItClicked));

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

        // PopupType
        public static readonly DependencyProperty PopupTypeProperty =
            DependencyProperty.Register("PopupType", typeof(PopupTypes), typeof(RadiatingButton), new PropertyMetadata(PopupTypes.Text, OnPopupTypeChanged));

        public PopupTypes PopupType
        {
            get => (PopupTypes)GetValue(PopupTypeProperty);
            set => SetValue(PopupTypeProperty, value);
        }

        // PopupText
        public static readonly DependencyProperty PopupTextProperty =
            DependencyProperty.Register("PopupText", typeof(string), typeof(RadiatingButton), new PropertyMetadata(null, OnPopupTextChanged));

        public string PopupText
        {
            get => (string)GetValue(PopupTextProperty);
            set => SetValue(PopupTextProperty, value);
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
                _indicatorRectangle.Opacity = 0d;
                _indicatorPath.Opacity = 0d;
                _radiateEllipse.Opacity = 0d;
            }
        }

        #endregion

        #region Static Methods

        public static Popup CreatePopup(PopupTypes type, string text, double width, double leftOffset = -1, double topOffset = -1)
        {
            // create the popup
            Popup popup = new Popup()
            {
                IsOpen = false,
                IsLightDismissEnabled = true,
                HorizontalOffset = leftOffset,
                VerticalOffset = topOffset
            };

            switch (type)
            {
                case PopupTypes.Fullscreen:
                    break;

                case PopupTypes.Text:
                    PopupContentText popupText = new PopupContentText()
                    {
                        Text = text,
                        AutoStart = true
                    };
                    if (!Double.IsInfinity(width) && !double.IsNaN(width))
                    {
                        popupText.Width = width;
                    }
                    popup.Child = popupText;
                    break;

                case PopupTypes.Video:
                    break;

                case PopupTypes.Image:
                    break;

                case PopupTypes.BatteryLife:
                    popup.Child = new PopupContentBatteryLife()
                    {
                        AutoStart = true
                    };
                    break;

                default:
                    break;
            }

            return popup;
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

        private static void OnPopupTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChildPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnTryItClicked(DependencyObject d, DependencyPropertyChangedEventArgs e)
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


        private void HostButton_Click(object sender, RoutedEventArgs e)
        {
            HandleClick();
        }

        private void HostButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            HandleClick();
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
            // if the button doesn't exist
            if (null == _hostButton)
            {
                // create the button style
                Style buttonStyle = StyleHelper.GetApplicationStyle("RoundRadiatingButton");

                // create it
                _hostButton = new Button()
                {
                    Name = this.Name + "HostButton",
                    Background = new SolidColorBrush(Colors.Transparent),
                    Width = BUTTON_SIZE,
                    Height = BUTTON_SIZE,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                };
                if (null != buttonStyle) { _hostButton.Style = buttonStyle; }
                _hostButton.PointerPressed += HostButton_PointerPressed;
                _hostButton.Click += HostButton_Click;

                // add it to the layout
                _layoutRoot.Child = _hostButton;

                // create the grid
                _grid = new Grid()
                {
                    Name = this.Name + "Grid",
                    Width = GRID_SIZE,
                    Height = GRID_SIZE,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                    RowSpacing = 0d,
                    ColumnSpacing = 0d
                };

                // add to the button
                _hostButton.Content = _grid;

                // add rows for try it message and indicator if enabled
                GridLength TRY_IT_ROW = (!this.TryItEnabled) ? new GridLength(0) : new GridLength(TRY_IT_HEIGHT);
                GridLength TRY_IT_INDICATOR_ROW = (!this.TryItEnabled) ? new GridLength(0) : new GridLength(TRY_IT_INDICATOR_HEIGHT);

                // define rows and columns
                _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GRID_SIZE) });
                _grid.RowDefinitions.Add(new RowDefinition() { Height = TRY_IT_ROW });
                _grid.RowDefinitions.Add(new RowDefinition() { Height = TRY_IT_INDICATOR_ROW });
                _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ROW_HEIGHT) });
                _grid.RowDefinitions.Add(new RowDefinition() { Height = TRY_IT_ROW });

                _indicatorRectangle = new Rectangle();
                _indicatorRectangle.Fill = new SolidColorBrush(Colors.Blue);

                Grid.SetRow(_indicatorRectangle, 0);
                Grid.SetColumn(_indicatorRectangle, 0);

                _grid.Children.Add(_indicatorRectangle);

                // create the tryit indicator
                PathFigure figure = new PathFigure();
                figure.StartPoint = new Point(0, 0);

                LineSegment Topright = new LineSegment();
                Topright.Point = new Point(20, 0);

                LineSegment Peak = new LineSegment();
                Peak.Point = new Point(10, 10);

                LineSegment TopLeft = new LineSegment();
                TopLeft.Point = new Point(0, 0);

                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                myPathSegmentCollection.Add(Topright);
                myPathSegmentCollection.Add(Peak);
                myPathSegmentCollection.Add(TopLeft);

                figure.Segments = myPathSegmentCollection;

                PathFigureCollection myPathFigureCollection = new PathFigureCollection();
                myPathFigureCollection.Add(figure);

                PathGeometry myPathGeometry = new PathGeometry();
                myPathGeometry.Figures = myPathFigureCollection;

                _indicatorPath = new Path()
                {
                    Stroke = new SolidColorBrush(Colors.Blue),
                    Fill = new SolidColorBrush(Colors.Blue),
                    StrokeThickness = 1,
                    Data = myPathGeometry
                };

                Grid.SetRow(_indicatorPath, 1);
                Grid.SetColumn(_indicatorPath, 0);
                _indicatorPath.HorizontalAlignment = HorizontalAlignment.Center;

                _grid.Children.Add(_indicatorPath);

                _tryItIndicatorRectangleStoryboard = AnimationHelper.CreateEasingAnimation(_indicatorRectangle, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds + TRY_IT_DELAY, false, false, new RepeatBehavior(1));
                _tryItIndicatorPathStoryboard = AnimationHelper.CreateEasingAnimation(_indicatorPath, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds + TRY_IT_DELAY, false, false, new RepeatBehavior(1));
                //_tryItTextStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1));

                // create the radiating ellipse
                _radiateEllipse = new Ellipse()
                {
                    Name = this.Name + "radiateEllipse",
                    Width = RADIATE_SIZE_DEFAULT,
                    Height = RADIATE_SIZE_DEFAULT,
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

                // create storyboards
                _radiatingStoryboardX = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Width", RADIATE_SIZE_DEFAULT, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _radiatingStoryboardY = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Height", RADIATE_SIZE_DEFAULT, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Opacity", 0.0, RADIATE_OPACITY_START, RADIATE_OPACITY_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                // create the entrance ellipse
                _entranceEllipse = new Ellipse()
                {
                    Name = this.Name + "entranceEllipse",
                    Width = ENTRANCE_SIZE,
                    Height = ENTRANCE_SIZE,
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

                // create the X image
                _imageX = new ImageEx()
                {
                    Name = this.Name + "ImageX",
                    ImageSource = URI_X_IMAGE,
                    Width = X_SIZE,
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
            }
        }

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
