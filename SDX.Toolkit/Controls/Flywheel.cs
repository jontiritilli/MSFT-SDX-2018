using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
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

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum ProgressColors
    {
        Burgundy,
        Cobalt,
        White,
        Black,
        Custom
    }


    public sealed class Flywheel : Control
    {
        #region Private Constants

        private const double CANVAS_X = 1200d;
        private const double CANVAS_Y = 800d;
        private const double WIDTH_RING = 200d;
        private const double WIDTH_PATH = 20d;


        private const int Z_ORDER_RING_BACKGROUND = 10;
        private const int Z_ORDER_RING_PROGRESS = 20;
        private const int Z_ORDER_TEXT = 100;

        private const double ANGLE_START = -160d;
        private const double ANGLE_END = 180d;
        private const double ANGLE_START_BACKGROUND = -179d;
        private const double ANGLE_END_BACKGROUND = 180d;
        private const double ANGLE_STEP1 = -70d;
        private const double ANGLE_STEP2 = -30d;
        private const double ANGLE_STEP3 = 30d;
        private const double ANGLE_STEP4 = 85d;
        private const double ANGLE_STEP5 = 135d;

        private const double HEIGHT_SPACER_VERTICAL = 20d;
        private const double WIDTH_SPACER_HORIZONTAL = 20d;
        private const double WIDTH_GAP_TEXT = 20d;
        private const double WIDTH_GAP_TEXT_LARGE = 40d;
        private const double WIDTH_BULLET_SPACER = 10d;

        private const string CHAR_BULLET = "•";
        private const string CHAR_NBSPACE = " ";    // MUST BE the non-breaking space

        private const double DURATION_FADE = 200;
        private const double DURATION_ANGLESEGMENT = 800;
        private const double DURATION_ANGLEPAUSE = 0;
        private const double DURATION_ANGLEEXIT = 400;

        #endregion


        #region Private Members

        private static LinearGradientBrush _lgbBurgundy = CreateLinearGradientBrush("FF957F80", "FF6A3337");
        private static LinearGradientBrush _lgbCobalt = CreateLinearGradientBrush("FF707F89", "FF385465");
        private static LinearGradientBrush _lgbWhite = CreateLinearGradientBrush("FFB6B6B6", "FFFFFFFF");
        private static LinearGradientBrush _lgbBlack = CreateLinearGradientBrush("FF6F6F6F", "FF2B2B2B");

        private Grid _layoutRoot = null;
        private Canvas _canvas = null;

        private RingSlice _ringBackground = null;
        private RingSlice _ringProgress = null;

        private Grid _gridBullet1 = null;
        private Grid _gridBullet2 = null;
        private Grid _gridBullet3 = null;
        private Grid _gridBullet4 = null;
        private Grid _gridBullet5 = null;

        private TextBlock _bullet1 = null;
        private TextBlock _bullet2 = null;
        private TextBlock _bullet3 = null;
        private TextBlock _bullet4 = null;
        private TextBlock _bullet5 = null;

        private List<Storyboard> _storyboardRingSliceEntranceList = new List<Storyboard>();
        private List<Storyboard> _storyboardTextBlockEntranceList = new List<Storyboard>();

        private List<Storyboard> _storyboardRingSliceExitList = new List<Storyboard>();
        private List<Storyboard> _storyboardTextBlockExitList = new List<Storyboard>();

        private bool entranceAnimationHasRun = false;
        private double _ringRadius = WIDTH_RING;

        #endregion


        #region Construction

        public Flywheel()
        {
            this.DefaultStyleKey = typeof(Flywheel);

            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateUI();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // set up the animations
            this.RenderUI();
        }

        #endregion

        #region Public Static Methods

        #endregion

        #region Dependency Properties

        // ProgressColor
        public static readonly DependencyProperty ProgressColorProperty =
            DependencyProperty.Register("ProgressColor", typeof(ProgressColors), typeof(Flywheel), new PropertyMetadata(null, OnProgressColorChanged));

        public ProgressColors ProgressColor
        {
            get { return (ProgressColors)GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }

        // ProgressBrush
        public static readonly DependencyProperty ProgressBrushProperty =
            DependencyProperty.Register("ProgressBrush", typeof(LinearGradientBrush), typeof(Flywheel), new PropertyMetadata(null, OnFlywheelPropertyChanged));

        public LinearGradientBrush ProgressBrush
        {
            get { return (LinearGradientBrush)GetValue(ProgressBrushProperty); }
            set { SetValue(ProgressBrushProperty, value); }
        }

        // BackgroundBrush
        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register("BackgroundBrush", typeof(SolidColorBrush), typeof(Flywheel), new PropertyMetadata(new SolidColorBrush() { Color = Colors.Black, Opacity = 0.1 }, OnFlywheelPropertyChanged));

        public SolidColorBrush BackgroundBrush
        {
            get { return (SolidColorBrush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }

        // MaxRingRadius
        public static readonly DependencyProperty MaxRingRadiusProperty =
            DependencyProperty.Register("MaxRingRadius", typeof(double), typeof(Flywheel), new PropertyMetadata(Double.PositiveInfinity, OnFlywheelPropertyChanged));

        public double MaxRingRadius
        {
            get { return (double)GetValue(MaxRingRadiusProperty); }
            set { SetValue(MaxRingRadiusProperty, value); }
        }

        // TextWidth
        public static readonly DependencyProperty TextWidthProperty =
            DependencyProperty.Register("TextWidth", typeof(double), typeof(Flywheel), new PropertyMetadata(150d, OnFlywheelPropertyChanged));

        public double TextWidth
        {
            get { return (double)GetValue(TextWidthProperty); }
            set { SetValue(TextWidthProperty, value); }
        }

        // FirstBulletText
        public static readonly DependencyProperty FirstBulletTextProperty =
            DependencyProperty.Register("FirstBulletText", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnFirstBulletTextChanged));

        public string FirstBulletText
        {
            get { return (string)GetValue(FirstBulletTextProperty); }
            set { SetValue(FirstBulletTextProperty, value); }
        }

        // SecondBulletText
        public static readonly DependencyProperty SecondBulletTextProperty =
            DependencyProperty.Register("SecondBulletText", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnSecondBulletTextChanged));

        public string SecondBulletText
        {
            get { return (string)GetValue(SecondBulletTextProperty); }
            set { SetValue(SecondBulletTextProperty, value); }
        }

        // ThirdBulletText
        public static readonly DependencyProperty ThirdBulletTextProperty =
            DependencyProperty.Register("ThirdBulletText", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnThirdBulletTextChanged));

        public string ThirdBulletText
        {
            get { return (string)GetValue(ThirdBulletTextProperty); }
            set { SetValue(ThirdBulletTextProperty, value); }
        }

        // FourthBulletText
        public static readonly DependencyProperty FourthBulletTextProperty =
            DependencyProperty.Register("FourthBulletText", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnFourthBulletTextChanged));

        public string FourthBulletText
        {
            get { return (string)GetValue(FourthBulletTextProperty); }
            set { SetValue(FourthBulletTextProperty, value); }
        }

        // FifthBulletText
        public static readonly DependencyProperty FifthBulletTextProperty =
            DependencyProperty.Register("FifthBulletText", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnFifthBulletTextChanged));

        public string FifthBulletText
        {
            get { return (string)GetValue(FifthBulletTextProperty); }
            set { SetValue(FifthBulletTextProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(Flywheel), new PropertyMetadata(100d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(Flywheel), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(Flywheel), new PropertyMetadata(false, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Public Properties



        #endregion

        #region Public Methods

        public void StartEntranceAnimation()
        {
            if (null != _storyboardRingSliceEntranceList)
            {
                foreach (Storyboard storyboard in _storyboardRingSliceEntranceList)
                {
                    storyboard.Begin();
                }
                // we've run an entrance animation
                entranceAnimationHasRun = true;
            }

            if (null != _storyboardTextBlockEntranceList)
            {
                foreach (Storyboard storyboard in _storyboardTextBlockEntranceList)
                {
                    storyboard.Begin();
                }

                // we've run an entrance animation
                entranceAnimationHasRun = true;
            }

        }

        public void StartExitAnimation()
        {
            // if the entrance animation has never run, then
            // don't run the exit animation yet
            if (!entranceAnimationHasRun)
            {
                return;
            }

            if (null != _storyboardRingSliceExitList)
            {
                foreach (Storyboard storyboard in _storyboardRingSliceExitList)
                {
                    storyboard.Begin();
                }
            }

            if (null != _storyboardTextBlockExitList)
            {
                foreach (Storyboard storyboard in _storyboardTextBlockExitList)
                {
                    storyboard.Begin();
                }
            }
        }

        public void ResetEntranceAnimation()
        {
            if (null != _storyboardRingSliceEntranceList)
            {
                foreach (Storyboard storyboard in _storyboardRingSliceEntranceList)
                {
                    storyboard.Stop();
                }
            }

            if (null != _storyboardTextBlockEntranceList)
            {
                foreach (Storyboard storyboard in _storyboardTextBlockEntranceList)
                {
                    storyboard.Stop();
                }
            }
        }

        public void ResetExitAnimation()
        {
            if (null != _storyboardRingSliceExitList)
            {
                foreach (Storyboard storyboard in _storyboardRingSliceExitList)
                {
                    storyboard.Stop();
                }
            }

            if (null != _storyboardTextBlockExitList)
            {
                foreach (Storyboard storyboard in _storyboardTextBlockExitList)
                {
                    storyboard.Stop();
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Custom Events

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartEntranceAnimation();
            }
        }

        private static void OnProgressColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                ProgressColors newColor = (ProgressColors)e.NewValue;

                switch (newColor)
                {
                    case ProgressColors.Burgundy:
                    default:
                        flywheel.ProgressBrush = _lgbBurgundy;
                        break;

                    case ProgressColors.Cobalt:
                        flywheel.ProgressBrush = _lgbCobalt;
                        break;

                    case ProgressColors.White:
                        flywheel.ProgressBrush = _lgbWhite;
                        break;

                    case ProgressColors.Black:
                        flywheel.ProgressBrush = _lgbBlack;
                        break;

                    case ProgressColors.Custom:
                        break;
                }

            }
        }

        private static void OnFlywheelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                flywheel.UpdateUI();
            }
        }

        private static void OnFirstBulletTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                if (null != flywheel)
                {
                    if (null != flywheel._bullet1)
                    {
                        flywheel._bullet1.Text = (string)e.NewValue;
                    }
                }
            }
        }

        private static void OnSecondBulletTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                if (null != flywheel)
                {
                    if (null != flywheel._bullet2)
                    {
                        flywheel._bullet2.Text = (string)e.NewValue;
                    }
                }
            }
        }

        private static void OnThirdBulletTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                if (null != flywheel)
                {
                    if (null != flywheel._bullet3)
                    {
                        flywheel._bullet3.Text = (string)e.NewValue;
                    }
                }
            }
        }

        private static void OnFourthBulletTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                if (null != flywheel)
                {
                    if (null != flywheel._bullet4)
                    {
                        flywheel._bullet4.Text = (string)e.NewValue;
                    }
                }
            }
        }

        private static void OnFifthBulletTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flywheel flywheel)
            {
                if (null != flywheel)
                {
                    if (null != flywheel._bullet5)
                    {
                        flywheel._bullet5.Text = (string)e.NewValue;
                    }
                }
            }
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

        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize;

            // get the ring radius
            _ringRadius = CalculateRingRadius(availableSize.Width, availableSize.Height);

            // calculate our width based on ring radius
            double width = 2 * (_ringRadius + WIDTH_GAP_TEXT + this.TextWidth + WIDTH_BULLET_SPACER + WIDTH_SPACER_HORIZONTAL);

            // calculate our height based on ring radius
            double height = 2 * (_ringRadius + HEIGHT_SPACER_VERTICAL);

            // return our preferred/adjusted size
            desiredSize.Width = Math.Max(0, width);
            desiredSize.Height = Math.Max(0, height);

            return desiredSize;
        }

        private double CalculateRingRadius(double width, double height)
        {
            // calculate the ring radius in each direction based on available size
            double ringRadiusX = Math.Max(WIDTH_PATH, (width - 2 * (WIDTH_GAP_TEXT + this.TextWidth + WIDTH_BULLET_SPACER + WIDTH_SPACER_HORIZONTAL)) / 2);
            double ringRadiusY = Math.Max(WIDTH_PATH, (height - 2 * HEIGHT_SPACER_VERTICAL) / 2);

            // take the smaller of the two (because we need a round circle with no clipping)
            _ringRadius = Math.Min(ringRadiusX, ringRadiusY);

            // make sure our radius is not greater than max
            _ringRadius = Math.Min(_ringRadius, this.MaxRingRadius);

            return _ringRadius;
        }

        private void RenderUI()
        {
            // layout calculations
            double width = 2 * (_ringRadius + WIDTH_GAP_TEXT + this.TextWidth + WIDTH_BULLET_SPACER + WIDTH_SPACER_HORIZONTAL);
            double height = 2 * (_ringRadius + HEIGHT_SPACER_VERTICAL);
            double ringX = WIDTH_GAP_TEXT + this.TextWidth + WIDTH_BULLET_SPACER;
            double ringY = HEIGHT_SPACER_VERTICAL;

            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // create canvas
            _canvas = new Canvas()
            {
                Name = "FlywheelCanvas",
                Width = width,
                Height = height,
                Margin = new Thickness(0),
                ManipulationMode = ManipulationModes.None
            };
            Grid.SetRow(_canvas, 0);
            Grid.SetColumn(_canvas, 0);
            _layoutRoot.Children.Add(_canvas);

            // create the background ring
            _ringBackground = new RingSlice()
            {
                Name = "BackgroundRing",
                StartAngle = ANGLE_START_BACKGROUND,
                EndAngle = ANGLE_END_BACKGROUND,
                Radius = _ringRadius,
                InnerRadius = _ringRadius - WIDTH_PATH,
                Opacity = 1.0,
                //Fill = new SolidColorBrush(Colors.Purple)
                Fill = new SolidColorBrush() { Color = Colors.Black, Opacity = 0.1 }
            };
            //_ringBackground.SetBinding(RingSlice.FillProperty,
            //        new Binding() { Source = this, Path = new PropertyPath("BackgroundBrush"), Mode = BindingMode.OneWay });
            Canvas.SetLeft(_ringBackground, ringX);
            Canvas.SetTop(_ringBackground, ringY);
            Canvas.SetZIndex(_ringBackground, Z_ORDER_RING_BACKGROUND);
            _canvas.Children.Add(_ringBackground);

            // create the progress ring
            _ringProgress = new RingSlice()
            {
                Name = "ProgressRing",
                StartAngle = ANGLE_START,
                EndAngle = ANGLE_END,
                Radius = _ringRadius,
                InnerRadius = _ringRadius - WIDTH_PATH
            };
            _ringProgress.SetBinding(RingSlice.FillProperty,
                                new Binding() { Source = this, Path = new PropertyPath("ProgressBrush"), Mode = BindingMode.OneWay });
            Canvas.SetLeft(_ringProgress, ringX);
            Canvas.SetTop(_ringProgress, ringY);
            Canvas.SetZIndex(_ringProgress, Z_ORDER_RING_BACKGROUND);
            _canvas.Children.Add(_ringProgress);

            // position of bullets
            Point point;

            // create bullet 1
            _gridBullet1 = CreateBullet("Bullet1", out _bullet1, this.FirstBulletText, ANGLE_STEP1);
            point = this.GetBulletPosition(ANGLE_STEP1);
            Canvas.SetLeft(_gridBullet1, point.X);
            Canvas.SetTop(_gridBullet1, point.Y);
            _canvas.Children.Add(_gridBullet1);

            // create bullet 2
            _gridBullet2 = CreateBullet("Bullet2", out _bullet2, this.SecondBulletText, ANGLE_STEP2);
            point = this.GetBulletPosition(ANGLE_STEP2);
            Canvas.SetLeft(_gridBullet2, point.X);
            Canvas.SetTop(_gridBullet2, point.Y);
            _canvas.Children.Add(_gridBullet2);

            // create bullet 3
            _gridBullet3 = CreateBullet("Bullet3", out _bullet3, this.ThirdBulletText, ANGLE_STEP3);
            point = this.GetBulletPosition(ANGLE_STEP3);
            Canvas.SetLeft(_gridBullet3, point.X);
            Canvas.SetTop(_gridBullet3, point.Y);
            _canvas.Children.Add(_gridBullet3);

            // create bullet 4
            _gridBullet4 = CreateBullet("Bullet4", out _bullet4, this.FourthBulletText, ANGLE_STEP4);
            point = this.GetBulletPosition(ANGLE_STEP4);
            Canvas.SetLeft(_gridBullet4, point.X);
            Canvas.SetTop(_gridBullet4, point.Y);
            _canvas.Children.Add(_gridBullet4);

            // create bullet 5
            _gridBullet5 = CreateBullet("Bullet5", out _bullet5, this.FifthBulletText, ANGLE_STEP5);
            point = this.GetBulletPosition(ANGLE_STEP5);
            Canvas.SetLeft(_gridBullet5, point.X);
            Canvas.SetTop(_gridBullet5, point.Y);
            _canvas.Children.Add(_gridBullet5);


            List<double> angles = new List<double>() { ANGLE_STEP1, ANGLE_STEP2, ANGLE_STEP3, ANGLE_STEP4, ANGLE_STEP5 };

            double staggerDelayEntrance = 0.0;
            double staggerDelayExit = 0.0;

            if (null != _ringProgress)
            {
                // entrance animation
                _storyboardRingSliceEntranceList.Add(SetupProgressAnimation(_ringProgress, ANGLE_START, angles, DURATION_ANGLESEGMENT, DURATION_ANGLEPAUSE, staggerDelayEntrance));

                // set the exit animation to trigger the entrance animation when it's done
                _storyboardRingSliceExitList.Add(AnimationHelper.CreateEasingAnimationWithNotify(_ringProgress, this.Storyboard_Complete, "EndAngle", ANGLE_STEP5, ANGLE_STEP5, ANGLE_START, null, null, DURATION_ANGLEEXIT, staggerDelayExit, false, false, new RepeatBehavior(1d)));
            }

            staggerDelayEntrance += DURATION_FADE + (DURATION_ANGLESEGMENT - DURATION_FADE);        // background ring fades in, progress ring animates to step 1,
                                                                                                    // text fade should start just before step 1 and finish at step 1
            staggerDelayExit += DURATION_ANGLEEXIT - DURATION_FADE;

            // we want text blocks to start to disappear just as ring drops
            staggerDelayExit += DURATION_ANGLEEXIT - DURATION_FADE;

            if (null != _gridBullet1)
            {
                _storyboardTextBlockEntranceList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet1, "Opacity", 0.0, 0.0, 1.0, DURATION_FADE, staggerDelayEntrance, false, false, new RepeatBehavior(1d)));
                _storyboardTextBlockExitList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet1, "Opacity", 1.0, 1.0, 0.0, DURATION_FADE, staggerDelayExit, false, false, new RepeatBehavior(1d)));
            }

            staggerDelayEntrance += DURATION_ANGLESEGMENT + DURATION_ANGLEPAUSE - DURATION_FADE;

            if (null != _gridBullet2)
            {
                _storyboardTextBlockEntranceList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet2, "Opacity", 0.0, 0.0, 1.0, DURATION_FADE, staggerDelayEntrance, false, false, new RepeatBehavior(1d)));
                _storyboardTextBlockExitList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet2, "Opacity", 1.0, 1.0, 0.0, DURATION_FADE, staggerDelayExit, false, false, new RepeatBehavior(1d)));
            }

            staggerDelayEntrance += DURATION_ANGLESEGMENT + DURATION_ANGLEPAUSE - DURATION_FADE;

            if (null != _gridBullet3)
            {
                _storyboardTextBlockEntranceList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet3, "Opacity", 0.0, 0.0, 1.0, DURATION_FADE, staggerDelayEntrance, false, false, new RepeatBehavior(1d)));
                _storyboardTextBlockExitList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet3, "Opacity", 1.0, 1.0, 0.0, DURATION_FADE, staggerDelayExit, false, false, new RepeatBehavior(1d)));
            }

            staggerDelayEntrance += DURATION_ANGLESEGMENT + DURATION_ANGLEPAUSE - DURATION_FADE;

            if (null != _gridBullet4)
            {
                _storyboardTextBlockEntranceList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet4, "Opacity", 0.0, 0.0, 1.0, DURATION_FADE, staggerDelayEntrance, false, false, new RepeatBehavior(1d)));
                _storyboardTextBlockExitList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet4, "Opacity", 1.0, 1.0, 0.0, DURATION_FADE, staggerDelayExit, false, false, new RepeatBehavior(1d)));
            }

            staggerDelayEntrance += DURATION_ANGLESEGMENT + DURATION_ANGLEPAUSE - DURATION_FADE;

            if (null != _gridBullet5)
            {
                _storyboardTextBlockEntranceList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet5, "Opacity", 0.0, 0.0, 1.0, DURATION_FADE, staggerDelayEntrance, false, false, new RepeatBehavior(1d)));
                _storyboardTextBlockExitList.Add(AnimationHelper.CreateEasingAnimation(_gridBullet5, "Opacity", 1.0, 1.0, 0.0, DURATION_FADE, staggerDelayExit, false, false, new RepeatBehavior(1d)));
            }
        }

        private void UpdateUI()
        {
            // calculate our size based on the ring radius, which we set in MeasureOverride()
            double width = this.ActualWidth;
            double height = this.ActualHeight;
            double ringX = WIDTH_GAP_TEXT + this.TextWidth + WIDTH_BULLET_SPACER;
            double ringY = HEIGHT_SPACER_VERTICAL;

            // if we don't have valid width values, exit; we're called before we've been called to measure
            if ((0 == width) || (Double.IsNaN(width)) || (0 == height) || (Double.IsNaN(height)))
            {
                return;
            }

            // calculate the new ring radius
            _ringRadius = CalculateRingRadius(width, height);

            // update background ring
            if (null != _ringBackground)
            {
                // set radius
                _ringBackground.Radius = _ringRadius;
                _ringBackground.InnerRadius = _ringRadius - WIDTH_PATH;

                // set x, y
                Canvas.SetLeft(_ringBackground, ringX);
                Canvas.SetTop(_ringBackground, ringY);
            }

            // update progress ring
            if (null != _ringProgress)
            {
                // set radius
                _ringProgress.Radius = _ringRadius;
                _ringProgress.InnerRadius = _ringRadius - WIDTH_PATH;

                // set x, y
                Canvas.SetLeft(_ringProgress, ringX);
                Canvas.SetTop(_ringProgress, ringY);
            }

            // update bullet positions
            Point point;

            // bullet one
            point = this.GetBulletPosition(ANGLE_STEP1);
            if (null != _gridBullet1)
            {
                Canvas.SetLeft(_gridBullet1, point.X);
                Canvas.SetTop(_gridBullet1, point.Y);
            }

            // bullet two
            point = this.GetBulletPosition(ANGLE_STEP2);
            if (null != _gridBullet2)
            {
                Canvas.SetLeft(_gridBullet2, point.X);
                Canvas.SetTop(_gridBullet2, point.Y);
            }

            // bullet three
            point = this.GetBulletPosition(ANGLE_STEP3);
            if (null != _gridBullet3)
            {
                Canvas.SetLeft(_gridBullet3, point.X);
                Canvas.SetTop(_gridBullet3, point.Y);
            }

            // bullet four
            point = this.GetBulletPosition(ANGLE_STEP4);
            if (null != _gridBullet4)
            {
                Canvas.SetLeft(_gridBullet4, point.X);
                Canvas.SetTop(_gridBullet4, point.Y);
            }

            // bullet five
            point = this.GetBulletPosition(ANGLE_STEP5);
            if (null != _gridBullet5)
            {
                Canvas.SetLeft(_gridBullet5, point.X);
                Canvas.SetTop(_gridBullet5, point.Y);
            }
        }

        private void Storyboard_Complete(object sender, object e)
        {
            // 2018 07 27 - This is no longer needed because there is no entrance animation for the
            //              background ring; so calling entrance animation here results in a double
            //              play and a "stutter".

            // we register this handler to catch exit animations ending so we can play entrance animations
            //this.StartEntranceAnimation();
        }

        #endregion


        #region UI Helpers

        private string GetBulletText(string text, double angle)
        {
            return (angle < 0) ? text + CHAR_NBSPACE + CHAR_BULLET : CHAR_BULLET + CHAR_NBSPACE + text;
        }

        private Grid CreateBullet(string name, out TextBlock _bulletText, string text, double angle)
        {
            // create the panel
            Grid grid = new Grid()
            {
                ColumnSpacing = WIDTH_BULLET_SPACER,
                HorizontalAlignment = (angle < 0) ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            // no null text
            if (String.IsNullOrEmpty(text))
            {
                text = String.Empty;
            }

            // create the bullet
            TextBlock bullet = new TextBlock()
            {
                Text = CHAR_BULLET,
                TextAlignment = (angle < 0) ? TextAlignment.Right : TextAlignment.Left,
                HorizontalAlignment = (angle < 0) ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };
            StyleHelper.SetFontCharacteristics(bullet, ControlStyles.GaugePointBullet);
            Grid.SetRow(bullet, 0);
            Grid.SetColumn(bullet, (angle < 0) ? 1 : 0);
            grid.Children.Add(bullet);

            // create a textblock for the bullet text and add it
            _bulletText = new TextBlock()
            {
                Text = text,
                Width = this.TextWidth,
                TextWrapping = TextWrapping.WrapWholeWords,
                TextAlignment = (angle < 0) ? TextAlignment.Right : TextAlignment.Left,
                HorizontalAlignment = (angle < 0) ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };
            StyleHelper.SetFontCharacteristics(_bulletText, ControlStyles.GaugePoint);
            Grid.SetRow(_bulletText, 0);
            Grid.SetColumn(_bulletText, (angle < 0) ? 0 : 1);
            grid.Children.Add(_bulletText);

            return grid;
        }

        private Point GetBulletPosition(double angle)
        {
            Point topLeftPoint;
            Point point;    // (x, y) for the angle

            // top left of the ring control
            topLeftPoint.X = WIDTH_GAP_TEXT + this.TextWidth + WIDTH_BULLET_SPACER;
            topLeftPoint.Y = 0;// HEIGHT_SPACER_VERTICAL;

            double widthGapText = WIDTH_GAP_TEXT;
            if ((angle > -45) && (angle < 45))
            {
                widthGapText = WIDTH_GAP_TEXT_LARGE;
            }

            // calculate position
            point.X = topLeftPoint.X + _ringRadius + (_ringRadius * Math.Sin(angle * Math.PI / 180d)) + ((angle < 0) ? -1 * (widthGapText + this.TextWidth + WIDTH_BULLET_SPACER) : widthGapText);
            point.Y = topLeftPoint.Y + _ringRadius - (_ringRadius * Math.Cos(angle * Math.PI / 180d));

            return point;
        }

        private static LinearGradientBrush CreateLinearGradientBrush(string colorStart, string colorEnd)
        {
            LinearGradientBrush brush = new LinearGradientBrush();

            GradientStopCollection stops = new GradientStopCollection()
            {
                new GradientStop()
                {
                    Color = SDX.Toolkit.Helpers.ColorHelper.ConvertHexToColor(colorStart),
                    Offset = 0.0
                },
                new GradientStop()
                {
                    Color = SDX.Toolkit.Helpers.ColorHelper.ConvertHexToColor(colorEnd),
                    Offset = 1.0
                }
            };

            brush.GradientStops = stops;

            return brush;
        }

        private Storyboard SetupProgressAnimation(RingSlice ring, double angleStart, List<double> angleSteps, double durationSegment, double durationPause, double staggerDelay)
        {
            // can't work without steps
            if (null == angleSteps) { return null; }

            double totalDuration = (angleSteps.Count * (durationSegment + durationPause)) + staggerDelay;

            double frameTime = 0.0;

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

            //// need cubic easing
            //CubicEase cubicEaseIn = new CubicEase()
            //{
            //    EasingMode = EasingMode.EaseIn
            //};
            // CubicEase cubicEaseOut = new CubicEase()
            // {
            //     EasingMode = EasingMode.EaseOut
            // };

            // create frame 0
            EasingDoubleKeyFrame _frame0 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                Value = angleStart,
                //EasingFunction = cubicEaseIn
            };
            _frames.KeyFrames.Add(_frame0);

            // create stagger frame
            if (staggerDelay > 0)
            {
                frameTime += staggerDelay;

                EasingDoubleKeyFrame _frameStagger = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                    Value = angleStart,
                    //EasingFunction = cubicEaseIn
                };
                _frames.KeyFrames.Add(_frameStagger);
            }

            // loop through the angles
            foreach (double angle in angleSteps)
            {
                frameTime += durationSegment;

                // move angle to position
                EasingDoubleKeyFrame _frameMove = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                    Value = angle,
                    //EasingFunction = cubicEaseIn
                };
                _frames.KeyFrames.Add(_frameMove);

                frameTime += durationPause;

                // create step 1 - pause
                EasingDoubleKeyFrame _framePause = new EasingDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(frameTime)),
                    Value = angle,
                    //EasingFunction = cubicEaseIn
                };
                _frames.KeyFrames.Add(_framePause);
            }

            // add frame collection to the storyboard
            storyboard.Children.Add(_frames);

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, ring);
            Storyboard.SetTargetProperty(storyboard, "EndAngle");
            //storyboard.SetValue(Storyboard.TargetPropertyProperty, "EndAngle");

            return storyboard;
        }

        #endregion


        #region Code Helpers

        #endregion
    }
}

//flywheel colors:
//Burgundy: start: 957f80 end: 6a3337
//Cobalt blue: start: 707f89 end: 385465
//White: start: b6b6b6 end: white
//Black: start: 6f6f6f end: 2b2b2b
//Base Rim: Black with 10 % opacity
