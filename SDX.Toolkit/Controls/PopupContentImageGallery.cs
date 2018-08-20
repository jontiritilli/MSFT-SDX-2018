using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using Microsoft.Toolkit.Uwp.Helpers;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Services;
using SDX.Toolkit.Views;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class PopupContentImageGallery : Control
    {
        #region Constants

        private readonly Size WINDOW_BOUNDS = PageHelper.GetViewSizeInfo();

        private readonly string URI_IMAGE_CLOSE = @"ms-appx:///Assets/Universal/close-icon.png";
        //private readonly string URI_IMAGE_FORMAT = @"ms-appx:///Assets/PopupContentImageGallery/image-{0}.jpg";
        //private readonly string URI_IMAGE_1 = @"ms-appx:///Assets/PopupContentImageGallery/image-1.jpg";
        //private readonly string URI_IMAGE_2 = @"ms-appx:///Assets/PopupContentImageGallery/image-2.jpg";
        //private readonly string URI_IMAGE_3 = @"ms-appx:///Assets/PopupContentImageGallery/image-3.jpg";
        //private readonly string URI_IMAGE_4 = @"ms-appx:///Assets/PopupContentImageGallery/image-4.jpg";
        //private readonly string URI_IMAGE_5 = @"ms-appx:///Assets/PopupContentImageGallery/image-5.jpg";

        private static readonly double LEFT_HEADER = 80d;
        private static readonly double TOP_HEADER = 80d;
        private static readonly double RIGHT_HEADER = 80d;
        private static readonly double BOTTOM_HEADER = 50d;

        private static readonly double SPACE_BELOW_HEADER = 30d;

        private static readonly double SPACE_BETWEEN_DOTS = 50d;

        private static readonly double WIDTH_GALLERY_IMAGE = 1800d;
        private static readonly double WIDTH_CLOSE_ELLIPSE = 50d;
        private static readonly double WIDTH_CLOSE_IMAGE = 16d;
        private static readonly double WIDTH_DOT_ACTIVE = 15d;
        private static readonly double WIDTH_DOT_INACTIVE = 15d;

        private static readonly SolidColorBrush COLOR_CONTROL_BACKGROUND = new SolidColorBrush(Colors.Black);
        private static readonly SolidColorBrush COLOR_CLOSE_STROKE = new SolidColorBrush(Colors.DarkGray);
        private static readonly SolidColorBrush COLOR_CLOSE_FILL = new SolidColorBrush(Colors.White);
        private static readonly SolidColorBrush COLOR_DOT_STROKE = new SolidColorBrush(Colors.DarkGray);
        private static readonly SolidColorBrush COLOR_DOT_ACTIVE = new SolidColorBrush(Colors.White);
        private static readonly SolidColorBrush COLOR_DOT_INACTIVE = new SolidColorBrush(Colors.Black);

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private FadeInHeader _galleryHeader = null;
        private Button _closeButton = null;
        private Grid _buttonGrid = null;
        private FlipView _flipView = null;

        private Grid _positionDotsGrid = null;
        private List<Ellipse> _dots = new List<Ellipse>();

        private int _selectedDot = 0;
        private int _countImages = 0;

        #endregion


        #region Construction

        public PopupContentImageGallery()
        {
            this.DefaultStyleKey = typeof(PopupContentImageGallery);
            this.Loaded += OnLoaded;
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

        #endregion

        #region Dependency Properties

        #endregion

        #region Public Methods

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

            // set to the first image
            if (null != _flipView)
            {
                _flipView.SelectedIndex = 0;
            }

            // show the header
            if (null != _galleryHeader)
            {
                _galleryHeader.StartFadeIn();
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

            // hide the header
            if (null != _galleryHeader)
            {
                _galleryHeader.StartFadeOut();
            }
        }

        #endregion

        #region Dependency Properties

        // Headline
        public static readonly DependencyProperty HeadlineProperty =
            DependencyProperty.Register("Headline", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnPropertyChanged));

        public string Headline
        {
            get { return (string)GetValue(HeadlineProperty); }
            set { SetValue(HeadlineProperty, value); }
        }

        // Lede
        public static readonly DependencyProperty LedeProperty =
            DependencyProperty.Register("Lede", typeof(string), typeof(Flywheel), new PropertyMetadata(null, OnPropertyChanged));

        public string Lede
        {
            get { return (string)GetValue(LedeProperty); }
            set { SetValue(LedeProperty, value); }
        }

        #endregion

        #region Custom Events

        public delegate void ClosedEvent(object sender, EventArgs e);

        public event ClosedEvent Closed;

        private void RaiseClosedEvent(PopupContentImageGallery gallery, EventArgs e)
        {
            Closed?.Invoke(gallery, e);
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void FlipView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is FlipView flipView)
            {
                // get the selected index
                int selected = flipView.SelectedIndex;

                // if it's not the same as our current selection
                if (selected != _selectedDot)
                {
                    // un-highlight the previously selected item
                    Ellipse oldDot = _dots.ElementAt<Ellipse>(_selectedDot);
                    if (null != oldDot)
                    {
                        oldDot.Fill = COLOR_DOT_INACTIVE;
                    }

                    // highlight the currently selected item
                    Ellipse newDot = _dots.ElementAt<Ellipse>(selected);
                    if (null != newDot)
                    {
                        newDot.Fill = COLOR_DOT_ACTIVE;
                    }

                    // save the selected item index
                    _selectedDot = selected;

                    // NEW: now we should show/hide the header; visible only on selindex = 0
                    if (0 == selected)
                    {
                        if ((null != _galleryHeader) && (!_galleryHeader.IsVisible()))
                        {
                            _galleryHeader.StartFadeIn();
                        }
                    }
                    else
                    {
                        if ((null != _galleryHeader) && (_galleryHeader.IsVisible()))
                        {
                            _galleryHeader.StartFadeOut();
                        }
                    }
                }
            }
        }

        private void CloseButton_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            HandleClick();
        }


        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            HandleClick();
        }

        private void HandleClick()
        {
            // hide ourself
            this.Hide();

            // let the world know we closed
            this.RaiseClosedEvent(this, new EventArgs());
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a border here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot)
            {
                return;
            }

            // configure the grid
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.Padding = new Thickness(0);
            _layoutRoot.Background = COLOR_CONTROL_BACKGROUND;
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(TOP_HEADER) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SPACE_BELOW_HEADER) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(BOTTOM_HEADER) });

            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(LEFT_HEADER) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(RIGHT_HEADER) });

            // create the vertical style for the flipview
            Style vertical = StyleHelper.GetApplicationStyle("VerticalFlipViewStyle");

            // create the flipview
            _flipView = new FlipView()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            if (null != vertical) { _flipView.Style = vertical; };
            Grid.SetRow(_flipView, 0);
            Grid.SetRowSpan(_flipView, 5);
            Grid.SetColumn(_flipView, 0);
            Grid.SetColumnSpan(_flipView, 3);
            _layoutRoot.Children.Add(_flipView);

            // add our event handler so we can update the indicator
            _flipView.SelectionChanged += this.FlipView_OnSelectionChanged;

            // if the configuration is loaded
            if (null != ConfigurationService.Current)
            {
                // get the gallery images
                IReadOnlyList<StorageFile> imageFiles = ConfigurationService.Current.GetGalleryImageFiles();

                // if we got some
                if ((null != imageFiles) && (imageFiles.Count > 0))
                {
                    // loop through them
                    foreach (StorageFile imageFile in imageFiles)
                    {
                        // create a flipviewitem and add it
                        FlipViewItem item = new FlipViewItem()
                        {
                            Margin = new Thickness(0),
                            Padding = new Thickness(0)
                        };
                        _flipView.Items.Add(item);

                        // create the bitmap image
                        BitmapImage bitmapImage = new BitmapImage()
                        {
                            DecodePixelWidth = (int)WINDOW_BOUNDS.Width
                        };

                        // load the bitmap from the source file
                        AsyncHelper.RunSync(() => this.LoadBitmapFromFileAsync(bitmapImage, imageFile));

                        // create an image and add it
                        Image image = new Image()
                        {
                            Source = bitmapImage,
                            //Width = WIDTH_GALLERY_IMAGE,
                            Stretch = Stretch.UniformToFill
                        };
                        item.Content = image;

                        // count this image
                        _countImages++;
                    }
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
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            if (null != buttonStyle) { _closeButton.Style = buttonStyle; }
            Grid.SetRow(_closeButton, 0);
            Grid.SetColumn(_closeButton, 2);
            _layoutRoot.Children.Add(_closeButton);

            // add event handlers
            _closeButton.PointerPressed += this.CloseButton_OnPointerPressed;
            _closeButton.Click += this.CloseButton_OnClick;

            // create a grid for the position dots
            _positionDotsGrid = new Grid()
            {
                RowSpacing = SPACE_BETWEEN_DOTS,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            for (int i = 0; i < _countImages; i++)
            {
                _positionDotsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            }
            _positionDotsGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // create the position dots
            for (int i = 0; i < _countImages; i++)
            {
                Ellipse dot = new Ellipse()
                {
                    Width = (0 == i) ? WIDTH_DOT_ACTIVE : WIDTH_DOT_INACTIVE,
                    Height = (0 == i) ? WIDTH_DOT_ACTIVE : WIDTH_DOT_INACTIVE,
                    Stroke = COLOR_DOT_STROKE,
                    Fill = (0 == i) ? COLOR_DOT_ACTIVE : COLOR_DOT_INACTIVE
                };
                Grid.SetRow(dot, i);
                Grid.SetColumn(dot, 0);

                // add to the collection
                _dots.Add(dot);

                // add to the grid
                _positionDotsGrid.Children.Add(dot);
            }

            // set the currently selected dot
            _selectedDot = 0;

            // add the dots grid to layout root
            Grid.SetRow(_positionDotsGrid, 0);
            Grid.SetRowSpan(_positionDotsGrid, 5);
            Grid.SetColumn(_positionDotsGrid, 2);
            _layoutRoot.Children.Add(_positionDotsGrid);

            // create the header
            _galleryHeader = new FadeInHeader()
            {
                Name = "Headline",
                Headline = this.Headline,
                Lede = this.Lede,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 450d,
                Opacity = 1.0,
                DurationInMilliseconds = 100d,
                StaggerDelayInMilliseconds = 0d,
                AutoStart = true
            };
            Grid.SetRow(_galleryHeader, 1);
            Grid.SetColumn(_galleryHeader, 1);
            _layoutRoot.Children.Add(_galleryHeader);

            //TestHelper.AddGridCellBorders(_layoutRoot, 5, 3, Colors.Black);
        }

        #endregion


        #region UI Helpers

        private async Task LoadBitmapFromFileAsync(BitmapImage bitmapImage, StorageFile imageFile)
        {
            if ((null == bitmapImage) || (null == imageFile))
            {
                return;
            }

            // set the bitmap source on the UI thread
            bitmapImage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                // create a stream for the image file
                using (IRandomAccessStream fileStream = await imageFile.OpenReadAsync())
                {
                    // set the image source
                    bitmapImage.SetSourceAsync(fileStream);
                }
            });
        }

        #endregion


        #region Code Helpers

        #endregion

    }
}
