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
    public enum SnapPositions
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }

    public sealed class ValueEventArgs : EventArgs
    {
        public double OldValue;
        public double NewValue;
    }

    public sealed class SnappedEventArgs : EventArgs
    {
        public SnapPositions SnapPosition;
    }

    public sealed class SnapDestination
    {
        public double DestinationValue;
        public SnapPositions SnapPosition;
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
        private const string URI_ICON_NIGHT_15 = "ms-apps:///Assets/DaySlider/sb2_15_moon.png";
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
            this.Position = SnapPositions.Morning;
            this.BatteryImageUri = uriBattery1;
        }

        private void DaySlider_Loaded(object sender, RoutedEventArgs e)
        {
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 3, 8, Colors.Purple);
            //TestHelper.AddGridCellBorders(this.BatteryGrid, 1, 3, Colors.CornflowerBlue);
        }

        #endregion


        #region Public Properties

        public SnapPositions Position { get; private set; }

        #endregion


        #region Public Methods

        public void SnapTo(SnapPositions snapPosition)
        {
            double startingValue = this.Value;
            double endingValue = this.Value;

            // calculate the eneding value
            switch (snapPosition)
            {
                case SnapPositions.Morning:
                    endingValue = SLIDER_MINIMUM;
                    break;

                case SnapPositions.Afternoon:
                    endingValue = (SLIDER_MAXIMUM - SLIDER_MINIMUM) / 3;
                    break;

                case SnapPositions.Evening:
                    endingValue = (SLIDER_MAXIMUM - SLIDER_MINIMUM) * (2 / 3);
                    break;

                case SnapPositions.Night:
                    endingValue = SLIDER_MAXIMUM;
                    break;
            }

            // snap the thumb
            this.SnapThumb(startingValue, endingValue);

            // save the position
            this.Position = snapPosition;

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

                slider.RaiseValueChangedEvent(slider, new ValueEventArgs() { NewValue = newValue, OldValue = (double)e.OldValue });
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

                    // move the thumb here
                    UpdateThumb(X);

                    //// get the starting value
                    //double startingValue = GetValueFromPosition(X);

                    // calculate the destination value for the snap animation
                    SnapDestination snapDestination = CalculateDestinationValue(X);

                    // snap to the destination
                    this.SnapThumb(startingValue, snapDestination.DestinationValue);

                    // update the thumb
                    UpdateThumb(GetPositionFromValue(snapDestination.DestinationValue));

                    // raise the snapped event
                    this.RaiseSnappedEvent(this, snapDestination.SnapPosition);
                }
            }
        }

        private void MorningTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Morning);
        }

        private void AfternoonTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Afternoon);
        }

        private void EveningTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Evening);
        }

        private void NightTick_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Night);
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

            // calculate the destination value
            SnapDestination snapDestination = CalculateDestinationValue(X);

            // raise the snapbegan event
            RaiseSnappedEvent(this, snapDestination.SnapPosition);

            // snap the thumb to the destination
            this.SnapThumb(this.Value, snapDestination.DestinationValue);
        }

        private SnapDestination CalculateDestinationValue(double currentX)
        {
            SnapDestination snapDestination = new SnapDestination();

            // calculate stops
            double morningStop = 0;
            double nightStop = (ContainerCanvas.ActualWidth);
            double afternoonStop = (nightStop - morningStop) / 3;
            double eveningStop = afternoonStop * 2;

            // calculate attractor boundaries
            double morningAfternoonAttractor = afternoonStop * 0.33;
            double afternoonEveningAttractor = afternoonStop + ((eveningStop - afternoonStop) * 0.33);
            double eveningNightAttractor = eveningStop + ((nightStop - eveningStop) * 0.33);

            // which stop are we going to move to?
            double destinationX = 0;

            if (currentX < morningAfternoonAttractor)
            {
                // move to the morning stop
                destinationX = morningStop;
                snapDestination.SnapPosition = SnapPositions.Morning;
            }
            else if (currentX < afternoonEveningAttractor)
            {
                // move to the afternoon stop
                destinationX = afternoonStop;
                snapDestination.SnapPosition = SnapPositions.Afternoon;
            }
            else if (currentX < eveningNightAttractor)
            {
                // move to the evening stop
                destinationX = eveningStop;
                snapDestination.SnapPosition = SnapPositions.Evening;
            }
            else
            {
                // move to the night stop
                destinationX = nightStop;
                snapDestination.SnapPosition = SnapPositions.Night;
            }

            // calculate the destination value
            snapDestination.DestinationValue = GetValueFromPosition(destinationX);

            return snapDestination;
        }

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

        private void SnapThumb(double startingValue, double endingValue)
        {
            // set thumb position by setting value
            this.Value = endingValue;

            // update the battery image
            switch (this.Position)
            {
                case SnapPositions.Morning:
                    this.BatteryImageUri = uriBattery1;
                    break;

                case SnapPositions.Afternoon:
                    this.BatteryImageUri = uriBattery2;
                    break;

                case SnapPositions.Evening:
                    this.BatteryImageUri = uriBattery3;
                    break;

                case SnapPositions.Night:
                    this.BatteryImageUri = uriBattery4;
                    break;

                default:
                    break;
            }
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


        #region Public Events

        // ValueChanged
        public delegate void ValueChangedEventHandler(object sender, ValueEventArgs e);

        public event ValueChangedEventHandler ValueChanged;

        private void RaiseValueChangedEvent(DaySlider daySlider, ValueEventArgs e)
        {
            ValueChanged?.Invoke(daySlider, e);
        }

        private void RaiseValueChangedEvent(DaySlider daySlider, double oldValue, double newValue)
        {
            ValueEventArgs args = new ValueEventArgs()
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            this.RaiseValueChangedEvent(daySlider, args);
        }

        // Snapped
        public delegate void SnappedEventHandler(object sender, SnappedEventArgs e);

        public event SnappedEventHandler Snapped;

        private void RaiseSnappedEvent(DaySlider daySlider, SnappedEventArgs e)
        {
            Snapped?.Invoke(daySlider, e);
        }

        private void RaiseSnappedEvent(DaySlider daySlider, SnapPositions snapPosition)
        {
            SnappedEventArgs args = new SnappedEventArgs()
            {
                SnapPosition = snapPosition
            };

            this.RaiseSnappedEvent(daySlider, args);
        }


        #endregion


        #region UI Methods


        #endregion
    }
}
