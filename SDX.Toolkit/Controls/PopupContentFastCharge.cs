using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using System.Threading.Tasks;
using Windows.UI;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class PopupContentFastCharge : Control
    {
        #region Constants

        private readonly string URI_IMAGE_BATTERY = @"ms-appx:///Assets/FastCharge/battery-outline.png";
        private readonly string URI_IMAGE_METER = @"ms-appx:///Assets/FastCharge/meter-line.png";
        private readonly string URI_IMAGE_CHARGE = @"ms-appx:///Assets/FastCharge/blue-charge.png";
        private readonly string URI_IMAGE_POSITION = @"ms-appx:///Assets/FastCharge/blue-line.png";

        private static readonly double CANVAS_X = 410d;
        private static readonly double CANVAS_Y = 280d;

        private static readonly double LEFT_HEADER = 20d;
        private static readonly double TOP_HEADER = 20d;

        private static readonly double TOP_BATTERY = TOP_HEADER;
        private static readonly double LEFT_BATTERY = LEFT_HEADER;
        private static readonly double WIDTH_BATTERY = 350d;
        private static readonly double HEIGHT_BATTERY = 134d;

        private static readonly double MARGIN_CHARGE_LEFT = 20d;
        private static readonly double MARGIN_CHARGE_RIGHT = 35d;
        private static readonly double MARGIN_CHARGE_TOP = MARGIN_CHARGE_LEFT;
        private static readonly double TOP_CHARGE = TOP_BATTERY + MARGIN_CHARGE_TOP;
        private static readonly double LEFT_CHARGE = LEFT_BATTERY + MARGIN_CHARGE_LEFT;
        private static readonly double WIDTH_CHARGE_START = 5d;
        private static readonly double WIDTH_CHARGE_END = (WIDTH_BATTERY - MARGIN_CHARGE_LEFT - MARGIN_CHARGE_RIGHT) * 0.8d;
        private static readonly double HEIGHT_CHARGE = HEIGHT_BATTERY - (2 * MARGIN_CHARGE_TOP);

        private static readonly double LEFT_PERCENT = LEFT_CHARGE + 50d;
        private static readonly double TOP_PERCENT = TOP_CHARGE + 0d;

        private static readonly double TOP_METER = TOP_BATTERY + HEIGHT_BATTERY + 30d;
        private static readonly double LEFT_METER = LEFT_CHARGE;
        private static readonly double RIGHT_METER = LEFT_METER + WIDTH_CHARGE_END;
        private static readonly double WIDTH_METER = 300d;
        private static readonly double HEIGHT_METER = 30d;

        private static readonly double TOP_POSITION = TOP_METER - 5d;
        private static readonly double WIDTH_POSITION = 3d;

        private static readonly double TOP_TEXT = TOP_METER + HEIGHT_METER + 10d;
        private static readonly double LEFT_TEXT0 = LEFT_METER;
        private static readonly double LEFT_TEXT15 = LEFT_METER + (WIDTH_METER * 0.7);

        private readonly int Z_ORDER_CONTROLS = 100;
        private readonly int Z_ORDER_BATTERY = 10;
        private readonly int Z_ORDER_METER = 10;
        private readonly int Z_ORDER_CHARGE = 20;
        private readonly int Z_ORDER_POSITION = 20;
        private readonly int Z_ORDER_BACKGROUND = 0;

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private Canvas _canvas = null;
        private FadeInHeader _fadeInHeader = null;
        private AnimatablePercent _percent = null;
        private TextBlock _0Min = null;
        private TextBlock _15Min = null;
        private TextBlock _legal = null;
        private Image _imageBattery = null;
        private Image _imageMeter = null;
        private Image _imageCharge = null;
        private Image _imagePosition = null;

        private BitmapImage _battery = null;
        private BitmapImage _meter = null;
        private BitmapImage _charge = null;
        private BitmapImage _position = null;

        private Storyboard _chargeStoryboard = null;
        private Storyboard _positionStoryboard = null;

        #endregion


        #region Construction

        public PopupContentFastCharge()
        {
            this.DefaultStyleKey = typeof(PopupContentFastCharge);

            Initialization = PreLoadImages();

            this.Loaded += OnLoaded;
        }

        private async Task PreLoadImages()
        {
            _battery = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_IMAGE_BATTERY), 400);
            _meter = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_IMAGE_METER), 380);
            _charge = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_IMAGE_CHARGE), 400);
            _position = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_IMAGE_POSITION), 2);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Static Methods

        #endregion

        #region Public Properties

        // used for async image loading
        public Task Initialization { get; private set; }

        #endregion

        #region Dependency Properties

        // Headline
        public static readonly DependencyProperty HeadlineProperty =
            DependencyProperty.Register("Headline", typeof(string), typeof(PopupContentFastCharge), new PropertyMetadata(null, OnPropertyChanged));

        public string Headline
        {
            get { return (string)GetValue(HeadlineProperty); }
            set { SetValue(HeadlineProperty, value); }
        }

        // Lede
        public static readonly DependencyProperty LedeProperty =
            DependencyProperty.Register("Lede", typeof(string), typeof(PopupContentFastCharge), new PropertyMetadata(null, OnPropertyChanged));

        public string Lede
        {
            get { return (string)GetValue(LedeProperty); }
            set { SetValue(LedeProperty, value); }
        }

        // Legal
        public static readonly DependencyProperty LegalProperty =
            DependencyProperty.Register("Legal", typeof(string), typeof(PopupContentFastCharge), new PropertyMetadata(null, OnPropertyChanged));

        public string Legal
        {
            get { return (string)GetValue(LegalProperty); }
            set { SetValue(LegalProperty, value); }
        }

        // TimeStart
        public static readonly DependencyProperty TimeStartProperty =
            DependencyProperty.Register("TimeStart", typeof(string), typeof(PopupContentFastCharge), new PropertyMetadata(null, OnPropertyChanged));

        public string TimeStart
        {
            get { return (string)GetValue(TimeStartProperty); }
            set { SetValue(TimeStartProperty, value); }
        }

        // TimeStop
        public static readonly DependencyProperty TimeStopProperty =
            DependencyProperty.Register("TimeStop", typeof(string), typeof(PopupContentFastCharge), new PropertyMetadata(null, OnPropertyChanged));

        public string TimeStop
        {
            get { return (string)GetValue(TimeStopProperty); }
            set { SetValue(TimeStopProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(PopupContentFastCharge), new PropertyMetadata(2000d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set
            {
                // correct out of range values
                value = (value < 0) ? 0 : value;

                // save it
                SetValue(DurationInMillisecondsProperty, value);
            }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(PopupContentFastCharge), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set
            {
                // correct out of range values
                value = (value < 0) ? 0 : value;

                // save it
                SetValue(StaggerDelayInMillisecondsProperty, value);
            }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(PopupContentFastCharge), new PropertyMetadata(false, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Public Methods

        public void StartAnimation()
        {
            if (null != _fadeInHeader)
            {
                _fadeInHeader.StartFadeIn();
            }

            if (null != _chargeStoryboard)
            {
                _chargeStoryboard.Begin();
            }

            if (null != _positionStoryboard)
            {
                _positionStoryboard.Begin();
            }

            if (null != _percent)
            {
                _percent.StartAnimation();
            }
        }

        public void ResetAnimation()
        {
            if (null != _fadeInHeader)
            {
                _fadeInHeader.ResetAnimation();
            }

            if (null != _chargeStoryboard)
            {
                _chargeStoryboard.Stop();
            }

            if (null != _positionStoryboard)
            {
                _positionStoryboard.Stop();
            }

            if (null != _percent)
            {
                _percent.ResetAnimation();
            }
        }

        #endregion

        #region Custom Events


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

        private static void OnDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a border here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot)
            {
                return;
            }

            // set the background of the grid (this won't work due to a bug/issue; see below where we add a rectangle)
            _layoutRoot.Background = new SolidColorBrush(Colors.LightGray) { Opacity = StyleHelper.PopupBackgroundOpacity };
            _layoutRoot.BorderBrush = new SolidColorBrush(Colors.White);
            _layoutRoot.BorderThickness = new Thickness(0, 3, 0, 0);
            //_layoutRoot.Margin = new Thickness(CANVAS_X, CANVAS_X, CANVAS_X, CANVAS_X);
            //_layoutRoot.Padding = new Thickness(20, 20, 20, 20);

            // create the header
            _fadeInHeader = new FadeInHeader()
            {
                Name = "FastCharge",
                HeaderStyle = FadeInHeaderStyles.FastChargePopup,
                Width = CANVAS_X,
                DurationInMilliseconds = 400d,
                StaggerDelayInMilliseconds = 0d,
                AutoStart = false
            };
            Grid.SetRow(_fadeInHeader, 1);
            Grid.SetColumn(_fadeInHeader, 1);
            _layoutRoot.Children.Add(_fadeInHeader);

            // set headline binding
            _fadeInHeader.SetBinding(FadeInHeader.HeadlineProperty,
                new Binding() { Source = this, Path = new PropertyPath("Headline"), Mode = BindingMode.OneWay });

            // set lede binding
            _fadeInHeader.SetBinding(FadeInHeader.LedeProperty,
                new Binding() { Source = this, Path = new PropertyPath("Lede"), Mode = BindingMode.OneWay });


            // create the legal notice
            _legal = new TextBlock()
            {
                Name = "Legal",
                //Text = this.Legal,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.WrapWholeWords,
                Width = CANVAS_X
            };
            StyleHelper.SetFontCharacteristics(_legal, ControlStyles.Footnote);
            Grid.SetRow(_legal, 4);
            Grid.SetColumn(_legal, 1);
            _layoutRoot.Children.Add(_legal);

            // set legal binding
            _legal.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("Legal"), Mode = BindingMode.OneWay });

            // create the canvas
            _canvas = new Canvas()
            {
                Width = CANVAS_X,
                Height = CANVAS_Y
            };
            Grid.SetRow(_canvas, 2);
            Grid.SetColumn(_canvas, 1);
            _layoutRoot.Children.Add(_canvas);

            // create the battery
            _imageBattery = new Image()
            {
                Source = _battery,
                Width = WIDTH_BATTERY
            };
            Canvas.SetLeft(_imageBattery, LEFT_HEADER);
            Canvas.SetTop(_imageBattery, TOP_BATTERY);
            Canvas.SetZIndex(_imageBattery, Z_ORDER_BATTERY);
            _canvas.Children.Add(_imageBattery);

            // create the charge
            _imageCharge = new Image()
            {
                Source = _charge,
                Stretch = Stretch.Fill,
                Width = WIDTH_CHARGE_START,
                Height = HEIGHT_CHARGE
            };
            Canvas.SetLeft(_imageCharge, LEFT_CHARGE);
            Canvas.SetTop(_imageCharge, TOP_CHARGE);
            Canvas.SetZIndex(_imageCharge, Z_ORDER_CHARGE);
            _canvas.Children.Add(_imageCharge);

            // create the percent overlay
            _percent = new AnimatablePercent()
            {
                PercentValue = 0.0
            };
            Canvas.SetLeft(_percent, LEFT_PERCENT);
            Canvas.SetTop(_percent, TOP_PERCENT);
            Canvas.SetZIndex(_percent, Z_ORDER_CONTROLS);
            _canvas.Children.Add(_percent);

            // create the meter
            _imageMeter = new Image()
            {
                Source = _meter,
                Width = WIDTH_METER
            };
            Canvas.SetLeft(_imageMeter, LEFT_METER);
            Canvas.SetTop(_imageMeter, TOP_METER);
            Canvas.SetZIndex(_imageMeter, Z_ORDER_METER);
            _canvas.Children.Add(_imageMeter);

            // create the meter position line
            _imagePosition = new Image()
            {
                Source = _position,
                Width = WIDTH_POSITION
            };
            Canvas.SetLeft(_imagePosition, LEFT_CHARGE + WIDTH_CHARGE_START);
            Canvas.SetTop(_imagePosition, TOP_POSITION);
            Canvas.SetZIndex(_imagePosition, Z_ORDER_POSITION);
            _canvas.Children.Add(_imagePosition);

            // Create the translatetransform and attach to the image
            TranslateTransform _imageTranslate = new TranslateTransform
            {
                X = 0,
                Y = 0
            };
            _imagePosition.RenderTransform = _imageTranslate;

            // create 0 Min
            _0Min = new TextBlock()
            {
                Text = this.TimeStart
            };
            StyleHelper.SetFontCharacteristics(_0Min, ControlStyles.FastChargeTimeline);
            Canvas.SetLeft(_0Min, LEFT_TEXT0);
            Canvas.SetTop(_0Min, TOP_TEXT);
            Canvas.SetZIndex(_0Min, Z_ORDER_CONTROLS);
            _canvas.Children.Add(_0Min);

            // set 0 Min binding
            _0Min.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("TimeStart"), Mode = BindingMode.OneWay });

            // create 15 Min
            _15Min = new TextBlock()
            {
                Text = this.TimeStop,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            StyleHelper.SetFontCharacteristics(_15Min, ControlStyles.FastChargeTimeline);
            Canvas.SetLeft(_15Min, LEFT_TEXT15);
            Canvas.SetTop(_15Min, TOP_TEXT);
            Canvas.SetZIndex(_15Min, Z_ORDER_CONTROLS);
            _canvas.Children.Add(_15Min);

            // set 15 Min binding
            _15Min.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("TimeStop"), Mode = BindingMode.OneWay });

            // create the charge animation
            _chargeStoryboard = SetupChargeAnimation(_imageCharge, WIDTH_CHARGE_START, WIDTH_CHARGE_END, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds);

            // create the position animation
            _positionStoryboard = SetupPositionAnimation(_imagePosition, 0d, WIDTH_CHARGE_END - WIDTH_CHARGE_START, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds);
        }

        private Storyboard SetupChargeAnimation(Image image, double startingWidth, double finalWidth, double duration, double staggerDelay)
        {
            double totalDuration = duration + staggerDelay;

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
                Value = startingWidth,
                EasingFunction = cubicEaseIn
            };

            // create delay frame
            EasingDoubleKeyFrame _frameStagger = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(staggerDelay)),
                Value = startingWidth,
                EasingFunction = cubicEaseIn
            };

            // create frame 1
            EasingDoubleKeyFrame _frame1 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration)),
                Value = finalWidth,
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
            Storyboard.SetTarget(storyboard, image);
            Storyboard.SetTargetProperty(storyboard, "Width");
            //storyboard.SetValue(Storyboard.TargetPropertyProperty, "Width");

            return storyboard;
        }

        private Storyboard SetupPositionAnimation(Image image, double start, double finish, double duration, double staggerDelay)
        {
            double totalDuration = duration + staggerDelay;

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
                Value = start,
                EasingFunction = cubicEaseIn
            };

            // create delay frame
            EasingDoubleKeyFrame _frameStagger = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(staggerDelay)),
                Value = start,
                EasingFunction = cubicEaseIn
            };

            // create frame 1
            EasingDoubleKeyFrame _frame1 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration)),
                Value = finish,
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
            Storyboard.SetTarget(storyboard, image);
            Storyboard.SetTargetProperty(storyboard, "(Image.RenderTransform).(TranslateTransform.X)");
            //storyboard.SetValue(Storyboard.TargetPropertyProperty, "(Image.RenderTransform).(TranslateTransform.X)");

            return storyboard;
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion

    }
}
