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
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using SDX.Toolkit.Helpers;


namespace SDX.Toolkit.Controls
{
    public sealed class PinchZoom : Control
    {
        #region Constants

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        #endregion

        #region Dependency Properties

        // ImageUri
        public static readonly DependencyProperty ImageUriProperty =
            DependencyProperty.Register("ImageUri", typeof(string), typeof(PinchZoom), new PropertyMetadata(URI_X_IMAGE));

        public string ImageUri
        {
            get { return (string)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        #endregion

        #region Private Members

        private Canvas _layoutRoot = null;
        private ScrollViewer _viewer = null;
        private Image _pinch_image = null;
        private Grid _closeGrid = null;
        private Ellipse _closeEllipse = null;
        private ImageEx x_image = null;

        #endregion

        #region Constructor

        public PinchZoom()
        {
            this.DefaultStyleKey = typeof(PinchZoom);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.RenderUI();
        }

        #endregion

        #region Public Methods

        public void ResetControl()
        {
            HandleReset();
        }

        #endregion

        #region Event Handlers

        public RoutedEventHandler ZoomEvent;

        private void HandleZoomChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (null != _viewer && _viewer.ZoomFactor > 1)
            {
                // show the ellipse
                _closeEllipse.Opacity = 1.0;
                x_image.Opacity = 1.0;
                ZoomEvent(sender, new RoutedEventArgs());
            }
            else
            {
                // hide the ellipse
                _closeEllipse.Opacity = 0.0;
                x_image.Opacity = 0.0;
            }
        }

        private void HandleReset()
        {
            if (null != _viewer && _viewer.ZoomFactor > 1)
            {
                // reset the zoom to normal
                _viewer.ChangeView(null, null, 1);
            }
            if (null != _closeEllipse)
            {
                // hide the ellipse
                _closeEllipse.Opacity = 0.0;
                x_image.Opacity = 0.0;
            }
        }

        private void HandleClick(object sender, PointerRoutedEventArgs e)
        {
            HandleReset();
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            _layoutRoot = (Canvas)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // get sizes for everything
            double _maxImageWidth = StyleHelper.GetApplicationDouble("CanvasWidth");
            double _maxImageHeight = StyleHelper.GetApplicationDouble("CanvasHeight");

            double _startImageWidth = StyleHelper.GetApplicationDouble("PinchZoomImageWidth");
            double _startImageHeight = StyleHelper.GetApplicationDouble("PinchZoomImageHeight");
            double _imageMargin = StyleHelper.GetApplicationDouble("PinchImageMargin");

            double _radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
            double _closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight)/2;
            double _closeEllipseTopMargin = StyleHelper.GetApplicationDouble("PinchTopMargin");
            double _closeEllipseRightMargin = StyleHelper.GetApplicationDouble("PinchSideMargin");

            double _ellipseGridCanvasSetLeft = _maxImageWidth - _closeEllipseRightMargin - _radiatingButtonRadius;

            // create the scroll viewer
            _viewer = new ScrollViewer
            {
                ZoomMode = ZoomMode.Enabled,
                Width = _maxImageWidth,
                Height = _maxImageHeight,
                MinZoomFactor = 1,
                MaxZoomFactor = 5,
                HorizontalScrollMode = ScrollMode.Enabled,
                VerticalScrollMode = ScrollMode.Enabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
            };

            // create the image for zooming
            _pinch_image = new Image
            {
                Name = this.Name + "ZoomImage",
                Source = new BitmapImage(new Uri(ImageUri)),
                Width = _startImageWidth, //account for left and right margins
                Height = _startImageHeight, //account for left and right margins
                MaxWidth = _maxImageWidth,
                MaxHeight = _maxImageHeight,
                Margin = new Thickness(0, _imageMargin, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            // add the image to the scroll viewer
            _viewer.Content = _pinch_image;

            // add manipulation events to viewer
            _viewer.ViewChanging += HandleZoomChanging;

            // add the scrollviewer to the root
            _layoutRoot.Children.Add(_viewer);

            // create grid for the close element if none exists
            if (null == _closeGrid)
            {
                _closeGrid = new Grid();
            }

            // create the close button
            _closeEllipse = new Ellipse()
            {
                Name = this.Name + "entranceEllipse",
                Width = _radiatingButtonRadius,
                Height = _radiatingButtonRadius,
                Fill = new SolidColorBrush(Colors.White),
                Stroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2"),
                StrokeThickness = 2,
                Opacity = 0d,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0)
            };
            // add ellipse to the grid
            _closeGrid.Children.Add(_closeEllipse);

            // create the X image
            x_image = new ImageEx()
            {
                Name = this.Name + "ImageX",
                ImageSource = URI_X_IMAGE,
                ImageWidth = _closeIconHeight,
                Opacity = 0.0d,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0)
            };

            // add the close image to the grid
            _closeGrid.Children.Add(x_image);

            // set canvas position for the close grid
            Canvas.SetLeft(_closeGrid, _ellipseGridCanvasSetLeft);
            Canvas.SetTop(_closeGrid, _closeEllipseTopMargin);

            // assign click handler
            _closeGrid.PointerPressed += HandleClick;

            // add the close button to the layout root
            _layoutRoot.Children.Add(_closeGrid);
        }
        #endregion
    }
}
