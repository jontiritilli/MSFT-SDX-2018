using System;
using System.Diagnostics;
using SDX.Toolkit.Helpers;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace SDX.Toolkit.Controls
{
    public sealed class SurfaceDial : Control
    {
        public float mainColorAngle;
        public float activeSaturationValue;
        public Grid _paletteRing;
        public Grid _saturationPaletteRing;
        public Storyboard _storyboard;
        public bool isRotating;
        public double _currentAngle;

        #region Private Members

        private Canvas _layoutRoot;
        private Grid _dialGrid;
        private Canvas _dialCanvas;
        private Ellipse _dial;
        private Path _colorSelector;
        private RadialControllerMenuItem _screenColorMenuItem;

        #endregion

        #region Constants

        private static readonly double DIAL_HEIGHT = StyleHelper.GetApplicationDouble("DialDiameter");
        private static readonly double SELECTOR_RADIUS = StyleHelper.GetApplicationDouble("SelectorDiameter");
        private static readonly double COLOR_LINE_RADIUS = StyleHelper.GetApplicationDouble("ColorRingRadius");
        private static readonly double COLOR_LINE_THICKNESS = StyleHelper.GetApplicationDouble("ColorRingThickness");
        private static readonly double DIAL_WIDTH = DIAL_HEIGHT;
        private static readonly double DIAL_RADIUS = DIAL_HEIGHT / 2d;
        private static readonly double SELECTOR_CENTER_MODIFIER = DIAL_HEIGHT - COLOR_LINE_RADIUS - COLOR_LINE_THICKNESS;
        private static readonly double SELECTOR_CENTER = DIAL_RADIUS + SELECTOR_CENTER_MODIFIER;
        private static readonly double ROTATION_MODIFIER = SELECTOR_RADIUS;
        private static readonly double DIAL_INITIAL_ANGLE = 0;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(SurfaceDial), new PropertyMetadata(false, IsActiveChangedCallback));

        public static readonly DependencyProperty CurrentColorProperty =
           DependencyProperty.Register(nameof(CurrentColor), typeof(Color), typeof(SurfaceDial), new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty SelectedColorProperty =
           DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(SurfaceDial), new PropertyMetadata(Colors.White));

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public Color CurrentColor
        {
            get
            {
                return (Color)GetValue(CurrentColorProperty);
            }
            set
            {
                SetValue(CurrentColorProperty, value);
            }
        }

        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }

        public RadialController Controller { get; set; }


        public SurfaceDial()
        {
            DefaultStyleKey = typeof(SurfaceDial);
        }

        #endregion

        #region Private Methods

        private static void IsActiveChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var palette = d as SurfaceDial;
            if (palette.IsActive)
            {
                palette.SetupController();
            }
            else
            {
                palette.SetupController();
            }
        }

        private static Point FindLineStart(double x, double y)
        {
            return new Point(x, y);
        }


        private void SetupController()
        {
            if (null == Controller)
            {
                Controller = RadialController.CreateForCurrentView();
            }
            // Remove standard menu items
            RadialControllerConfiguration _dialConfiguration = RadialControllerConfiguration.GetForCurrentView();
            _dialConfiguration.SetDefaultMenuItems(new RadialControllerSystemMenuItemKind[] { });

            // Create an icon for the custom tool.
            RandomAccessStreamReference icon =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/dial_icon_custom_visual.png"));

            // Create a menu item for the custom tool.
            _screenColorMenuItem =
                RadialControllerMenuItem.CreateFromIcon("Screen Color", icon);

            // Add the custom tool to the RadialController menu.
            Controller.Menu.Items.Add(_screenColorMenuItem);

            // Set rotation degrees
            Controller.RotationResolutionInDegrees = 20;

            // Bind dial controls to local methods
            _screenColorMenuItem.Invoked += ColorMenuItem_Invoked;
            Controller.RotationChanged += Controller_RotationChanged;
            Controller.ButtonClicked += Controller_ButtonClicked;
            Controller.ScreenContactStarted += Controller_ScreenContactStarted;
            Controller.ScreenContactContinued += Controller_ScreenContactContinued;
            Controller.ScreenContactEnded += Controller_ScreenContactEnded;
        }

        private void ColorMenuItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            IsActive = true;
        }

        // Dial rotation handler
        private void Controller_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            double RotationInterval;

            if (args.RotationDeltaInDegrees < 0)
            {
                RotationInterval = args.RotationDeltaInDegrees - ROTATION_MODIFIER;
            } else
            {
                RotationInterval = args.RotationDeltaInDegrees + ROTATION_MODIFIER;
            }

            if (IsActive)
            {
                Rotate(RotationInterval);
                Debug.WriteLine("RotationInterval {0}", RotationInterval);
            }
        }

        // Dial Screen contact initial
        private void Controller_ScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            Canvas.SetLeft(_dialCanvas, args.Contact.Position.X - DIAL_RADIUS);
            Canvas.SetTop(_dialCanvas, args.Contact.Position.Y - DIAL_RADIUS);

            _dialCanvas.Visibility = Visibility.Visible;
        }

        // Dial lifted and placed again
        private void Controller_ScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (IsActive)
            {
                Canvas.SetLeft(_dialCanvas, args.Contact.Position.X - DIAL_RADIUS);
                Canvas.SetTop(_dialCanvas, args.Contact.Position.Y - DIAL_RADIUS);
            }
        }

        // Dial screen contact ended and placed again
        private void Controller_ScreenContactEnded(RadialController sender, object args)
        {
            if (IsActive)
            {
                _dialCanvas.Visibility = Visibility.Collapsed;
                Controller.Menu.Items.Clear();
            }
        }

        // Dial button pressed
        private void Controller_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            if (IsActive)
            {
                mainColorAngle = (float)Utilities.ClampAngle(_currentAngle);
                FillSelectionRing();
            }
        }

        // Show or hide selection and saturation rings
        private void FillSelectionRing()
        {
            _paletteRing.Visibility = Visibility.Visible;
        }

        private void Storyboard_Completed(object sender, object e)
        {
            isRotating = false;
            CompleteRotation();
        }

        private void Rotate(double angle = 1)
        {
            if (isRotating)
            {
                CompleteRotation();
            }
            
            _currentAngle = (double)Utilities.ClampAngle(_currentAngle + angle);

            Debug.WriteLine("current angle in rotate {0}",_currentAngle);

            DoubleAnimation colorSelectorAnimation = new DoubleAnimation();
            colorSelectorAnimation.To = _currentAngle;
            colorSelectorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            _storyboard.Children.Add(colorSelectorAnimation);
            Storyboard.SetTarget(colorSelectorAnimation, _colorSelector);
            Storyboard.SetTargetProperty(colorSelectorAnimation, "(ColorEllipse.RenderTransform).(RotateTransform.Angle)");

            _storyboard.Begin();
            isRotating = true;

            Color activeColor;

            activeColor = Utilities.ConvertHSV2RGB((float)Utilities.ClampAngle(_currentAngle));

            _colorSelector.Fill = new SolidColorBrush(activeColor);
        }

        private void CompleteRotation()
        {
            _storyboard.SkipToFill();
            _storyboard.Stop();
            RotateTransform rotateTransform = new RotateTransform()
            {
                Angle = _currentAngle
            };
            _colorSelector.RenderTransform = rotateTransform;
            _storyboard.Children.Clear();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Constructor

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetupController();
            this.RenderUI();
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            if (null == _layoutRoot) { _layoutRoot = (Canvas)this.GetTemplateChild("LayoutRoot"); }

            _dialGrid = new Grid()
            {
                Name = "DialGrid",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Collapsed
            };

            _dial = new Ellipse()
            {
                Name = "Dial",
                Height = DIAL_HEIGHT,
                Width = DIAL_WIDTH,
                Fill = new SolidColorBrush(Colors.Black),
                Opacity = 0.7
            };

            _colorSelector = new Path()
            {
                Name = "ColorEllipse",
                Stroke = new SolidColorBrush(Colors.Gray),
                Fill = new SolidColorBrush(Colors.Red),
                RenderTransformOrigin = new Point(.5, .5),
                StrokeThickness = 2d
            };

            RotateTransform _colorSelectorTransform = new RotateTransform()
            {
                Angle = DIAL_INITIAL_ANGLE
            };
            _colorSelector.RenderTransform = _colorSelectorTransform;
            _colorSelector.Data = new EllipseGeometry()
            {
                Center = new Point(SELECTOR_CENTER, SELECTOR_CENTER),
                RadiusX = SELECTOR_RADIUS,
                RadiusY = SELECTOR_RADIUS
            };

            _storyboard = new Storyboard();

            _storyboard.Completed += Storyboard_Completed;

            _currentAngle = DIAL_INITIAL_ANGLE;

            _paletteRing = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Color current = Utilities.ConvertHSV2RGB(0);

            for (int sliceCount = 1; sliceCount <= 360; sliceCount++)
            {
                RotateTransform rt = new RotateTransform() { Angle = sliceCount };

                LinearGradientBrush colorBrush = new LinearGradientBrush();

                colorBrush.GradientStops.Add(new GradientStop() { Color = current });

                current = Utilities.ConvertHSV2RGB(sliceCount);

                colorBrush.GradientStops.Add(new GradientStop() { Color = current, Offset = 1 });

                Path line = DrawColorLine(rt, colorBrush);

                _paletteRing.Children.Add(line);
            }

            _dialGrid.Children.Add(_dial);
            _dialGrid.Children.Add(_paletteRing);
            _dialGrid.Children.Add(_colorSelector);

            _layoutRoot.Children.Add(_dialGrid);
        }

        // Builds color wheel
        private static Path DrawColorLine(RotateTransform rt, LinearGradientBrush colorBrush)
        {
            double _colorLineEnd = COLOR_LINE_RADIUS + COLOR_LINE_THICKNESS;
            return new Path()
            {
                Data = new LineGeometry()
                {
                    StartPoint = new Point(COLOR_LINE_RADIUS, COLOR_LINE_RADIUS),
                    EndPoint = new Point(_colorLineEnd, _colorLineEnd)
                },
                StrokeThickness = 3,
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = rt,
                Stroke = colorBrush
            };
        }

        #endregion
    }
}
