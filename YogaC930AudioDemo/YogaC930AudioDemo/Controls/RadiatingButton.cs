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

using System.Globalization;

using YogaC930AudioDemo.Helpers;


namespace YogaC930AudioDemo.Controls
{
    public sealed class RadiatingButton : Control
    {
        #region Constants
        
        private const double RADIATE_SIZE_DEFAULT = 0d;
        private const double RADIATE_OPACITY_DEFAULT = 0.0;
        private const double RADIATE_OPACITY_START = 0.8;
        private const double RADIATE_OPACITY_END = 0.0;
        private const double RADIATE_STROKE_DEFAULT = 8.0;
        private const double RADIATE_STROKE_START = 12.0;
        private const double RADIATE_STROKE_END = 0.0;
        private const double TRY_IT_DELAY = 1500;
        
        #endregion

        #region Private Members

        // ui elements to track
        Border _layoutRoot = null;
        Grid _grid = null;
        Ellipse _entranceEllipse = null;
        Ellipse _radiateEllipse = null;
        Ellipse _backgroundEllipse = null;

        Storyboard _entranceStoryboard = null;
        Storyboard _backgroundStoryboard = null;
        Storyboard _radiatingStoryboard = null;
        Storyboard _radiatingStoryboardX = null;
        Storyboard _radiatingStoryboardY = null;
        Storyboard _radiatingStoryboardOpacity = null;

        private Popup _popupChild = null;

        private DispatcherTimer _timerRadiate = null;
        private int _dispatchCountRadiate = 0;
        private DispatcherTimer _timerEntrance = null;
        private int _dispatchCountEntrance = 0;

        #endregion

        #region Construction/Destruction

        public RadiatingButton()
        {
            this.DefaultStyleKey = typeof(RadiatingButton);

            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Dependency Properties

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true));

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }

        // AnimationEnabled
        public static readonly DependencyProperty AnimationEnabledProperty =
            DependencyProperty.Register("AnimationEnabled", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true));

        public bool AnimationEnabled
        {
            get => (bool)GetValue(AnimationEnabledProperty);
            set => SetValue(AnimationEnabledProperty, value);
        }

        // AnimationOrder
        public static readonly DependencyProperty AnimationOrderProperty =
            DependencyProperty.Register("AnimationOrder", typeof(int), typeof(RadiatingButton), new PropertyMetadata(0));

        public int AnimationOrder
        {
            get => (int)GetValue(AnimationOrderProperty);
            set => SetValue(AnimationOrderProperty, (value < 0) ? 0 : value);
        }

        // AnimationRepeat
        public static readonly DependencyProperty AnimationRepeatProperty =
            DependencyProperty.Register("AnimationRepeat", typeof(RepeatBehavior), typeof(RadiatingButton), new PropertyMetadata(new RepeatBehavior(new TimeSpan(0, 0, 1))));

        public RepeatBehavior AnimationRepeat
        {
            get => (RepeatBehavior)GetValue(AnimationRepeatProperty);
            set => SetValue(AnimationRepeatProperty, value);
        }

        // EntranceDurationInMilliseconds
        public static readonly DependencyProperty EntranceDurationInMillisecondsProperty =
            DependencyProperty.Register("EntranceDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(750d));

        public double EntranceDurationInMilliseconds
        {
            get { return (double)GetValue(EntranceDurationInMillisecondsProperty); }
            set { SetValue(EntranceDurationInMillisecondsProperty, value); }
        }

        // EntranceStaggerDelayInMilliseconds
        public static readonly DependencyProperty EntranceStaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("EntranceStaggerDelayInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(0d));

        public double EntranceStaggerDelayInMilliseconds
        {
            get { return (double)GetValue(EntranceStaggerDelayInMillisecondsProperty); }
            set { SetValue(EntranceStaggerDelayInMillisecondsProperty, value); }
        }

        // RadiateDurationInMilliseconds
        public static readonly DependencyProperty RadiateDurationInMillisecondsProperty =
            DependencyProperty.Register("RadiateDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(1450d));

        public double RadiateDurationInMilliseconds
        {
            get { return (double)GetValue(RadiateDurationInMillisecondsProperty); }
            set { SetValue(RadiateDurationInMillisecondsProperty, value); }
        }

        // RadiateStaggerDelayInMilliseconds
        public static readonly DependencyProperty RadiateStaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("RadiateStaggerDelayInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(1450d));

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
                    // catch the image gallery Closed event
                    object contentChild = _popupChild.Child;
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

        public void FadeOutButton()
        {
            if(null != _grid)
            {
                _grid.Opacity = 0.0d;
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
                        //if (null != _radiateEllipse)
                        //{
                        //    _radiateEllipse.Opacity = RADIATE_OPACITY_END;
                        //}

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
                //// set opacity
                //if (null != _radiateEllipse)
                //{
                //    _radiateEllipse.Opacity = RADIATE_OPACITY_END;
                //}

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

            //// reset opacity
            //if (null != _radiateEllipse)
            //{
            //    _radiateEllipse.Opacity = RADIATE_OPACITY_END;
            //}
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
            if (null != _backgroundStoryboard)
            {
                _backgroundStoryboard.Begin();
            }
            if (null != _radiatingStoryboard)
            {
                _radiatingStoryboard.Begin();
            }
        }

        public void ResetEntranceAnimation()
        {
            if (null != _entranceStoryboard)
            {
                _entranceStoryboard.Stop();
                _entranceEllipse.Opacity = 0d;
            }
            if (null != _backgroundStoryboard)
            {
                _backgroundStoryboard.Stop();
                _backgroundEllipse.Opacity = 0d;
            }
            if (null != _radiatingStoryboard)
            {
                _radiatingStoryboard.Stop();
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ///TODO Determine dual startup strategy
            if (this.AutoStart)
            {
                this.StartEntranceAnimation();
                this.StartRadiateAnimation();
            }
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            RaiseClickedEvent(this, new EventArgs());
            HandleClick();
        }

        public void HandleClick()
        {
             if (null != this.PopupChild)
            {
                if (this.PopupChild.IsOpen)
                {
                    this.PopupChild.IsOpen = false;
                }
                else
                {
                    this.PopupChild.IsOpen = true;

                    AnimationHelper.PerformPageEntranceAnimation((Page)this.PopupChild.Child);

                    // telemetry
                    //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                }
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
                double TargetBorderHeight = 59d;

                double RadiatingButtonHeight = 24d;

                double radiateEllipseHeight = RadiatingButtonHeight * .9; // set this slightly smaller so it doesn't peek out from behind the button

                // height of the radiating button with a little added space for the radiating animation
                double RadiatingButtonRowHeight = TargetBorderHeight;
                    
                // calculate beginning and end of animation
                double RADIATE_SIZE_START = radiateEllipseHeight;
                double RADIATE_SIZE_END = TargetBorderHeight;

                _grid = new Grid();

                _grid.PointerReleased += Grid_PointerReleased;
                _grid.Width = TargetBorderHeight;

                // add to the button
                _layoutRoot.Child = _grid;

                // only one column
                _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TargetBorderHeight) });

                // create the background
                _backgroundEllipse = new Ellipse()
                {
                    Name = this.Name + "backgroundEllipse",
                    Width = TargetBorderHeight,
                    Height = TargetBorderHeight,
                    Fill = new SolidColorBrush(Colors.White),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 2,
                    Opacity = 0.7d,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_backgroundEllipse, 2);
                Grid.SetColumn(_backgroundEllipse, 0);

                // add it to the canvas
                _grid.Children.Add(_backgroundEllipse);

                // create the radiating ellipse
                _radiateEllipse = new Ellipse()
                {
                    Name = this.Name + "radiateEllipse",
                    Width = RADIATE_SIZE_START,
                    Height = RADIATE_SIZE_START,
                    Stroke = GetSolidColorBrush("#FF3E8DDD"),
                    StrokeThickness = RADIATE_STROKE_START,
                    Opacity = 0d,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_radiateEllipse, 2);
                Grid.SetColumn(_radiateEllipse, 0);

                // add it to the canvas
                _grid.Children.Add(_radiateEllipse);

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

                // create storyboard
                _entranceStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                _backgroundStoryboard = AnimationHelper.CreateEasingAnimation(_backgroundEllipse, "Opacity", 0.0, 0.0, 0.7, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                _radiatingStoryboard = AnimationHelper.CreateEasingAnimation(_radiateEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                _radiatingStoryboardX = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Width", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, true, new RepeatBehavior(1d));

                _radiatingStoryboardY = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Height", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, true, new RepeatBehavior(1d));

                //_radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Opacity", RADIATE_OPACITY_START, RADIATE_OPACITY_START, RADIATE_OPACITY_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, false, true, new RepeatBehavior(1d));

                _radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "StrokeThickness", RADIATE_STROKE_DEFAULT, RADIATE_STROKE_START, RADIATE_STROKE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, false, true, new RepeatBehavior(1d));
            }
        }

        #endregion

    }
}
