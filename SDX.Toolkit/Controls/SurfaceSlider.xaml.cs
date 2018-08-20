using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Services;
using System.Globalization;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SDX.Toolkit.Controls
{
    public enum SnapPositions
    {
        None,
        Left,
        Middle,
        Right
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

    public sealed partial class SurfaceSlider : UserControl
    {
        #region Private Constants

        private const double VIDEO_MILLISECONDS = 5380;     // THIS IS BASED ON VALUES IN SEEKPLAYER.CS = (STUDIO_REST_MILLISECONDS - TABLET_REST_MILLISECONDS)

        private const double THUMB_HEIGHT = 60.0;
        private const double THUMB_WIDTH = 55.0;

        private const double THUMB_X_ADJUSTMENT = 0;

        #endregion

        #region Construction

        public SurfaceSlider()
        {
            this.InitializeComponent();

            this.Loaded += this.OnLoaded;

            this.Position = SnapPositions.Left;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

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
                case SnapPositions.Left:
                    endingValue = this.Minimum;
                    break;

                case SnapPositions.Middle:
                    endingValue = (this.Maximum - this.Minimum) / 2;
                    break;

                case SnapPositions.Right:
                    endingValue = this.Maximum;
                    break;
            }

            // snap the thumb
            this.SnapThumb(startingValue, endingValue);

            // raise the snapped event
            this.RaiseSnappedEvent(this, snapPosition);

            // save the position
            this.Position = snapPosition;

            // telemetry
            TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_PRODUCTROTATION, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(SurfaceSlider), new PropertyMetadata(0.0));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(SurfaceSlider), new PropertyMetadata(1.0));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(SurfaceSlider), new PropertyMetadata(0.0, OnValuePropertyChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty TabletStopTextProperty = DependencyProperty.Register("TabletStopText", typeof(string), typeof(SurfaceSlider), new PropertyMetadata(String.Empty, OnPropertyChanged));

        public string TabletStopText
        {
            get { return (string)GetValue(TabletStopTextProperty); }
            set { SetValue(TabletStopTextProperty, value); }
        }

        public static readonly DependencyProperty StudioStopTextProperty = DependencyProperty.Register("StudioStopText", typeof(string), typeof(SurfaceSlider), new PropertyMetadata(String.Empty, OnPropertyChanged));

        public string StudioStopText
        {
            get { return (string)GetValue(StudioStopTextProperty); }
            set { SetValue(StudioStopTextProperty, value); }
        }

        public static readonly DependencyProperty LaptopStopTextProperty = DependencyProperty.Register("LaptopStopText", typeof(string), typeof(SurfaceSlider), new PropertyMetadata(String.Empty, OnPropertyChanged));

        public string LaptopStopText
        {
            get { return (string)GetValue(LaptopStopTextProperty); }
            set { SetValue(LaptopStopTextProperty, value); }
        }

        #endregion

        #region Event Handlers

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SurfaceSlider slider)
            {
                var newValue = (double)e.NewValue;

                if (Double.IsNaN(newValue)) { return; }

                newValue = Math.Max(slider.Minimum, newValue);
                newValue = Math.Min(slider.Maximum, newValue);

                slider.UpdateThumb(slider.GetPositionFromValue(newValue));

                slider.RaiseValueChangedEvent(slider, new ValueEventArgs() { NewValue = newValue, OldValue = (double)e.OldValue });
            }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

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

                    // raise the snapped event
                    this.RaiseSnappedEvent(this, snapDestination.SnapPosition);
                }
            }
        }

        private void Tablet_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Left);
        }

        private void Studio_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Middle);
        }

        private void Laptop_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SnapTo(SnapPositions.Right);
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
            nextPosition = Math.Min(ContainerCanvas.ActualWidth - THUMB_WIDTH, nextPosition);

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
            double leftStop = 0;
            double rightStop = (ContainerCanvas.ActualWidth - THUMB_WIDTH);
            double middleStop = (rightStop - leftStop) / 2;

            // calculate attractor boundaries
            double middleAttractor = middleStop * 0.25;
            double rightAttractor = middleStop + ((rightStop - middleStop) * 0.25);

            // which stop are we going to move to?
            double destinationX = 0;

            if (currentX < middleAttractor)
            {
                // move to the left stop
                destinationX = leftStop;
                snapDestination.SnapPosition = SnapPositions.Left;
            }
            else if (currentX < rightAttractor)
            {
                // move to the middle stop
                destinationX = middleStop;
                snapDestination.SnapPosition = SnapPositions.Middle;
            }
            else
            {
                // move to the right stop
                destinationX = rightStop;
                snapDestination.SnapPosition = SnapPositions.Right;
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
            // how long should this take?
            double duration = (Math.Abs(endingValue - startingValue) / this.Maximum) * VIDEO_MILLISECONDS;

            // create the storyboard
            Storyboard storyboard = AnimationHelper.CreateStandardAnimation(this, "(Value)", startingValue, startingValue, endingValue, duration, 0.0, false, false, new RepeatBehavior(1d));

            // start the storyboard
            storyboard.Begin();
        }

        private double GetPositionFromValue(double value)
        {
            return (((value - Minimum) / (Maximum - Minimum)) * (ContainerCanvas.ActualWidth - THUMB_WIDTH));
        }

        private double GetValueFromPosition(double position)
        {
            return Minimum + ((position / (ContainerCanvas.ActualWidth - THUMB_WIDTH)) * (Maximum - Minimum));
        }

        #endregion

        #region Public Events

        // ValueChanged
        public delegate void ValueChangedEventHandler(object sender, ValueEventArgs e);

        public event ValueChangedEventHandler ValueChanged;

        private void RaiseValueChangedEvent(SurfaceSlider surfaceSlider, ValueEventArgs e)
        {
            ValueChanged?.Invoke(surfaceSlider, e);
        }

        private void RaiseValueChangedEvent(SurfaceSlider surfaceSlider, double oldValue, double newValue)
        {
            ValueEventArgs args = new ValueEventArgs()
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            this.RaiseValueChangedEvent(surfaceSlider, args);
        }

        // Snapped
        public delegate void SnappedEventHandler(object sender, SnappedEventArgs e);

        public event SnappedEventHandler Snapped;

        private void RaiseSnappedEvent(SurfaceSlider surfaceSlider, SnappedEventArgs e)
        {
            Snapped?.Invoke(surfaceSlider, e);
        }

        private void RaiseSnappedEvent(SurfaceSlider surfaceSlider, SnapPositions snapPosition)
        {
            SnappedEventArgs args = new SnappedEventArgs()
            {
                SnapPosition = snapPosition
            };

            this.RaiseSnappedEvent(surfaceSlider, args);
        }

        #endregion

        #region UI

        #endregion
    }
}
