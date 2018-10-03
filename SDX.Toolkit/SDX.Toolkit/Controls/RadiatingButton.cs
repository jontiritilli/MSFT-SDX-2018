using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Devices.Input;
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

    public enum RadiatingButtonIcons
    {
        Dial,
        Pen,        
        Pinch,
        Touch
    }

    public sealed class RadiatingButton : Control
    {
        #region Constants
        
        private const double RADIATE_SIZE_DEFAULT = 0d;
        private const double RADIATE_OPACITY_DEFAULT = 0.0;
        private const double RADIATE_OPACITY_START = 0.8;
        private const double RADIATE_OPACITY_END = 0.0;
        private const double TRY_IT_DELAY = 2500;

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";
        private const string URI_TRY_IT_IMAGE = @"ms-appx:///Assets/RadiatingButton/tryit_dot.png";
        private const string URI_TRY_IT_PEN_IMAGE = @"ms-appx:///Assets/RadiatingButton/tryit_pen.png";
        private const string URI_TRY_IT_DIAL_IMAGE = @"ms-appx:///Assets/RadiatingButton/tryit_dial.png";
        private const string URI_PINCH_ZOOM_IMAGE = @"ms-appx:///Assets/RadiatingButton/pinch.png";
        
        #endregion

        #region Private Members

        // ui elements to track
        Border _layoutRoot = null;
        Grid _grid = null;
        Ellipse _entranceEllipse = null;
        Ellipse _radiateEllipse = null;
        Grid _tryItBox = null;
        ImageEx _imageX = null;
        ImageEx _tryItImage= null;
        TextBlockEx _tryItButtonCaption = null;

        Storyboard _entranceStoryboard = null;
        Storyboard _radiatingStoryboardX = null;
        Storyboard _radiatingStoryboardY = null;
        Storyboard _radiatingStoryboardOpacity = null;
        Storyboard _tryItBoxStoryboard = null;
        Storyboard _tryItCaptionStoryboard = null; 
        Storyboard _tryItIconStoryboard = null;

        private Popup _popupChild = null;

        private DispatcherTimer _timerRadiate = null;
        private int _dispatchCountRadiate = 0;
        private DispatcherTimer _timerEntrance = null;
        private int _dispatchCountEntrance = 0;
        private string TRY_IT_IMAGE = null;
        //private double popup_spacer = popup_margin + (radiate_size_end - radiate_size_start) * 1.1;

        #endregion

        #region Construction/Destruction

        public RadiatingButton()
        {
            this.DefaultStyleKey = typeof(RadiatingButton);

            this.Loaded += OnLoaded;
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

        // IsDialOnly
        public static readonly DependencyProperty IsDialOnlyProperty =
            DependencyProperty.Register("IsDialOnly", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool IsDialOnly
        {
            get => (bool)GetValue(IsDialOnlyProperty);
            set => SetValue(IsDialOnlyProperty, value);
        }

        // TryItText
        public static readonly DependencyProperty TryItTextProperty =
            DependencyProperty.Register("TryItText", typeof(string), typeof(RadiatingButton), new PropertyMetadata("Try It"));

        public string TryItText
        {
            get => (string)GetValue(TryItTextProperty);
            set => SetValue(TryItTextProperty, value);
        }

        // TryItCaptionColor
        public static readonly DependencyProperty TryItCaptionColorProperty =
            DependencyProperty.Register("TryItCaptionColor", typeof(string), typeof(RadiatingButton), new PropertyMetadata("Black"));

        public string TryItCaptionColor
        {
            get => (string)GetValue(TryItCaptionColorProperty);
            set => SetValue(TryItCaptionColorProperty, value);
        }

        // TryItCaption
        public static readonly DependencyProperty TryItCaptionProperty =
            DependencyProperty.Register("TryItCaption", typeof(string), typeof(RadiatingButton), new PropertyMetadata(""));

        public string TryItCaption
        {
            get => (string)GetValue(TryItCaptionProperty);
            set => SetValue(TryItCaptionProperty, value);
        }

        // IsRemovedOnInteraction
        public static readonly DependencyProperty IsRemovedOnInteractionProperty =
            DependencyProperty.Register("IsRemovedOnInteraction", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool IsRemovedOnInteraction
        {
            get => (bool)GetValue(IsRemovedOnInteractionProperty);
            set => SetValue(IsRemovedOnInteractionProperty, value);
        }

        // RBIcon
        public static readonly DependencyProperty RadiatingButtonIconProperty =
            DependencyProperty.Register("RadiatingButtonIcon", typeof(RadiatingButtonIcons), typeof(RadiatingButton), new PropertyMetadata(RadiatingButtonIcons.Touch));

        public RadiatingButtonIcons RadiatingButtonIcon
        {
            get => (RadiatingButtonIcons)GetValue(RadiatingButtonIconProperty);
            set => SetValue(RadiatingButtonIconProperty, value);
        }

        // TryItEnabled
        public static readonly DependencyProperty TryItEnabledProperty =
            DependencyProperty.Register("TryItEnabled", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false));

        public bool TryItEnabled
        {
            get => (bool)GetValue(TryItEnabledProperty);
            set => SetValue(TryItEnabledProperty, value);
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
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
            DependencyProperty.Register("AnimationRepeat", typeof(RepeatBehavior), typeof(RadiatingButton), new PropertyMetadata(new RepeatBehavior(new TimeSpan(0, 0, 1)), OnAnimationOrderChanged));

        public RepeatBehavior AnimationRepeat
        {
            get => (RepeatBehavior)GetValue(AnimationRepeatProperty);
            set => SetValue(AnimationRepeatProperty, value);
        }

        // EntranceDurationInMilliseconds
        public static readonly DependencyProperty EntranceDurationInMillisecondsProperty =
            DependencyProperty.Register("EntranceDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(750d, OnEntranceDurationInMillisecondsChanged));

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
            DependencyProperty.Register("RadiateDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(1250d, OnRadiateDurationInMillisecondsChanged));

        public double RadiateDurationInMilliseconds
        {
            get { return (double)GetValue(RadiateDurationInMillisecondsProperty); }
            set { SetValue(RadiateDurationInMillisecondsProperty, value); }
        }

        // RadiateStaggerDelayInMilliseconds
        public static readonly DependencyProperty RadiateStaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("RadiateStaggerDelayInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(1250d, OnRadiateStaggerDelayInMillisecondsChanged));

        public double RadiateStaggerDelayInMilliseconds
        {
            get { return (double)GetValue(RadiateStaggerDelayInMillisecondsProperty); }
            set { SetValue(RadiateStaggerDelayInMillisecondsProperty, value); }
        }

      
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
                    //_popupChild.HorizontalOffset = -1;
                    //_popupChild.VerticalOffset = -1;
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
        #endregion

        #region Public Properties

        //public double RadiateOffset { get => -1 * (BUTTON_SIZE - ENTRANCE_SIZE); }
        public double RadiateOffset { get => -16; }

        public string TelemetryId { get; set; }

        #endregion

        #region Public Methods

        public static SolidColorBrush GetSolidColorBrush(string hex)
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

            // if there is a delay for the radiating button, we can't start the radiate
            if (this.EntranceStaggerDelayInMilliseconds > 0)
            {
                // create a timer
                if (null == _timerRadiate)
                {
                    _timerRadiate = new DispatcherTimer()
                    {
                        Interval = TimeSpan.FromMilliseconds(EntranceStaggerDelayInMilliseconds)
                    };
                    _timerRadiate.Tick += DispatcherTimerRadiate_Tick;
                    _timerRadiate.Tick += (sender, args) =>
                    {// well this works? but ew
                        _timerRadiate.Stop();

                        // set opacity
                        if (null != _radiateEllipse)
                        {
                            _radiateEllipse.Opacity = RADIATE_OPACITY_END;
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
                    };
                }
                // start it
                _timerRadiate.Start();
            }
            else
            {
                // set opacity
                if (null != _radiateEllipse)
                {
                    _radiateEllipse.Opacity = RADIATE_OPACITY_END;
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
            if (null != _tryItBoxStoryboard)
            {
                _tryItBoxStoryboard.Begin();
            }
            if (null != _tryItCaptionStoryboard)
            {
                _tryItCaptionStoryboard.Begin();
            }
            if (null != _tryItIconStoryboard)
            {
                _tryItIconStoryboard.Begin();
            }
        }

        public void ResetEntranceAnimation()
        {
            if (null != _entranceStoryboard)
            {
                _entranceStoryboard.Stop();
                _entranceEllipse.Opacity = 0d;
                _grid.Opacity = 1d;
                if (null != _tryItBox)
                {
                    _tryItBox.Opacity = 0d;
                }
            }
        }

        public void CloseTryIt()
        {
            HandleClick();
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ///TODO Determine dual startup strategy
            if (this.AutoStart)
            {
                this.StartEntranceAnimation();
                this.StartRadiateAnimation();
            }
        }

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

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // declare potential types
            PointerDeviceType pen = PointerDeviceType.Pen;
            PointerDeviceType touch = PointerDeviceType.Touch;
            PointerDeviceType mouse = PointerDeviceType.Mouse;

            // get current device type from event
            PointerDeviceType pointerType = e.Pointer.PointerDeviceType;

            if(IsPenOnly && pointerType != pen)
            {
                return;
            }

            if (IsTouchOnly && pointerType != touch)
            {
                return;
            }

            if (IsMouseOnly && pointerType != mouse)
            {
                return;
            }

            if (IsDialOnly)
            {
                return;
            }

            HandleClick();
        }

        public void HandleClick()
        {
            // for try it, to hide the grid if interaction has occurred
            if (IsRemovedOnInteraction)
            {
                _grid.Opacity = 0.0;
            }

            // stop the storyboard if user clicks, so they don't get a try it after clicking
            if (null != _tryItBoxStoryboard)
            {
                _tryItBoxStoryboard.Stop();
            }
            if (null != _tryItCaptionStoryboard)
            {
                _tryItCaptionStoryboard.Stop();
            }
            if (null != _tryItIconStoryboard)
            {
                _tryItIconStoryboard.Stop();
            }

            if (TryItEnabled && null != this.PopupChild)
            {
                if (this.PopupChild.IsOpen)
                {
                    // close it
                    this.PopupChild.IsOpen = false;

                    // swap the images
                    _imageX.Opacity = 0.0;

                    _tryItImage.Opacity = 1.0;
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

                    // swap the images
                    _imageX.Opacity = 1.0;

                    _tryItImage.Opacity = 0.0;

                    if (null != _tryItBox)
                    {
                        _tryItBox.Opacity = 0.0;
                    }
                    if (null != _tryItButtonCaption)
                    {
                        _tryItButtonCaption.Opacity = 0.0;
                    }
                    

                    // telemetry
                    //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                }
            }
            else  if (null != this.PopupChild)
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
            if (null != _tryItImage)
            {
                // Show the tryit icon
                _tryItImage.Opacity = 1.0;
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

                double radiateEllipseHeight = RadiatingButtonHeight * .9; // set this slightly smaller so it doesn't peek out from behind the button

                // get the size for close icons
                double CloseIconWidth = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonCloseIconWidth);

                // height of the radiating button with a little added space for the radiating animation
                double RadiatingButtonRowHeight = RadiatingButtonHeight * 1.7;
                    
                // calculate beginning and end of animation
                double RADIATE_SIZE_START = RadiatingButtonHeight;
                double RADIATE_SIZE_END = RadiatingButtonHeight * 1.6;

                // create the grid
                _grid = new Grid()
                {
                    Name = this.Name + "Grid",
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                    RowSpacing = 0d,
                    ColumnSpacing = 0d,
                    MinWidth = GridWidth,
                    MaxWidth = 215d
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
                    //height of the blue box
                    double TryItBoxHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItBoxHeight);
                    //height of the triangle
                    double TryItPathHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathHeight);
                    // width of the triangle
                    double TryItPathWidth = TryItPathHeight * 2;
                    // bottom margin of the cover triangle
                    double TryItPathCoverBottomMargin = StyleHelper.GetApplicationDouble(LayoutSizes.TryItPathCoverBottomMargin);
                    // space between message box and ellipse
                    double RadiatingButtonTopSpacerHeight = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseTopSpacer);
                    // space between ellipse and caption
                    double RadiatingButtonEllipseBottomSpacer = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseBottomSpacer);
                    // height of the caption
                    double ButtonCaptionHeight = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonCaptionHeight);
                    // size of the icon for try it buttons
                    double TryItIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight);
                    // size of the icon for try it buttons
                    double TryItDotHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItDotHeight);

                    // define rows and columns
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TryItBoxHeight+TryItPathHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonTopSpacerHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonRowHeight) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonEllipseBottomSpacer) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                    // create the TryIt box content
                    _tryItBox = new Grid()
                    {
                        Opacity = 0.0
                    };

                    // create main blue message box
                    Border Background = new Border
                    {
                        Name = "TryItBg",
                        Height = TryItBoxHeight,
                        Background = TryItColor,
                        BorderBrush = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(2),
                        Padding = new Thickness(5,0,5,0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    // create text box for "Try It" text
                    TextBlockEx TryItBoxText = new TextBlockEx
                    {
                        Name = "TryItText",
                        Text = this.TryItText,
                        TextStyle = TextStyles.TryIt,
                        TextWrapping = TextWrapping.WrapWholeWords,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };

                    // create the blue triangle with white border
                    Polygon Indicator = new Polygon
                    {
                        Name = "TryItTriangle",
                        Fill = TryItColor,
                        Stroke = new SolidColorBrush(Colors.White),
                        StrokeThickness = 2,
                        Margin = new Thickness(0),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                    };
                    Indicator.Points.Add(new Point(TryItPathWidth,0));
                    Indicator.Points.Add(new Point(TryItPathHeight, TryItPathHeight));
                    Indicator.Points.Add(new Point(0, 0));
                    
                    // create a triangle to cover white border at joining of triangle and rectangle
                    Polygon IndicatorStrokeCover = new Polygon
                    {
                        Name = "TryItTriangleCover",
                        Fill = TryItColor,
                        Margin = new Thickness(0,0,1, TryItPathCoverBottomMargin),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                    };
                    IndicatorStrokeCover.Points.Add(new Point(TryItPathWidth, 0));
                    IndicatorStrokeCover.Points.Add(new Point(TryItPathHeight, TryItPathHeight));
                    IndicatorStrokeCover.Points.Add(new Point(0, 0));

                    // add it all to the box (to be added to the main grid)
                    Background.Child = TryItBoxText;
                    _tryItBox.Children.Add(Background);
                    _tryItBox.Children.Add(Indicator);
                    _tryItBox.Children.Add(IndicatorStrokeCover);

                    // set the rows and grids
                    Grid.SetRow(_tryItBox, 0);
                    Grid.SetColumn(_tryItBox, 0);

                    // add it to the main grid
                    _grid.Children.Add(_tryItBox);

                    _tryItBoxStoryboard = AnimationHelper.CreateEasingAnimation(_tryItBox, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds + TRY_IT_DELAY, false, false, new RepeatBehavior(1d));

                    TextStyles TryItCaption = TextStyles.ButtonCaption;

                    switch (this.TryItCaptionColor)
                    {
                        case "Black":
                            TryItCaption = TextStyles.ButtonCaption;
                            break;
                        case "White":
                            TryItCaption = TextStyles.ButtonCaptionDark;
                            break;
                    }

                    _tryItButtonCaption = new TextBlockEx
                    {
                        Name = "TryItCaption",
                        Text = this.TryItCaption,
                        TextStyle = TryItCaption,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.WrapWholeWords,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = 1.0d // QUICK FIX while animation helper isn't working properly
                    };

                    Grid.SetRow(_tryItButtonCaption, 4);
                    Grid.SetColumn(_tryItButtonCaption, 0);

                    // add the caption
                    _grid.Children.Add(_tryItButtonCaption);

                    // set up the story board
                    _tryItCaptionStoryboard = AnimationHelper.CreateEasingAnimation(_tryItButtonCaption, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds + TRY_IT_DELAY, false, false, new RepeatBehavior(1d));

                    _radiateEllipse = new Ellipse()
                    {
                        Name = this.Name + "radiateEllipse",
                        Width = radiateEllipseHeight,
                        Height = radiateEllipseHeight,
                        Fill = new SolidColorBrush(Colors.White),
                        Opacity = 0d,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0)
                    };
                    Grid.SetRow(_radiateEllipse, 2);
                    Grid.SetColumn(_radiateEllipse, 0);

                    // add it to the grid
                    _grid.Children.Add(_radiateEllipse);

                    // create storyboards
                    _radiatingStoryboardX = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Width", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, true, new RepeatBehavior(new TimeSpan(0, 0, 1)));
                    _radiatingStoryboardY = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Height", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, true, new RepeatBehavior(new TimeSpan(0, 0, 1)));
                    _radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Opacity", RADIATE_OPACITY_START, RADIATE_OPACITY_START, RADIATE_OPACITY_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, false, true, new RepeatBehavior(1d));

                    // create the entrance ellipse
                    _entranceEllipse = new Ellipse()
                    {
                        Name = this.Name + "entranceEllipse",
                        Width = RadiatingButtonHeight,
                        Height = RadiatingButtonHeight,
                        Fill = new SolidColorBrush(Colors.White),
                        Stroke = GetSolidColorBrush("#FFD2D2D2"),
                        StrokeThickness = 2,
                        Opacity = 0d,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0)
                    };
                    Grid.SetRow(_entranceEllipse, 2);
                    Grid.SetColumn(_entranceEllipse, 0);

                    // add to the grid
                    _grid.Children.Add(_entranceEllipse);

                    // create storyboard
                    _entranceStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                    // set correct icon for radiating button
                    switch (RadiatingButtonIcon) {
                        case RadiatingButtonIcons.Dial:
                            TRY_IT_IMAGE = URI_TRY_IT_DIAL_IMAGE;
                            break;

                        case RadiatingButtonIcons.Pen:
                            TRY_IT_IMAGE = URI_TRY_IT_PEN_IMAGE;
                            break;

                        case RadiatingButtonIcons.Pinch:
                            TRY_IT_IMAGE = URI_PINCH_ZOOM_IMAGE;
                            break;

                        case RadiatingButtonIcons.Touch:
                            TRY_IT_IMAGE = URI_TRY_IT_IMAGE;
                            TryItIconHeight = TryItDotHeight;
                            break;

                        default:
                            break;
                    }

                    // create the try it icon
                    _tryItImage = new ImageEx
                    {
                        Name = this.Name + "TryItImage",
                        ImageSource = TRY_IT_IMAGE,
                        ImageWidth = TryItIconHeight,
                        Opacity = 0d,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0)
                    };
                    Grid.SetRow(_tryItImage, 2);
                    Grid.SetColumn(_tryItImage, 0);

                    _grid.Children.Add(_tryItImage);

                    _tryItIconStoryboard = AnimationHelper.CreateEasingAnimation(_tryItImage, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                    // create the X image
                    _imageX = new ImageEx()
                    {
                        Name = this.Name + "ImageX",
                        ImageSource = URI_X_IMAGE,
                        ImageWidth = CloseIconWidth,
                        Opacity = 0.0d,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0)
                    };
                    Grid.SetRow(_imageX, 2);
                    Grid.SetColumn(_imageX, 0);

                    // add it to the grid
                    _grid.Children.Add(_imageX);
                }
                // if try it is NOT enabled
                else
                {
                    //// set the grid width
                    //_grid.Width = GridWidth;

                    // only one column
                    _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RadiatingButtonRowHeight) });
                    // create the radiating ellipse
                    _radiateEllipse = new Ellipse()
                    {
                        Name = this.Name + "radiateEllipse",
                        Width = RadiatingButtonHeight,
                        Height = RadiatingButtonHeight,
                        Fill = new SolidColorBrush(Colors.White),
                        Opacity = 0d,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0)
                    };
                    Grid.SetRow(_radiateEllipse, 2);
                    Grid.SetColumn(_radiateEllipse, 0);

                    // add it to the canvas
                    _grid.Children.Add(_radiateEllipse);

                    // create storyboards
                    _radiatingStoryboardX = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Width", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, true, new RepeatBehavior(1d));
                    _radiatingStoryboardY = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Height", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, true, new RepeatBehavior(1d));
                    _radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Opacity", RADIATE_OPACITY_START, RADIATE_OPACITY_START, RADIATE_OPACITY_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, false, true, new RepeatBehavior(1d));
                    // create the entrance ellipse
                    _entranceEllipse = new Ellipse()
                    {
                        Name = this.Name + "entranceEllipse",
                        Width = RadiatingButtonHeight,
                        Height = RadiatingButtonHeight,
                        Fill = new SolidColorBrush(Colors.White),
                        Stroke = GetSolidColorBrush("#FFD2D2D2"),
                        StrokeThickness = 2,
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
                        ImageWidth = CloseIconWidth,
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
                    _entranceStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                }
            }
        }

        #endregion

        #region UI Helpers

        private double GetPopupHorizontalOffset()
        {
            double offset = 0d;
            double RadiatingButtonHeight = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
            if ((null != _grid) && (null != this.PopupChild) && (null != this.PopupChild.Child))
            {
                // get the position of our host button on the window
                var ttv = _grid.TransformToVisual(Window.Current.Content);
                Point point = ttv.TransformPoint(new Point(0, 0));

                // get the width of the child of the popup
                double popupWidth = 0;

                double POPUP_SPACER = StyleHelper.GetApplicationDouble("PopupSpacer");

                object popupContent = this.PopupChild.Child;
                Thickness Padding = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupPadding);
                double defaultPopupWidth = StyleHelper.GetApplicationDouble(LayoutSizes.PopupDefaultWidth);
                if (popupContent is PopupMedia popup)
                {
                    switch (popup.PopupType)
                    {
                        case PopupTypes.Text:
                            //Thickness Padding = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupPadding);
                            popupWidth = popup.Width;// - Padding.Left - Padding.Right;
                            if (Double.IsNaN(popupWidth))
                            {
                                popupWidth = defaultPopupWidth;
                            }
                            break;
                        case PopupTypes.Image:
                            // media width doesnt take into account padding inside thepop up so this shores up the popup width for offset
                            popupWidth = popup.MediaWidth + Padding.Left + Padding.Right;
                            // the width of the popup is defaulted. unsure if a larger width on the media will win but it should
                            // however this will ensure that the width used for position is correct
                            if (defaultPopupWidth > popupWidth)
                            {
                                popupWidth = defaultPopupWidth;
                            }
                            break;
                        case PopupTypes.Video:
                            // media width doesnt take into account padding inside thepop up so this shores up the popup width for offset
                            popupWidth = popup.MediaWidth + Padding.Left + Padding.Right;
                            // the width of the popup is defaulted. unsure if a larger width on the media will win but it should
                            // however this will ensure that the width used for position is correct
                            if (defaultPopupWidth > popupWidth)
                            {
                                popupWidth = defaultPopupWidth;
                            }
                            break;
                        default:
                            break;
                    }
                }
                
                // which position?
                switch (this.PopupPosition)
                {
                    case PopupPositions.Above:
                    default:
                        offset = point.X + (popupWidth / 2);
                        break;

                    case PopupPositions.Below:
                        offset = point.X + (popupWidth / 2);
                        break;

                    case PopupPositions.Left:
                        // try it button grid could be huge so center on the 1/2 grid b/c the button will always be in the middle
                        // of the grid and then +- the button width + the spacer
                        // + (_grid.ActualWidth /2) centers the x point to the center of the button
                        offset = point.X + (_grid.ActualWidth / 2) - popupWidth - (RadiatingButtonHeight / 2) - POPUP_SPACER ;
                        break;

                    case PopupPositions.Right:
                        offset = point.X + (_grid.ActualWidth / 2) + (RadiatingButtonHeight / 2) + POPUP_SPACER;                       
                        break;
                }
            }

            return offset;
        }

        private double GetPopupVerticalOffset()
        {
            double offset = 0d;

            double POPUP_SPACER = StyleHelper.GetApplicationDouble("PopupSpacer");

            if ((null != _grid) && (null != this.PopupChild) && (null != this.PopupChild.Child))
            {
                // get the position of our host button on the window
                var ttv = _grid.TransformToVisual(Window.Current.Content);
                Point point = ttv.TransformPoint(new Point(0, 0));

                // get the height of the child of the popup
                double popupHeight = 0;

                object popupContent = this.PopupChild.Child;

                // not working. popupheight is NaN and then /2 causes exception. 
                //if (popupContent is PopupMedia popup)
                //{
                //    switch (popup.PopupType)
                //    {
                //        case PopupTypes.Text:
                //            popupHeight = popup.Height;
                //            break;
                //        case PopupTypes.Image:
                //            popupHeight = popup.MediaHeight;
                //            break;
                //        case PopupTypes.Video:
                //            popupHeight = popup.MediaHeight;
                //            break;
                //        default:
                //            break;
                //    }
                //}

                // which position?
                switch (this.PopupPosition)
                {
                    case PopupPositions.Above:
                    default:
                        offset = point.Y - popupHeight - POPUP_SPACER;
                        break;

                    case PopupPositions.Below:
                        offset = point.Y + POPUP_SPACER;
                        break;

                    case PopupPositions.Left:
                        offset = point.Y - (popupHeight / 2);
                        break;

                    case PopupPositions.Right:
                        offset = point.Y - (popupHeight / 2);
                        break;
                }
            }

            return offset;
        }

        #endregion
    }
}
