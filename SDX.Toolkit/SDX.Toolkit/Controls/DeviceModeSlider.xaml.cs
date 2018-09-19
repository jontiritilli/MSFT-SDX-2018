using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using SDX.Toolkit.Helpers;


namespace SDX.Toolkit.Controls
{
    public enum DeviceModeSliderSnapPositions
    {
        Studio,
        Laptop,
        Tablet,
    }

    public sealed class DeviceModeSliderValueEventArgs : EventArgs
    {
        public double OldValue;
        public double NewValue;
    }

    public sealed class DeviceModeSliderSnappedEventArgs : EventArgs
    {
        public DeviceModeSliderSnapPositions SnapPosition;
    }

    public sealed partial class DeviceModeSlider : UserControl
    {
        #region Constants

        private const double SLIDER_MINIMUM = 0;
        private const double SLIDER_MAXIMUM = 100;
        private const double MILLISECONDS_PER_VALUE = 50;

        private const double THUMB_WIDTH = 25;  // MUST KEEP IN SYNC WITH XAML VALUE

        #endregion


        #region Private Members

        #endregion


        #region Construction

        public DeviceModeSlider()
        {
            this.InitializeComponent();

            this.Loaded += this.DeviceModeSlider_Loaded;

            // which device are we?
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    break;

                case DeviceType.Book13:
                default:
                    break;
            }

            // set defaults
            this.Position = DeviceModeSliderSnapPositions.Studio;
        }

        private void DeviceModeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 3, 8, Colors.Purple);
            //TestHelper.AddGridCellBorders(this.BatteryGrid, 1, 3, Colors.CornflowerBlue);
        }

        #endregion


        #region Public Properties

        public DeviceModeSliderSnapPositions Position { get; private set; }

        #endregion


        #region Public Methods

        public void SnapTo(DeviceModeSliderSnapPositions snapPosition)
        {
            double endingValue = SLIDER_MINIMUM;    // default to setting to the leftmost position

            // calculate the eneding value and update the battery image
            switch (snapPosition)
            {
                case DeviceModeSliderSnapPositions.Studio:
                    endingValue = SLIDER_MINIMUM;
                    break;

                case DeviceModeSliderSnapPositions.Laptop:
                    endingValue = SLIDER_MINIMUM + ((SLIDER_MAXIMUM - SLIDER_MINIMUM) / 2.0);
                    break;

                case DeviceModeSliderSnapPositions.Tablet:
                    endingValue = SLIDER_MAXIMUM;
                    break;
            }

            // save the position
            this.Position = snapPosition;

            // save the value
            this.Value = endingValue;

            // update the thumb
            UpdateThumb(GetPositionFromValue(endingValue));

            // raise the snapped event
            this.RaiseSnappedEvent(this, snapPosition);

            // telemetry
            //TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_PRODUCTROTATION, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion


        #region Dependency Properties

        // Value
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(DeviceModeSlider), new PropertyMetadata(0.0, OnValuePropertyChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // StudioCaptionText
        public static readonly DependencyProperty StudioCaptionTextProperty =
            DependencyProperty.Register("StudioCaptionText", typeof(string), typeof(DeviceModeSlider), new PropertyMetadata(String.Empty));

        public string StudioCaptionText
        {
            get => (string)GetValue(StudioCaptionTextProperty);
            set => SetValue(StudioCaptionTextProperty, value);
        }

        // LaptopCaptionText
        public static readonly DependencyProperty LaptopCaptionTextProperty =
            DependencyProperty.Register("LaptopCaptionText", typeof(string), typeof(DeviceModeSlider), new PropertyMetadata(String.Empty));

        public string LaptopCaptionText
        {
            get => (string)GetValue(LaptopCaptionTextProperty);
            set => SetValue(LaptopCaptionTextProperty, value);
        }

        // TabletCaptionText
        public static readonly DependencyProperty TabletCaptionTextProperty =
            DependencyProperty.Register("TabletCaptionText", typeof(string), typeof(DeviceModeSlider), new PropertyMetadata(String.Empty));

        public string TabletCaptionText
        {
            get => (string)GetValue(TabletCaptionTextProperty);
            set => SetValue(TabletCaptionTextProperty, value);
        }

        #endregion


        #region Event Handlers

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DeviceModeSlider slider)
            {
                var newValue = (double)e.NewValue;

                if (Double.IsNaN(newValue)) { return; }

                newValue = Math.Max(SLIDER_MINIMUM, newValue);
                newValue = Math.Min(SLIDER_MAXIMUM, newValue);

                slider.UpdateThumb(slider.GetPositionFromValue(newValue));

                slider.RaiseValueChangedEvent(slider, new DeviceModeSliderValueEventArgs() { NewValue = newValue, OldValue = (double)e.OldValue });
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // get the grid
            if (sender is Grid grid)
            {
                // get the tap position
                Point tapPoint = e.GetPosition(this);

                if (null != tapPoint)
                {
                    // get our starting value; where we are now
                    double startingValue = this.Value;

                    // get the position of the tap in relation to the ContainerCanvas
                    GeneralTransform ttv = grid.TransformToVisual(this.ContainerCanvas);
                    Point p = ttv.TransformPoint(tapPoint);

                    // get the tap x
                    double X = p.X;

                    // calculate the snap-to position from the position of the tap
                    DeviceModeSliderSnapPositions snapPosition = CalculateSnapToFromPosition(X);

                    // snap to the destination
                    this.SnapTo(snapPosition);
                }
            }
        }

        private void StudioTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DeviceModeSliderSnapPositions.Studio);
        }

        private void LaptopTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DeviceModeSliderSnapPositions.Laptop);
        }

        private void TabletTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DeviceModeSliderSnapPositions.Tablet);
        }

        private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateThumb(GetPositionFromValue(Value));
        }

        private void Thumb_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // get where we are
            double currentPosition = this.ManipulationTranslation.X;

            // get where they want to go
            double offset = e.Delta.Translation.X;

            // calculate where we will actually go
            double nextPosition = currentPosition + offset;
            nextPosition = Math.Max(0, nextPosition);
            nextPosition = Math.Min(ContainerCanvas.ActualWidth, nextPosition);

            // update the thumb
            UpdateThumb(nextPosition, true);

            // set our new value
            Value = GetValueFromPosition(nextPosition);
        }

        private void Thumb_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // once the manipulation ends, we need to snap to one of the stop points

            // get the current X position; this will have been updated by Thumb_ManipulationDelta
            double X = this.ManipulationTranslation.X;

            // calculate the snap position from the current X position
            DeviceModeSliderSnapPositions snapPosition = CalculateSnapToFromPosition(X);

            // snap to the destination
            SnapTo(snapPosition);
        }

        #endregion


        #region Public Events

        // ValueChanged
        public delegate void ValueChangedEventHandler(object sender, DeviceModeSliderValueEventArgs e);

        public event ValueChangedEventHandler ValueChanged;

        private void RaiseValueChangedEvent(DeviceModeSlider deviceModeSlider, DeviceModeSliderValueEventArgs e)
        {
            ValueChanged?.Invoke(deviceModeSlider, e);
        }

        private void RaiseValueChangedEvent(DeviceModeSlider deviceModeSlider, double oldValue, double newValue)
        {
            DeviceModeSliderValueEventArgs args = new DeviceModeSliderValueEventArgs()
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            this.RaiseValueChangedEvent(deviceModeSlider, args);
        }

        // Snapped
        public delegate void SnappedEventHandler(object sender, DeviceModeSliderSnappedEventArgs e);

        public event SnappedEventHandler Snapped;

        private void RaiseSnappedEvent(DeviceModeSlider daySlider, DeviceModeSliderSnappedEventArgs e)
        {
            Snapped?.Invoke(daySlider, e);
        }

        private void RaiseSnappedEvent(DeviceModeSlider daySlider, DeviceModeSliderSnapPositions snapPosition)
        {
            DeviceModeSliderSnappedEventArgs args = new DeviceModeSliderSnappedEventArgs()
            {
                SnapPosition = snapPosition
            };

            this.RaiseSnappedEvent(daySlider, args);
        }


        #endregion


        #region UI Methods

        private void UpdateThumb(double position, bool update = false)
        {
            if (ContainerCanvas != null)
            {
                if (update || !Thumb.IsDragging)
                {
                    this.ManipulationTranslation.X = position;
                }
            }
        }

        private DeviceModeSliderSnapPositions CalculateSnapToFromPosition(double currentX)
        {
            DeviceModeSliderSnapPositions snapPosition;

            // calculate stops
            double studioStop = 0;
            double tabletStop = (ContainerCanvas.ActualWidth);
            double laptopStop = (tabletStop - studioStop) / 2;

            // calculate attractor boundaries
            double studioLaptopAttractor = laptopStop * 0.33;
            double laptopTabletAttractor = studioStop + ((tabletStop - laptopStop) * 0.33);

            if (currentX < studioLaptopAttractor)
            {
                // move to the morning stop
                snapPosition = DeviceModeSliderSnapPositions.Studio;
            }
            else if (currentX < laptopTabletAttractor)
            {
                // move to the afternoon stop
                snapPosition = DeviceModeSliderSnapPositions.Laptop;
            }
            else
            {
                // move to the night stop
                snapPosition = DeviceModeSliderSnapPositions.Tablet;
            }

            return snapPosition;
        }

        private double GetPositionFromValue(double value)
        {
            //return (((value - SLIDER_MINIMUM) / (SLIDER_MAXIMUM - SLIDER_MINIMUM)) * (ContainerCanvas.ActualWidth - THUMB_WIDTH));
            return (((value - SLIDER_MINIMUM) / (SLIDER_MAXIMUM - SLIDER_MINIMUM)) * ContainerCanvas.ActualWidth);
            //return (((value - SLIDER_MINIMUM) / (SLIDER_MAXIMUM - SLIDER_MINIMUM)) * (ContainerCanvas.ActualWidth - (THUMB_WIDTH / 2)));
        }

        private double GetValueFromPosition(double position)
        {
            //return SLIDER_MINIMUM + ((position / (ContainerCanvas.ActualWidth - THUMB_WIDTH)) * (SLIDER_MAXIMUM - SLIDER_MINIMUM));
            return SLIDER_MINIMUM + ((position / ContainerCanvas.ActualWidth) * (SLIDER_MAXIMUM - SLIDER_MINIMUM));
            //return SLIDER_MINIMUM + ((position / (ContainerCanvas.ActualWidth - (THUMB_WIDTH / 2))) * (SLIDER_MAXIMUM - SLIDER_MINIMUM));
        }

        #endregion
    }
}
