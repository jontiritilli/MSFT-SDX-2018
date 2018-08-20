using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Services;
using System.Globalization;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum ColorSelectorColors
    {
        None,
        Burgundy,
        Cobalt,
        Black,
        Silver
    }

    public sealed class ColorSelector : Control
    {
        #region Private Constants

        private const string URI_BURGUNDY_SELECTED = "ms-appx:///Assets/ColorSelector/burgundy-selected.png";
        private const string URI_BURGUNDY_NOTSEL = "ms-appx:///Assets/ColorSelector/burgundy.png";
        private const string URI_COBALT_SELECTED = "ms-appx:///Assets/ColorSelector/cobalt-selected.png";
        private const string URI_COBALT_NOTSEL = "ms-appx:///Assets/ColorSelector/cobalt.png";
        private const string URI_BLACK_SELECTED = "ms-appx:///Assets/ColorSelector/black-selected.png";
        private const string URI_BLACK_NOTSEL = "ms-appx:///Assets/ColorSelector/black.png";
        private const string URI_SILVER_SELECTED = "ms-appx:///Assets/ColorSelector/silver-selected.png";
        private const string URI_SILVER_NOTSEL = "ms-appx:///Assets/ColorSelector/silver.png";

        private const double WIDTH_GRID = 340d;
        private const double WIDTH_GRID_COLUMNSPACING = 10d;
        private const double WIDTH_IMAGE_SELECTED = 62d;
        private const double WIDTH_IMAGE_NOTSEL = 50d;

        #endregion


        #region Private Members

        private Grid _layoutRoot = null;
        private Button _buttonBurgundy = null;
        private Grid _gridBurgundy = null;
        private Image _imageBurgundySelected = null;
        private Image _imageBurgundyNotSelected = null;
        private Button _buttonCobalt = null;
        private Grid _gridCobalt = null;
        private Image _imageCobaltSelected = null;
        private Image _imageCobaltNotSelected = null;
        private Button _buttonBlack = null;
        private Grid _gridBlack = null;
        private Image _imageBlackSelected = null;
        private Image _imageBlackNotSelected = null;
        private Button _buttonSilver = null;
        private Grid _gridSilver = null;
        private Image _imageSilverSelected = null;
        private Image _imageSilverNotSelected = null;


        private Storyboard _storyboardFadeIn = null;
        private Storyboard _storyboardFadeOut = null;

        private ColorSelectorColors _syncColor = ColorSelectorColors.None;

        #endregion


        #region Construction

        public ColorSelector()
        {
            this.DefaultStyleKey = typeof(ColorSelector);

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

        public void StartFadeIn()
        {
            // start the textblock
            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Begin();
            }
        }

        public void StartFadeOut()
        {
            // start the textblock
            if (null != _storyboardFadeOut)
            {
                _storyboardFadeOut.Begin();
            }
        }

        public void ResetAnimation()
        {
            // reset the textblock
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
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }

            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = opacity;
            }
        }

        public void SyncColor(ColorSelectorColors color)
        {
            // save this color so the handler will know we changed it
            _syncColor = color;

            this.SelectedColor = color;
        }

        #endregion

        #region Public Properties

        public string TelemetryId { get; set; }

        #endregion

        #region Dependency Properties

        // SelectedColor
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(ColorSelectorColors), typeof(ColorSelector), new PropertyMetadata(ColorSelectorColors.Burgundy, OnSelectedColorChanged));

        public ColorSelectorColors SelectedColor
        {
            get { return (ColorSelectorColors)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(ColorSelector), new PropertyMetadata(200d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // FadeInCompletedHandler
        public static readonly DependencyProperty FadeInCompletedHandlerProperty =
            DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(ColorSelector), new PropertyMetadata(null));

        public EventHandler<object> FadeInCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
            set { SetValue(FadeInCompletedHandlerProperty, value); }
        }

        // FadeOutCompletedHandler
        public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
            DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(ColorSelector), new PropertyMetadata(null));

        public EventHandler<object> FadeOutCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
            set { SetValue(FadeOutCompletedHandlerProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(ColorSelector), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(ColorSelector), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Custom Events

        public delegate void OnSelectedColorChangedEvent(object sender, EventArgs e);

        public event OnSelectedColorChangedEvent SelectedColorChanged;

        private void RaiseSelectedColorChangedEvent(ColorSelector colorSelector, EventArgs e)
        {
            SelectedColorChanged?.Invoke(colorSelector, e);
        }

        private void RaiseSelectedColorChangedEvent(ColorSelector colorSelector)
        {
            this.RaiseSelectedColorChangedEvent(colorSelector, new EventArgs());
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartFadeIn();
            }
        }

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorSelector selector)
            {
                // update the UI with the new color
                selector.UpdateUI();

                // only raise the event if this was NOT a sync change
                if (selector.SelectedColor != selector._syncColor)
                {
                    // reset the sync color
                    selector._syncColor = ColorSelectorColors.None;

                    // raise the selected color changed event
                    selector.RaiseSelectedColorChangedEvent(selector);
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

        private static void OnDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ColorSelector_BurgundyClick(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = ColorSelectorColors.Burgundy;

            // telemetry
            TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        private void ColorSelector_CobaltClick(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = ColorSelectorColors.Cobalt;

            // telemetry
            TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        private void ColorSelector_BlackClick(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = ColorSelectorColors.Black;

            // telemetry
            TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        private void ColorSelector_SilverClick(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = ColorSelectorColors.Silver;

            // telemetry
            TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }


        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a canvas here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // update the grid
            _layoutRoot.Name = "ColorSelectorGrid";
            _layoutRoot.Opacity = 0.0;
            _layoutRoot.Width = WIDTH_GRID;
            _layoutRoot.ColumnSpacing = WIDTH_GRID_COLUMNSPACING;
            _layoutRoot.RowDefinitions.Add(new RowDefinition());
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.25, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.25, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.25, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.25, GridUnitType.Star) });

            // create the button style
            Style buttonStyle = StyleHelper.GetApplicationStyle("ColorSelectorButton");

            // burgundy
            _gridBurgundy = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };

            // burgundy not selected image
            _imageBurgundyNotSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_BURGUNDY_NOTSEL), DecodePixelWidth = (int)WIDTH_IMAGE_NOTSEL },
                Width = WIDTH_IMAGE_NOTSEL,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageBurgundyNotSelected, 0);
            Grid.SetColumn(_imageBurgundyNotSelected, 0);
            _gridBurgundy.Children.Add(_imageBurgundyNotSelected);

            // burgundy selected image
            _imageBurgundySelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_BURGUNDY_SELECTED), DecodePixelWidth = (int)WIDTH_IMAGE_SELECTED },
                Width = WIDTH_IMAGE_SELECTED,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageBurgundySelected, 0);
            Grid.SetColumn(_imageBurgundySelected, 0);
            _gridBurgundy.Children.Add(_imageBurgundySelected);

            // burgundy button
            _buttonBurgundy = new Button()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = _gridBurgundy
            };
            if (null != buttonStyle) { _buttonBurgundy.Style = buttonStyle; };
            _buttonBurgundy.Click += ColorSelector_BurgundyClick;
            Grid.SetRow(_buttonBurgundy, 0);
            Grid.SetColumn(_buttonBurgundy, 0);
            _layoutRoot.Children.Add(_buttonBurgundy);


            // cobalt
            _gridCobalt = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };

            // cobalt not selected image
            _imageCobaltNotSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_COBALT_NOTSEL), DecodePixelWidth = (int)WIDTH_IMAGE_NOTSEL },
                Width = WIDTH_IMAGE_NOTSEL,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageCobaltNotSelected, 0);
            Grid.SetColumn(_imageCobaltNotSelected, 0);
            _gridCobalt.Children.Add(_imageCobaltNotSelected);

            // cobalt selected image
            _imageCobaltSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_COBALT_SELECTED), DecodePixelWidth = (int)WIDTH_IMAGE_SELECTED },
                Width = WIDTH_IMAGE_SELECTED,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageCobaltSelected, 0);
            Grid.SetColumn(_imageCobaltSelected, 0);
            _gridCobalt.Children.Add(_imageCobaltSelected);

            // cobalt button
            _buttonCobalt = new Button()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = _gridCobalt
            };
            if (null != buttonStyle) { _buttonCobalt.Style = buttonStyle; };
            _buttonCobalt.Click += ColorSelector_CobaltClick;
            Grid.SetRow(_buttonCobalt, 0);
            Grid.SetColumn(_buttonCobalt, 1);
            _layoutRoot.Children.Add(_buttonCobalt);


            // black
            _gridBlack = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };

            // black not selected image
            _imageBlackNotSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_BLACK_NOTSEL), DecodePixelWidth = (int)WIDTH_IMAGE_NOTSEL },
                Width = WIDTH_IMAGE_NOTSEL,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageBlackNotSelected, 0);
            Grid.SetColumn(_imageBlackNotSelected, 0);
            _gridBlack.Children.Add(_imageBlackNotSelected);

            // black selected image
            _imageBlackSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_BLACK_SELECTED), DecodePixelWidth = (int)WIDTH_IMAGE_SELECTED },
                Width = WIDTH_IMAGE_SELECTED,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageBlackSelected, 0);
            Grid.SetColumn(_imageBlackSelected, 0);
            _gridBlack.Children.Add(_imageBlackSelected);

            // black button
            _buttonBlack = new Button()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = _gridBlack
            };
            if (null != buttonStyle) { _buttonBlack.Style = buttonStyle; };
            _buttonBlack.Click += ColorSelector_BlackClick;
            Grid.SetRow(_buttonBlack, 0);
            Grid.SetColumn(_buttonBlack, 2);
            _layoutRoot.Children.Add(_buttonBlack);


            // silver
            _gridSilver = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };

            // silver not selected image
            _imageSilverNotSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_SILVER_NOTSEL), DecodePixelWidth = (int)WIDTH_IMAGE_NOTSEL },
                Width = WIDTH_IMAGE_NOTSEL,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageSilverNotSelected, 0);
            Grid.SetColumn(_imageSilverNotSelected, 0);
            _gridSilver.Children.Add(_imageSilverNotSelected);

            // silver selected image
            _imageSilverSelected = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_SILVER_SELECTED), DecodePixelWidth = (int)WIDTH_IMAGE_SELECTED },
                Width = WIDTH_IMAGE_SELECTED,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 1.0
            };
            Grid.SetRow(_imageSilverSelected, 0);
            Grid.SetColumn(_imageSilverSelected, 0);
            _gridSilver.Children.Add(_imageSilverSelected);

            // silver button
            _buttonSilver = new Button()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = _gridSilver
            };
            if (null != buttonStyle) { _buttonSilver.Style = buttonStyle; };
            _buttonSilver.Click += ColorSelector_SilverClick;
            Grid.SetRow(_buttonSilver, 0);
            Grid.SetColumn(_buttonSilver, 3);
            _layoutRoot.Children.Add(_buttonSilver);

            // set up animations
            _storyboardFadeIn = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 0.0, 0.0, 1.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            _storyboardFadeOut = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 1.0, 1.0, 0.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

            this.UpdateUI();
        }

        private void UpdateUI()
        {
            // test the first image and return if it hasn't been created
            if (null == _imageBurgundySelected) { return; }

            // set the opacity
            if (ColorSelectorColors.Burgundy == this.SelectedColor)
            {
                _imageBurgundySelected.Opacity = 1.0;
                _imageBurgundyNotSelected.Opacity = 0.0;
            }
            else
            {
                _imageBurgundySelected.Opacity = 0.0;
                _imageBurgundyNotSelected.Opacity = 1.0;
            }

            if (ColorSelectorColors.Cobalt == this.SelectedColor)
            {
                _imageCobaltSelected.Opacity = 1.0;
                _imageCobaltNotSelected.Opacity = 0.0;
            }
            else
            {
                _imageCobaltSelected.Opacity = 0.0;
                _imageCobaltNotSelected.Opacity = 1.0;
            }

            if (ColorSelectorColors.Black == this.SelectedColor)
            {
                _imageBlackSelected.Opacity = 1.0;
                _imageBlackNotSelected.Opacity = 0.0;
            }
            else
            {
                _imageBlackSelected.Opacity = 0.0;
                _imageBlackNotSelected.Opacity = 1.0;
            }

            if (ColorSelectorColors.Silver == this.SelectedColor)
            {
                _imageSilverSelected.Opacity = 1.0;
                _imageSilverNotSelected.Opacity = 0.0;
            }
            else
            {
                _imageSilverSelected.Opacity = 0.0;
                _imageSilverNotSelected.Opacity = 1.0;
            }
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
