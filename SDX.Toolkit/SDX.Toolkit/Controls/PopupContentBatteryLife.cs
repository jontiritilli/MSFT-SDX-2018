﻿using System;
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
    public sealed class PopupContentBatteryLife : Control
    {
        #region Constants

        private readonly string URI_IMAGE_BATTERY = @"ms-appx:///Assets/BatteryLifePopup/battery-outline.png";
        private readonly string URI_IMAGE_CHARGE = @"ms-appx:///Assets/BatteryLifePopup/blue-charge.png";

        private static readonly double TOP_BATTERY = 0d;
        private static readonly double LEFT_BATTERY = 0d;

        private static readonly double MARGIN_CHARGE_LEFT = 20d;
        private static readonly double MARGIN_CHARGE_TOP = 20d;
        private static readonly double CHARGE_START_WIDTH = 10d;

        private static readonly double TOP_CHARGE = TOP_BATTERY + MARGIN_CHARGE_TOP;
        private static readonly double LEFT_CHARGE = LEFT_BATTERY + MARGIN_CHARGE_LEFT;

        private readonly int Z_ORDER_CONTROLS = 100;
        private readonly int Z_ORDER_BATTERY = 10;
        private readonly int Z_ORDER_CHARGE = 20;

        #endregion

        #region Private Members

        private Canvas _layoutRoot = null;
        private AnimatableInteger _hours = null;
        private TextBlockEx _hoursText = null;
        private ImageEx _imageBattery = null;
        private Image _imageCharge = null;

        private Storyboard _chargeStoryboard = null;

        #endregion

        #region Construction

        public PopupContentBatteryLife()
        {
            this.DefaultStyleKey = typeof(PopupContentBatteryLife);
            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.RenderUI();
        }

        #endregion

        #region Public Properties

        // used for async image loading
        public Task Initialization { get; private set; }

        #endregion

        #region Dependency Properties

        // HourIntegerMax
        public static readonly DependencyProperty HourIntegerMaxProperty =
            DependencyProperty.Register("HourIntegerMax", typeof(double), typeof(PopupContentBatteryLife), new PropertyMetadata(17d, OnPropertyChanged));

        public double HourIntegerMax
        {
            get { return (double)GetValue(HourIntegerMaxProperty); }
            set { SetValue(HourIntegerMaxProperty, value); }
        }

        // HourText
        public static readonly DependencyProperty HourTextProperty =
            DependencyProperty.Register("Hour", typeof(string), typeof(PopupContentBatteryLife), new PropertyMetadata("hrs", OnPropertyChanged));

        public string HourText
        {
            get { return (string)GetValue(HourTextProperty); }
            set { SetValue(HourTextProperty, value); }
        }

        // TimeStart
        public static readonly DependencyProperty TimeStartProperty =
            DependencyProperty.Register("TimeStart", typeof(string), typeof(PopupContentBatteryLife), new PropertyMetadata(null, OnPropertyChanged));

        public string TimeStart
        {
            get { return (string)GetValue(TimeStartProperty); }
            set { SetValue(TimeStartProperty, value); }
        }

        // TimeStop
        public static readonly DependencyProperty TimeStopProperty =
            DependencyProperty.Register("TimeStop", typeof(string), typeof(PopupContentBatteryLife), new PropertyMetadata(null, OnPropertyChanged));

        public string TimeStop
        {
            get { return (string)GetValue(TimeStopProperty); }
            set { SetValue(TimeStopProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(PopupContentBatteryLife), new PropertyMetadata(2000d, OnDurationInMillisecondsChanged));

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
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(PopupContentBatteryLife), new PropertyMetadata(200d, OnStaggerDelayInMillisecondsChanged));

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
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(PopupContentBatteryLife), new PropertyMetadata(false, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Public Methods

        public void StartAnimation()
        {
            if (null != _chargeStoryboard)
            {
                _chargeStoryboard.Begin();
            }

            if (null != _hours)
            {
                _hours.StartAnimation();
            }
        }

        public void ResetAnimation()
        {
            //if (null != _header)
            //{
            //    _header.ResetAnimation();
            //}

            if (null != _chargeStoryboard)
            {
                _chargeStoryboard.Stop();
            }

            if (null != _hours)
            {
                _hours.ResetAnimation();
            }
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.StartAnimation();
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
            _layoutRoot = (Canvas)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot)
            {
                return;
            }

            double batteryHeight = this.Width * .383;
            double chargeHeight = batteryHeight - (2 * MARGIN_CHARGE_TOP);
            double batteryHorizontalCenter = this.Width/2;
            double batteryVerticalCenter = batteryHeight / 2;

            double TEXT_WIDTH = 100d;
            double LEFT_HOURS = batteryHorizontalCenter - TEXT_WIDTH - (MARGIN_CHARGE_LEFT/2);
            double LEFT_HOURS_TEXT = batteryHorizontalCenter + (MARGIN_CHARGE_LEFT / 2);
            double TOP_TEXT = batteryVerticalCenter - (chargeHeight/2);

            // set up the canvas
            _layoutRoot.Width = this.Width;
            _layoutRoot.Height = batteryHeight;

            // create the battery
            _imageBattery = new ImageEx()
            {
                Name = "Battery",
                ImageSource = URI_IMAGE_BATTERY,
                ImageWidth = this.Width
            };

            Canvas.SetLeft(_imageBattery, LEFT_BATTERY);
            Canvas.SetTop(_imageBattery, TOP_BATTERY);
            Canvas.SetZIndex(_imageBattery, Z_ORDER_BATTERY);
            _layoutRoot.Children.Add(_imageBattery);

            // create the charge
            _imageCharge = new Image()
            {
                Name = "ChargeBar",
                Source = new BitmapImage(new Uri(URI_IMAGE_CHARGE)),
                Stretch = Stretch.Fill,
                Width = CHARGE_START_WIDTH,
                Height = chargeHeight
            };

            Canvas.SetLeft(_imageCharge, LEFT_CHARGE);
            Canvas.SetTop(_imageCharge, TOP_CHARGE);
            Canvas.SetZIndex(_imageCharge, Z_ORDER_CHARGE);
            _layoutRoot.Children.Add(_imageCharge);

            // create the percent overlay
            _hours = new AnimatableInteger()
            {
                HourIntegerMax = this.HourIntegerMax,
                Width = TEXT_WIDTH,
                Height = chargeHeight,
                VerticalAlignment = VerticalAlignment.Center,
                DurationInMilliseconds = this.DurationInMilliseconds,
                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds
            };

            Canvas.SetLeft(_hours, LEFT_HOURS);
            Canvas.SetTop(_hours, TOP_TEXT);
            Canvas.SetZIndex(_hours, Z_ORDER_CONTROLS);
            _layoutRoot.Children.Add(_hours);

            double chargeEndWidth = this.Width - ((2 * MARGIN_CHARGE_LEFT) + CHARGE_START_WIDTH + 5);

            // create the charge animation
            _chargeStoryboard = SetupChargeAnimation(_imageCharge, CHARGE_START_WIDTH, chargeEndWidth, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds);

            // create hour text overlay
            _hoursText = new TextBlockEx()
            {
                Text = this.HourText,
                Width = TEXT_WIDTH,
                Height = chargeHeight,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                TextStyle = TextStyles.PopupBatteryLife
            };

            Canvas.SetLeft(_hoursText, LEFT_HOURS_TEXT);
            Canvas.SetTop(_hoursText, TOP_TEXT);
            Canvas.SetZIndex(_hoursText, Z_ORDER_CONTROLS);
            _layoutRoot.Children.Add(_hoursText);
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

    }
}
