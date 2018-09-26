using System;
using System.Diagnostics;
using SDX.Toolkit.Converters;
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
        public bool isRotating;
        public double _currentAngle;
        private int prevColorId = 0;

        #region Private Members

        private Grid _paletteRing;
        private Storyboard _storyboard;
        private Canvas _layoutRoot;
        private Grid _dialGrid;
        private Ellipse _dial;
        private Path _colorSelector;
        private RadialControllerMenuItem _brushColorMenuItem;

        #endregion

        #region Constants

        private static readonly double DIAL_DIAMETER = StyleHelper.GetApplicationDouble("DialDiameter");
        private static readonly double DIAL_RADIUS = DIAL_DIAMETER / 2;
        private static readonly double SELECTOR_RADIUS = StyleHelper.GetApplicationDouble("SelectorDiameter");
        private static readonly double COLOR_RING_THICKNESS = StyleHelper.GetApplicationDouble("ColorRingThickness");
        private static readonly double COLOR_RING_INNER_DIAMETER = DIAL_DIAMETER - COLOR_RING_THICKNESS;
        private static readonly double SELECTOR_CENTER = StyleHelper.GetApplicationDouble("SelectorCenterOffset");
        private static readonly double ROTATION_DEGREES = 36d;
        private static readonly double DIAL_INITIAL_ANGLE = 36d;
        private static readonly double ROTATION_MODIFIER = DIAL_INITIAL_ANGLE - ROTATION_DEGREES;
        private static readonly double TOTAL_ROTATION = ROTATION_DEGREES + ROTATION_MODIFIER;

        #endregion

        #region Dependency Properties

        // color id
        public static readonly DependencyProperty ColorIDProperty =
            DependencyProperty.Register("ColorID", typeof(int), typeof(SurfaceDial), new PropertyMetadata(0, OnColorIDChanged));

        public int ColorID
        {
            get { return (int)GetValue(ColorIDProperty); }
            set { SetValue(ColorIDProperty, value); }
        }

        // dial contact occurred
        public static readonly DependencyProperty DialScreenContactStartedProperty =
            DependencyProperty.Register("DialScreenContactStarted", typeof(bool), typeof(SurfaceDial), new PropertyMetadata(false));

        public bool DialScreenContactStarted
        {
            get { return (bool)GetValue(DialScreenContactStartedProperty); }
            set { SetValue(DialScreenContactStartedProperty, value); }
        }

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

        public void ActivateOnNavigate()
        {
            IsActive = true;
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

        private void SetupController()
        {
            if (null == Controller)
            {
                Controller = RadialController.CreateForCurrentView();
            }

            RadialControllerConfiguration _dialConfiguration = RadialControllerConfiguration.GetForCurrentView();

            // Remove standard menu items
            _dialConfiguration.SetDefaultMenuItems(new RadialControllerSystemMenuItemKind[] { });

            // Create an icon for the custom tool.
            RandomAccessStreamReference icon =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/List/specs_creative.png"));

            // Create a menu item for the custom tool.
            _brushColorMenuItem = RadialControllerMenuItem.CreateFromIcon("Brush Color", icon);

            // Add the custom tool to the RadialController menu.
            Controller.Menu.Items.Add(_brushColorMenuItem);

            // Set rotation degrees
            Controller.RotationResolutionInDegrees = ROTATION_DEGREES;

            // Bind dial controls to local methods
            _brushColorMenuItem.Invoked += ColorMenuItem_Invoked;
            Controller.RotationChanged += Controller_RotationChanged;
            Controller.ButtonClicked += Controller_ButtonClicked;
            Controller.ScreenContactStarted += Controller_ScreenContactStarted;
            Controller.ScreenContactContinued += Controller_ScreenContactContinued;
            Controller.ScreenContactEnded += Controller_ScreenContactEnded;
        }

        private void ColorMenuItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            if (!IsActive)
            {
                IsActive = true;
            }
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
            RaiseOnDialScreenContactEvent(this);

            Controller.Menu.Items.Remove(_brushColorMenuItem);
            Canvas.SetLeft(_dialGrid, args.Contact.Position.X - DIAL_RADIUS);
            Canvas.SetTop(_dialGrid, args.Contact.Position.Y - DIAL_RADIUS);

            _dialGrid.Opacity = 1.0d;
        }

        // Dial lifted and placed again
        private void Controller_ScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (IsActive)
            {
                Canvas.SetLeft(_dialGrid, args.Contact.Position.X - DIAL_RADIUS);
                Canvas.SetTop(_dialGrid, args.Contact.Position.Y - DIAL_RADIUS);
            }
        }

        // Dial screen contact ended and placed again
        private void Controller_ScreenContactEnded(RadialController sender, object args)
        {
            if (IsActive)
            {
                _dialGrid.Opacity = 0.0d;
            }
        }

        // Dial button pressed
        private void Controller_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            Controller.Menu.Items.Remove(_brushColorMenuItem);
        }

        private void Storyboard_Completed(object sender, object e)
        {
            isRotating = false;
            CompleteRotation();
        }

        private void Rotate(double angle)
        {
            double RotationInterval = angle;

            _currentAngle = (double)Utilities.ClampAngle(_currentAngle + RotationInterval);

            if (isRotating)
            {
                CompleteRotation();
                prevColorId = ColorID;
                UpdateColorId();
            }

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

        private void UpdateColorId()
        {
            double angleToIndex = _currentAngle / 64.8;

            ColorID = System.Convert.ToInt32(Math.Round((double)angleToIndex, 0)) - 1;
        }

        #endregion

        #region Custom Events

        public delegate void OnDialScreenContactedEvent(object sender, EventArgs e);

        public event OnDialScreenContactedEvent OnDialScreenContactStarted;

        private void RaiseOnDialScreenContactEvent(SurfaceDial Dial, EventArgs e)
        {
            OnDialScreenContactStarted?.Invoke(Dial, e);
        }

        private void RaiseOnDialScreenContactEvent(SurfaceDial Dial)
        {
            this.RaiseOnDialScreenContactEvent(Dial, new EventArgs());
        }

        public delegate void OnColorIDChangedEvent(object sender, EventArgs e);

        public event OnColorIDChangedEvent ColorIDChanged;

        private void RaiseColorIDChangedEvent(SurfaceDial Dial, EventArgs e)
        {
            ColorIDChanged?.Invoke(Dial, e);
        }

        private void RaiseColorIDChangedEvent(SurfaceDial Dial)
        {
            this.RaiseColorIDChangedEvent(Dial, new EventArgs());
        }

        #endregion

        #region Event Handlers

        public void ForceRotation(int ID)
        {
            this.ColorID = ID;
            prevColorId = ColorID;

            if (isRotating)
            {
                CompleteRotation();
            }

            _currentAngle = (ColorID * 72) + ROTATION_DEGREES;

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

        public static void OnColorIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is SurfaceDial dial)
            {
                // raise the changed event
                dial.RaiseColorIDChangedEvent(dial);
            }
        }

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
                Opacity = 0.0d
            };

            _dial = new Ellipse()
            {
                Name = "DialBG",
                Height = DIAL_DIAMETER,
                Width = DIAL_DIAMETER,
                Fill = new SolidColorBrush(Colors.Black),
                Opacity = 0.9
            };

            _colorSelector = new Path()
            {
                Name = "ColorSelectorEllipse",
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
                Name="ColorRing",
                Height = DIAL_DIAMETER - COLOR_RING_THICKNESS,
                Width = DIAL_DIAMETER - COLOR_RING_THICKNESS,
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
            double _colorLineStart = SELECTOR_CENTER + 7;
            double _colorLineEnd = _colorLineStart + COLOR_RING_THICKNESS;

            return new Path()
            {
                Data = new LineGeometry()
                {
                    StartPoint = new Point(_colorLineStart, _colorLineStart),
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
