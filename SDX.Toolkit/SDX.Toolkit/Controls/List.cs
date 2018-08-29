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
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Helpers;
// UNCOMMENT once telemtry is added
//using SDX.Telemetry.Services;

namespace SDX.Toolkit.Controls
{
    public enum ListStyles
    {
        ListItemHeader,       // List has a header and lede-only items (i.e. interactive pages)
        ListItemHeadline,     // List items have individual headlines (i.e. product highlights page)
    }

    public sealed class List : Control
    {
        #region Private Members

        private List<ListItem> _listItems = new List<ListItem>();

        private Grid _layoutRoot = null;
        private ListLede _listLedeHeadline = null;
        private List<ListImage> _images = new List<ListImage>();
        private List<ListLede> _listLedes = new List<ListLede>();

        private DispatcherTimer _timer = null;
        private int _dispatchCount = 0;

        #endregion

        #region Constructor

        public List()
        {
            this.DefaultStyleKey = typeof(List);
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
            // if we're not rendered yet
            if ((null == _listLedeHeadline) && (0 == _images.Count) && (0 == _listLedes.Count))
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

            if (null != _listLedeHeadline)
            {
                _listLedeHeadline.StartFadeIn();
            }

            if (null != _images)
            {
                foreach (ListImage image in _images)
                {
                    image.StartFadeIn();
                }
            }

            if (null != _listLedes)
            {
                foreach (ListLede item in _listLedes)
                {
                    item.StartFadeIn();
                }
            }
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timer) { _timer.Stop(); }

            // call the method that sets up the timer
            this.StartAnimation();
        }

        public void ResetAnimation()
        {
            if (null != _listLedeHeadline)
            {
                _listLedeHeadline.ResetAnimation();
            }

            if (null != _images)
            {
                foreach (ListImage image in _images)
                {
                    image.ResetAnimation();
                }
            }

            if (null != _listLedes)
            {
                foreach (ListLede item in _listLedes)
                {
                    item.ResetAnimation();
                }
            }
        }

        #endregion

        #region Dependency Properties

        // ListWidth
        public static readonly DependencyProperty ListWidthProperty =
            DependencyProperty.Register("ListWidth", typeof(double), typeof(List), new PropertyMetadata(500d, OnListWidthChanged));

        public double ListWidth
        {
            get { return (double)GetValue(ListWidthProperty); }
            set { SetValue(ListWidthProperty, value); }
        }

        // ListStyle
        public static readonly DependencyProperty ListStyleProperty =
            DependencyProperty.Register("ListStyle", typeof(ListStyles), typeof(List), new PropertyMetadata(ListStyles.ListItemHeader, OnListStyleChanged));

        public ListStyles ListStyle
        {
            get { return (ListStyles)GetValue(ListStyleProperty); }
            set { SetValue(ListStyleProperty, value); }
        }

        // ListLedeHeadline
        public static readonly DependencyProperty ListLedeHeadlineProperty =
            DependencyProperty.Register("ListLedeHeadline", typeof(string), typeof(List), new PropertyMetadata(String.Empty, OnListLedeHeadlineChanged));

        public string ListLedeHeadline
        {
            get { return (string)GetValue(ListLedeHeadlineProperty); }
            set { SetValue(ListLedeHeadlineProperty, value); }
        }

        // ListLede
        public static readonly DependencyProperty ListLedeProperty =
            DependencyProperty.Register("ListLede", typeof(string), typeof(List), new PropertyMetadata(String.Empty, OnListLedeChanged));

        public string ListLede
        {
            get { return (string)GetValue(ListLedeProperty); }
            set { SetValue(ListLedeProperty, value); }
        }

        // ListItems
        public static readonly DependencyProperty ListItemsProperty =
            DependencyProperty.Register("ListStyle", typeof(ListItem[]), typeof(List), new PropertyMetadata(null, OnListItemsChanged));

        public ListItem[] ListItems
        {
            get { return (ListItem[])GetValue(ListItemsProperty); }
            set
            {
                // save the value
                SetValue(ListItemsProperty, value);

                // clear our internal items list
                _listItems.Clear();

                foreach (ListItem item in value)
                {
                    _listItems.Add(item);
                }
            }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(List), new PropertyMetadata(5000d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(List), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(List), new PropertyMetadata(false, OnAutoStartChanged));

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
                this.StartAnimation();
            }
        }

        private static void OnListStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }

        private static void OnListWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }

        private static void OnListHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }
        private static void OnListLedeHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }
        private static void OnListLedeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }

        private static void OnListItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }

        private static void OnDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }

        private static void OnStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((FadeInList)d).RenderUI();
        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // try to get layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // get sizes
            //double windowWidth = PageHelper.GetViewSizeInfo().Width;
            double parentWidth = this.ListWidth;
            double rowSpacing = 15d;
            double columnSpacing = 20d;

            // how many rows do we need? (items plus the header for ListHeader style)
            int rowCount = _listItems.Count;

            // configure grid
            _layoutRoot.Name = "ListGrid";
            _layoutRoot.ColumnSpacing = columnSpacing;
            _layoutRoot.RowSpacing = rowSpacing;
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1d, GridUnitType.Star) });
            for (int i = 0; i < rowCount; i++)
            {
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // calculate item duration
            double itemDuration = 0d;
            double childDelay = 0d;

            // based on liststyle, do we need a list header and list panel?
            switch (this.ListStyle)
            {
                case ListStyles.ListItemHeader: // used for interactive pages with header followed by list of items
                    {
                        // divide duration by items to get time for each item (header + items)
                        itemDuration = this.DurationInMilliseconds / (_listItems.Count + 1);

                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // create icon image
                            ListImage icon = new ListImage()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Center,       // center items when it's just a lede
                                VerticalContentAlignment = VerticalAlignment.Center,
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, item.Order + 1);
                            _layoutRoot.Children.Add(icon);

                            // add to animation list
                            _images.Add(icon);

                            // create the lede
                            ListLede lede = new ListLede()
                            {
                                Name = String.Format("Text_{0}", item.Order),
                                LedeStyle = LedeStyles.LedeOnly,
                                Lede = item.Lede,
                                TelemetryId = item.TelemetryId,
                                Width = itemTextWidth,
                                LedeAlignment = TextAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,       // center items when it's just a lede
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(lede, 1);
                            Grid.SetRow(lede, item.Order);
                            _layoutRoot.Children.Add(lede);

                            // add to animation list
                            _listLedes.Add(lede);

                            // bump childDelay for this item
                            childDelay += itemDuration;
                        }
                    }
                    break;

                case ListStyles.ListItemHeadline: // Used for product highlights page wherein each item has its own lede-headline
                    {
                        // divide duration by items to get time for each item (items only)
                        itemDuration = this.DurationInMilliseconds / (_listItems.Count);

                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // create icon image
                            ListImage icon = new ListImage()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Top,  // items align to top when there's a header
                                VerticalContentAlignment = VerticalAlignment.Top,
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, item.Order);
                            _layoutRoot.Children.Add(icon);

                            // add to animation list
                            _images.Add(icon);

                            // create the headline (bold text)
                            ListLede ledeHeadline = new ListLede()
                            {
                                Name = String.Format("Headline_{0}", item.Order),
                                LedeStyle = LedeStyles.HeadlineAndLede,
                                LedeHeadline = item.LedeHeadline,
                                TelemetryId = item.TelemetryId,
                                Width = itemTextWidth,
                                VerticalAlignment = VerticalAlignment.Top,       // items align to top when they're a header
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(ledeHeadline, 1);
                            Grid.SetRow(ledeHeadline, item.Order);
                            _layoutRoot.Children.Add(ledeHeadline);

                            // add to animation list
                            _listLedes.Add(ledeHeadline);
                            // create the lede
                            ListLede lede = new ListLede()
                            {
                                Name = String.Format("lede_{0}", item.Order),
                                LedeStyle = LedeStyles.HeadlineAndLede,
                                Lede = item.Lede,
                                TelemetryId = item.TelemetryId,
                                Width = itemTextWidth,
                                VerticalAlignment = VerticalAlignment.Top,       // items align to top when they're a header
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(lede, 2);
                            Grid.SetRow(lede, item.Order);
                            _layoutRoot.Children.Add(lede);

                            // add to animation list
                            _listLedes.Add(lede);

                            // bump childDelay for this item
                            childDelay += itemDuration;
                        }
                    }
                    break;
            }
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}
