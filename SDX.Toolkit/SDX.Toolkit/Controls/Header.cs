using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.System;
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
    public sealed class Header : Control
    {

        #region Private Constants

        private const string CHAR_BULLET = "•";
        private const string CHAR_NBSPACE = " ";    // MUST BE the non-breaking space

        #endregion

        #region Private Members

        private Border _layoutRoot = null;
        private Grid _layoutGrid = null;
        private TextBlockEx _headline = null;
        private TextBlockEx _lede = null;     // using TextBlock here because we need access to .Inlines and can't expose that on TextBlockEx
        private Run _ledeRun = null;
        private Hyperlink _ledeCTALink = null;
        private Run _ledeCTAText = null;

        #endregion

        #region Constructor

        public Header()
        {
            this.DefaultStyleKey = typeof(Header);
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

        public bool IsVisible()
        {
            bool visible = false;

            if (null != _headline)
            {
                visible = visible || (_headline.Opacity > 0.0);
            }

            if (null != _lede)
            {
                visible = visible || (_lede.Opacity > 0.0);
            }

            return visible;
        }

        public void SetOpacity(double opacity)
        {
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }

            if (null != _headline)
            {
                _headline.Opacity = opacity;
            }
            if (null != _lede)
            {
                _lede.Opacity = opacity;
            }
        }

        #endregion

        #region Public Properties

        public string TelemetryId { get; set; }

        #endregion

        #region Dependency Properties

        // Headline
        public static readonly DependencyProperty HeadlineProperty =
            DependencyProperty.Register("Headline", typeof(string), typeof(Header), new PropertyMetadata(null, OnHeadlineChanged));

        public string Headline
        {
            get { return (string)GetValue(HeadlineProperty); }
            set { SetValue(HeadlineProperty, value); }
        }

        // Lede
        public static readonly DependencyProperty LedeProperty =
            DependencyProperty.Register("Lede", typeof(string), typeof(Header), new PropertyMetadata(null, OnLedeChanged));

        public string Lede
        {
            get { return (string)GetValue(LedeProperty); }
            set { SetValue(LedeProperty, value); }
        }

        // CTAText
        public static readonly DependencyProperty CTATextProperty =
            DependencyProperty.Register("CTAText", typeof(string), typeof(Header), new PropertyMetadata(null, OnCTATextChanged));

        public string CTAText
        {
            get { return (string)GetValue(CTATextProperty); }
            set { SetValue(CTATextProperty, value); }
        }

        // CTAUri
        public static readonly DependencyProperty CTAUriProperty =
            DependencyProperty.Register("CTAUri", typeof(Uri), typeof(Header), new PropertyMetadata(null, OnCTAUriChanged));

        public Uri CTAUri
        {
            get { return (Uri)GetValue(CTAUriProperty); }
            set { SetValue(CTAUriProperty, value); }
        }

        // HeaderAlignment
        public static readonly DependencyProperty HeaderAlignmentProperty =
        DependencyProperty.Register("HeaderAlignment", typeof(TextAlignment), typeof(Header), new PropertyMetadata(TextAlignment.Left, OnHeaderAlignmentChanged));

        public TextAlignment HeaderAlignment
        {
            get { return (TextAlignment)GetValue(HeaderAlignmentProperty); }
            set { SetValue(HeaderAlignmentProperty, value); }
        }

        // HeadlineStyle
        public static readonly DependencyProperty HeadlineStyleProperty =
        DependencyProperty.Register("HeadlineStyle", typeof(TextStyles), typeof(Header), new PropertyMetadata(TextStyles.PageHeadline, OnHeadlineStyleChanged));


        public TextStyles HeadlineStyle
        {
            get { return (TextStyles)GetValue(HeadlineStyleProperty); }
            set { SetValue(HeadlineStyleProperty, value); }
        }

        // LedeStyle
        public static readonly DependencyProperty LedeStyleProperty =
        DependencyProperty.Register("LedeStyle", typeof(TextStyles), typeof(Header), new PropertyMetadata(TextStyles.PageLede, OnLedeStyleChanged));

        public TextStyles LedeStyle
        {
            get { return (TextStyles)GetValue(LedeStyleProperty); }
            set { SetValue(LedeStyleProperty, value); }
        }

        // CTATextStyle
        public static readonly DependencyProperty CTATextStyleProperty =
        DependencyProperty.Register("CTATextStyle", typeof(TextStyles), typeof(Header), new PropertyMetadata(TextStyles.ListItemCTAText, OnCTATextStyleChanged));

        public TextStyles CTATextStyle
        {
            get { return (TextStyles)GetValue(CTATextStyleProperty); }
            set { SetValue(CTATextStyleProperty, value); }
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnLedeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnCTATextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnCTAUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnHeadlineStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnLedeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnCTATextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private static void OnHeaderAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Header header)
            {
                header.RenderUI();
            }
        }

        private void OnOpacityChanged(object sender, double e)
        {
            double opacity = e;

            if ((null != _headline) && (null != _lede))
            {
                // correct opacity range
                opacity = Math.Max(0.0, opacity);
                opacity = Math.Min(1.0, opacity);

                // set opacity
                _headline.Opacity = opacity;
                _lede.Opacity = opacity;
            }
        }

        private void CTALink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (null != this.CTAUri)
            {
#pragma warning disable CS4014
                Launcher.LaunchUriAsync(this.CTAUri, new LauncherOptions() { DisplayApplicationPicker = false });
#pragma warning restore CS4014
            }

            if (!String.IsNullOrEmpty(this.TelemetryId))
            {
                // telemetry
                //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
            }
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Border)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // clear its children
            _layoutRoot.Child = null;

            // calculate element widths
            double gridWidth = this.Width;

            // calculate rows
            GridLength headlineRowHeight = (String.IsNullOrWhiteSpace(this.Headline)) ? new GridLength(0) : GridLength.Auto;
            GridLength ledeRowHeight = (String.IsNullOrWhiteSpace(this.Lede)) ? new GridLength(0) : new GridLength(1.0, GridUnitType.Star);
            double rowSpacing = (String.IsNullOrWhiteSpace(this.Headline) || String.IsNullOrWhiteSpace(this.Lede)) ? 0d : 10d;

            // create the grid
            _layoutGrid = new Grid()
            {
                Name = "LayoutGrid",
                Width = gridWidth,
                RowSpacing = rowSpacing
            };
            _layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = headlineRowHeight });
            _layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = ledeRowHeight });
            _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // add it to the root
            _layoutRoot.Child = _layoutGrid;

            // if we have a headline
            if (!String.IsNullOrWhiteSpace(this.Headline))
            {
                // create headline
                // =================
                _headline = new TextBlockEx()
                {
                    Name = "Headline",
                    TextAlignment = this.HeaderAlignment,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Width = gridWidth,
                    TextStyle = this.HeadlineStyle,
                };
                Grid.SetRow(_headline, 0);
                Grid.SetColumn(_headline, 0);

                // set headline binding
                _headline.SetBinding(TextBlockEx.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("Headline"), Mode = BindingMode.OneWay });

                // add to the grid
                _layoutGrid.Children.Add(_headline);

                //// update layout
                //this.UpdateLayout();
            }

            // if we have a lede
            if (!String.IsNullOrWhiteSpace(this.Lede))
            {
                // create lede
                // =================
                _lede = new TextBlockEx()
                {
                    Name = "Lede",
                    TextAlignment = this.HeaderAlignment,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Width = gridWidth,
                    TextStyle = this.LedeStyle,
                };
                Grid.SetRow(_lede, 1);
                Grid.SetColumn(_lede, 0);

                // if there's a call to action
                if ((!String.IsNullOrWhiteSpace(this.CTAText)) && (null != this.CTAUri))
                {
                    // get style for the cta run
                    Style ledeStyle = StyleHelper.GetApplicationStyle(this.LedeStyle);
                    Style ctaStyle = StyleHelper.GetApplicationStyle(this.CTATextStyle);

                    // create the lede
                    _ledeRun = new Run()
                    {
                        Text = this.Lede + CHAR_NBSPACE + CHAR_NBSPACE
                    };
                    StyleHelper.SetRunStyleFromStyle(_ledeRun, ledeStyle);
                    _lede.AddInline(_ledeRun);

                    // create the hyperlink
                    _ledeCTALink = new Hyperlink()
                    {
                        //NavigateUri = (this.CTAUri)   // can't do this because we need to catch the Click event ourselves
                    };

                    // create the text for the hyperlink
                    _ledeCTAText = new Run()
                    {
                        Text = this.CTAText.ToUpper()
                    };

                    StyleHelper.SetRunStyleFromStyle(_ledeCTAText, ctaStyle);
                    _ledeCTALink.Inlines.Add(_ledeCTAText);
                    _lede.AddInline(_ledeCTALink);

                    // add the click handler for the link
                    _ledeCTALink.Click += CTALink_Click;
                }
                else
                {
                    // no CTA, so this is simple text; set lede binding
                    _lede.SetBinding(TextBlockEx.TextProperty,
                        new Binding() { Source = this, Path = new PropertyPath("Lede"), Mode = BindingMode.OneWay });
                }

                // add to the grid
                _layoutGrid.Children.Add(_lede);
            }
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}

