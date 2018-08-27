using System;
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
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "SelectedColorBorder", Type = typeof(Border))]
    [TemplatePart(Name = "CurrentColorBorder", Type = typeof(Border))]
    public sealed class SurfaceDial : Control
    {

        public enum ColorSelectionMode { Value, Saturation }

        public ColorSelectionMode SelectionMode;

        public float mainColorAngle;
        public float activeSaturationValue;
        public Grid _paletteRing;
        public Grid _saturationPaletteRing;
        public Storyboard _storyboard;
        public bool isRotating;
        public double _currentAngle;

        #region Private Members

        private Grid _layoutRoot;
        private Grid _dialGrid;
        private Canvas _dialCanvas;
        private Ellipse _dial;
        private Path _colorSelectorEllipse;
        private Border _selectedColorBorder;
        private Border _currentColorBorder;
        private RadialControllerMenuItem _screenColorMenuItem;

        #endregion

        #region Constants

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(SurfaceDial), new PropertyMetadata(false, IsActiveChangedCallback));

        public static readonly DependencyProperty CurrentColorProperty =
           DependencyProperty.Register(nameof(CurrentColor), typeof(Color), typeof(SurfaceDial), new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty SelectedColorProperty =
           DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(SurfaceDial), new PropertyMetadata(Colors.White));


        private bool IsInSaturationBounds(double destinationAngle)
        {
            if (mainColorAngle >= 100)
            {
                return (destinationAngle > mainColorAngle ||
                    destinationAngle < Utilities.ClampAngle(mainColorAngle - 100)) ? false : true;
            }
            else
            {
                return (destinationAngle < mainColorAngle ||
                    (destinationAngle > mainColorAngle && destinationAngle > Utilities.ClampAngle(mainColorAngle - 100))) ? true : false;
            }
        }

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
            Controller.RotationResolutionInDegrees = 30;

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
            if (IsActive)
            {
                Rotate(args.RotationDeltaInDegrees);
            }
        }

        // Dial Screen contact initial
        private void Controller_ScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            Canvas.SetLeft(_dialCanvas, args.Contact.Position.X - 150);
            Canvas.SetTop(_dialCanvas, args.Contact.Position.Y - 150);

            _dialCanvas.Visibility = Visibility.Visible;
        }

        // Dial lifted and placed again
        private void Controller_ScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (IsActive)
            {
                Canvas.SetLeft(_dialCanvas, args.Contact.Position.X - 150);
                Canvas.SetTop(_dialCanvas, args.Contact.Position.Y - 150);
            }
        }

        // Dial screen contact ended and placed again
        private void Controller_ScreenContactEnded(RadialController sender, object args)
        {
            if (IsActive)
            {
                _dialCanvas.Visibility = Visibility.Collapsed;
            }
        }

        // Dial button pressed
        private void Controller_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            if (IsActive)
            {
                if (SelectionMode == ColorSelectionMode.Value)
                {
                    mainColorAngle = (float)Utilities.ClampAngle(_currentAngle);
                    _selectedColorBorder.Background = new SolidColorBrush(Utilities.ConvertHSV2RGB(mainColorAngle, 1, 1));
                    SelectionMode = ColorSelectionMode.Saturation;
                }
                else
                {
                    _selectedColorBorder.Background = new SolidColorBrush(Utilities.ConvertHSV2RGB(mainColorAngle, 1 - Math.Abs(activeSaturationValue), 1));
                    SelectionMode = ColorSelectionMode.Value;
                    activeSaturationValue = 0;
                }
                FillSelectionRing();
            }
        }

        // Show or hide selection and saturation rings
        private void FillSelectionRing()
        {
            if (SelectionMode == ColorSelectionMode.Value)
            {
                _paletteRing.Visibility = Visibility.Visible;
                _saturationPaletteRing.Visibility = Visibility.Collapsed;
            }
            else
            {
                _paletteRing.Visibility = Visibility.Collapsed;
                FillSaturationRing();
                _saturationPaletteRing.Visibility = Visibility.Visible;
            }
        }

        // Show saturation selector for selected color
        private void FillSaturationRing()
        {
            _saturationPaletteRing.Children.Clear();

            Color current = Utilities.ConvertHSV2RGB(mainColorAngle, 0, 1);

            double rotationAngle = Utilities.ClampAngle(mainColorAngle - 100);

            for (int i = 0; i < 100; i++)
            {
                RotateTransform rt = new RotateTransform() { Angle = Utilities.ClampAngle(rotationAngle) };
                rotationAngle++;
                LinearGradientBrush colorBrush = new LinearGradientBrush();
                colorBrush.GradientStops.Add(new GradientStop() { Color = current });
                current = Utilities.ConvertHSV2RGB(mainColorAngle, i * 0.01f, 1);
                colorBrush.GradientStops.Add(new GradientStop() { Color = current, Offset = 1 });
                Path line = DrawColorLine(rt, colorBrush);
                _saturationPaletteRing.Children.Add(line);
            }
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

            DoubleAnimation rayAnimation = new DoubleAnimation();

            if (SelectionMode == ColorSelectionMode.Saturation)
            {
                double destinationAngle = Utilities.ClampAngle(_currentAngle + angle);

                if (!IsInSaturationBounds(destinationAngle))
                {
                    return;
                }
                activeSaturationValue += (float)angle / 100;
            }

            _currentAngle += angle;
            rayAnimation.To = _currentAngle;
            rayAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            DoubleAnimation colorEllipseAnimation = new DoubleAnimation();
            colorEllipseAnimation.To = _currentAngle;
            colorEllipseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            _storyboard.Children.Add(colorEllipseAnimation);
            Storyboard.SetTarget(colorEllipseAnimation, _colorSelectorEllipse);
            Storyboard.SetTargetProperty(colorEllipseAnimation, "(ColorEllipse.RenderTransform).(RotateTransform.Angle)");

            _storyboard.Begin();
            isRotating = true;

            Color activeColor;
            if (SelectionMode == ColorSelectionMode.Value)
            {
                activeColor = Utilities.ConvertHSV2RGB((float)Utilities.ClampAngle(_currentAngle), 1, 1);
            }
            else
            {
                activeColor = Utilities.ConvertHSV2RGB(mainColorAngle, 1 - Math.Abs(activeSaturationValue), 1);
            }

            _colorSelectorEllipse.Fill = _currentColorBorder.Background = new SolidColorBrush(activeColor);
        }

        private void CompleteRotation()
        {
            _storyboard.SkipToFill();
            _storyboard.Stop();
            RotateTransform rotateTransform = new RotateTransform() { Angle = _currentAngle };
            _colorSelectorEllipse.RenderTransform = rotateTransform;
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
            if (null == _layoutRoot) { _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot"); }
            if (null == _currentColorBorder) { _currentColorBorder = (Border)this.GetTemplateChild("CurrentColorBorder"); }
            if (null == _selectedColorBorder) { _selectedColorBorder = (Border)this.GetTemplateChild("SelectedColorBorder"); }

            _dialCanvas = new Canvas()
            {
                Name = "DialCanvas"
            };

            _dialGrid = new Grid()
            {
                Name = "DialGrid"
            }; ;
            _dialGrid.HorizontalAlignment = HorizontalAlignment.Center;
            _dialGrid.VerticalAlignment = VerticalAlignment.Center;

            _dial = new Ellipse()
            {
                Name = "Dial",
                Height = 400,
                Width = 400,
                Fill = new SolidColorBrush(Colors.Black),
                Opacity = 0.7
            };

            _colorSelectorEllipse = new Path()
            {
                Name = "ColorEllipse",
                Stroke = new SolidColorBrush(Colors.Gray),
                Fill = new SolidColorBrush(Colors.Red),
                RenderTransformOrigin = new Point(.5,.5),
                StrokeThickness = 2d
            };

            RotateTransform _colorEllipseTransform = new RotateTransform();
            _colorEllipseTransform.Angle = 0;
            _colorSelectorEllipse.RenderTransform = _colorEllipseTransform;
            _colorSelectorEllipse.Data = new EllipseGeometry() {
                RadiusX = 10, RadiusY = 10
            };

            _dialCanvas.Visibility = Visibility.Collapsed;

            _storyboard = new Storyboard();
            _storyboard.Completed += Storyboard_Completed;
            _currentAngle = 0;


            SelectionMode = ColorSelectionMode.Value;

            _paletteRing = new Grid();

            Color current = Utilities.ConvertHSV2RGB(0, 1, 1);
            for (int i = 0; i < 360; i++)
            {
                RotateTransform rt = new RotateTransform() { Angle = i };

                LinearGradientBrush colorBrush = new LinearGradientBrush();

                colorBrush.GradientStops.Add(new GradientStop() { Color = current });

                current = Utilities.ConvertHSV2RGB(i, 1, 1);

                colorBrush.GradientStops.Add(new GradientStop() { Color = current, Offset = 1 });

                Path line = DrawColorLine(rt, colorBrush);

                _paletteRing.Children.Add(line);
            }

            _paletteRing.HorizontalAlignment = HorizontalAlignment.Center;
            _paletteRing.VerticalAlignment = VerticalAlignment.Center;

            _saturationPaletteRing = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            _dialGrid.Children.Add(_dial);
            _dialGrid.Children.Add(_paletteRing);
            _dialGrid.Children.Add(_saturationPaletteRing);

            _dialCanvas.Children.Add(_dialGrid);
            _layoutRoot.Children.Add(_dialCanvas);
        }

        // Builds color wheel
        private static Path DrawColorLine(RotateTransform rt, LinearGradientBrush colorBrush)
        {
            return new Path()
            {
                Data = new LineGeometry() { StartPoint = FindLineStart(300, 300), EndPoint = FindLineStart(310, 310) },
                StrokeThickness = 3,
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = rt,
                Stroke = colorBrush
            };
        }

        private void UpdateUI()
        {

        }

        #endregion

    }
}
