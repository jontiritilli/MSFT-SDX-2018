using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

using Microsoft.Toolkit.Uwp.UI.Controls;

using SDX.Toolkit.Helpers;
using Windows.Foundation;
using SDX.Toolkit.Views;
using SDX.Toolkit.Services;
using System.Globalization;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    // TEST ONLY
    public enum PathStyles
    {
        LineOnly,
        EllipseWithLineFill,
        EllipseWithEllipseFill
    }

    public sealed class DriftingPenPath : Control
    {
        private static readonly Size WINDOW_BOUNDS = PageHelper.GetViewSizeInfo();
        private static readonly double CANVAS_X = WINDOW_BOUNDS.Width;
        private static readonly double CANVAS_Y = WINDOW_BOUNDS.Height;

        private readonly double GRID_X = CANVAS_X * 0.7;
        private readonly double GRID_Y = CANVAS_Y * 0.3;

        private const double GRID_ROWSPACING = 15d;
        private const double GRID_COLUMNSPACING = 0;


        private const int Z_ORDER_CONTROLS = 100;
        private const int Z_ORDER_SHAPES = 0;


        #region Private Members

        private Grid _layoutRoot = null;
        private Canvas _canvas = null;
        private Grid _penTouchPointGrid = null;
        private TranslateTransform _gridMove = null;
        private RadiatingPenTouch _penTouchPoint = null;
        private FadeInText _caption = null;

        private bool _pointerCaptured = false;
        private long _moveCount = 0;
        private long _ellipseCount = 0;
        private object _ellipseCountLock = new object();
        private long _lineCount = 0;
        private object _lineCountLock = new object();
        private double _lastX = 0;
        private double _lastY = 0;
        private TranslateTransform _lastTranslateTransform = null;

        private PathStyles _pathStyle = PathStyles.LineOnly;

        #endregion

        #region Constructor

        public DriftingPenPath()
        {
            this.DefaultStyleKey = typeof(DriftingPenPath);

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
            if (null != _caption)
            {
                _caption.StartFadeIn();
            }

            if (null != _penTouchPoint)
            {
                _penTouchPoint.StartEntranceAnimation();
                _penTouchPoint.StartRadiateAnimation();
            }
        }

        public void ResetAnimation()
        {
            // reset the animations
            if (null != _caption)
            {
                _caption.ResetAnimation();
            }

            if (null != _penTouchPoint)
            {
                _penTouchPoint.ResetEntranceAnimation();
                _penTouchPoint.ResetRadiateAnimation();
            }
        }

        public void ClearPath()
        {
            if (null != _canvas)
            {
                _canvas.Children.Remove<Ellipse>();
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

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Pointer pointer = e.Pointer;

            if ((null != sender) && (null != pointer) && (null != _caption))
            {
                if (sender is Canvas canvas)
                {
                    if (((PointerDeviceType.Pen == pointer.PointerDeviceType) || (PointerDeviceType.Touch == pointer.PointerDeviceType)) && (pointer.IsInContact))
                    {
                        // capture the pointer
                        if (!_pointerCaptured)
                        {
                            _pointerCaptured = canvas.CapturePointer(pointer);

                            // if we captured the pointer
                            if (_pointerCaptured)
                            {
                                // fade out the caption
                                _caption.StartFadeOut();

                                // tell the pivot not to respond to manipulation
                                if (null != PivotPage.Current)
                                {
                                    // set manipulation mode
                                    PivotPage.Current.ManipulationMode = ManipulationModes.None;
                                }

                                // set the clip region for the canvas
                                canvas.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight) };
                            }
                        }
                    }
                }
            }

            // telemetry
            TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_STARTPEN, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);

        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // we only care about moves if the pointer is captured
            if (_pointerCaptured)
            {
                // get the pointer
                Pointer pointer = e.Pointer;

                if ((null != sender) && (null != pointer) && (null != _caption))
                {
                    if (sender is Canvas canvas)
                    {
                        if (((PointerDeviceType.Pen == pointer.PointerDeviceType) && (pointer.IsInContact)) || (PointerDeviceType.Touch == pointer.PointerDeviceType))
                        {
                            // get pointer properties
                            PointerPoint point = e.GetCurrentPoint(canvas);
                            double X = point.Position.X; // - GRID_X - (_penTouchPointGrid.ActualWidth / 2);
                            double Y = point.Position.Y; // - GRID_Y - (96 / 2);

                            // calculate width, height, and thickness
                            double width = 0;
                            double height = 0;
                            double thickness = 0;
                            float pressure = 0;
                            if (PointerDeviceType.Pen == pointer.PointerDeviceType)
                            {
                                pressure = point.Properties.Pressure;
                                width = 3 + (60 * pressure);
                                height = 3 + (60 * pressure);
                                thickness = 3 + (60 * pressure);
                            }
                            else
                            {
                                // get contact area details
                                double crWidth = point.Properties.ContactRect.Width;
                                double crHeight = point.Properties.ContactRect.Height;

                                // lingo has very small contact rects
                                if (crWidth < 30) { crWidth *= 10; }
                                if (crHeight < 30) { crHeight *= 10; }

                                // set thickness
                                thickness = 10 + crWidth;

                                // calculate the width/height
                                width = 10 + crWidth;
                                height = 10 + crHeight;
                            }

                            // calculate the fill color
                            byte red = 0;
                            byte green = 0;
                            byte blue = 0;
                            //byte colorValue = 32;
                            //byte colorFadeCutoff = 50;

                            // how many rounds of 255 have we done?
                            // for each 3 rounds, we hit all 3 colors,
                            // so take the remainder of rounds/3
                            //int rounds = (int)Math.Round((double)(_moveCount / 255));
                            int rounds = (int)Math.Round((double)(_moveCount / 50));
                            int colorBucket = rounds % 4;

                            // take the remainder of the rounds-of-255
                            //byte alphaValue = (byte)(255 - (_moveCount % 255));
                            byte alphaValue = (byte)((_moveCount % 50) * 5);

                            // color is which bucket we're changing
                            if (0 == colorBucket)
                            {
                                // cobalt
                                red = 53;
                                green = 73;
                                blue = 85;
                                //blue = colorValue;

                                //if (alphaValue < colorFadeCutoff)
                                //{
                                //    green = colorValue;
                                //}
                            }
                            else if (1 == colorBucket)
                            {
                                // silver
                                red = 192;
                                green = 192;
                                blue = 192;
                                //green = colorValue;

                                //if (alphaValue < colorFadeCutoff)
                                //{
                                //    red = colorValue;
                                //}
                            }
                            else if (2 == colorBucket)
                            {
                                // burgundy
                                red = 85;
                                green = 53;
                                blue = 55;
                                //red = colorValue;

                                //if (alphaValue < colorFadeCutoff)
                                //{
                                //    blue = colorValue;
                                //}
                            }
                            else
                            {
                                // black
                                red = 0;
                                green = 0;
                                blue = 0;
                            }

                            // finally, a color!
                            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(alphaValue, red, green, blue));

                            // set default last X and Y
                            if ((0 == _lastX) && (0 == _lastY))
                            {
                                _lastX = X;
                                _lastY = Y;
                            }

                            // if we have a last transform, use its X value as a delta on the current X
                            double deltaX = 0;
                            if (null != _lastTranslateTransform)
                            {
                                deltaX = _lastTranslateTransform.X;

                                X += deltaX;
                            }

                            // ellipse interpolation fill
                            if (PathStyles.EllipseWithEllipseFill == _pathStyle)
                            {
                                // interpolate between the last point and current point
                                if (X != _lastX)
                                {
                                    double stepValue = (X - _lastX) / 5;
                                    double factorY = (Y - _lastY) / (X - _lastX);

                                    double interpolateY = 0;

                                    if (X > _lastX)
                                    {
                                        // moving right
                                        for (double interpolateX = _lastX; interpolateX < X; interpolateX += stepValue)
                                        {
                                            interpolateY = (interpolateX - _lastX) * factorY + _lastY;
                                            this.CreateAndAddEllipse(canvas, interpolateX, interpolateY, width, height, brush);
                                        }
                                    }
                                    else
                                    {
                                        // moving left
                                        for (double interpolateX = _lastX; interpolateX > X; interpolateX += stepValue)
                                        {
                                            interpolateY = (interpolateX - _lastX) * factorY + _lastY;
                                            this.CreateAndAddEllipse(canvas, interpolateX, interpolateY, width, height, brush);
                                        }
                                    }
                                }
                            }

                            // line interpolation fill
                            if (PathStyles.EllipseWithEllipseFill != _pathStyle)
                            {
                                // draw a line from the last point to the current point
                                _lastTranslateTransform = this.CreateAndAddLine(canvas, _lastX, _lastY, X, Y, thickness, brush);
                            }

                            // draw closing ellipse
                            if (PathStyles.LineOnly != _pathStyle)
                            {
                                // draw an ellipse on the final point
                                _lastTranslateTransform = this.CreateAndAddEllipse(canvas, X, Y, width, height, brush);
                            }

                            // last, move the grid containing the radiatingpentouch
                            _gridMove.X = X - GRID_X - (_penTouchPointGrid.ActualWidth / 2);
                            _gridMove.Y = Y - GRID_Y - (96 / 2);  // half of the radiatingpentouch circle

                            //// save this point as the last one
                            //_lastPoint = point;
                            _lastX = X;
                            _lastY = Y;

                            // increment move count
                            _moveCount++;
                        }
                    }
                }
            }
        }

        private TranslateTransform CreateAndAddLine(Canvas canvas, double lastX, double lastY, double X, double Y, double thickness, SolidColorBrush brush)
        {
            // create a name for the line
            lock (_lineCountLock)
            {
                ++_lineCount;
            }
            string lineName = String.Format("Line{0}", _lineCount);

            // draw a line between the last point and the current point
            Line line = new Line()
            {
                Name = lineName,
                X1 = lastX,
                Y1 = lastY,
                X2 = X,
                Y2 = Y,
                Stroke = brush,
                StrokeThickness = thickness
            };
            Canvas.SetZIndex(line, Z_ORDER_SHAPES);
            TranslateTransform lineTransform = new TranslateTransform()
            {
                X = 0,
                Y = 0
            };
            line.RenderTransform = lineTransform;
            canvas.Children.Add(line);

            // animate off the screen to the left
            double xOffScreen = -1 * Math.Max(X, lastX) - (CANVAS_X * 0.3);
            Storyboard lineStoryboard = AnimationHelper.CreateStandardAnimation(line,
                "(Line.RenderTransform).(TranslateTransform.X)", 0, 0, xOffScreen, Math.Abs(xOffScreen * 4), 0,
                false, false, new RepeatBehavior(1));

            // set the storyboard target name
            Storyboard.SetTargetName(lineStoryboard, lineName);

            // add an event handler for the end of the storyboard
            lineStoryboard.Completed += ScrollAway_Completed;

            lineStoryboard.Begin();

            return lineTransform;
        }

        private TranslateTransform CreateAndAddEllipse(Canvas canvas, double X, double Y, double width, double height, SolidColorBrush brush)
        {
            // draw an ellipse
            double centerX = X - (width / 2);
            double centerY = Y - (height / 2);

            // create a name for the ellipse
            lock (_ellipseCountLock)
            {
                ++_ellipseCount;
            }

            string ellipseName = String.Format("Ellipse{0}", _ellipseCount);

            Ellipse ellipse = new Ellipse()
            {
                Name = ellipseName,
                Width = width,
                Height = height,
                Fill = brush
            };

            // set canvas properties
            Canvas.SetLeft(ellipse, centerX);
            Canvas.SetTop(ellipse, centerY);
            Canvas.SetZIndex(ellipse, Z_ORDER_SHAPES);

            // Create the translatetransform and attach to the image
            TranslateTransform ellipseTransform = new TranslateTransform()
            {
                X = 0,
                Y = 0
            };
            ellipse.RenderTransform = ellipseTransform;
            canvas.Children.Add(ellipse);

            // animate off the screen to the left
            double xOffScreen = -1 * (X + width) - (CANVAS_X * 0.3);
            Storyboard ellipseStoryboard = AnimationHelper.CreateStandardAnimation(ellipse,
                "(Ellipse.RenderTransform).(TranslateTransform.X)", 0, 0, xOffScreen, Math.Abs(xOffScreen * 4), 0,
                false, false, new RepeatBehavior(1));

            // set the storyboard target name
            Storyboard.SetTargetName(ellipseStoryboard, ellipseName);

            // add an event handler for the end of the storyboard
            ellipseStoryboard.Completed += ScrollAway_Completed;

            // start the drift left
            ellipseStoryboard.Begin();

            return ellipseTransform;
        }

        private void ScrollAway_Completed(object sender, object e)
        {
            // get the storyboard
            if (sender is Storyboard storyboard)
            {
                // try to get the name of the target control (this depends on target name being set on the storyboard)
                string shapeName = Storyboard.GetTargetName(storyboard);

                // try to get the ellipse
                UIElement elementShape = (UIElement)this.FindName(shapeName);

                // if we got it
                if ((null != _canvas) && (null != elementShape))
                {
                    // remove it
                    _canvas.Children.Remove(elementShape);
                }
            }
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _pointerCaptured = false;
            _moveCount = 0;

            this.HandlePenUp();
        }

        private void Canvas_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _pointerCaptured = false;
            _moveCount = 0;

            this.HandlePenUp();
        }

        private void Canvas_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            _pointerCaptured = false;
            _moveCount = 0;

            this.HandlePenUp();
        }

        private void HandlePenUp()
        {
            // tell the pivot page to listen to manipulation again
            if (null != PivotPage.Current)
            {
                // set manipulation mode
                PivotPage.Current.ManipulationMode = ManipulationModes.System;
            }


            if ((null != _penTouchPointGrid) && (null != _gridMove) && (null != _caption))
            {
                // move the pen touch back home
                _gridMove.X = 0;
                _gridMove.Y = 0;

                // fade back in the caption
                _caption.StartFadeIn();
            }

            // telemetry
            TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_ENDPEN, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);

        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            //// set up grid
            //_layoutRoot.Width = this.Width;

            // create control canvas
            _canvas = new Canvas()
            {
                Name = "DriftingPenPathCanvasControls",
                //Width = this.Width,
                //Height = this.Height,
                Margin = new Thickness(0),
                ManipulationMode = ManipulationModes.None
            };
            Grid.SetRow(_canvas, 0);
            Grid.SetColumn(_canvas, 0);
            _layoutRoot.Children.Add(_canvas);

            // set canvas event handlers
            _canvas.PointerPressed += Canvas_PointerPressed;
            _canvas.PointerMoved += Canvas_PointerMoved;
            _canvas.PointerReleased += Canvas_PointerReleased;
            _canvas.PointerCaptureLost += Canvas_PointerCaptureLost;
            _canvas.PointerCanceled += Canvas_PointerCanceled;

            // create the button grid
            _penTouchPointGrid = new Grid()
            {
                Name = "AccessoriesPenButtonGrid",
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            _gridMove = new TranslateTransform()
            {
                X = 0,
                Y = 0
            };
            _penTouchPointGrid.RenderTransform = _gridMove;
            _penTouchPointGrid.ColumnDefinitions.Add(new ColumnDefinition());
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(GRID_ROWSPACING) });
            _penTouchPointGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            Canvas.SetLeft(_penTouchPointGrid, GRID_X);
            Canvas.SetTop(_penTouchPointGrid, GRID_Y);
            Canvas.SetZIndex(_penTouchPointGrid, Z_ORDER_CONTROLS);
            _canvas.Children.Add(_penTouchPointGrid);

            // create the pen touch point
            _penTouchPoint = new RadiatingPenTouch()
            {
                Name = "RadiatingPenTouch",
                EntranceDurationInMilliseconds = this.DurationInMilliseconds,
                EntranceStaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds,
                RadiateDurationInMilliseconds = 1200d,
                RadiateStaggerDelayInMilliseconds = 1000d,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                AutoStart = false,
            };
            Grid.SetRow(_penTouchPoint, 0);
            Grid.SetColumn(_penTouchPoint, 0);
            _penTouchPointGrid.Children.Add(_penTouchPoint);

            // create the caption
            _caption = new FadeInText()
            {
                Name = "DriftingPenPathCaption",
                ControlStyle = this.ControlStyle,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                DurationInMilliseconds = this.DurationInMilliseconds,
                StaggerDelayInMilliseconds = this.StaggerDelayInMilliseconds,
                AutoStart = false
            };
            Grid.SetRow(_caption, 2);
            Grid.SetColumn(_caption, 0);
            _penTouchPointGrid.Children.Add(_caption);

            // set caption binding
            _caption.SetBinding(FadeInText.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("Caption"), Mode = BindingMode.OneWay });


            // TEST ONLY
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            //Button lineOnly = new Button()
            //{
            //    Content = "Line Only"
            //};
            //Grid.SetRow(lineOnly, 1);
            //Grid.SetColumn(lineOnly, 1);
            //_layoutRoot.Children.Add(lineOnly);
            //lineOnly.Click += this.LineOnly_Click;

            //Button ellipseWithLineFill = new Button()
            //{
            //    Content = "Ellipse with Line Fill"
            //};
            //Grid.SetRow(ellipseWithLineFill, 1);
            //Grid.SetColumn(ellipseWithLineFill, 2);
            //_layoutRoot.Children.Add(ellipseWithLineFill);
            //ellipseWithLineFill.Click += this.EllipseWithLineFill_Click;

            //Button ellipseWithEllipseFill = new Button()
            //{
            //    Content = "Ellipse with Ellipse Fill"
            //};
            //Grid.SetRow(ellipseWithEllipseFill, 1);
            //Grid.SetColumn(ellipseWithEllipseFill, 3);
            //_layoutRoot.Children.Add(ellipseWithEllipseFill);
            //ellipseWithEllipseFill.Click += this.EllipseWithEllipseFill_Click;
        }

        private void EllipseWithEllipseFill_Click(object sender, RoutedEventArgs e)
        {
            _pathStyle = PathStyles.EllipseWithEllipseFill;
        }

        private void EllipseWithLineFill_Click(object sender, RoutedEventArgs e)
        {
            _pathStyle = PathStyles.EllipseWithLineFill;
        }

        private void LineOnly_Click(object sender, RoutedEventArgs e)
        {
            _pathStyle = PathStyles.LineOnly;
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}
