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
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Helpers;
using Windows.Foundation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{

    public sealed class SwipeToContinue : Control
    {
        #region Private Constants

        private const double CONTROL_WIDTH = 300d;
        private const double CONTROL_HEIGHT = 60d;
        private const double CONTROL_COLUMNSPACING = 5d;
        private const double CONTROL_RIGHTMARGIN = 30d;

        private const string ARROW_URI = @"ms-appx:///Assets/SwipeLeft/swipe-arrow.png";
        private const int ARROW_WIDTH = 10;

        private const double ANIM_STAGGER_TEXT = 1000d;
        private const double ANIM_STAGGER_FIRSTARROW = 200;
        private const double ANIM_STAGGER_NEXTARROWS = 200d;
        private const double ANIM_FADEIN_TEXT = 200d;
        private const double ANIM_FADEIN_ARROW = 200d;
        private const double ANIM_REST_ARROW = 0d;


        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private TranslateTransform _translateGrid = null;
        private Image _arrowLeftmost = null;
        private Image _arrowLeft = null;
        private Image _arrowMiddle = null;
        private Image _arrowRight = null;
        private Image _arrowRightmost = null;
        private TextBlock _textSwipe = null;

        private BitmapImage _arrowBitmapImage = null;

        private Storyboard _storyboardText = null;
        private Storyboard _storyboardLeftmostArrow = null;
        private Storyboard _storyboardLeftArrow = null;
        private Storyboard _storyboardMiddleArrow = null;
        private Storyboard _storyboardRightArrow = null;
        private Storyboard _storyboardRightmostArrow = null;

        private Storyboard _storyboardFadeIn = null;
        private Storyboard _storyboardFadeOut = null;

        private bool _pointerCaptured = false;

        #endregion

        #region Constructor

        public SwipeToContinue()
        {
            this.DefaultStyleKey = typeof(SwipeToContinue);
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
            if (null != _storyboardText)
            {
                _storyboardText.Begin();
            }
            if (null != _storyboardLeftmostArrow)
            {
                _storyboardLeftmostArrow.Begin();
            }
            if (null != _storyboardLeftArrow)
            {
                _storyboardLeftArrow.Begin();
            }
            if (null != _storyboardMiddleArrow)
            {
                _storyboardMiddleArrow.Begin();
            }
            if (null != _storyboardRightArrow)
            {
                _storyboardRightArrow.Begin();
            }
            if (null != _storyboardRightmostArrow)
            {
                _storyboardRightmostArrow.Begin();
            }
        }

        public void ResetAnimation()
        {
            // reset the storyboards
            if (null != _storyboardText)
            {
                _storyboardText.Stop();

            }
            if (null != _storyboardLeftmostArrow)
            {
                _storyboardLeftmostArrow.Stop();
            }
            if (null != _storyboardLeftArrow)
            {
                _storyboardLeftArrow.Stop();
            }
            if (null != _storyboardMiddleArrow)
            {
                _storyboardMiddleArrow.Stop();
            }
            if (null != _storyboardRightArrow)
            {
                _storyboardRightArrow.Stop();
            }
            if (null != _storyboardRightmostArrow)
            {
                _storyboardRightmostArrow.Stop();
            }

            // reset opacities
            if (null != _textSwipe)
            {
                _textSwipe.Opacity = 0.0;
            }
            if (null != _arrowLeftmost)
            {
                _arrowLeftmost.Opacity = 0.0;
            }
            if (null != _arrowLeft)
            {
                _arrowLeft.Opacity = 0.0;
            }
            if (null != _arrowMiddle)
            {
                _arrowMiddle.Opacity = 0.0;
            }
            if (null != _arrowRight)
            {
                _arrowRight.Opacity = 0.0;
            }
            if (null != _arrowRightmost)
            {
                _arrowRightmost.Opacity = 0.0;
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
            DependencyProperty.Register("SwipeText", typeof(string), typeof(FadeInHeader), new PropertyMetadata(null, OnSwipeTextChanged));

        public string SwipeText
        {
            get { return (string)GetValue(SwipeTextProperty); }
            set { SetValue(SwipeTextProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(FadeInHeader), new PropertyMetadata(200d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(FadeInHeader), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(FadeInHeader), new PropertyMetadata(true, OnAutoStartChanged));

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

        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
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
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(CONTROL_HEIGHT) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ARROW_WIDTH) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ARROW_WIDTH) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ARROW_WIDTH) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ARROW_WIDTH) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ARROW_WIDTH) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(CONTROL_COLUMNSPACING) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(CONTROL_RIGHTMARGIN) });

            // add a translate transform to the layout root
            _translateGrid = new TranslateTransform()
            {
                X = 0
            };
            _layoutRoot.RenderTransform = _translateGrid;

            // manipulation mode
            _layoutRoot.ManipulationMode = ManipulationModes.TranslateRailsX;

            // attach to the manipulation events
            _layoutRoot.ManipulationStarted += Grid_ManipulationStarted;
            _layoutRoot.ManipulationDelta += Grid_ManipulationDelta;
            _layoutRoot.ManipulationCompleted += Grid_ManipulationCompleted;

            // create arrow bitmap
            _arrowBitmapImage = new BitmapImage()
            {
                UriSource = new Uri(ARROW_URI),
                DecodePixelWidth = ARROW_WIDTH
            };

            // create arrows
            _arrowLeftmost = new Image()
            {
                Width = ARROW_WIDTH,
                Opacity = 0.0,
                Source = _arrowBitmapImage
            };
            Grid.SetRow(_arrowLeftmost, 0);
            Grid.SetColumn(_arrowLeftmost, 0);
            _layoutRoot.Children.Add(_arrowLeftmost);

            _arrowLeft = new Image()
            {
                Width = ARROW_WIDTH,
                Opacity = 0.0,
                Source = _arrowBitmapImage
            };
            Grid.SetRow(_arrowLeft, 0);
            Grid.SetColumn(_arrowLeft, 1);
            _layoutRoot.Children.Add(_arrowLeft);

            _arrowMiddle = new Image()
            {
                Width = ARROW_WIDTH,
                Opacity = 0.0,
                Source = _arrowBitmapImage
            };
            Grid.SetRow(_arrowMiddle, 0);
            Grid.SetColumn(_arrowMiddle, 2);
            _layoutRoot.Children.Add(_arrowMiddle);

            _arrowRight = new Image()
            {
                Width = ARROW_WIDTH,
                Opacity = 0.0,
                Source = _arrowBitmapImage
            };
            Grid.SetRow(_arrowRight, 0);
            Grid.SetColumn(_arrowRight, 3);
            _layoutRoot.Children.Add(_arrowRight);

            _arrowRightmost = new Image()
            {
                Width = ARROW_WIDTH,
                Opacity = 0.0,
                Source = _arrowBitmapImage
            };
            Grid.SetRow(_arrowRightmost, 0);
            Grid.SetColumn(_arrowRightmost, 4);
            _layoutRoot.Children.Add(_arrowRightmost);

            // create textblock
            _textSwipe = new TextBlock()
            {
                Opacity = 0.0,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            _textSwipe.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("SwipeText"), Mode = BindingMode.OneWay });
            StyleHelper.SetFontCharacteristics(_textSwipe, ControlStyles.SwipeLeft);
            Grid.SetRow(_textSwipe, 0);
            Grid.SetColumn(_textSwipe, 6);
            _layoutRoot.Children.Add(_textSwipe);

            // animation calculations
            double staggerDelayIn = ANIM_STAGGER_TEXT;

            // text fade in
            _storyboardText = AnimationHelper.CreateEasingAnimation(_textSwipe, "(UIElement.Opacity)", 0.0, 0.0, 1.0, null, null, ANIM_FADEIN_TEXT, staggerDelayIn, false, false, new RepeatBehavior(1d));
            staggerDelayIn += ANIM_FADEIN_TEXT + ANIM_STAGGER_FIRSTARROW;

            // right most arrow fade in
            _storyboardRightmostArrow = AnimationHelper.CreateEasingAnimation(_arrowRightmost, "(UIElement.Opacity)", 0.0, 0.0, 1.0, null, null, ANIM_FADEIN_ARROW, staggerDelayIn, false, false, new RepeatBehavior(1d));
            staggerDelayIn += ANIM_FADEIN_ARROW + ANIM_STAGGER_NEXTARROWS + ANIM_REST_ARROW;

            // right arrow fade in
            _storyboardRightArrow = AnimationHelper.CreateEasingAnimation(_arrowRight, "(UIElement.Opacity)", 0.0, 0.0, 1.0, null, null, ANIM_FADEIN_ARROW, staggerDelayIn, false, false, new RepeatBehavior(1d));
            staggerDelayIn += ANIM_FADEIN_ARROW + ANIM_STAGGER_NEXTARROWS + ANIM_REST_ARROW;

            // middle arrow fade in
            _storyboardMiddleArrow = AnimationHelper.CreateEasingAnimation(_arrowMiddle, "(UIElement.Opacity)", 0.0, 0.0, 1.0, null, null, ANIM_FADEIN_ARROW,  staggerDelayIn, false, false, new RepeatBehavior(1d));
            staggerDelayIn += ANIM_FADEIN_ARROW + ANIM_STAGGER_NEXTARROWS + ANIM_REST_ARROW;

            // left arrow fade in
            _storyboardLeftArrow = AnimationHelper.CreateEasingAnimation(_arrowLeft, "(UIElement.Opacity)", 0.0, 0.0, 1.0, null, null, ANIM_FADEIN_ARROW, staggerDelayIn, false, false, new RepeatBehavior(1d));
            staggerDelayIn += ANIM_FADEIN_ARROW + ANIM_STAGGER_NEXTARROWS + ANIM_REST_ARROW;

            // left most arrow fade in
            _storyboardLeftmostArrow = AnimationHelper.CreateEasingAnimation(_arrowLeftmost, "(UIElement.Opacity)", 0.0, 0.0, 1.0, null, null, ANIM_FADEIN_ARROW, staggerDelayIn, false, false, new RepeatBehavior(1d));
            staggerDelayIn += ANIM_FADEIN_ARROW + ANIM_STAGGER_NEXTARROWS + ANIM_REST_ARROW;

            //// set up fade animations
            //_storyboardFadeIn = AnimationHelper.CreateStandardEasingAnimation(_layoutRoot, "Opacity", 0.0, 0.0, 1.0, 100d, 0d, false, false, new RepeatBehavior(1d));
            //_storyboardFadeOut = AnimationHelper.CreateStandardEasingAnimation(_layoutRoot, "Opacity", 1.0, 1.0, 0.0, 100d, 0d, false, false, new RepeatBehavior(1d));

        }

        #endregion

        #region UI Helpers

        #endregion
    }
}

