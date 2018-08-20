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


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class RadiatingPenTouch : Control
    {
        private const double ENTRANCE_SIZE = 60d;

        private const double RADIATE_SIZE_START = 60d;
        private const double RADIATE_SIZE_END = 88d;
        private const double RADIATE_OPACITY_DEFAULT = 0.0;
        private const double RADIATE_OPACITY_START = 0.6;
        private const double RADIATE_OPACITY_END = 0.4;

        private const double GRID_WIDTH = 95d;
        private const double GRID_HEIGHT = 95d;

        #region Private Members

        // ui elements to track
        Grid _layoutRoot = null;
        Ellipse _entranceEllipse = null;
        Ellipse _radiateEllipse = null;

        Storyboard _entranceStoryboard = null;
        Storyboard _radiatingStoryboardX = null;
        Storyboard _radiatingStoryboardY = null;
        Storyboard _radiatingStoryboardOpacity = null;

        #endregion

        #region Construction/Destruction

        public RadiatingPenTouch()
        {
            this.DefaultStyleKey = typeof(RadiatingPenTouch);

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

        // AnimationRepeat
        public static readonly DependencyProperty AnimationRepeatProperty =
            DependencyProperty.Register("AnimationRepeat", typeof(RepeatBehavior), typeof(RadiatingButton), new PropertyMetadata(new RepeatBehavior(1d)));

        public RepeatBehavior AnimationRepeat
        {
            get => (RepeatBehavior)GetValue(AnimationRepeatProperty);
            set => SetValue(AnimationRepeatProperty, value);
        }

        // EntranceDurationInMilliseconds
        public static readonly DependencyProperty EntranceDurationInMillisecondsProperty =
            DependencyProperty.Register("EntranceDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(200d));

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
            DependencyProperty.Register("RadiateDurationInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(200d));

        public double RadiateDurationInMilliseconds
        {
            get { return (double)GetValue(RadiateDurationInMillisecondsProperty); }
            set { SetValue(RadiateDurationInMillisecondsProperty, value); }
        }

        // RadiateStaggerDelayInMilliseconds
        public static readonly DependencyProperty RadiateStaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("RadiateStaggerDelayInMilliseconds", typeof(double), typeof(RadiatingButton), new PropertyMetadata(0d));

        public double RadiateStaggerDelayInMilliseconds
        {
            get { return (double)GetValue(RadiateStaggerDelayInMillisecondsProperty); }
            set { SetValue(RadiateStaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true));

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public void StartRadiateAnimation()
        {
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
                _radiateEllipse.Opacity = RADIATE_OPACITY_DEFAULT;
            }
        }

        public void StartEntranceAnimation()
        {
            if (null != _entranceStoryboard)
            {
                _entranceStoryboard.Begin();
            }
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

        public delegate void OnClicked(object sender, EventArgs e);

        public event OnClicked ClickedEvent;

        private void RaiseClickedEvent(RadiatingButton radiatingButton, EventArgs e)
        {
            ClickedEvent?.Invoke(radiatingButton, e);
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // we can't work without it, so return if that failed
            if (null == _layoutRoot) { return; }

            // configure the grid
            _layoutRoot.Name = this.Name + "Grid";
            _layoutRoot.Width = GRID_WIDTH;
            _layoutRoot.Height = GRID_HEIGHT;
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.Padding = new Thickness(0);
            _layoutRoot.RowSpacing = 0d;
            _layoutRoot.ColumnSpacing = 0d;
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_HEIGHT) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(GRID_WIDTH) });

            // create the radiating ellipse
            _radiateEllipse = new Ellipse()
            {
                Name = this.Name + "RadiateEllipse",
                Width = RADIATE_SIZE_START,
                Height = RADIATE_SIZE_START,
                Fill = new SolidColorBrush(Colors.White),
                Opacity = 0d,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0)
            };
            Grid.SetRow(_radiateEllipse, 0);
            Grid.SetColumn(_radiateEllipse, 0);
            _layoutRoot.Children.Add(_radiateEllipse);

            // create storyboards
            _radiatingStoryboardX = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Width", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            _radiatingStoryboardY = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Height", RADIATE_SIZE_START, RADIATE_SIZE_START, RADIATE_SIZE_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            _radiatingStoryboardOpacity = AnimationHelper.CreateInOutAnimation(_radiateEllipse, "Opacity", 0.0, RADIATE_OPACITY_START, RADIATE_OPACITY_END, this.RadiateDurationInMilliseconds, this.RadiateDurationInMilliseconds, this.RadiateStaggerDelayInMilliseconds, 0.0, this.RadiateStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

            // create the entrance ellipse
            _entranceEllipse = new Ellipse()
            {
                Name = this.Name + "EntranceEllipse",
                Width = ENTRANCE_SIZE,
                Height = ENTRANCE_SIZE,
                Fill = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0)
            };
            Grid.SetRow(_entranceEllipse, 0);
            Grid.SetColumn(_entranceEllipse, 0);
            _layoutRoot.Children.Add(_entranceEllipse);

            // create storyboard
            _entranceStoryboard = AnimationHelper.CreateEasingAnimation(_entranceEllipse, "Opacity", 0.0, 0.0, 1.0, this.EntranceDurationInMilliseconds, this.EntranceStaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
        }

        #endregion

        #region Code Helpers

        #endregion

        #region UI Helpers

        #endregion
    }
}
