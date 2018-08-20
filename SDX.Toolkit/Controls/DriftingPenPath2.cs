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
    public sealed class DriftingPenPath2 : Control
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
        private LinearGradientBrush _gradientBrush = null;
        private Path _path = null;
        private PathGeometry _pathGeometry = null;
        private PathFigure _pathFigure = null;
        private TranslateTransform _pathTransform = null;

        private TextBlock _testOutput = null;

        private bool _pointerCaptured = false;
        private int _pathCount = 0;
        private object _pathCountLock = new object();

        #endregion

        #region Constructor

        public DriftingPenPath2()
        {
            this.DefaultStyleKey = typeof(DriftingPenPath2);

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
                _canvas.Children.Remove<Path>();
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

                                // clear the path
                                _path = null;
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

                            // if we have no path, we're at the start of drawing
                            if (null == _path)
                            {
                                lock (_pathCountLock)
                                {
                                    _pathCount++;
                                }
                                string pathName = String.Format("Path_{0}", _pathCount);

                                // create the path
                                _path = new Path()
                                {
                                    Name = pathName,
                                    Stroke = _gradientBrush,  //new SolidColorBrush(Colors.Blue),
                                    StrokeThickness = thickness,
                                    StrokeLineJoin = PenLineJoin.Round
                                };
                                
                                _pathGeometry = new PathGeometry()
                                {
                                    FillRule = FillRule.Nonzero
                                };

                                _pathFigure = new PathFigure()
                                {
                                    StartPoint = new Point(X, Y),
                                    IsClosed = false
                                };

                                _pathFigure.Segments.Add(new LineSegment() { Point = new Point(X, Y) });

                                _pathGeometry.Figures.Add(_pathFigure);
                                _path.Data = _pathGeometry;

                                // create its translate transform
                                _pathTransform = new TranslateTransform()
                                {
                                    X = 0,
                                    Y = 0
                                };
                                _path.RenderTransform = _pathTransform;

                                // animate off the screen to the left
                                double xOffScreen = -3 * CANVAS_X;
                                Storyboard pathStoryboard = AnimationHelper.CreateStandardAnimation(_path,
                                    "(Path.RenderTransform).(TranslateTransform.X)", 0, 0, xOffScreen, Math.Abs(xOffScreen * 4), 0,
                                    false, false, new RepeatBehavior(1));

                                // set the storyboard target name
                                Storyboard.SetTargetName(pathStoryboard, pathName);

                                // add an event handler for the end of the storyboard
                                pathStoryboard.Completed += ScrollAway_Completed;

                                pathStoryboard.Begin();

                                // add it to the canvas
                                Canvas.SetLeft(_path, 0);
                                Canvas.SetTop(_path, 0);
                                canvas.Children.Add(_path);
                            }
                            else
                            {
                                // here because the path exists,

                                // update thickness of the path
                                if (null != _path)
                                {
                                    _path.StrokeThickness = thickness;
                                }

                                // the path is moving, so add the translateX to the X
                                Point linePoint = new Point(X, Y);

                                if ((null != _path) && (null != _canvas))
                                {
                                    GeneralTransform ttv = _canvas.TransformToVisual(_path);

                                    linePoint = ttv.TransformPoint(new Point(X, Y));

                                    //if (null != _testOutput)
                                    //{
                                    //    _testOutput.Text = String.Format("Raw: ({0}, {1}), TTV: ({2}, {3})", X, Y, linePoint.X, linePoint.Y);
                                    //}
                                }

                                // get our X position
                                //double lineX = X + deltaX;

                                LineSegment line = new LineSegment()
                                {
                                    Point = linePoint  // new Point(lineX, Y)
                                };

                                if (null != _pathFigure)
                                {
                                    // add line segment to path
                                    _pathFigure.Segments.Add(line);
                                }
                            }

                            // last, move the grid containing the radiatingpentouch
                            _gridMove.X = X - GRID_X - (_penTouchPointGrid.ActualWidth / 2);
                            _gridMove.Y = Y - GRID_Y - (96 / 2);  // half of the radiatingpentouch circle
                        }
                    }
                }

            }
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

            this.HandlePenUp();
        }

        private void Canvas_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _pointerCaptured = false;

            this.HandlePenUp();
        }

        private void Canvas_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            _pointerCaptured = false;

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

            _path = null;
            _pathGeometry = null;
            _pathFigure = null;
            _pathTransform = null;

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

            // create the brush
            _gradientBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };
            _gradientBrush.GradientStops.Add(new GradientStop() { Offset = 0, Color = Color.FromArgb(255, 33, 33, 33) });
            _gradientBrush.GradientStops.Add(new GradientStop() { Offset = 0.33, Color = Color.FromArgb(255, 192, 192, 192) });
            _gradientBrush.GradientStops.Add(new GradientStop() { Offset = 0.66, Color = Color.FromArgb(255, 53, 73, 85) });
            _gradientBrush.GradientStops.Add(new GradientStop() { Offset = 1.0, Color = Color.FromArgb(255, 85, 53, 55) });

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

            //_testOutput = new TextBlock()
            //{
            //    Text = "(0,0)",
            //    HorizontalAlignment = HorizontalAlignment.Right,
            //    VerticalAlignment = VerticalAlignment.Bottom
            //};
            //_layoutRoot.Children.Add(_testOutput);
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}
