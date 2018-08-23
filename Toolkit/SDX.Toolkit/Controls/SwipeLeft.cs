using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;


namespace SDX.Toolkit.Controls
{

    public sealed class SwipeLeft : Control
    {
        #region Private Constants

        private static readonly Size WINDOW_BOUNDS = WindowHelper.GetViewSizeInfo();

        private readonly double WIDTH_CONTROL = WINDOW_BOUNDS.Width / 2;
        private const double HEIGHT_ROWSPACING = 30d;
        private const double SIZE_ELLIPSE = 60d;

        private const double ANIM_DURATION_ELLIPSE_FADE = 200d;
        private const double ANIM_DURATION_ELLIPSE_MOVE = 1400d;

        private const double ANIM_STAGGER_TEXT_FADEIN = 0d;
        private const double ANIM_STAGGER_TEXT_FADEOUT = 0d;

        private const double ANIM_STAGGER_ELLIPSE_FADEIN = ANIM_STAGGER_TEXT_FADEIN + 400d;
        private const double ANIM_STAGGER_ELLIPSE_MOVE = ANIM_STAGGER_ELLIPSE_FADEIN + 600d;
        private const double ANIM_STAGGER_ELLIPSE_FADEOUT = ANIM_STAGGER_ELLIPSE_MOVE + ANIM_DURATION_ELLIPSE_MOVE + 400d;

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private TextBlock _textSwipe = null;
        private Ellipse _ellipseSwipe = null;
        private TranslateTransform _translateEllipse = null;

        private Storyboard _storyboardEllipseFadeIn = null;
        private Storyboard _storyboardEllipseFadeOut = null;
        private Storyboard _storyboardEllipseMove = null;

        private Storyboard _storyboardFadeIn = null;
        private Storyboard _storyboardFadeOut = null;

        #endregion

        #region Constructor

        public SwipeLeft()
        {
            this.DefaultStyleKey = typeof(SwipeLeft);
            this.Loaded += OnLoaded;

            // inherited dependency property
            new PropertyChangeEventSource<double>(
                this, "Opacity", BindingMode.OneWay).ValueChanged +=
                OnOpacityChanged;
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
            // start the storyboards
            if (null != _storyboardEllipseFadeIn)
            {
                _storyboardEllipseFadeIn.Begin();
            }

            // these are now triggered by the
            // completion of fadein
            //if (null != _storyboardEllipseMove)
            //{
            //    _storyboardEllipseMove.Begin();
            //}

            //if (null != _storyboardEllipseFadeOut)
            //{
            //    _storyboardEllipseFadeOut.Begin();
            //}
        }

        public void ResetAnimation()
        {
            // reset the storyboards
            if (null != _storyboardEllipseFadeIn)
            {
                _storyboardEllipseFadeIn.Stop();

            }

            if (null != _storyboardEllipseMove)
            {
                _storyboardEllipseMove.Stop();
            }

            if (null != _storyboardEllipseFadeOut)
            {
                _storyboardEllipseFadeOut.Stop();
            }

            // reset ellipse opacity and position
            if (null != _ellipseSwipe)
            {
                _ellipseSwipe.Opacity = 0.0;
            }
            if (null != _translateEllipse)
            {
                _translateEllipse.X = 0;
            }
        }

        public void StartFadeIn()
        {
            // start the fade in
            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Begin();
            }
        }

        public void StartFadeOut()
        {
            // start the fade out
            if (null != _storyboardFadeOut)
            {
                _storyboardFadeOut.Begin();
            }
        }

        public void ResetFadeAnimations()
        {
            // reset the animations
            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Stop();
            }
            if (null != _storyboardFadeOut)
            {
                _storyboardFadeOut.Stop();
            }

            // reset opacity to starting point
            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = 0.0;
            }
            if (null != _ellipseSwipe)
            {
                _ellipseSwipe.Opacity = 0.0;
            }

        }

        public void SetOpacity(double opacity)
        {
            opacity = Math.Max(0.0, opacity);
            opacity = Math.Min(1.0, opacity);

            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = opacity;
            }
        }

        #endregion

        #region Dependency Properties

        // SwipeText
        public static readonly DependencyProperty SwipeTextProperty =
            DependencyProperty.Register("SwipeText", typeof(string), typeof(SwipeLeft), new PropertyMetadata(null, OnSwipeTextChanged));

        public string SwipeText
        {
            get { return (string)GetValue(SwipeTextProperty); }
            set { SetValue(SwipeTextProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(SwipeLeft), new PropertyMetadata(200d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(SwipeLeft), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(SwipeLeft), new PropertyMetadata(false, OnAutoStartChanged));

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
                this.StartFadeIn();
                this.StartAnimation();
            }
        }

        private static void OnSwipeTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // clear any children
            _layoutRoot.Children.Clear();

            // set up grid
            _layoutRoot.Width = WIDTH_CONTROL;
            _layoutRoot.RowSpacing = HEIGHT_ROWSPACING;
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SIZE_ELLIPSE) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });

            // test
            //TestHelper.AddGridCellBorders(_layoutRoot, 2, 1, Colors.DeepSkyBlue);

            // create textblock
            _textSwipe = new TextBlock()
            {
                Name = "SwipeText",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            _textSwipe.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("SwipeText"), Mode = BindingMode.OneWay });
            StyleHelper.SetFontCharacteristics(_textSwipe, ControlStyles.SwipeToContinue);
            Grid.SetRow(_textSwipe, 1);
            Grid.SetColumn(_textSwipe, 0);
            _layoutRoot.Children.Add(_textSwipe);

            // create the ellipse
            _ellipseSwipe = new Ellipse()
            {
                Width = SIZE_ELLIPSE,
                Height = SIZE_ELLIPSE,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Stroke = new SolidColorBrush(Colors.White),
                Fill = new SolidColorBrush(Colors.White),
                Opacity = 0.0
            };
            Grid.SetRow(_ellipseSwipe, 0);
            Grid.SetColumn(_ellipseSwipe, 0);
            _layoutRoot.Children.Add(_ellipseSwipe);

            // add a translate transform to the ellipse
            _translateEllipse = new TranslateTransform()
            {
                X = 0
            };
            _ellipseSwipe.RenderTransform = _translateEllipse;

            // set up control fades
            _storyboardFadeIn = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 0.0, 0.0, 1.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds + ANIM_STAGGER_TEXT_FADEIN, false, false, new RepeatBehavior(1));
            _storyboardFadeOut = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 0.0, 1.0, 0.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds + ANIM_STAGGER_TEXT_FADEOUT, false, false, new RepeatBehavior(1));

            // set up ellipse fades
            _storyboardEllipseFadeIn = AnimationHelper.CreateEasingAnimationWithNotify(_ellipseSwipe, this.FadeIn_Complete, "Opacity", 0.0, 0.0, 1.0, null, null, ANIM_DURATION_ELLIPSE_FADE, this.StaggerDelayInMilliseconds + ANIM_STAGGER_ELLIPSE_FADEIN, false, false, new RepeatBehavior(1));
            _storyboardEllipseFadeOut = AnimationHelper.CreateEasingAnimation(_ellipseSwipe, "Opacity", 0.0, 1.0, 0.0, ANIM_DURATION_ELLIPSE_FADE, 0d, false, false, new RepeatBehavior(1));
            // fade out original stagger:  this.StaggerDelayInMilliseconds + ANIM_STAGGER_ELLIPSE_FADEOUT

            QuinticEase quinticEase = new QuinticEase()
            {
                EasingMode = EasingMode.EaseInOut
            };

            // set up ellipse move
            _storyboardEllipseMove = AnimationHelper.CreateEasingAnimationWithNotify(_ellipseSwipe, this.Move_Complete, "(Ellipse.RenderTransform).(TranslateTransform.X)", 0.0, 0.0, -1 * WIDTH_CONTROL + SIZE_ELLIPSE, quinticEase, quinticEase, ANIM_DURATION_ELLIPSE_MOVE, 0d, false, false, new RepeatBehavior(1));
            // move original stagger: this.StaggerDelayInMilliseconds + ANIM_STAGGER_ELLIPSE_MOVE
        }

        #endregion

        #region UI Helpers

        private void FadeIn_Complete(object sender, object e)
        {
            // once fade in is complete, we need to start the move
            if (null != _storyboardEllipseMove)
            {
                _storyboardEllipseMove.Begin();
            }
        }

        private void Move_Complete(object sender, object e)
        {
            // once move is complete, we need to fade out
            if (null != _storyboardEllipseFadeOut)
            {
                _storyboardEllipseFadeOut.Begin();
            }
        }

        #endregion
    }
}

