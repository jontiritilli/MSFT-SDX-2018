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
using SDX.Telemetry.Services;


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
        //private Image _pinch_image = null;
        private ImageEx _pinch_image = null;
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

        public delegate void OnZoomResetEvent(object sender, EventArgs e);

        public event OnZoomResetEvent OnZoomReset;

        private void RaiseOnZoomResetEvent(PinchZoom zoom, EventArgs e)
        {
            OnZoomReset?.Invoke(zoom, e);
        }

        private void RaiseOnZoomResetEvent(PinchZoom zoom)
        {
            this.RaiseOnZoomResetEvent(zoom, new EventArgs());
        }

        public delegate void OnZoomChangingEvent(object sender, EventArgs e);

        public event OnZoomChangingEvent OnZoomChanging;

        private void RaiseOnZoomChangingEvent(PinchZoom zoom, EventArgs e)
        {
            OnZoomChanging?.Invoke(zoom, e);
        }

        private void RaiseOnZoomChangingEvent(PinchZoom zoom)
        {
            this.RaiseOnZoomChangingEvent(zoom, new EventArgs());
        }

        private void HandleZoomChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            ToggleCloseButton();

            if(_viewer.ZoomFactor < 1.001f)
            {
                // raise the reset event
                this.RaiseOnZoomResetEvent(this);
            }
            else
            {
                // raise the changing event
                this.RaiseOnZoomChangingEvent(this);
            }

            // telemetry
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.StartPinch);
        }

        private void HandleReset()
        {
            if (null != _viewer && _viewer.ZoomFactor > 1.00f)
            {
                // reset the zoom to normal
                _viewer.ChangeView(null, null, 1.00f);
            }

            // telemetry
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.EndPinch);
        }

        private void ToggleCloseButton()
        {
            if (null != _viewer && null != _closeGrid)
            {
                if (_viewer.ZoomFactor < 1.001f)
                {
                    // show the ellipse
                    _closeGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // hide the ellipse
                    _closeGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void HandleClick(object sender, PointerRoutedEventArgs e)
        {
            HandleReset();
            ToggleCloseButton();
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            _layoutRoot = (Canvas)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // get sizes for everything
            double _canvasWidth = StyleHelper.GetApplicationDouble("CanvasWidth");
            double _canvasHeight = StyleHelper.GetApplicationDouble("CanvasHeight");

            double _startImageWidth = StyleHelper.GetApplicationDouble("PinchZoomImageWidth");
            double _startImageHeight = StyleHelper.GetApplicationDouble("PinchZoomImageHeight");

            double _radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
            double _closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight)/2;
            double _closeEllipseTopMargin = StyleHelper.GetApplicationDouble("PinchTopMargin");
            double _closeEllipseRightMargin = StyleHelper.GetApplicationDouble("PinchSideMargin");

            double _ellipseGridCanvasSetLeft = _canvasWidth - _closeEllipseRightMargin - _radiatingButtonRadius;

            // create the scroll viewer
            _viewer = new ScrollViewer
            {
                ZoomMode = ZoomMode.Enabled,
                Width = _canvasWidth,
                Height = _canvasHeight,
                MinZoomFactor = 1,
                MaxZoomFactor = 4,
                HorizontalScrollMode = ScrollMode.Enabled,
                VerticalScrollMode = ScrollMode.Enabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
            };

            // create the image for zooming
            _pinch_image = new ImageEx
            {
                Name = this.Name + "ZoomImage",
                ImageSource = ImageUri,
                ImageWidth = _startImageWidth, //account for left and right margins
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
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
                _closeGrid = new Grid()
                {
                    Name = this.Name + "closeGrid",
                    Visibility = Visibility.Collapsed
                };
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
