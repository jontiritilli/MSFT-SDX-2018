using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using SDX.Toolkit.Helpers;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class AnimatableInteger : Control
    {

        #region Private Members

        private Border _layoutRoot = null;
        private TextBlockEx _integer = null;

        private Storyboard _storyboardInteger = null;

        #endregion

        #region Constructor

        public AnimatableInteger()
        {
            this.DefaultStyleKey = typeof(AnimatableInteger);
            this.Loaded += OnLoaded;
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
            // start the percent
            if (null != _storyboardInteger)
            {
                _storyboardInteger.Begin();
            }
        }

        public void ResetAnimation()
        {
            // reset the headline
            if (null != _storyboardInteger)
            {
                _storyboardInteger.Stop();
            }
        }

        #endregion

        #region Dependency Properties

        // HourValue
        public static readonly DependencyProperty HourValueProperty =
            DependencyProperty.Register("HourValue", typeof(double), typeof(AnimatableInteger), new PropertyMetadata(0.0, OnHourValueChanged));

        public double HourValue
        {
            get => (double)GetValue(HourValueProperty);
            set => SetValue(HourValueProperty, value);
        }

        // HourIntegerMax
        public static readonly DependencyProperty HourIntegerMaxProperty =
            DependencyProperty.Register("HourIntegerMax", typeof(double), typeof(AnimatableInteger), new PropertyMetadata(0.0));

        public double HourIntegerMax
        {
            get => (double)GetValue(HourIntegerMaxProperty);
            set => SetValue(HourIntegerMaxProperty, value);
        }

        // hourText
        public static readonly DependencyProperty HourTextProperty =
            DependencyProperty.Register("HourText", typeof(string), typeof(AnimatableInteger), new PropertyMetadata(0.0, OnHourTextChanged));

        public string HourText
        {
            get => (string)GetValue(HourTextProperty);
            set => SetValue(HourTextProperty, value);
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(AnimatableInteger), new PropertyMetadata(2000d, OnDurationInMillisecondsChanged));

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
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(AnimatableInteger), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get => (double)GetValue(StaggerDelayInMillisecondsProperty);
            set => SetValue(StaggerDelayInMillisecondsProperty, (value < 0) ? 0 : value);
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(AnimatableInteger), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.StartAnimation();
        }

        private static void OnHourValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AnimatableInteger hours = (AnimatableInteger)d;
            double hour;

            if (null != hours)
            {
                hour = Math.Ceiling(hours.HourValue * 2) / 2;
                hours.HourText = String.Format("{0:0.#}", hours.HourValue);
                //hours.HourText = String.Format("{0:0}", hours.HourValue);
            }
        }

        private static void OnHourTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        #region UI Methods

        private void RenderUI()
        {
            // if we've done this already
            if (null != _layoutRoot) { return; }

            // get the layoutroot
            _layoutRoot = (Border)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot)
            {
                return;
            }

            // create the text block
            _integer = new TextBlockEx()
            {
                TextStyle = TextStyles.PopupBatteryLife,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = this.Width
            };

            _integer.SetBinding(TextBlockEx.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("HourText"), Mode = BindingMode.OneWay });

            // add it to the root
            _layoutRoot.Child = _integer;

            // set up animation
            _storyboardInteger = SetupAnimation(this, 0.0, this.HourIntegerMax, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds);
        }

        private Storyboard SetupAnimation(AnimatableInteger integer, double start, double finish, double duration, double staggerDelay)
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
            Storyboard.SetTarget(storyboard, integer);
            Storyboard.SetTargetProperty(storyboard, "HourValue");
            //storyboard.SetValue(Storyboard.TargetPropertyProperty, "HourValue");

            return storyboard;
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}

