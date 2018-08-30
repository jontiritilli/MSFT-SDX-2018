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
        private const double ENTRANCE_SIZE = 40d;

        private const double RADIATE_SIZE_DEFAULT = 0d;
        private const double RADIATE_SIZE_START = 40d;
        private const double RADIATE_SIZE_END = 68d;
        private const double RADIATE_OPACITY_DEFAULT = 0.0;
        private const double RADIATE_OPACITY_START = 0.6;
        private const double RADIATE_OPACITY_END = 0.4;

        private const double GRID_SIZE = 75d;
        private const double BUTTON_SIZE = 75d;
        private const double X_SIZE = 8d;

        private const double POPUP_MARGIN = 30d;
        private double POPUP_SPACER = POPUP_MARGIN + (RADIATE_SIZE_END - RADIATE_SIZE_START) * 1.1;

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        #region Private Members

        // ui elements to track
        Border _layoutRoot = null;
        Button _hostButton = null;
        Grid _grid = null;
        Ellipse _entranceEllipse = null;
        Ellipse _radiateEllipse = null;
        Image _imageX = null;

        Storyboard _entranceStoryboard = null;
        Storyboard _radiatingStoryboardX = null;
        Storyboard _radiatingStoryboardY = null;
        Storyboard _radiatingStoryboardOpacity = null;

        private PopupPositions _popupPosition;
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
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Dependency Properties

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

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
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

        public PopupPositions PopupPosition
        {
            get => _popupPosition;
            set => _popupPosition = value;
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

        private void DispatcherTimerRadiate_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timerRadiate) { _timerRadiate.Stop(); }

            // call the method that set up the timer
            this.StartRadiateAnimation();
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
        }

        private void DispatcherTimerEntrance_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timerEntrance) { _timerEntrance.Stop(); }

            // call the method that set up the timer
            this.StartEntranceAnimation();
        }

        public void ResetEntranceAnimation()
        {
            if (null != _entranceStoryboard)
            {
                _entranceStoryboard.Stop();
                _entranceEllipse.Opacity = 0d;
                _radiateEllipse.Opacity = 0d;
            }
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
                //// toggle IsOpen
                //this.PopupChild.IsOpen = !this.PopupChild.IsOpen;

                //// if it's open, show the X
                //_imageX.Opacity = (this._popupChild.IsOpen) ? 1.0 : 0.0;

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

                //// add it to the layout
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
                _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_SIZE) });
                _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GRID_SIZE) });

                // add to the button
                _hostButton.Content = _grid;

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
                Grid.SetRow(_radiateEllipse, 0);
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
                Grid.SetRow(_entranceEllipse, 0);
                Grid.SetColumn(_entranceEllipse, 0);

                // add to the canvas
                _grid.Children.Add(_entranceEllipse);

                // create the X image
                _imageX = new Image()
                {
                    Name = this.Name + "ImageX",
                    Source = new BitmapImage(new Uri(URI_X_IMAGE)),
                    Width = X_SIZE,
                    Opacity = 0.0d,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0)
                };
                Grid.SetRow(_imageX, 0);
                Grid.SetColumn(_imageX, 0);

                // add it to the canvas
                _grid.Children.Add(_imageX);

                // create storyboard
                _entranceStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1));
                //_entranceStoryboard = SetupEntranceAnimation(_entranceEllipse, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds);
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

        //private Storyboard SetupRadiateAnimation(Ellipse ellipse, string propertyName, double startValue, double endValue, double duration, double staggerDelay)
        //{
        //    double expandDuration = (duration * 0.6);
        //    double totalDuration = duration + staggerDelay;

        //    // create the storyboard
        //    Storyboard storyboard = new Storyboard()
        //    {
        //        Duration = TimeSpan.FromMilliseconds(totalDuration),
        //        AutoReverse = false,
        //        RepeatBehavior = new RepeatBehavior(1d)
        //    };

        //    // create the key frames holder
        //    DoubleAnimationUsingKeyFrames _frames = new DoubleAnimationUsingKeyFrames
        //    {
        //        Duration = TimeSpan.FromMilliseconds(totalDuration),
        //        EnableDependentAnimation = true,
        //        AutoReverse = true,
        //        RepeatBehavior = new RepeatBehavior(1d)
        //    };

        //    // need cubic easing
        //    CubicEase cubicEaseIn = new CubicEase()
        //    {
        //        EasingMode = EasingMode.EaseIn
        //    };
        //    CubicEase cubicEaseOut = new CubicEase()
        //    {
        //        EasingMode = EasingMode.EaseOut
        //    };

        //    // create frame 0
        //    EasingDoubleKeyFrame _frame0 = new EasingDoubleKeyFrame
        //    {
        //        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0d)),
        //        Value = startValue,
        //        EasingFunction = cubicEaseIn
        //    };

        //    // create delay frame
        //    EasingDoubleKeyFrame _frameStagger = new EasingDoubleKeyFrame
        //    {
        //        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(staggerDelay)),
        //        Value = startValue,
        //        EasingFunction = cubicEaseIn
        //    };

        //    // create frame 1
        //    EasingDoubleKeyFrame _frame1 = new EasingDoubleKeyFrame
        //    {
        //        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(expandDuration + staggerDelay)),
        //        Value = endValue,
        //        EasingFunction = cubicEaseIn
        //    };

        //    // create frame 2
        //    EasingDoubleKeyFrame _frame2 = new EasingDoubleKeyFrame
        //    {
        //        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration)),
        //        Value = startValue,
        //        EasingFunction = cubicEaseOut
        //    };

        //    // add frames to the collection
        //    _frames.KeyFrames.Add(_frame0);
        //    if (staggerDelay > 0)
        //    {
        //        _frames.KeyFrames.Add(_frameStagger);
        //    }
        //    _frames.KeyFrames.Add(_frame1);
        //    _frames.KeyFrames.Add(_frame2);

        //    // add frame collection to the storyboard
        //    storyboard.Children.Add(_frames);

        //    // set the target of the storyboard
        //    Storyboard.SetTarget(storyboard, ellipse);
        //    Storyboard.SetTargetProperty(storyboard, propertyName);
        //    //storyboard.SetValue(Storyboard.TargetPropertyProperty, propertyName);

        //    return storyboard;
        //}


        #endregion

        #region Code Helpers
        public static Popup CreatePopup(PopupTypes type, string text, double leftOffset, double topOffset, double width)
        {
            // create the popup
            Popup popup = new Popup()
            {
                IsOpen = false,
                IsLightDismissEnabled = true,
                HorizontalOffset = leftOffset,
                VerticalOffset = topOffset
            };
            PopupMedia popupMedia = new PopupMedia()
            {
                PopupType = type,
                Text = text,
                AutoStart = true
            };
            if (!Double.IsInfinity(width) && !double.IsNaN(width))
            {
                popupMedia.Width = width;
            }
            popup.Child = popupMedia;
            //switch (type)
            //{
            //    case PopupTypes.Text:
            //        PopupMedia popupMedia = new PopupMedia()
            //        {
            //            PopupType = type,
            //            Text = text,
            //            AutoStart = true
            //        };
            //        if (!Double.IsInfinity(width) && !double.IsNaN(width))
            //        {
            //            popupMedia.Width = width;
            //        }
            //        popup.Child = popupMedia;
            //        break;

            //    case PopupTypes.Image:
            //        break;

            //    //case PopupTypes.ImageGallery:
            //    //    // using ImageGallery and not creating it here
            //    //    //popup.Child = new PopupContentImageGallery()
            //    //    //{
            //    //    //    Width = Window.Current.Bounds.Width,
            //    //    //    Height = Window.Current.Bounds.Height
            //    //    //};
            //    //    break;

            //    //case PopupTypes.FastCharge:
            //    //    popup.Child = new PopupContentFastCharge()
            //    //    {
            //    //        AutoStart = true
            //    //    };
            //    //    break;

            //    //case PopupTypes.CompareSKUs:
            //    //    popup.Child = new PopupContentCompareGallery();
            //    //    break;

            //    default:
            //        break;
            //}

            return popup;
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
