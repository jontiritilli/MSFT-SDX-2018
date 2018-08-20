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
    public enum ImageStyles
    {
        ShowKeyboard,
        ShowMouse
    }

    public sealed class ColorSelectorImage : Control
    {
        #region Private Constants

        private const string URI_BURGUNDY_LEFT = "ms-appx:///Assets/ColorSelectorImage/burgundy-keyboard.png";
        private const string URI_BURGUNDY_RIGHT = "ms-appx:///Assets/ColorSelectorImage/burgundy-mouse.png";
        private const string URI_COBALT_LEFT = "ms-appx:///Assets/ColorSelectorImage/cobalt-keyboard.png";
        private const string URI_COBALT_RIGHT = "ms-appx:///Assets/ColorSelectorImage/cobalt-mouse.png";
        private const string URI_BLACK_LEFT = "ms-appx:///Assets/ColorSelectorImage/black-keyboard.png";
        private const string URI_BLACK_RIGHT = "ms-appx:///Assets/ColorSelectorImage/black-mouse.png";
        private const string URI_SILVER_LEFT = "ms-appx:///Assets/ColorSelectorImage/silver-keyboard.png";
        private const string URI_SILVER_RIGHT = "ms-appx:///Assets/ColorSelectorImage/silver-mouse.png";

        private static readonly Size BOUNDS = PageHelper.GetScreenResolutionInfo();

        private static readonly double WIDTH_ORIGINAL = 1800;
        private static readonly double HEIGHT_ORIGINAL = 1200;
        private static readonly double WIDTH_IMAGE = BOUNDS.Width;
        private static readonly double HEIGHT_IMAGE = BOUNDS.Height;

        #endregion


        #region Private Members

        public Grid _layoutRoot = null;
        private Image _imageBurgundy = null;
        private Image _imageCobalt = null;
        private Image _imageBlack = null;
        private Image _imageSilver = null;

        private ColorSelector _previousColorSelector = null;

        //private Storyboard _storyboardFadeIn = null;
        //private Storyboard _storyboardFadeOut = null;

        #endregion


        #region Construction

        public ColorSelectorImage()
        {
            this.DefaultStyleKey = typeof(ColorSelectorImage);

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

        //public void StartFadeIn()
        //{
        //    // start the textblock
        //    if (null != _storyboardFadeIn)
        //    {
        //        _storyboardFadeIn.Begin();
        //    }
        //}

        //public void StartFadeOut()
        //{
        //    // start the textblock
        //    if (null != _storyboardFadeOut)
        //    {
        //        _storyboardFadeOut.Begin();
        //    }
        //}

        //public void ResetAnimation()
        //{
        //    // reset the textblock
        //    if (null != _storyboardFadeIn)
        //    {
        //        _storyboardFadeIn.Stop();
        //    }
        //    if (null != _storyboardFadeOut)
        //    {
        //        _storyboardFadeOut.Stop();
        //    }

        //    // reset opacity to starting point
        //    if (null != _layoutRoot)
        //    {
        //        _layoutRoot.Opacity = 0.0;
        //    }
        //}

        public void SetOpacity(double opacity)
        {
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }

            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = opacity;
            }
        }

        public void ForceColor(ColorSelectorColors color)
        {
            this.UpdateUI(color);
        }

        #endregion


        #region Dependency Properties

        // ColorSelector
        public static readonly DependencyProperty ColorSelectorProperty =
            DependencyProperty.Register("ColorSelector", typeof(ColorSelector), typeof(ColorSelectorImage), new PropertyMetadata(null, OnColorSelectorChanged));

        public ColorSelector ColorSelector
        {
            get { return (ColorSelector)GetValue(ColorSelectorProperty); }
            set { SetValue(ColorSelectorProperty, value); }
        }

        // ImageStyle
        public static readonly DependencyProperty ImageStyleProperty =
            DependencyProperty.Register("ImageStyle", typeof(ImageStyles), typeof(ColorSelectorImage), new PropertyMetadata(ImageStyles.ShowKeyboard, OnImageStyleChanged));

        public ImageStyles ImageStyle
        {
            get { return (ImageStyles)GetValue(ImageStyleProperty); }
            set { SetValue(ImageStyleProperty, value); }
        }

        //// DurationInMilliseconds
        //public static readonly DependencyProperty DurationInMillisecondsProperty =
        //    DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(ColorSelectorImage), new PropertyMetadata(200d, OnDurationInMillisecondsChanged));

        //public double DurationInMilliseconds
        //{
        //    get { return (double)GetValue(DurationInMillisecondsProperty); }
        //    set { SetValue(DurationInMillisecondsProperty, value); }
        //}

        //// FadeInCompletedHandler
        //public static readonly DependencyProperty FadeInCompletedHandlerProperty =
        //    DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(ColorSelectorImage), new PropertyMetadata(null));

        //public EventHandler<object> FadeInCompletedHandler
        //{
        //    get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
        //    set { SetValue(FadeInCompletedHandlerProperty, value); }
        //}

        //// FadeOutCompletedHandler
        //public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
        //    DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(ColorSelectorImage), new PropertyMetadata(null));

        //public EventHandler<object> FadeOutCompletedHandler
        //{
        //    get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
        //    set { SetValue(FadeOutCompletedHandlerProperty, value); }
        //}

        //// StaggerDelayInMilliseconds
        //public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
        //    DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(ColorSelectorImage), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        //public double StaggerDelayInMilliseconds
        //{
        //    get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
        //    set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        //}

        //// AutoStart
        //public static readonly DependencyProperty AutoStartProperty =
        //DependencyProperty.Register("AutoStart", typeof(bool), typeof(ColorSelectorImage), new PropertyMetadata(true, OnAutoStartChanged));

        //public bool AutoStart
        //{
        //    get { return (bool)GetValue(AutoStartProperty); }
        //    set { SetValue(AutoStartProperty, value); }
        //}

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (this.AutoStart)
            //{
            //    this.StartFadeIn();
            //}
        }

        private static void OnColorSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorSelectorImage colorSelectorImage)
            {
                // clean up event handler from previous colorselector
                if (null != colorSelectorImage._previousColorSelector)
                {
                    colorSelectorImage._previousColorSelector.SelectedColorChanged -= colorSelectorImage.ColorSelector_OnSelectionChanged;
                }

                // save this as the previous
                colorSelectorImage._previousColorSelector = colorSelectorImage.ColorSelector;

                // add new event handler
                if (null != colorSelectorImage.ColorSelector)
                {
                    colorSelectorImage.ColorSelector.SelectedColorChanged += colorSelectorImage.ColorSelector_OnSelectionChanged;
                }
            }
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

        //private static void OnDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}

        //private static void OnStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}

        //private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}

        private void ColorSelector_OnSelectionChanged(object sender, EventArgs e)
        {
            if (sender is ColorSelector selector)
            {
                this.UpdateUI();
            }
        }

        private static void OnImageStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a border here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // configure grid
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.Padding = new Thickness(0);
            //_layoutRoot.Opacity = 0.0;

            // create the burgundy image
            _imageBurgundy = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri((ImageStyles.ShowKeyboard == this.ImageStyle) ? URI_BURGUNDY_LEFT : URI_BURGUNDY_RIGHT), DecodePixelWidth = (int)WIDTH_IMAGE },
                Width = WIDTH_IMAGE,
                HorizontalAlignment = ((ImageStyles.ShowKeyboard == this.ImageStyle) ? HorizontalAlignment.Right : HorizontalAlignment.Left),
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.0
            };
            Grid.SetRow(_imageBurgundy, 0);
            Grid.SetColumn(_imageBurgundy, 0);
            _layoutRoot.Children.Add(_imageBurgundy);

            // create the cobalt image
            _imageCobalt = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri((ImageStyles.ShowKeyboard == this.ImageStyle) ? URI_COBALT_LEFT : URI_COBALT_RIGHT), DecodePixelWidth = (int)WIDTH_IMAGE },
                Width = WIDTH_IMAGE,
                HorizontalAlignment = ((ImageStyles.ShowKeyboard == this.ImageStyle) ? HorizontalAlignment.Right : HorizontalAlignment.Left),
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.0
            };
            Grid.SetRow(_imageCobalt, 0);
            Grid.SetColumn(_imageCobalt, 0);
            _layoutRoot.Children.Add(_imageCobalt);

            // create the black image
            _imageBlack = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri((ImageStyles.ShowKeyboard == this.ImageStyle) ? URI_BLACK_LEFT : URI_BLACK_RIGHT), DecodePixelWidth = (int)WIDTH_IMAGE },
                Width = WIDTH_IMAGE,
                HorizontalAlignment = ((ImageStyles.ShowKeyboard == this.ImageStyle) ? HorizontalAlignment.Right : HorizontalAlignment.Left),
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.0
            };
            Grid.SetRow(_imageBlack, 0);
            Grid.SetColumn(_imageBlack, 0);
            _layoutRoot.Children.Add(_imageBlack);

            // create the silver image
            _imageSilver = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri((ImageStyles.ShowKeyboard == this.ImageStyle) ? URI_SILVER_LEFT : URI_SILVER_RIGHT), DecodePixelWidth = (int)WIDTH_IMAGE },
                Width = WIDTH_IMAGE,
                HorizontalAlignment = ((ImageStyles.ShowKeyboard == this.ImageStyle) ? HorizontalAlignment.Right : HorizontalAlignment.Left),
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.0
            };
            Grid.SetRow(_imageSilver, 0);
            Grid.SetColumn(_imageSilver, 0);
            _layoutRoot.Children.Add(_imageSilver);

            //// set up animations
            //_storyboardFadeIn = AnimationHelper.CreateStandardEasingAnimation(_layoutRoot, "Opacity", 0.0, 0.0, 1.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            //_storyboardFadeOut = AnimationHelper.CreateStandardEasingAnimation(_layoutRoot, "Opacity", 1.0, 1.0, 0.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

            UpdateUI();
        }

        private void UpdateUI(ColorSelectorColors forcedColor = ColorSelectorColors.None)
        {
            if (null != this.ColorSelector)
            {
                ColorSelectorColors newColor = (ColorSelectorColors.None == forcedColor) ? this.ColorSelector.SelectedColor : forcedColor;

                // change images in two steps to avoid flashing

                // make the new color visible
                _imageBurgundy.Opacity = (ColorSelectorColors.Burgundy == newColor) ? 1.0 : _imageBurgundy.Opacity;
                _imageCobalt.Opacity = (ColorSelectorColors.Cobalt == newColor) ? 1.0 : _imageCobalt.Opacity;
                _imageBlack.Opacity = (ColorSelectorColors.Black == newColor) ? 1.0 : _imageBlack.Opacity;
                _imageSilver.Opacity = (ColorSelectorColors.Silver == newColor) ? 1.0 : _imageSilver.Opacity;

                // hide the old color
                _imageBurgundy.Opacity = (ColorSelectorColors.Burgundy != newColor) ? 0.0 : _imageBurgundy.Opacity;
                _imageCobalt.Opacity = (ColorSelectorColors.Cobalt != newColor) ? 0.0 : _imageCobalt.Opacity;
                _imageBlack.Opacity = (ColorSelectorColors.Black != newColor) ? 0.0 : _imageBlack.Opacity;
                _imageSilver.Opacity = (ColorSelectorColors.Silver != newColor) ? 0.0 : _imageSilver.Opacity;
            }
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
