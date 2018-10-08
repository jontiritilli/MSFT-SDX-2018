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
    public enum DaySliderSnapPositions
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }

    public sealed class DaySliderValueEventArgs : EventArgs
    {
        public double OldValue;
        public double NewValue;
    }

    public sealed class DaySliderSnappedEventArgs : EventArgs
    {
        public DaySliderSnapPositions SnapPosition;
    }

    public sealed partial class DaySlider : UserControl
    {
        #region Constants

        private const double SLIDER_MINIMUM = 0;
        private const double SLIDER_MAXIMUM = 100;
        private const double MILLISECONDS_PER_VALUE = 50;

        private const double THUMB_WIDTH = 25;  // MUST KEEP IN SYNC WITH XAML VALUE

        private const string URI_ICON_DAY_13 = "ms-appx:///Assets/DaySlider/sb2_13_sun.png";
        private const string URI_ICON_NIGHT_13 = "ms-appx:///Assets/DaySlider/sb2_13_moon.png";
        private const string URI_ICON_BATTERY_1_13 = "ms-appx:///Assets/DaySlider/sb2_13_battery_1.png";
        private const string URI_ICON_BATTERY_2_13 = "ms-appx:///Assets/DaySlider/sb2_13_battery_2.png";
        private const string URI_ICON_BATTERY_3_13 = "ms-appx:///Assets/DaySlider/sb2_13_battery_3.png";
        private const string URI_ICON_BATTERY_4_13 = "ms-appx:///Assets/DaySlider/sb2_13_battery_4.png";

        private const string URI_ICON_DAY_15 = "ms-appx:///Assets/DaySlider/sb2_15_sun.png";
        private const string URI_ICON_NIGHT_15 = "ms-appx:///Assets/DaySlider/sb2_15_moon.png";
        private const string URI_ICON_BATTERY_1_15 = "ms-appx:///Assets/DaySlider/sb2_15_battery_1.png";
        private const string URI_ICON_BATTERY_2_15 = "ms-appx:///Assets/DaySlider/sb2_15_battery_2.png";
        private const string URI_ICON_BATTERY_3_15 = "ms-appx:///Assets/DaySlider/sb2_15_battery_3.png";
        private const string URI_ICON_BATTERY_4_15 = "ms-appx:///Assets/DaySlider/sb2_15_battery_4.png";

        #endregion


        #region Private Members

        private string uriBattery1;
        private string uriBattery2;
        private string uriBattery3;
        private string uriBattery4;

        #endregion


        #region Construction

        public DaySlider()
        {
            this.InitializeComponent();

            this.Loaded += this.DaySlider_Loaded;

            // which device are we?
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.DayImageUri = URI_ICON_DAY_15;
                    this.NightImageUri = URI_ICON_NIGHT_15;
                    uriBattery1 = URI_ICON_BATTERY_1_15;
                    uriBattery2 = URI_ICON_BATTERY_2_15;
                    uriBattery3 = URI_ICON_BATTERY_3_15;
                    uriBattery4 = URI_ICON_BATTERY_4_15;
                    break;

                case DeviceType.Book13:
                default:
                    this.DayImageUri = URI_ICON_DAY_13;
                    this.NightImageUri = URI_ICON_NIGHT_13;
                    uriBattery1 = URI_ICON_BATTERY_1_13;
                    uriBattery2 = URI_ICON_BATTERY_2_13;
                    uriBattery3 = URI_ICON_BATTERY_3_13;
                    uriBattery4 = URI_ICON_BATTERY_4_13;
                    break;
            }

            // set defaults
            this.Position = DaySliderSnapPositions.Morning;
            this.BatteryImageUri = uriBattery1;
        }

        private void DaySlider_Loaded(object sender, RoutedEventArgs e)
        {
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 3, 8, Colors.Purple);
            //TestHelper.AddGridCellBorders(this.BatteryGrid, 1, 3, Colors.CornflowerBlue);
        }

        #endregion


        #region Public Properties

        public DaySliderSnapPositions Position { get; private set; }

        #endregion


        #region Public Methods

        public void SnapTo(DaySliderSnapPositions snapPosition)
        {
            double endingValue = SLIDER_MINIMUM;    // default to setting to the leftmost position

            // calculate the eneding value and update the battery image
            switch (snapPosition)
            {
                case DaySliderSnapPositions.Morning:
                    endingValue = SLIDER_MINIMUM;
                    this.BatteryImageUri = uriBattery1;
                    break;

                case DaySliderSnapPositions.Afternoon:
                    endingValue = SLIDER_MINIMUM + ((SLIDER_MAXIMUM - SLIDER_MINIMUM) / 3.0);
                    this.BatteryImageUri = uriBattery2;
                    break;

                case DaySliderSnapPositions.Evening:
                    endingValue = SLIDER_MINIMUM + ((SLIDER_MAXIMUM - SLIDER_MINIMUM) * (2.0 / 3.0));
                    this.BatteryImageUri = uriBattery3;
                    break;

                case DaySliderSnapPositions.Night:
                    endingValue = SLIDER_MAXIMUM;
                    this.BatteryImageUri = uriBattery4;
                    break;
            }

            // save the position
            this.Position = snapPosition;

            // save the value
            this.Value = endingValue;

            // update the thumb
            UpdateThumb(GetPositionFromValue(endingValue), true);

            // raise the snapped event
            this.RaiseSnappedEvent(this, snapPosition);

            // telemetry
            //TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_PRODUCTROTATION, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion


        #region Dependency Properties

        // Value
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(DaySlider), new PropertyMetadata(0.0, OnValuePropertyChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // BatteryLifeText
        public static readonly DependencyProperty BatteryLifeTextProperty =
            DependencyProperty.Register("BatteryLifeText", typeof(string), typeof(DaySlider), new PropertyMetadata(String.Empty));

        public string BatteryLifeText
        {
            get => (string)GetValue(BatteryLifeTextProperty);
            set => SetValue(BatteryLifeTextProperty, value);
        }

        // BatteryImageUri
        public static readonly DependencyProperty BatteryImageUriProperty =
            DependencyProperty.Register("BatteryImageUri", typeof(string), typeof(DaySlider), new PropertyMetadata(String.Empty));

        public string BatteryImageUri
        {
            get => (string)GetValue(BatteryImageUriProperty);
            set => SetValue(BatteryImageUriProperty, value);
        }

        // DayImageUri
        public static readonly DependencyProperty DayImageUriProperty =
            DependencyProperty.Register("DayImageUri", typeof(string), typeof(DaySlider), new PropertyMetadata(String.Empty));

        public string DayImageUri
        {
            get => (string)GetValue(DayImageUriProperty);
            set => SetValue(DayImageUriProperty, value);
        }

        // NightImageUri
        public static readonly DependencyProperty NightImageUriProperty =
            DependencyProperty.Register("NightImageUri", typeof(string), typeof(DaySlider), new PropertyMetadata(String.Empty));

        public string NightImageUri
        {
            get => (string)GetValue(NightImageUriProperty);
            set => SetValue(NightImageUriProperty, value);
        }

        #endregion


        #region Event Handlers

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DaySlider slider)
            {
                var newValue = (double)e.NewValue;

                if (Double.IsNaN(newValue)) { return; }

                newValue = Math.Max(SLIDER_MINIMUM, newValue);
                newValue = Math.Min(SLIDER_MAXIMUM, newValue);

                slider.UpdateThumb(slider.GetPositionFromValue(newValue));

                slider.RaiseValueChangedEvent(slider, new DaySliderValueEventArgs() { NewValue = newValue, OldValue = (double)e.OldValue });
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
                    DaySliderSnapPositions snapPosition = CalculateSnapToFromPosition(X);

                    // snap to the destination
                    this.SnapTo(snapPosition);
                }
            }
        }

        private void MorningTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DaySliderSnapPositions.Morning);
        }

        private void AfternoonTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DaySliderSnapPositions.Afternoon);
        }

        private void EveningTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DaySliderSnapPositions.Evening);
        }

        private void NightTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(DaySliderSnapPositions.Night);
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
            DaySliderSnapPositions snapPosition = CalculateSnapToFromPosition(X);

            // snap to the destination
            SnapTo(snapPosition);
        }

        #endregion


        #region Public Events

        // ValueChanged
        public delegate void ValueChangedEventHandler(object sender, DaySliderValueEventArgs e);

        public event ValueChangedEventHandler ValueChanged;

        private void RaiseValueChangedEvent(DaySlider daySlider, DaySliderValueEventArgs e)
        {
            ValueChanged?.Invoke(daySlider, e);
        }

        private void RaiseValueChangedEvent(DaySlider daySlider, double oldValue, double newValue)
        {
            DaySliderValueEventArgs args = new DaySliderValueEventArgs()
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            this.RaiseValueChangedEvent(daySlider, args);
        }

        // Snapped
        public delegate void SnappedEventHandler(object sender, DaySliderSnappedEventArgs e);

        public event SnappedEventHandler Snapped;

        private void RaiseSnappedEvent(DaySlider daySlider, DaySliderSnappedEventArgs e)
        {
            Snapped?.Invoke(daySlider, e);
        }

        private void RaiseSnappedEvent(DaySlider daySlider, DaySliderSnapPositions snapPosition)
        {
            DaySliderSnappedEventArgs args = new DaySliderSnappedEventArgs()
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

        private DaySliderSnapPositions CalculateSnapToFromPosition(double currentX)
        {
            DaySliderSnapPositions snapPosition;

            // calculate stops
            double morningStop = 0;
            double nightStop = (ContainerCanvas.ActualWidth);
            double afternoonStop = (nightStop - morningStop) / 3;
            double eveningStop = afternoonStop * 2;

            // calculate attractor boundaries
            double morningAfternoonAttractor = afternoonStop * 0.33;
            double afternoonEveningAttractor = afternoonStop + ((eveningStop - afternoonStop) * 0.33);
            double eveningNightAttractor = eveningStop + ((nightStop - eveningStop) * 0.33);

            if (currentX < morningAfternoonAttractor)
            {
                // move to the morning stop
                snapPosition = DaySliderSnapPositions.Morning;
            }
            else if (currentX < afternoonEveningAttractor)
            {
                // move to the afternoon stop
                snapPosition = DaySliderSnapPositions.Afternoon;
            }
            else if (currentX < eveningNightAttractor)
            {
                // move to the evening stop
                snapPosition = DaySliderSnapPositions.Evening;
            }
            else
            {
                // move to the night stop
                snapPosition = DaySliderSnapPositions.Night;
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
