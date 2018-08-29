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
    public enum LedeStyles
    {
        HeadlineAndLede,   // shows the headline only in normal style
        LedeOnly,           // shows headline/lede in normal style
    }

    public sealed class ListLede : Control
    {

        #region Private Constants

        private const string CHAR_BULLET = "•";
        private const string CHAR_NBSPACE = " ";    // MUST BE the non-breaking space

        #endregion

        #region Private Members

        private Border _layoutRoot = null;
        private Grid _layoutGrid = null;
        private TextBlock _ledeHeadline = null;
        private TextBlock _lede = null;
        private Run _ledeHeadlineText = null;
        private Run _ledeText = null;

        private Storyboard _storyboardFadeInLedeHeadline = null;
        private Storyboard _storyboardFadeOutLedeHeadline = null;

        private Storyboard _storyboardFadeInLede = null;
        private Storyboard _storyboardFadeOutLede = null;

        private DispatcherTimer _timer = null;
        private int _dispatchCount = 0;

        #endregion

        #region Constructor

        public ListLede()
        {
            this.DefaultStyleKey = typeof(ListLede);
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

            if (null != _ledeHeadline)
            {
                visible = visible || (_ledeHeadline.Opacity > 0.0);
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
            if ((null == _storyboardFadeInLedeHeadline) && (null == _storyboardFadeInLede))
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
            if (null != _storyboardFadeInLedeHeadline)
            {
                _storyboardFadeInLedeHeadline.Begin();
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
            if (null != _storyboardFadeOutLedeHeadline)
            {
                _storyboardFadeOutLedeHeadline.Begin();
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
            if (null != _storyboardFadeInLedeHeadline)
            {
                _storyboardFadeInLedeHeadline.Stop();
            }
            if (null != _storyboardFadeOutLedeHeadline)
            {
                _storyboardFadeOutLedeHeadline.Stop();
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
            if (null != _ledeHeadline)
            {
                _ledeHeadline.Opacity = 0.0;
            }
            if (null != _lede)
            {
                _lede.Opacity = 0.0;
            }
        }

        public void SetOpacity(double opacity)
        {
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }

            if ((null != _ledeHeadline) && (null != _lede))
            {
                _ledeHeadline.Opacity = opacity;
                _lede.Opacity = opacity;
            }
        }

        #endregion

        #region Public Properties

        public string TelemetryId { get; set; }

        #endregion

        #region Dependency Properties

        // LedeHeadline
        public static readonly DependencyProperty LedeHeadlineProperty =
            DependencyProperty.Register("LedeHeadline", typeof(string), typeof(ListLede), new PropertyMetadata(null, OnLedeHeadlineChanged));

        public string LedeHeadline
        {
            get { return (string)GetValue(LedeHeadlineProperty); }
            set { SetValue(LedeHeadlineProperty, value); }
        }

        // Lede
        public static readonly DependencyProperty LedeProperty =
            DependencyProperty.Register("Lede", typeof(string), typeof(Header), new PropertyMetadata(null, OnLedeChanged));

        public string Lede
        {
            get { return (string)GetValue(LedeProperty); }
            set { SetValue(LedeProperty, value); }
        }
        
        // ListLedeStyle
        public static readonly DependencyProperty LedeStyleProperty =
        DependencyProperty.Register("ListLedeStyle", typeof(LedeStyles), typeof(Header), new PropertyMetadata(LedeStyles.HeadlineAndLede, OnAutoStartChanged));

        public LedeStyles LedeStyle
        {
            get { return (LedeStyles)GetValue(LedeStyleProperty); }
            set { SetValue(LedeStyleProperty, value); }
        }

        // LedeAlignment
        public static readonly DependencyProperty HeaderAlignmentProperty =
        DependencyProperty.Register("LedeAlignment", typeof(TextAlignment), typeof(Header), new PropertyMetadata(TextAlignment.Left, OnAutoStartChanged));

        public TextAlignment LedeAlignment
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

        private static void OnLedeHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            if ((null != _ledeHeadline) && (null != _lede))
            {
                // correct opacity range
                opacity = Math.Max(0.0, opacity);
                opacity = Math.Min(1.0, opacity);

                // set opacity
                _ledeHeadline.Opacity = opacity;
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
            double gridWidth = this.Width;
            //double headlineWidth = this.Width;
            //double ledeWidth = this.Width * leftColPercent;

            // calculate rows
            GridLength headlineRowHeight = (LedeStyles.HeadlineAndLede == this.LedeStyle) ? new GridLength(0) : GridLength.Auto;
            GridLength ledeRowHeight = (LedeStyles.LedeOnly == this.LedeStyle) ? new GridLength(0) : new GridLength(1.0, GridUnitType.Star);
            double rowSpacing = (LedeStyles.LedeOnly == this.LedeStyle) ? 0d : 10d;

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
            double itemDelay = (LedeStyles.LedeOnly == this.LedeStyle) ? 0d : this.DurationInMilliseconds / 2;

            // set styles 
            ControlStyles ledeHeadlineControlStyle;
            ControlStyles ledeControlStyle;
            switch (this.LedeStyle)
            {
                case LedeStyles.HeadlineAndLede:
                    ledeHeadlineControlStyle = ControlStyles.ListHeadline;
                    ledeControlStyle = ControlStyles.ListLede;
                    break;
                case LedeStyles.LedeOnly:
                    ledeHeadlineControlStyle = ControlStyles.ListHeadline;
                    ledeControlStyle = ControlStyles.ListLede;
                    break;
                default:
                    ledeHeadlineControlStyle = ControlStyles.ListHeadline;
                    ledeControlStyle = ControlStyles.ListLede;
                    break;
            }

            // if we're in lede-only mode, don't create the headline
            if (LedeStyles.LedeOnly != this.LedeStyle)
            {
                // create headline
                // =================
                _ledeHeadline = new TextBlock()
                {
                    Name = "Headline",
                    HorizontalTextAlignment = this.LedeAlignment,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    Width = gridWidth,
                    //Width = headlineWidth,
                    Opacity = 0d
                };
                StyleHelper.SetFontCharacteristics(_ledeHeadline, ledeHeadlineControlStyle);
                Grid.SetRow(_ledeHeadline, 0);
                Grid.SetColumn(_ledeHeadline, 0);
                //Grid.SetColumn(_headline, 0);

                // set headline binding
                _ledeHeadline.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("LedeHeadline"), Mode = BindingMode.OneWay });

                // add to the grid
                _layoutGrid.Children.Add(_ledeHeadline);

                // set up animations
                //_storyboardFadeInHeadline = SetupAnimation(_headline, this.DurationInMilliseconds, 0d, this.StaggerDelayInMilliseconds);
                _storyboardFadeInLedeHeadline = AnimationHelper.CreateEasingAnimation(_ledeHeadline, "Opacity", 0.0, 0.0, 1.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
                _storyboardFadeOutLedeHeadline = AnimationHelper.CreateEasingAnimation(_ledeHeadline, "Opacity", 1.0, 1.0, 0.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

                // update layout
                this.UpdateLayout();
            }

            // create lede
            // =================
            _lede = new TextBlock()
            {
                Name = "Lede",
                HorizontalTextAlignment = this.LedeAlignment,
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

            // add to the grid
            _layoutGrid.Children.Add(_lede);

            // set up animation
            //_storyboardFadeInLede = SetupAnimation(_lede, this.DurationInMilliseconds, itemDelay, this.StaggerDelayInMilliseconds);
            _storyboardFadeInLede = AnimationHelper.CreateEasingAnimationWithNotify(_lede, this.FadeInCompletedHandler, "Opacity", 0.0, 0.0, 1.0, null, null, this.DurationInMilliseconds, itemDelay + this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            _storyboardFadeOutLede = AnimationHelper.CreateEasingAnimationWithNotify(_lede, this.FadeOutCompletedHandler, "Opacity", 1.0, 1.0, 0.0, null, null, this.DurationInMilliseconds, itemDelay + this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
        }
        
        #endregion

        #region UI Helpers

        #endregion
    }
}

