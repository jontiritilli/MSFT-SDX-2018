using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Text;
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
using SDX.Toolkit.Views;
using SDX.Toolkit.Services;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class DriftingPenPath3 : Control
    {

        #region Private Constants

        private const string URI_PENICON = @"ms-appx:///Assets/DriftingPenPath/ink-pen-icon.png";

        private static readonly Size WINDOW_BOUNDS = PageHelper.GetViewSizeInfo();
        private static readonly double CANVAS_X = WINDOW_BOUNDS.Width;
        private static readonly double CANVAS_Y = WINDOW_BOUNDS.Height;

        private readonly double GRID_X = CANVAS_X * 0.7;
        private readonly double GRID_Y = CANVAS_Y * 0.4;

        private const double GRID_ROWSPACING = 30d;
        private const double GRID_COLUMNSPACING = 0;

        private const double WIDTH_PENICON = 151d;

        private const int Z_ORDER_CONTROLS = 100;
        private const int Z_ORDER_SHAPES = 0;

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private InkCanvas _inkCanvas = null;
        private Canvas _touchHereCanvas = null;
        private Grid _penTouchPointGrid = null;
        private Image _touchHereImage = null;
        private StyledText _caption = null;

        private bool _touchHereWasHidden = false;
        private int _currentColor = -1;
        private List<Color> _inkColors = new List<Color>()
            {
                Color.FromArgb(255, 85, 53, 55),
                Color.FromArgb(255, 53, 73, 85),
                Color.FromArgb(255, 192, 192, 192),
                Color.FromArgb(255, 33, 33, 33)
            };

        #endregion

        #region Constructor

        public DriftingPenPath3()
        {
            this.DefaultStyleKey = typeof(DriftingPenPath3);

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
            // start the fade ins
            if (null != _penTouchPointGrid)
            {
                AnimationHelper.PerformFadeIn(_penTouchPointGrid, 100d, 0);
            }
        }

        public void ResetAnimation()
        {
            // reset the animations
            if (null != _penTouchPointGrid)
            {
                AnimationHelper.PerformFadeOut(_penTouchPointGrid, 100d, 0);
            }
        }

        public void ClearPath()
        {
            if (null != _inkCanvas)
            {
                _inkCanvas.InkPresenter.StrokeContainer.Clear();
            }

        }

        #endregion

        #region Dependency Properties

        // Caption
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(DriftingPenPath), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // ControlStyle
        public static readonly DependencyProperty ControlStyleProperty =
            DependencyProperty.Register("ControlStyle", typeof(ControlStyles), typeof(DriftingPenPath), new PropertyMetadata(null));

        public ControlStyles ControlStyle
        {
            get { return (ControlStyles)GetValue(ControlStyleProperty); }
            set { SetValue(ControlStyleProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(DriftingPenPath), new PropertyMetadata(100d));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // FadeInCompletedHandler
        public static readonly DependencyProperty FadeInCompletedHandlerProperty =
            DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(DriftingPenPath), new PropertyMetadata(null));

        public EventHandler<object> FadeInCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
            set { SetValue(FadeInCompletedHandlerProperty, value); }
        }

        // FadeOutCompletedHandler
        public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
            DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(DriftingPenPath), new PropertyMetadata(null));

        public EventHandler<object> FadeOutCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
            set { SetValue(FadeOutCompletedHandlerProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(DriftingPenPath), new PropertyMetadata(0d));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(DriftingPenPath), new PropertyMetadata(false));

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

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // create touch here canvas
            _touchHereCanvas = new Canvas()
            {
                Name = "DriftingPenPathCanvasControls",
                Margin = new Thickness(0),
                ManipulationMode = ManipulationModes.None
            };
            Grid.SetRow(_touchHereCanvas, 0);
            Grid.SetColumn(_touchHereCanvas, 0);
            _layoutRoot.Children.Add(_touchHereCanvas);

            // create the button grid
            _penTouchPointGrid = new Grid()
            {
                Name = "AccessoriesPenButtonGrid",
                Margin = new Thickness(0),
                Padding = new Thickness(0),
                Opacity = 0.0
            };
            _penTouchPointGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_ROWSPACING) });
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            Canvas.SetLeft(_penTouchPointGrid, GRID_X);
            Canvas.SetTop(_penTouchPointGrid, GRID_Y);
            Canvas.SetZIndex(_penTouchPointGrid, Z_ORDER_CONTROLS);
            _touchHereCanvas.Children.Add(_penTouchPointGrid);

            // create touch here image
            _touchHereImage = new Image()
            {
                Source = new BitmapImage() { UriSource = new Uri(URI_PENICON), DecodePixelWidth = (int)WIDTH_PENICON },
                Width = WIDTH_PENICON,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(_touchHereImage, 0);
            Grid.SetColumn(_touchHereImage, 0);
            _penTouchPointGrid.Children.Add(_touchHereImage);

            // create the caption
            _caption = new StyledText()
            {
                Name = "DriftingPenPathCaption",
                Text = this.Caption,
                ControlStyle = this.ControlStyle,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(_caption, 2);
            Grid.SetColumn(_caption, 0);
            _penTouchPointGrid.Children.Add(_caption);

            // create the ink canvas
            _inkCanvas = new InkCanvas()
            {
                Name = "InkCanvas",
                Margin = new Thickness(0),
            };
            Grid.SetRow(_inkCanvas, 0);
            Grid.SetColumn(_inkCanvas, 0);
            _layoutRoot.Children.Add(_inkCanvas);

            // set the default drawing attribs
            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes()
            {
                Color = GetNextColor(),
                //FitToCurve = true,
                PenTip = PenTipShape.Circle,
                Size = new Size(WINDOW_BOUNDS.Width * 0.02, WINDOW_BOUNDS.Height * 0.02)
            };
            _inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);

            // set up events
            _inkCanvas.InkPresenter.StrokeInput.StrokeStarted += this.InkPresenter_StrokeStarted;
            _inkCanvas.InkPresenter.StrokeInput.StrokeEnded += this.InkPresenter_StrokeEnded;
        }

        private void InkPresenter_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
        {
            // hide the pen icon
            if (!_touchHereWasHidden)
            {
                if (null != _penTouchPointGrid)
                {
                    AnimationHelper.PerformFadeOut(_penTouchPointGrid, 100d, 0d);
                }

                _touchHereWasHidden = true;
            }

            // telemetry
            TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_STARTPEN, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        private void InkPresenter_StrokeEnded(InkStrokeInput sender, PointerEventArgs args)
        {
            //// show the pen icon
            //if (null != _touchHereImage)
            //{
            //    AnimationHelper.PerformFadeIn(_touchHereImage, 100d, 0d);
            //}

            // change the ink color
            if (null != _inkCanvas)
            {
                InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes()
                {
                    Color = GetNextColor(),
                    //FitToCurve = true,
                    PenTip = PenTipShape.Circle,
                    Size = new Size(WINDOW_BOUNDS.Width * 0.02, WINDOW_BOUNDS.Height * 0.02)
                };
                _inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
            }

            // telemetry
            TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_ENDPEN, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion

        #region UI Helpers

        private Color GetNextColor()
        {
            _currentColor++;

            if (_currentColor >= _inkColors.Count())
            {
                _currentColor = 0;
            }

            return _inkColors.ElementAt<Color>(_currentColor);
        }

        #endregion
    }
}
