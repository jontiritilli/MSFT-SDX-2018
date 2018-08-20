using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;

using Windows.UI;
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
using SDX.Toolkit.Models;
using SDX.Toolkit.Views;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum ItemColors
    {
        Burgundy,
        Cobalt,
        Black,
        Silver
    }


    public sealed class PopupContentCompareGallery : Control
    {
        #region Constants

        private readonly string URI_IMAGE_CLOSE = @"ms-appx:///Assets/Universal/close-icon.png";
        private readonly string URI_DEVICE_BURGUNDY = @"ms-appx:///Assets/PopupContentCompareGallery/burgundy-straight.png";
        private readonly string URI_DEVICE_COBALT = @"ms-appx:///Assets/PopupContentCompareGallery/cobalt-straight.png";
        private readonly string URI_DEVICE_SILVER = @"ms-appx:///Assets/PopupContentCompareGallery/grey-straight.png";
        private readonly string URI_DEVICE_BLACK = @"ms-appx:///Assets/PopupContentCompareGallery/black-straight.png";

        private static readonly Size WINDOW_BOUNDS = PageHelper.GetViewSizeInfo();

        private static readonly double LEFT_HEADER = 80d;
        private static readonly double TOP_HEADER = 80d;
        private static readonly double RIGHT_HEADER = 80d;
        private static readonly double BOTTOM_HEADER = 50d;

        private static readonly double SPACE_BELOW_HEADER = 30d;

        private static readonly double LEFT_ITEM = 30d;
        private static readonly double TOP_ITEM = 10d;
        private static readonly double RIGHT_ITEM = 10d;
        private static readonly double BOTTOM_ITEM = 10d;

        private static readonly double ITEM_IMAGE_ORIGINAL_WIDTH = 1000d;
        private static readonly double ITEM_IMAGE_ORIGINAL_HEIGHT = 600d;
        private static readonly double ITEM_IMAGE_WIDTH = 200d;
        private static readonly double ITEM_IMAGE_HEIGHT = ITEM_IMAGE_WIDTH * (ITEM_IMAGE_ORIGINAL_HEIGHT) / ITEM_IMAGE_ORIGINAL_WIDTH;
        private static readonly double TITLE_ROW_HEIGHT = 60d;
        private static readonly double LINE_ROW_HEIGHT = 24d;
        private static readonly double LINE_HEIGHT = 3d;
        private static readonly double LINE_WIDTH = 100d;

        private static readonly double WIDTH_CLOSE_ELLIPSE = 50d;
        private static readonly double WIDTH_CLOSE_IMAGE = 16d;
        private static readonly SolidColorBrush COLOR_CLOSE_STROKE = new SolidColorBrush(Colors.DarkGray);
        private static readonly SolidColorBrush COLOR_CLOSE_FILL = new SolidColorBrush(Colors.White);

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private Grid _headerGrid = null;
        private Grid _itemGrid = null;
        private ScrollViewer _scrollViewer = null;
        private TextBlock _masterLegal = null;

        private Button _closeButton = null;
        private Grid _buttonGrid = null;

        private Storyboard _storyboard = null;

        private BitmapImage[] _images = new BitmapImage[4];
        private BitmapImage _closeImage = null;

        #endregion


        #region Construction

        public PopupContentCompareGallery()
        {
            this.DefaultStyleKey = typeof(PopupContentCompareGallery);

            Initialization = PreLoadImages();

            this.Loaded += OnLoaded;
        }

        private async Task PreLoadImages()
        {
            _closeImage = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_IMAGE_CLOSE), 50);

            _images[(int)ItemColors.Burgundy] = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_DEVICE_BURGUNDY), (int)ITEM_IMAGE_WIDTH);
            _images[(int)ItemColors.Cobalt] = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_DEVICE_COBALT), (int)ITEM_IMAGE_WIDTH);
            _images[(int)ItemColors.Black] = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_DEVICE_BLACK), (int)ITEM_IMAGE_WIDTH);
            _images[(int)ItemColors.Silver] = await BitmapHelper.LoadBitMapFromFileAsync(new Uri(URI_DEVICE_SILVER), (int)ITEM_IMAGE_WIDTH);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Static Methods

        #endregion

        #region Public Properties

        // used for async image loading
        public Task Initialization { get; private set; }

        #endregion

        #region Dependency Properties

        // Master
        public static readonly DependencyProperty MasterProperty =
            DependencyProperty.Register("Master", typeof(SKUConfiguration), typeof(FadeInHeader), new PropertyMetadata(null, OnPropertyChanged));

        public SKUConfiguration Master
        {
            get { return (SKUConfiguration)GetValue(MasterProperty); }
            set { SetValue(MasterProperty, value); }
        }

        // Items
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<SKUConfiguration>), typeof(FadeInHeader), new PropertyMetadata(null, OnPropertyChanged));

        public List<SKUConfiguration> Items
        {
            get { return (List<SKUConfiguration>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        #endregion

        #region Public Methods

        public void StartAnimation()
        {
            if (null != _storyboard)
            {
                _storyboard.Begin();
            }
        }

        public void ResetAnimation()
        {
            if (null != _storyboard)
            {
                _storyboard.Stop();
            }
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;

            // hide the app close
            if (null != PivotPage.Current)
            {
                PivotPage.Current.HideAppClose();
            }
            if (null != FlipViewPage.Current)
            {
                FlipViewPage.Current.HideAppClose();
            }
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;

            // show the app close
            if (null != PivotPage.Current)
            {
                PivotPage.Current.ShowAppClose();
            }
            if (null != FlipViewPage.Current)
            {
                FlipViewPage.Current.ShowAppClose();
            }
        }

        #endregion
        #region Custom Events

        public delegate void OnClosed(object sender, EventArgs e);

        public event OnClosed ClosedEvent;

        private void RaiseClosedEvent(PopupContentCompareGallery gallery, EventArgs e)
        {
            ClosedEvent?.Invoke(gallery, e);
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.RaiseClosedEvent(this, new EventArgs());

            this.Hide();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.RaiseClosedEvent(this, new EventArgs());

            this.Hide();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout root
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // no master, no work
            if (null == this.Master) { return; }

            // no items, no work
            if (null == this.Items) { return; }

            // get the header grid
            _headerGrid = (Grid)this.GetTemplateChild("HeaderGrid");

            // get the scrollviewer
            _scrollViewer = (ScrollViewer)this.GetTemplateChild("ItemScrollViewer");
            if (null != _scrollViewer)
            {
                _scrollViewer.Width = WINDOW_BOUNDS.Width * 2 / 3;
            }

            // add the master
            Grid master = RenderItemGrid(this.Master, true, ItemColors.Burgundy);    // color won't matter
            Grid.SetRow(master, 0);
            Grid.SetColumn(master, 0);
            _headerGrid.Children.Add(master);

            // add the master legal
            _masterLegal = new TextBlock()
            {
                Text = this.Master.Legal,
                Margin = new Thickness(LEFT_ITEM, TOP_ITEM, RIGHT_ITEM, BOTTOM_ITEM)
            };
            StyleHelper.SetFontCharacteristics(_masterLegal, ControlStyles.Footnote);
            Grid.SetRow(_masterLegal, 1);
            Grid.SetColumn(_masterLegal, 1);
            _layoutRoot.Children.Add(_masterLegal);
            
            // get the item grid
            _itemGrid = (Grid)this.GetTemplateChild("ItemGrid");

            // track column and color
            int column = 0;
            ItemColors color = ItemColors.Burgundy;

            // loop through the sku items
            foreach (SKUConfiguration item in this.Items)
            {
                // add a column for this item
                _itemGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Auto) });

                // create the item and add to grid
                Grid itemGrid = RenderItemGrid(item, false, color);
                Grid.SetRow(itemGrid, 0);
                Grid.SetColumn(itemGrid, column);
                _itemGrid.Children.Add(itemGrid);

                // increment column and color
                column++;

                if (ItemColors.Silver == color)
                {
                    color = ItemColors.Burgundy;
                }
                else
                {
                    color++;
                }
            }

            // create the close button grid
            _buttonGrid = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };

            // add the ellipse to the grid
            Ellipse closeEllipse = new Ellipse()
            {
                Width = WIDTH_CLOSE_ELLIPSE,
                Height = WIDTH_CLOSE_ELLIPSE,
                Stroke = COLOR_CLOSE_STROKE,
                Fill = COLOR_CLOSE_FILL,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(closeEllipse, 0);
            Grid.SetColumn(closeEllipse, 0);
            _buttonGrid.Children.Add(closeEllipse);

            // add the close X
            Image closeImage = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_IMAGE_CLOSE), DecodePixelWidth = (int)WIDTH_CLOSE_IMAGE },
                Width = WIDTH_CLOSE_IMAGE,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(closeImage, 0);
            Grid.SetColumn(closeImage, 0);
            _buttonGrid.Children.Add(closeImage);

            // get the button style
            Style buttonStyle = StyleHelper.GetApplicationStyle("XCloseButton");

            // create the close button
            _closeButton = new Button()
            {
                Content = _buttonGrid,
                Margin = new Thickness(0, 0, 30, 0),    // need right margin because right has zero padding in layoutRoot
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            if (null != buttonStyle) { _closeButton.Style = buttonStyle; }
            Grid.SetRow(_closeButton, 0);
            Grid.SetColumn(_closeButton, 1);
            _layoutRoot.Children.Add(_closeButton);

            // add event handlers
            _closeButton.PointerPressed += this.CloseButton_OnPointerPressed;
            _closeButton.Click += this.CloseButton_OnClick;
        }

        #endregion


        #region UI Helpers

        private Grid RenderItemGrid(SKUConfiguration item, bool isMaster, ItemColors deviceColor)
        {
            // create the grid
            Grid itemGrid = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };

            // add column
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // add image row or master row (blank)
            itemGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TOP_HEADER + ITEM_IMAGE_HEIGHT) });

            // add the line row
            itemGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(LINE_ROW_HEIGHT) });

            // add the item title row
            itemGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TITLE_ROW_HEIGHT) });

            // add remaining rows
            for (int i = 0; i < 8; i++)
            {
                itemGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }

            // add a spacer row at the bottom
            itemGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            // create the image row
            if (!isMaster)
            {
                Image image = new Image()
                {
                    Width = ITEM_IMAGE_WIDTH,
                    Height = ITEM_IMAGE_HEIGHT,
                    Margin = new Thickness(LEFT_ITEM, TOP_ITEM, RIGHT_ITEM, BOTTOM_ITEM),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Source = _images[(int)deviceColor]
                };
                Grid.SetRow(image, 0);
                Grid.SetColumn(image, 0);
                itemGrid.Children.Add(image);
            }

            // create the master title or item line/title
            if (isMaster)
            {
                TextBlock masterHeader = new TextBlock()
                {
                    Text = item.Title,
                    Margin = new Thickness(0, TOP_ITEM, 30d, BOTTOM_ITEM),
                    Padding = new Thickness(0)
                };
                StyleHelper.SetFontCharacteristics(masterHeader, ControlStyles.CompareHeader);
                Grid.SetRow(masterHeader, 1);
                Grid.SetRowSpan(masterHeader, 2);
                Grid.SetColumn(masterHeader, 0);
                itemGrid.Children.Add(masterHeader);
            }
            else
            {
                // create the line
                Line line = new Line()
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = LINE_WIDTH,
                    Y2 = 0,
                    Margin = new Thickness(LEFT_ITEM, TOP_ITEM, RIGHT_ITEM, BOTTOM_ITEM),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = LINE_HEIGHT
                };
                Grid.SetRow(line, 1);
                Grid.SetColumn(line, 0);
                itemGrid.Children.Add(line);

                // create the item title
                TextBlock itemTitle = new TextBlock()
                {
                    Text = item.Title,
                    Margin = new Thickness(LEFT_ITEM, TOP_ITEM, RIGHT_ITEM, BOTTOM_ITEM),
                    Padding = new Thickness(0)
                };
                StyleHelper.SetFontCharacteristics(itemTitle, ControlStyles.CompareTitle);
                Grid.SetRow(itemTitle, 2);
                Grid.SetColumn(itemTitle, 0);
                itemGrid.Children.Add(itemTitle);
            }

            // display
            itemGrid.Children.Add(RenderItemCell(item.Display, 3, 0, true, ControlStyles.CompareText));

            // processor
            itemGrid.Children.Add(RenderItemCell(item.Processor, 4, 0, false, ControlStyles.CompareText));

            // memory
            itemGrid.Children.Add(RenderItemCell(item.Memory, 5, 0, true, ControlStyles.CompareText));

            // storage
            itemGrid.Children.Add(RenderItemCell(item.Storage, 6, 0, false, ControlStyles.CompareText));

            // graphics
            itemGrid.Children.Add(RenderItemCell(item.Graphics, 7, 0, true, ControlStyles.CompareText));

            // os
            itemGrid.Children.Add(RenderItemCell(item.OS, 8, 0, false, ControlStyles.CompareText));

            // battery
            itemGrid.Children.Add(RenderItemCell(item.Battery, 9, 0, true, ControlStyles.CompareText));

            // wifi
            itemGrid.Children.Add(RenderItemCell(item.WiFi, 10, 0, false, ControlStyles.CompareText));

            // spacer
            itemGrid.Children.Add(RenderItemCell(String.Empty, 11, 0, false, ControlStyles.CompareText));

            return itemGrid;
        }

        public Border RenderItemCell(String text, int row, int column, bool isHighlighted, ControlStyles style)
        {
            Border itemBorder = new Border()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            if (isHighlighted)
            {
                itemBorder.Background = new SolidColorBrush(Colors.White) { Opacity = StyleHelper.PopupBackgroundOpacity };
            }
            Grid.SetRow(itemBorder, row);
            Grid.SetColumn(itemBorder, column);

            TextBlock itemTextBlock = new TextBlock()
            {
                Text = text,
                Margin = new Thickness(LEFT_ITEM, TOP_ITEM, RIGHT_ITEM, BOTTOM_ITEM),
                Padding = new Thickness(0)
            };
            StyleHelper.SetFontCharacteristics(itemTextBlock, style);
            itemBorder.Child = itemTextBlock;

            return itemBorder;
        }

        #endregion


        #region Code Helpers

        #endregion
    }
}
