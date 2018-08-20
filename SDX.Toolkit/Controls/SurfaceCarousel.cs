using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using Microsoft.Toolkit.Uwp.UI.Controls;


using SDX.Toolkit.Helpers;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum CarouselItems
    {
        Lingo,
        Laptop,
        Pro,
        Book,
        Studio
    }

    public class SurfaceCarouselEventArgs : EventArgs
    {
        #region Private Members

        #endregion

        #region Constructors

        public SurfaceCarouselEventArgs()
        {

        }


        public SurfaceCarouselEventArgs(CarouselItems item)
        {
            this.CarouselItem = item;
        }

        #endregion

        #region Public Properties

        public CarouselItems CarouselItem { get; set; }

        #endregion
    }

    public sealed class SurfaceCarouselImage
    {
        public string Item { get; set; }
    }

    public sealed class SurfaceCarousel : Control
    {
        #region Private Constants

        private const string URI_LINGO = "ms-appx:///Assets/SurfaceCarousel/lingo.png";
        private const string URI_LAPTOP = "ms-appx:///Assets/SurfaceCarousel/laptop.png";
        private const string URI_PRO = "ms-appx:///Assets/SurfaceCarousel/pro.png";
        private const string URI_BOOK = "ms-appx:///Assets/SurfaceCarousel/book.png";
        private const string URI_STUDIO = "ms-appx:///Assets/SurfaceCarousel/studio.png";

        private static readonly Rect WINDOW_BOUNDS = ApplicationView.GetForCurrentView().VisibleBounds;

        private static readonly double WIDTH_CONTROL = WINDOW_BOUNDS.Width;
        private static readonly double HEIGHT_CONTROL = WINDOW_BOUNDS.Height * 0.4;

        private static readonly double WIDTH_ORIGINAL = 3592d;
        private static readonly double HEIGHT_ORIGINAL = 2296d;
        private static readonly double WIDTH_IMAGE = WINDOW_BOUNDS.Width * 0.4;
        private static readonly double HEIGHT_IMAGE = WIDTH_IMAGE * HEIGHT_ORIGINAL / WIDTH_ORIGINAL;

        private static readonly double CAROUSEL_DEPTH = WINDOW_BOUNDS.Width * 0.25;

        #endregion


        #region Private Members

        private ObservableCollection<SurfaceCarouselImage> _surfaceItems = new ObservableCollection<SurfaceCarouselImage>();

        private Grid _layoutRoot = null;
        private Carousel _carousel = null;

        #endregion


        #region Construction

        public SurfaceCarousel()
        {
            this.DefaultStyleKey = typeof(SurfaceCarousel);

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

        #region Dependency Properties

        // SelectedIndex
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(CarouselItems), typeof(SurfaceCarousel), new PropertyMetadata(null, OnSelectedIndexChanged));

        public CarouselItems SelectedIndex
        {
            get { return (CarouselItems)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        #endregion

        #region Public Properties

        #endregion


        #region Private Methods

        #endregion

        #region Custom Events

        public delegate void SelectionChangedEvent(object sender, SurfaceCarouselEventArgs e);

        public event SelectionChangedEvent SelectionChanged;

        private void RaiseSelectionChangedEvent(SurfaceCarousel carousel, SurfaceCarouselEventArgs e)
        {
            SelectionChanged?.Invoke(carousel, e);
        }

        private void RaiseSelectionChangedEvent(SurfaceCarousel carousel, CarouselItems item)
        {
            RaiseSelectionChangedEvent(carousel, new SurfaceCarouselEventArgs() { CarouselItem = item });
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        // dependency property changed event
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SurfaceCarousel carousel)
            {
                if (null != carousel)
                {
                    // update the inner carousel
                    if (null != carousel._carousel)
                    {
                        // need to check this to avoid infinite loop
                        if ((int)carousel.SelectedIndex != carousel._carousel.SelectedIndex)
                        {
                            carousel._carousel.SelectedIndex = (int)carousel.SelectedIndex;
                        }
                    }

                    // raise the event
                    carousel.RaiseSelectionChangedEvent(carousel, carousel.SelectedIndex);
                }
            }
        }

        // inner carousel selection changed event
        private void Carousel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != _carousel)
            {
                // get the current selected index
                int index = _carousel.SelectedIndex;

                // if inner is different, update; need to check this to avoid infinite loop
                if ((int)this.SelectedIndex != index)
                {
                    // update the outer carousel
                    this.SelectedIndex = (CarouselItems)index;
                }
            }
        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a border here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // set up the grid
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.Padding = new Thickness(0);

            // create the carousel
            _carousel = new Carousel()
            {
                InvertPositive = false,
                ItemDepth = (int)CAROUSEL_DEPTH,
                ItemMargin = 0,
                ItemRotationX = 0,
                ItemRotationY = 0,
                ItemRotationZ = 0,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0),
                Padding = new Thickness(0),
            };
            _layoutRoot.Children.Add(_carousel);

            // set event handlers
            _carousel.SelectionChanged += Carousel_SelectionChanged;

            // set the data template
            _carousel.ItemTemplate = (DataTemplate)Application.Current.Resources["Carousel"];

            // create the children collection
            if (null == _surfaceItems)
            {
                _surfaceItems = new ObservableCollection<SurfaceCarouselImage>();
            }
            _surfaceItems.Clear();

            _surfaceItems.Add(new SurfaceCarouselImage() { Item = URI_LINGO });
            _surfaceItems.Add(new SurfaceCarouselImage() { Item = URI_LAPTOP });
            _surfaceItems.Add(new SurfaceCarouselImage() { Item = URI_PRO });
            _surfaceItems.Add(new SurfaceCarouselImage() { Item = URI_BOOK });
            _surfaceItems.Add(new SurfaceCarouselImage() { Item = URI_STUDIO });

            // add to the carousel
            _carousel.ItemsSource = _surfaceItems;

            // set selected index
            _carousel.SelectedIndex = (int)this.SelectedIndex;

        }



        private void UpdateUI()
        {

        }

        #endregion


        #region UI Helpers


        #endregion


        #region Code Helpers

        #endregion
    }
}
