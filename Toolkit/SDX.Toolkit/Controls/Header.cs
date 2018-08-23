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
    public enum HeaderStyles
    {
        HeaderOnly,         // shows the headline only in normal style
        HeaderAndLede,      // shows headline/lede in normal style
        ListHeader,         // shows headline/lede in list style
        ListItemLedeOnly,   // shows lede in list style
        FastChargePopup     // shows headline/lede in popup text style
    }

    public sealed class Header : Control
    {

        #region Private Constants

        private const string CHAR_BULLET = "•";
        private const string CHAR_NBSPACE = " ";    // MUST BE the non-breaking space

        #endregion

        #region Private Members

        private Border _layoutRoot = null;
        private Grid _layoutGrid = null;
        private TextBlock _headline = null;
        private TextBlock _lede = null;
        private Run _ledeText = null;
        private Hyperlink _ledeCTALink = null;
        private Run _ledeCTAText = null;

        private Storyboard _storyboardFadeInHeadline = null;
        private Storyboard _storyboardFadeOutHeadline = null;

        private Storyboard _storyboardFadeInLede = null;
        private Storyboard _storyboardFadeOutLede = null;

        private DispatcherTimer _timer = null;
        private int _dispatchCount = 0;

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

        public void StartFadeIn()
        {
            // if we're not rendered yet; note: need to use AND here because we don't always get both
            // a headline and a lede; some styles create only one or the other.
            if ((null == _storyboardFadeInHeadline) && (null == _storyboardFadeInLede))
            {
                // inc the counter
                _dispatchCount++;

                // limit the number of times we do this
                if (_dispatchCount < 10)
                {
                    // create a timer
                    if (null == _timer)
                    {
                        _timer = new DispatcherTimer()
                        {
                            Interval = TimeSpan.FromMilliseconds(500d)
                        };
                        _timer.Tick += DispatcherTimer_Tick;
                    }

                    // start it
                    _timer.Start();
                }

                // return
                return;
            }

            // start the headline
            if (null != _storyboardFadeInHeadline)
            {
                _storyboardFadeInHeadline.Begin();
            }

            // start the lede
            if (null != _storyboardFadeInLede)
            {
                _storyboardFadeInLede.Begin();
            }
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timer) { _timer.Stop(); }

            // call the method that sets up the timer
            this.StartFadeIn();
        }

        public void StartFadeOut()
        {
            // start the headline
            if (null != _storyboardFadeOutHeadline)
            {
                _storyboardFadeOutHeadline.Begin();
            }

            // start the lede
            if (null != _storyboardFadeOutLede)
            {
                _storyboardFadeOutLede.Begin();
            }
        }

        public void ResetAnimation()
        {
            // reset the headline
            if (null != _storyboardFadeInHeadline)
            {
                _storyboardFadeInHeadline.Stop();
            }
            if (null != _storyboardFadeOutHeadline)
            {
                _storyboardFadeOutHeadline.Stop();
            }

            // reset the lede
            if (null != _storyboardFadeInLede)
            {
                _storyboardFadeInLede.Stop();
            }
            if (null != _storyboardFadeOutLede)
            {
                _storyboardFadeOutLede.Stop();
            }

            // reset opacity to starting point
            if (null != _headline)
            {
                _headline.Opacity = 0.0;
            }
            if (null != _lede)
            {
                _lede.Opacity = 0.0;
            }
        }

        public void SetOpacity(double opacity)
        {
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }

            if ((null != _headline) && (null != _lede))
            {
                _headline.Opacity = opacity;
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

        // HeaderStyle
        public static readonly DependencyProperty HeaderStyleProperty =
        DependencyProperty.Register("HeaderStyle", typeof(HeaderStyles), typeof(Header), new PropertyMetadata(HeaderStyles.HeaderAndLede, OnAutoStartChanged));

        public HeaderStyles HeaderStyle
        {
            get { return (HeaderStyles)GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }

        // HeaderAlignment
        public static readonly DependencyProperty HeaderAlignmentProperty =
        DependencyProperty.Register("HeaderAlignment", typeof(TextAlignment), typeof(Header), new PropertyMetadata(TextAlignment.Left, OnAutoStartChanged));

        public TextAlignment HeaderAlignment
        {
            get { return (TextAlignment)GetValue(HeaderAlignmentProperty); }
            set { SetValue(HeaderAlignmentProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(Header), new PropertyMetadata(2000d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // FadeInCompletedHandler
        public static readonly DependencyProperty FadeInCompletedHandlerProperty =
            DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(Header), new PropertyMetadata(null));

        public EventHandler<object> FadeInCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
            set { SetValue(FadeInCompletedHandlerProperty, value); }
        }

        // FadeOutCompletedHandler
        public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
            DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(Header), new PropertyMetadata(null));

        public EventHandler<object> FadeOutCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
            set { SetValue(FadeOutCompletedHandlerProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(Header), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(Header), new PropertyMetadata(true, OnAutoStartChanged));

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
            }
        }

        private static void OnHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnLedeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }


        private static void OnCTATextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnCTAUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

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
            // get the layoutroot
            _layoutRoot = (Border)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // clear its children
            _layoutRoot.Child = null;

            // calculate element widths
            double leftColPercent = 0.7;
            double gridWidth = this.Width;
            //double headlineWidth = this.Width;
            //double ledeWidth = this.Width * leftColPercent;

            // calculate rows
            GridLength headlineRowHeight = (HeaderStyles.ListItemLedeOnly == this.HeaderStyle) ? new GridLength(0) : GridLength.Auto;
            GridLength ledeRowHeight = (HeaderStyles.HeaderOnly == this.HeaderStyle) ? new GridLength(0) : new GridLength(1.0, GridUnitType.Star);
            double rowSpacing = ((HeaderStyles.ListItemLedeOnly == this.HeaderStyle) || (HeaderStyles.HeaderOnly == this.HeaderStyle)) ? 0d : 10d;

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
            //_layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(leftColPercent, GridUnitType.Star) });
            //_layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((1 - leftColPercent), GridUnitType.Star) });

            // add it to the root
            _layoutRoot.Child = _layoutGrid;

            // calculate item delay
            double itemDelay = (HeaderStyles.ListItemLedeOnly == this.HeaderStyle) ? 0d : this.DurationInMilliseconds / 2;

            // calculate styles 
            ControlStyles headlineControlStyle;
            ControlStyles ledeControlStyle;
            switch (this.HeaderStyle)
            {
                case HeaderStyles.HeaderAndLede:
                case HeaderStyles.HeaderOnly:
                default:
                    headlineControlStyle = ControlStyles.Headline;
                    ledeControlStyle = ControlStyles.Lede;
                    break;

                case HeaderStyles.ListHeader:
                case HeaderStyles.ListItemLedeOnly:
                    headlineControlStyle = ControlStyles.ListHeadline;
                    ledeControlStyle = ControlStyles.ListLede;
                    break;

                case HeaderStyles.FastChargePopup:
                    headlineControlStyle = ControlStyles.FastChargeHeader;
                    ledeControlStyle = ControlStyles.FastChargeLede;
                    break;
            }

            // if we're in lede-only mode, don't create the headline
            if (HeaderStyles.ListItemLedeOnly != this.HeaderStyle)
            {
                // create headline
                // =================
                _headline = new TextBlock()
                {
                    Name = "Headline",
                    HorizontalTextAlignment = this.HeaderAlignment,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Width = gridWidth,
                    //Width = headlineWidth,
                    Opacity = 0d
                };
                StyleHelper.SetFontCharacteristics(_headline, headlineControlStyle);
                Grid.SetRow(_headline, 0);
                Grid.SetColumn(_headline, 0);
                //Grid.SetColumn(_headline, 0);
                //Grid.SetColumnSpan(_headline, 2);

                // set headline binding
                _headline.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("Headline"), Mode = BindingMode.OneWay });

                // add to the grid
                _layoutGrid.Children.Add(_headline);

                // set up animations
                //_storyboardFadeInHeadline = SetupAnimation(_headline, this.DurationInMilliseconds, 0d, this.StaggerDelayInMilliseconds);
                _storyboardFadeInHeadline = AnimationHelper.CreateEasingAnimation(_headline, "Opacity", 0.0, 0.0, 1.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _storyboardFadeOutHeadline = AnimationHelper.CreateEasingAnimation(_headline, "Opacity", 1.0, 1.0, 0.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                // update layout
                this.UpdateLayout();
            }

            // if we're in headline-only mode, don't create the lede
            if (HeaderStyles.HeaderOnly != this.HeaderStyle)
            {
                // create lede
                // =================
                _lede = new TextBlock()
                {
                    Name = "Lede",
                    HorizontalTextAlignment = this.HeaderAlignment,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Width = gridWidth,
                    //MaxWidth = gridWidth,
                    //Width = ledeWidth,
                    Opacity = 0d
                };
                StyleHelper.SetFontCharacteristics(_lede, ledeControlStyle);
                Grid.SetRow(_lede, 1);
                Grid.SetColumn(_lede, 0);
                //Grid.SetColumn(_lede, 0);
                //Grid.SetColumnSpan(_lede, 1);

                // create the runs
                _ledeText = new Run()
                {
                    Text = this.Lede + CHAR_NBSPACE + CHAR_NBSPACE
                };
                StyleHelper.SetFontCharacteristics(_ledeText, ledeControlStyle);
                _lede.Inlines.Add(_ledeText);

                // if there's a call to action
                if ((!String.IsNullOrEmpty(this.CTAText)) && (null != this.CTAUri))
                {
                    // create the hyperlink
                    _ledeCTALink = new Hyperlink()
                    {
                        //NavigateUri = (this.CTAUri)
                    };

                    // create the text for the hyperlink
                    _ledeCTAText = new Run()
                    {
                        Text = this.CTAText.ToUpper()
                    };
                    StyleHelper.SetFontCharacteristics(_ledeCTAText, ControlStyles.CTAText);
                    _ledeCTALink.Inlines.Add(_ledeCTAText);
                    _lede.Inlines.Add(_ledeCTALink);

                    // add the click handler for the link
                    _ledeCTALink.Click += CTALink_Click;
                }

                // can't do this anymore now that we have a hyperlink in here
                //// set lede binding
                //_lede.SetBinding(TextBlock.TextProperty,
                //    new Binding() { Source = this, Path = new PropertyPath("Lede"), Mode = BindingMode.OneWay });

                // add to the grid
                _layoutGrid.Children.Add(_lede);

                // set up animation
                //_storyboardFadeInLede = SetupAnimation(_lede, this.DurationInMilliseconds, itemDelay, this.StaggerDelayInMilliseconds);
                _storyboardFadeInLede = AnimationHelper.CreateEasingAnimationWithNotify(_lede, this.FadeInCompletedHandler, "Opacity", 0.0, 0.0, 1.0, null, null, this.DurationInMilliseconds, itemDelay + this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _storyboardFadeOutLede = AnimationHelper.CreateEasingAnimationWithNotify(_lede, this.FadeOutCompletedHandler, "Opacity", 1.0, 1.0, 0.0, null, null, this.DurationInMilliseconds, itemDelay + this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            }
        }

        private void CTALink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            if (null != this.CTAUri)
            {
                Launcher.LaunchUriAsync(this.CTAUri, new LauncherOptions() { DisplayApplicationPicker = false });
            }

            if (!String.IsNullOrEmpty(this.TelemetryId))
            {
                // telemetry
                //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
            }
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}

