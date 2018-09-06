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
        LedeOnly,            // List has a header and lede-only items (i.e. interactive pages)
        HeadlineAndLede,     // List items have individual headlines (i.e. product highlights page)
        BestOf               // best of page
    }

    public sealed class List : Control
    {
        #region Private Members

        private List<ListItem> _listItems = new List<ListItem>();

        private Grid _layoutRoot = null;

        #endregion

        #region Constructor

        public List()
        {
            this.DefaultStyleKey = typeof(List);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.RenderUI();
        }

        #endregion

        #region Public Methods

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
            DependencyProperty.Register("ListStyle", typeof(ListStyles), typeof(List), new PropertyMetadata(ListStyles.LedeOnly, OnListStyleChanged));

        public ListStyles ListStyle
        {
            get { return (ListStyles)GetValue(ListStyleProperty); }
            set { SetValue(ListStyleProperty, value); }
        }

        //// ListHeadline
        //public static readonly DependencyProperty ListHeadlineProperty =
        //    DependencyProperty.Register("ListHeadline", typeof(string), typeof(List), new PropertyMetadata(String.Empty, OnListHeadlineChanged));

        //public string ListHeadline
        //{
        //    get { return (string)GetValue(ListHeadlineProperty); }
        //    set { SetValue(ListHeadlineProperty, value); }
        //}

        //// ListLede
        //public static readonly DependencyProperty ListLedeProperty =
        //    DependencyProperty.Register("ListLede", typeof(string), typeof(List), new PropertyMetadata(String.Empty, OnListLedeChanged));

        //public string ListLede
        //{
        //    get { return (string)GetValue(ListLedeProperty); }
        //    set { SetValue(ListLedeProperty, value); }
        //}

        // ListItems
        public static readonly DependencyProperty ListItemsProperty =
            DependencyProperty.Register("ListItems", typeof(List<ListItem>), typeof(List), new PropertyMetadata(null, OnListItemsChanged));

        public List<ListItem> ListItems
        {
            get { return (List<ListItem>)GetValue(ListItemsProperty); }
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

        #endregion

        #region Event Handlers

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
            double columnSpacing = 20d;

            // how many rows do we need? (items plus the header for ListHeader style)
            int rowCount = _listItems.Count + 1;

            // configure grid
            _layoutRoot.Name = "ListGrid";
            //_layoutRoot.ColumnSpacing = columnSpacing;
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnSpacing) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            for (int i = 0; i < rowCount; i++)
            {
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // based on liststyle, do we need a list header and list panel?
            switch (this.ListStyle)
            {
                case ListStyles.LedeOnly: // used for interactive pages with header followed by list of items
                    {
                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // set row spacing
                            double rowSpacing = 15d;
                            _layoutRoot.RowSpacing = rowSpacing;

                            // create icon image
                            ImageEx icon = new ImageEx()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,                                
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Center,       // center items when it's just a lede
                                VerticalContentAlignment = VerticalAlignment.Center,
                            };
                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, item.Order + 1);
                            _layoutRoot.Children.Add(icon);

                            // create the lede
                            Header lede = new Header()
                            {
                                HeadlineStyle = TextStyles.ListHeadline,
                                Headline = "",
                                LedeStyle = TextStyles.ListLede,
                                Lede = item.Lede,
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top
                            };
                            Grid.SetColumn(lede, 2);
                            Grid.SetRow(lede, item.Order + 1);
                            
                            _layoutRoot.Children.Add(lede);
                        }
                    }
                    break;

                case ListStyles.HeadlineAndLede: // Used for product highlights page wherein each item has its own lede-headline
                    {
                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // set row spacing
                            double rowSpacing = 30d;
                            _layoutRoot.RowSpacing = rowSpacing;

                            GridLength ledeHeadlineRowHeight = (item.Headline == null) ? new GridLength(0) : GridLength.Auto;

                            // create icon image
                            ImageEx icon = new ImageEx()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,                                
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Top,  // items align to top when there's a header
                                VerticalContentAlignment = VerticalAlignment.Top,
                            };

                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, item.Order + 1);
                            _layoutRoot.Children.Add(icon);

                            // create the headline (bold text) if one is provided
                            Header headline = new Header()
                            {
                                HeadlineStyle = TextStyles.ListHeadline,
                                Headline = item.Headline,
                                LedeStyle = TextStyles.ListLede,
                                Lede = item.Lede,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top
                            };
                            Grid.SetColumn(headline, 2);
                            Grid.SetRow(headline, item.Order + 1);

                            _layoutRoot.Children.Add(headline);
                        }
                    }
                    break;

                case ListStyles.BestOf: // Used for product highlights page wherein each item has its own lede-headline
                    {
                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // how wide is the text
                            double itemTextWidth = parentWidth - columnSpacing - item.IconWidth;

                            // set row spacing
                            double rowSpacing = 60d;
                            _layoutRoot.RowSpacing = rowSpacing;

                            GridLength ledeHeadlineRowHeight = (item.Headline == null) ? new GridLength(0) : GridLength.Auto;

                            // create icon image
                            ImageEx icon = new ImageEx()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Top,  // items align to top when there's a header
                                VerticalContentAlignment = VerticalAlignment.Top,
                            };

                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, item.Order + 1);
                            _layoutRoot.Children.Add(icon);

                            // create the headline (bold text) if one is provided
                            Header headline = new Header()
                            {
                                HeadlineStyle = TextStyles.ListItemHeadlineBestOf,
                                Headline = item.Headline,
                                LedeStyle = TextStyles.ListItemLedeBestOf,
                                Lede = item.Lede,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top
                            };
                            Grid.SetColumn(headline, 2);
                            Grid.SetRow(headline, item.Order + 1);

                            _layoutRoot.Children.Add(headline);
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
