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
using SDX.Toolkit.Services;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum FadeInListStyles
    {
        ListHeader,         // shows a header and lede for the list itself, items have text only
        ListItemHeader      // shows no header or lede for the list, list items have header and lede instead
    }

    public sealed class FadeInList : Control
    {
        #region Private Members

        private List<FadeInListListItem> _listItems = new List<FadeInListListItem>();

        private Grid _layoutRoot = null;
        private FadeInHeader _listHeader = null;
        private List<FadeInImage> _images = new List<FadeInImage>();
        private List<FadeInHeader> _headers = new List<FadeInHeader>();

        private DispatcherTimer _timer = null;
        private int _dispatchCount = 0;

        #endregion

        #region Constructor

        public FadeInList()
        {
            this.DefaultStyleKey = typeof(FadeInList);
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
            if ((null == _listHeader) && (0 == _images.Count) && (0 == _headers.Count))
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

            if (null != _listHeader)
            {
                _listHeader.StartFadeIn();
            }

            if (null != _images)
            {
                foreach (FadeInImage image in _images)
                {
                    image.StartFadeIn();
                }
            }

            if (null != _headers)
            {
                foreach (FadeInHeader item in _headers)
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
            if (null != _listHeader)
            {
                _listHeader.ResetAnimation();
            }

            if (null != _images)
            {
                foreach (FadeInImage image in _images)
                {
                    image.ResetAnimation();
                }
            }

            if (null != _headers)
            {
                foreach (FadeInHeader item in _headers)
                {
                    item.ResetAnimation();
                }
            }
        }

        #endregion

        #region Dependency Properties

        // ListWidth
        public static readonly DependencyProperty ListWidthProperty =
            DependencyProperty.Register("ListWidth", typeof(double), typeof(FadeInList), new PropertyMetadata(500d, OnListWidthChanged));

        public double ListWidth
        {
            get { return (double)GetValue(ListWidthProperty); }
            set { SetValue(ListWidthProperty, value); }
        }

        // ListStyle
        public static readonly DependencyProperty ListStyleProperty =
            DependencyProperty.Register("ListStyle", typeof(FadeInListStyles), typeof(FadeInList), new PropertyMetadata(FadeInListStyles.ListHeader, OnListStyleChanged));

        public FadeInListStyles ListStyle
        {
            get { return (FadeInListStyles)GetValue(ListStyleProperty); }
            set { SetValue(ListStyleProperty, value); }
        }

        // ListHeadline
        public static readonly DependencyProperty ListHeadlineProperty =
            DependencyProperty.Register("ListHeadline", typeof(string), typeof(FadeInList), new PropertyMetadata(String.Empty, OnListHeadlineChanged));

        public string ListHeadline
        {
            get { return (string)GetValue(ListHeadlineProperty); }
            set { SetValue(ListHeadlineProperty, value); }
        }

        // ListLede
        public static readonly DependencyProperty ListLedeProperty =
            DependencyProperty.Register("ListLede", typeof(string), typeof(FadeInList), new PropertyMetadata(String.Empty, OnListLedeChanged));

        public string ListLede
        {
            get { return (string)GetValue(ListLedeProperty); }
            set { SetValue(ListLedeProperty, value); }
        }

        // ListItems
        public static readonly DependencyProperty ListItemsProperty =
            DependencyProperty.Register("ListStyle", typeof(FadeInListListItem[]), typeof(FadeInList), new PropertyMetadata(null, OnListItemsChanged));

        public FadeInListListItem[] ListItems
        {
            get { return (FadeInListListItem[])GetValue(ListItemsProperty); }
            set
            {
                // save the value
                SetValue(ListItemsProperty, value);

                // clear our internal items list
                _listItems.Clear();

                foreach (FadeInListListItem item in value)
                {
                    _listItems.Add(item);
                }
            }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(FadeInList), new PropertyMetadata(5000d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(FadeInList), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(FadeInList), new PropertyMetadata(false, OnAutoStartChanged));

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
            int rowCount = _listItems.Count + (FadeInListStyles.ListHeader == this.ListStyle ? 1 : 0);

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

            //TestHelper.AddGridCellBorders(_layoutRoot, rowCount, 2, Colors.AliceBlue);

            // calculate item duration
            double itemDuration = 0d;
            double childDelay = 0d;

            // based on liststyle, do we need a list header and list panel?
            switch (this.ListStyle)
            {
                case FadeInListStyles.ListHeader:
                    {
                        // divide duration by items to get time for each item (header + items)
                        itemDuration = this.DurationInMilliseconds / (_listItems.Count + 1);

                        // need a list header
                        _listHeader = new FadeInHeader()
                        {
                            Name = "ListHeader",
                            Headline = this.ListHeadline,
                            Lede = this.ListLede,
                            Width = parentWidth, // TODAY
                            DurationInMilliseconds = itemDuration,
                            StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                            AutoStart = false
                        };
                        Grid.SetColumnSpan(_listHeader, 2);
                        Grid.SetRow(_listHeader, 0);
                        _layoutRoot.Children.Add(_listHeader);

                        // bump childDelay (we have one child above)
                        childDelay += itemDuration;

                        // create the list items and add to the grid
                        foreach (FadeInListListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // create icon image
                            FadeInImage icon = new FadeInImage()
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
                            FadeInHeader lede = new FadeInHeader()
                            {
                                Name = String.Format("Text_{0}", item.Order),
                                HeaderStyle = FadeInHeaderStyles.ListItemLedeOnly,
                                Lede = item.Lede,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                TelemetryId = item.TelemetryId,
                                Width = itemTextWidth,
                                HeaderAlignment = TextAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,       // center items when it's just a lede
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(lede, 1);
                            Grid.SetRow(lede, item.Order + 1);
                            _layoutRoot.Children.Add(lede);

                            // add to animation list
                            _headers.Add(lede);

                            // bump childDelay for this item
                            childDelay += itemDuration;
                        }
                    }
                    break;

                case FadeInListStyles.ListItemHeader:
                    {
                        // divide duration by items to get time for each item (items only)
                        itemDuration = this.DurationInMilliseconds / (_listItems.Count);

                        // create the list items and add to the grid
                        foreach (FadeInListListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // create icon image
                            FadeInImage icon = new FadeInImage()
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

                            //Size WINDOW_BOUNDS = PageHelper.GetViewSizeInfo();
                            //string temp = String.Format("CW={0}, PW={1}, IW={2};", WINDOW_BOUNDS.Width, parentWidth, itemTextWidth);

                            // create the lede
                            FadeInHeader lede = new FadeInHeader()
                            {
                                Name = String.Format("Header_{0}", item.Order),
                                HeaderStyle = FadeInHeaderStyles.ListHeader,
                                Headline = item.Headline,
                                Lede = item.Lede,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                TelemetryId = item.TelemetryId,
                                Width = itemTextWidth,
                                VerticalAlignment = VerticalAlignment.Top,       // items align to top when they're a header
                                DurationInMilliseconds = itemDuration,
                                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds + childDelay,
                                AutoStart = false
                            };
                            Grid.SetColumn(lede, 1);
                            Grid.SetRow(lede, item.Order);
                            _layoutRoot.Children.Add(lede);

                            // add to animation list
                            _headers.Add(lede);

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
