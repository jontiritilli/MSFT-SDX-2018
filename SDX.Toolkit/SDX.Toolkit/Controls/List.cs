﻿using System;
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
        BestOf,               // best of page
        Compare,
        Specs
    }

    public sealed class List : Control, IAnimate
    {
        #region Private Members

        private List<ListItem> _listItems = new List<ListItem>();

        private List<UIElement> _specListElements = new List<UIElement>();

        private Grid _layoutRoot = null;

        private Grid ColGrid = null;

        private Grid RowGrid = null;

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

        // AnimationDirection
        public static readonly DependencyProperty PageEntranceDirectionProperty =
        DependencyProperty.Register("PageEntranceDirection", typeof(AnimationDirection), typeof(List), new PropertyMetadata(AnimationDirection.Left));

        public AnimationDirection PageEntranceDirection
        {
            get { return (AnimationDirection)GetValue(PageEntranceDirectionProperty); }
            set { SetValue(PageEntranceDirectionProperty, value); }
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

            // generic, but doesn't work for "best of list". Will have to be set seperate in the bestof case below
            double ColumnSpacing = StyleHelper.GetApplicationDouble(LayoutSizes.ListColumnSpacerWidth);
            double RowSpacing = StyleHelper.GetApplicationDouble(LayoutSizes.ListRowSpacerHeight);

            // configure grid
            _layoutRoot.Name = "ListGrid";
            _layoutRoot.Margin = new Thickness(0);

            // based on liststyle, do we need a list header and list panel?
            switch (this.ListStyle)
            {
                case ListStyles.LedeOnly: // used for interactive pages with header followed by list of items
                    {
                        // add columns
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ColumnSpacing) });
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                        // create a counter to know when we reach the end of list
                        int Index = 0;
                        int ListLength = _listItems.Count() - 1;

                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // get correct row to add content, don't want to add to a spacer row
                            int RowToAdd = Index == 0 ? item.Order : item.Order + Index;

                            // create the row for content
                            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                            // if it's not the last row, create a spacer row
                            if (Index < ListLength)
                            {
                                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RowSpacing) });
                            }

                            // create icon image and add to grid
                            ImageEx icon = new ImageEx()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,                                
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Center,       // center items when it's just a lede
                                VerticalContentAlignment = VerticalAlignment.Center,
                                PageEntranceDirection = this.PageEntranceDirection
                            };
                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, RowToAdd);
                            _layoutRoot.Children.Add(icon);

                            // create the lede and add to grid
                            Header lede = new Header()
                            {
                                LedeStyle = TextStyles.ListItemLedePenTouch,
                                Lede = item.Lede,
                                Width = StyleHelper.GetApplicationDouble("AccessoriesPenListTextWidth"),
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                HeadlineOpacity = 0,
                                LedeOpacity = 0,
                                PageEntranceDirection = this.PageEntranceDirection
                            };
                            Grid.SetColumn(lede, 2);
                            Grid.SetRow(lede, RowToAdd);
                            _layoutRoot.Children.Add(lede);

                            // increment the counter
                            Index++;
                        }
                    }
                    break;

                case ListStyles.HeadlineAndLede: // Used for product highlights page wherein each item has its own lede-headline
                    {
                        // add columns
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ColumnSpacing) });
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                        // create a counter to know when we reach the end of list
                        int Index = 0;
                        int ListLength = _listItems.Count() - 1;

                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // get correct row to add content, don't want to add to a spacer row
                            int RowToAdd = Index == 0 ? item.Order : item.Order + Index;

                            // create the row for content
                            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                            // if it's not the last row, create a spacer row
                            if (Index < ListLength)
                            {
                                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(RowSpacing) });
                            }

                            GridLength ledeHeadlineRowHeight = (item.Headline == null) ? new GridLength(0) : GridLength.Auto;

                            // create icon image
                            ImageEx icon = new ImageEx()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,                                
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Top,  // items align to top when there's a header
                                VerticalContentAlignment = VerticalAlignment.Top,
                                PageEntranceDirection = this.PageEntranceDirection
                            };

                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, RowToAdd);

                            _layoutRoot.Children.Add(icon);

                            // create the headline (bold text) if one is provided
                            Header headline = new Header()
                            {
                                HeadlineStyle = TextStyles.ListHeadline,
                                Headline = item.Headline,
                                LedeStyle = TextStyles.ListLede,
                                Lede = item.Lede,
                                Width = this.Width,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                HeadlineOpacity = 0,
                                LedeOpacity = 0,
                                PageEntranceDirection = this.PageEntranceDirection
                                
                            };
                            Grid.SetColumn(headline, 2);
                            Grid.SetRow(headline, RowToAdd);

                            _layoutRoot.Children.Add(headline);

                            // increment index
                            Index++;
                        }
                    }
                    break;

                case ListStyles.BestOf: // Used for product highlights page wherein each item has its own lede-headline
                    {
                        // create a grid of grids. 2 columns and 2 or 3 rows base don # of units. star space them
                        // and each inner grid handles its icon/text/spacer
                        double rowPercent = (_listItems.Count > 4 ? 0.333 : 0.5);
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });                        
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
                        _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowPercent, GridUnitType.Star) });
                        _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowPercent, GridUnitType.Star) });
                        if (_listItems.Count > 4)
                        {//5th item handling
                            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowPercent, GridUnitType.Star) });
                        }
                        // create a counter to know when we reach the end of list
                        int Index = 0;
                        int ListLength = _listItems.Count() - 1;
                        int rowPos = 0;
                        int colPos = 0;
                        double ListTextWidth = StyleHelper.GetApplicationDouble("BestOfMicrosoftListTextWidth");

                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // get correct row to add content, don't want to add to a spacer row
                            Grid grid = new Grid();
                            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.BestOfMicrosoftColumnSpacerWidth)) });
                            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                            int RowToAdd = Index == 0 ? item.Order : item.Order + Index;

                            // create the row for content
                            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                            // create icon image
                            ImageEx icon = new ImageEx()
                            {
                                Name = String.Format("Icon_{0}", item.Order),
                                ImageSource = item.IconPath,
                                ImageWidth = item.IconWidth,
                                VerticalAlignment = VerticalAlignment.Top,  // items align to top when there's a header
                                VerticalContentAlignment = VerticalAlignment.Top,
                                PageEntranceDirection = this.PageEntranceDirection,
                                Opacity = 0.0
                            };

                            Grid.SetColumn(icon, 0);
                            Grid.SetRow(icon, RowToAdd);
                            grid.Children.Add(icon);

                            // create the headline (bold text) if one is provided
                            Header headline = new Header()
                            {
                                HeadlineStyle = TextStyles.ListItemHeadlineBestOf,
                                Headline = item.Headline,
                                LedeStyle = TextStyles.ListItemLedeBestOf,
                                Lede = item.Lede,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                Width = ListTextWidth,
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                PageEntranceDirection = this.PageEntranceDirection,
                                HeadlineOpacity = 0,
                                LedeOpacity = 0
                            };
                            Grid.SetColumn(headline, 2);
                            Grid.SetRow(headline, RowToAdd);

                            grid.Children.Add(headline);
                            Grid.SetRow(grid, rowPos);
                            Grid.SetColumn(grid, colPos);                            
                            _layoutRoot.Children.Add(grid);
                            // increment index
                            Index++;
                            rowPos = (Index < 2 ? 0 : (Index < 4 ? 1 : 2));
                            colPos = (colPos > 0 ? 0 : 1);

                        }
                    }
                    break;

                case ListStyles.Compare: // Used for product highlights page wherein each item has its own lede-headline
                    {
                        // add columns
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(StyleHelper.GetApplicationDouble("CompareColumnSpacerWidth")) });
                        _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                        // create a counter to know when we reach the end of list
                        int Index = 0;
                        int ListLength = _listItems.Count() - 1;

                        double ListTextWidth = StyleHelper.GetApplicationDouble("CompareListTextWidth");

                        // create the list items and add to the grid
                        foreach (ListItem item in _listItems)
                        {
                            // get correct row to add content, don't want to add to a spacer row
                            int RowToAdd = Index == 0 ? item.Order : item.Order + Index;

                            // set row spacing
                            double rowSpacing = StyleHelper.GetApplicationDouble("CompareRowSpacerHeight");

                            // create the row for content
                            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                            // if it's not the last row, create a spacer row
                            if (Index < ListLength)
                            {
                                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowSpacing) });
                            }

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
                            Grid.SetRow(icon, RowToAdd);
                            _layoutRoot.Children.Add(icon);

                            // create the headline (bold text) if one is provided
                            Header headline = new Header()
                            {
                                HeadlineStyle = TextStyles.ListItemHeadlineCompare,
                                Headline = item.Headline,
                                LedeStyle = TextStyles.ListItemLedeCompare,
                                Lede = item.Lede,
                                CTAText = item.CTAText,
                                CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                Width = ListTextWidth,
                                HeaderAlignment = TextAlignment.Left,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top
                            };
                            Grid.SetColumn(headline, 2);
                            Grid.SetRow(headline, RowToAdd);

                            _layoutRoot.Children.Add(headline);

                            // increment index
                            Index++;
                        }
                    }
                    break;
                case ListStyles.Specs:
                    {
                        _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                        // set row spacing
                        double rowSpacing = StyleHelper.GetApplicationDouble("CompareRowSpacerHeight");

                        // create a counter to know when we reach the end of list
                        int Index = 0;

                        int ListLength = _listItems.Count() - 1;
                        var ListContentCount = new List<int> { 4, 2, 3 };
                        double ListTextWidth = StyleHelper.GetApplicationDouble("SpecsListTextWidth");
                        for (int col = 0; col < 3; col++)
                        {
                            int rowIndex = 0;
                            ColGrid = new Grid()
                            {
                                Name = "ColumnGrid",
                            };

                            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                            // create the list items and add to the grid
                            for (int row = 0; row < ListContentCount[col]; row++)
                            {
                                ColGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                                if(Index < ListLength)
                                {
                                    ColGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowSpacing) });
                                }

                                RowGrid = new Grid()
                                {
                                    Name = "RowGrid"
                                };

                                ListItem item = _listItems[Index];

                                int RowToAdd = rowIndex == 0 ? 0 : row + rowIndex;
                                // get correct row to add content, don't want to add to a spacer row

                                RowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                                RowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(StyleHelper.GetApplicationDouble("SpecsListColumnSpacerWidth")) });

                                RowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                                RowGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                                // create icon image
                                ImageEx icon = new ImageEx()
                                {
                                    Name = String.Format("Icon_{0}", item.Order),
                                    ImageSource = item.IconPath,
                                    ImageWidth = item.IconWidth,
                                    VerticalAlignment = VerticalAlignment.Top,  // items align to top when there's a header
                                    VerticalContentAlignment = VerticalAlignment.Top,
                                    PageEntranceDirection = this.PageEntranceDirection,
                                    Opacity = 1
                                };

                                Grid.SetColumn(icon, 0);
                                Grid.SetRow(icon, 0);
                                RowGrid.Children.Add(icon);

                                // create the headline (bold text) if one is provided
                                Header headline = new Header()
                                {
                                    HeadlineStyle = TextStyles.SpecItemHeadline,
                                    Headline = item.Headline,
                                    LedeStyle = TextStyles.SpecItemLede,
                                    Lede = item.Lede,
                                    IsRowSpacingActive = false,
                                    CTAText = item.CTAText,
                                    CTAUri = String.IsNullOrWhiteSpace(item.CTAUri) ? null : new Uri(item.CTAUri),
                                    Width = ListTextWidth,
                                    HeaderAlignment = TextAlignment.Left,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    PageEntranceDirection = this.PageEntranceDirection,
                                    Opacity = 1
                                };

                                Grid.SetColumn(headline, 2);
                                Grid.SetRow(headline, 0);
                                RowGrid.Children.Add(headline);

                                _specListElements.Add(RowGrid);

                                Grid.SetColumn(RowGrid, col);
                                Grid.SetRow(RowGrid, RowToAdd);
                                ColGrid.Children.Add(RowGrid);

                                // increment index
                                Index++;
                                rowIndex++;
                            }
                            Grid.SetColumn(ColGrid, col);
                            Grid.SetRow(ColGrid, 0);
                            _layoutRoot.Children.Add(ColGrid);

                            rowIndex = 0;
                        }
                    }
                    break;
            }
        }

        public bool HasAnimateChildren()
        {
            return true;
        }

        public bool HasPageEntranceAnimation()
        {
            return true;
        }

        public AnimationDirection Direction()
        {
            return PageEntranceDirection;
        }

        public List<UIElement> AnimatableChildren()
        {
            List<UIElement> Items = new List<UIElement>();
            UIElement uiElement;
            if (null != _layoutRoot)
            {
                switch (this.ListStyle)
                {
                    case ListStyles.BestOf: // used for interactive pages with header followed by list of items
                        {
                            foreach (UIElement item in _layoutRoot.Children)
                            {
                                if (item is IAnimate)
                                {
                                    uiElement = (UIElement)item;
                                    Items.Add(uiElement);
                                }
                                else if (item is Grid)
                                {
                                    Grid grid = (Grid)item;
                                    foreach (UIElement gridElement in grid.Children)
                                    {
                                        if (gridElement is IAnimate)
                                        {
                                            uiElement = (UIElement)gridElement;
                                            Items.Add(uiElement);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case ListStyles.Compare:
                    case ListStyles.HeadlineAndLede:
                    case ListStyles.LedeOnly:
                        {
                            foreach (UIElement item in _layoutRoot.Children)
                            {
                                if (item is IAnimate)
                                {
                                    uiElement = (UIElement)item;
                                    Items.Add(uiElement);
                                }
                            }
                        }
                        break;
                    case ListStyles.Specs:
                        return _specListElements;
                }
            }


            
            return Items;
        }

        public bool HasPageEntranceTranslation()
        {
            return true;    
        }

        #endregion
    }
}
