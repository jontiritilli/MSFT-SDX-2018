using System;
using SDX.Toolkit.Helpers;
using Windows.Foundation;
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
    [TemplatePart(Name = "DialCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "Dial", Type = typeof(Ellipse))]
    [TemplatePart(Name = "ColorRay", Type = typeof(Path))]
    [TemplatePart(Name = "ColorEllipse", Type = typeof(Path))]
    [TemplatePart(Name = "SelectedColorBorder", Type = typeof(Border))]
    [TemplatePart(Name = "CurrentColorBorder", Type = typeof(Border))]
    public sealed class SurfaceDial : Control
    {

        public enum ColorSelectionMode { Value, Saturation }

        public ColorSelectionMode SelectionMode;

        float mainColorAngle;
        float activeSaturationValue;
        Grid PaletteRing;
        Grid SaturationPaletteRing;
        Storyboard storyboard;
        bool isRotating;
        double currentAngle;
        double dialCenterX;
        double dialCenterY;

        #region Private Members

        private Grid layoutRoot;
        private Canvas dialCanvas;
        private Ellipse dial;
        private Path colorRay;
        private Path colorEllipse;
        private Border selectedColorBorder;
        private Border currentColorBorder;

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
            if (Controller == null) Controller = RadialController.CreateForCurrentView();
            // Set rotation degrees
            Controller.RotationResolutionInDegrees = 30;

            // Bind dial controls to local methods
            Controller.RotationChanged += Controller_RotationChanged;
            Controller.ButtonClicked += Controller_ButtonClicked;
            Controller.ScreenContactStarted += Controller_ScreenContactStarted;
            Controller.ScreenContactContinued += Controller_ScreenContactContinued;
            Controller.ScreenContactEnded += Controller_ScreenContactEnded;
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
        private async void Controller_ScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                dialCenterX = args.Contact.Position.X;
                dialCenterY = args.Contact.Position.Y;

                Canvas.SetLeft(dial, args.Contact.Position.X - 150);
                Canvas.SetTop(dial, args.Contact.Position.Y - 150);

                dialCanvas.Visibility = Visibility.Visible;
            });
        }

        // Dial lifted and placed again
        private async void Controller_ScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (IsActive)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    dialCenterX = args.Contact.Position.X;
                    dialCenterY = args.Contact.Position.Y;

                    Canvas.SetLeft(dial, args.Contact.Position.X - 150);
                    Canvas.SetTop(dial, args.Contact.Position.Y - 150);
                });
            }
        }

        // Dial screen contact ended and placed again
        private async void Controller_ScreenContactEnded(RadialController sender, object args)
        {
            if (IsActive)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    dialCanvas.Visibility = Visibility.Collapsed;
                });
            }
        }

        // Dial button pressed
        private void Controller_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            if (IsActive)
            {
                if (SelectionMode == ColorSelectionMode.Value)
                {
                    mainColorAngle = (float)Utilities.ClampAngle(currentAngle);
                    selectedColorBorder.Background = new SolidColorBrush(Utilities.ConvertHSV2RGB(mainColorAngle, 1, 1));
                    SelectionMode = ColorSelectionMode.Saturation;
                }
                else
                {
                    selectedColorBorder.Background = new SolidColorBrush(Utilities.ConvertHSV2RGB(mainColorAngle, 1 - Math.Abs(activeSaturationValue), 1));
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
                PaletteRing.Visibility = Visibility.Visible;
                SaturationPaletteRing.Visibility = Visibility.Collapsed;
            }
            else
            {
                PaletteRing.Visibility = Visibility.Collapsed;
                FillSaturationRing();
                SaturationPaletteRing.Visibility = Visibility.Visible;
            }
        }

        // Show saturation selector for selected color
        private void FillSaturationRing()
        {
            SaturationPaletteRing.Children.Clear();

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
                SaturationPaletteRing.Children.Add(line);
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
                double destinationAngle = Utilities.ClampAngle(currentAngle + angle);

                if (!IsInSaturationBounds(destinationAngle))
                {
                    return;
                }
                activeSaturationValue += (float)angle / 100;
            }

            currentAngle += angle;
            rayAnimation.To = currentAngle;
            rayAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            DoubleAnimation colorEllipseAnimation = new DoubleAnimation();
            colorEllipseAnimation.To = currentAngle;
            colorEllipseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            storyboard.Children.Add(rayAnimation);
            Storyboard.SetTarget(rayAnimation, colorRay);
            Storyboard.SetTargetProperty(rayAnimation, "(ColorRay.RenderTransform).(RotateTransform.Angle)");

            storyboard.Children.Add(colorEllipseAnimation);
            Storyboard.SetTarget(colorEllipseAnimation, colorEllipse);
            Storyboard.SetTargetProperty(colorEllipseAnimation, "(ColorEllipse.RenderTransform).(RotateTransform.Angle)");

            storyboard.Begin();
            isRotating = true;

            Color activeColor;
            if (SelectionMode == ColorSelectionMode.Value)
            {
                activeColor = Utilities.ConvertHSV2RGB((float)Utilities.ClampAngle(currentAngle), 1, 1);
            }
            else
            {
                activeColor = Utilities.ConvertHSV2RGB(mainColorAngle, 1 - Math.Abs(activeSaturationValue), 1);
            }

            colorEllipse.Fill = currentColorBorder.Background = new SolidColorBrush(activeColor);
        }

        private void CompleteRotation()
        {
            storyboard.SkipToFill();
            storyboard.Stop();
            RotateTransform rotateTransform = new RotateTransform() { Angle = currentAngle };
            colorRay.RenderTransform = rotateTransform;
            colorEllipse.RenderTransform = rotateTransform;
            storyboard.Children.Clear();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Constructor

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.RenderUI();
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            dialCanvas = GetTemplateChild("DialCanvas") as Canvas;
            dial = GetTemplateChild("Dial") as Ellipse;
            colorEllipse = GetTemplateChild("ColorEllipse") as Path;
            colorRay = GetTemplateChild("ColorRay") as Path;
            currentColorBorder = GetTemplateChild("CurrentColorBorder") as Border;
            selectedColorBorder = GetTemplateChild("SelectedColorBorder") as Border;

            dialCanvas.Visibility = Visibility.Collapsed;

            storyboard = new Storyboard();
            storyboard.Completed += Storyboard_Completed;
            currentAngle = 0;
            colorRay.Data = new LineGeometry() { StartPoint = new Point(200, 200), EndPoint = new Point(400, 200) };
            colorEllipse.Data = new EllipseGeometry() { Center = new Point(400, 200), RadiusX = 10, RadiusY = 10 };

            SelectionMode = ColorSelectionMode.Value;

            PaletteRing = new Grid();

            PaletteRing.Width = 400;

            PaletteRing.Height = 400;
            Color current = Utilities.ConvertHSV2RGB(0, 1, 1);
            for (int i = 0; i < 360; i++)
            {
                RotateTransform rt = new RotateTransform() { Angle = i };

                LinearGradientBrush colorBrush = new LinearGradientBrush();

                colorBrush.GradientStops.Add(new GradientStop() { Color = current });

                current = Utilities.ConvertHSV2RGB(i, 1, 1);

                colorBrush.GradientStops.Add(new GradientStop() { Color = current, Offset = 1 });

                Path line = DrawColorLine(rt, colorBrush);

                PaletteRing.Children.Add(line);
            }
            PaletteRing.HorizontalAlignment = HorizontalAlignment.Center;

            PaletteRing.VerticalAlignment = VerticalAlignment.Center;

            // need to append color wheel to dial canvas
            dial.Child = PaletteRing;

            SaturationPaletteRing = new Grid()
            {
                Width = 375,
                Height = 375,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            dialCanvas.Children.Add(SaturationPaletteRing);
        }

        // Builds color wheel
        private static Path DrawColorLine(RotateTransform rt, LinearGradientBrush colorBrush)
        {
            return new Path()
            {
                Data = new LineGeometry() { StartPoint = FindLineStart(280, 280), EndPoint = FindLineStart(300, 300) },
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
