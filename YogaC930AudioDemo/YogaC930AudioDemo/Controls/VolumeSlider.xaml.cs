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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using YogaC930AudioDemo.Helpers;


namespace YogaC930AudioDemo.Controls
{
    public enum VolumePositions
    {
        Low,
        Mid,
        Max,
    }

    public sealed class VolumeSliderEventArgs : EventArgs
    {
        public double OldValue;
        public double NewValue;
    }

    public sealed partial class VolumeSlider : UserControl
    {
        #region Constants

        private const double SLIDER_MINIMUM = 0;
        private const double SLIDER_MAXIMUM = 100;
        private const double SLIDER_DEFAULT = 35;
        private static Uri VOLUME_LOW_IMAGE_URI = new Uri("ms-appx:///Assets/Volume/ui_volumeLow.png");
        private static Uri VOLUME_MID_IMAGE_URI = new Uri("ms-appx:///Assets/Volume/ui_volumeMid.png");
        private static Uri VOLUME_MAX_IMAGE_URI = new Uri("ms-appx:///Assets/Volume/ui_volumeMax.png");

        #endregion

        #region Public Members

        public BitmapImage VolumeImage = new BitmapImage()
        {
            UriSource = VOLUME_MID_IMAGE_URI,
            DecodePixelWidth = 32,
            DecodePixelType = DecodePixelType.Logical
        };

        public double BlueLineLength = SLIDER_DEFAULT;

        #endregion

        #region Construction

        public VolumeSlider()
        {
            this.InitializeComponent();

            this.Loaded += this.VolumeSlider_Loaded;

            // set default Volume
            this.Volume = SLIDER_DEFAULT / 100;
        }

        private void VolumeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            // update volume level caption
            UpdateVolumeLevelText(this.Volume);

            // change the volume level indicator
            SetCurrentVolumeUI();

            //TestHelper.AddGridCellBorders(this.LayoutRoot, 3, 8, Colors.Purple);
            //TestHelper.AddGridCellBorders(this.BatteryGrid, 1, 3, Colors.CornflowerBlue);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Dependency Properties

        // Volume
        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(VolumeSlider), new PropertyMetadata(0.0, OnVolumePropertyChanged));

        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        // VolumeControlCaptionTextFirst
        public static readonly DependencyProperty VolumeControlCaptionTextFirstProperty =
            DependencyProperty.Register("VolumeControlCaptionTextFirst", typeof(string), typeof(VolumeSlider), new PropertyMetadata(String.Empty));

        public string VolumeControlCaptionTextFirst
        {
            get => (string)GetValue(VolumeControlCaptionTextFirstProperty);
            set => SetValue(VolumeControlCaptionTextFirstProperty, value);
        }

        // VolumeControlCaptionTextLast
        public static readonly DependencyProperty VolumeControlCaptionTextLastProperty =
            DependencyProperty.Register("VolumeControlCaptionTextLast", typeof(string), typeof(VolumeSlider), new PropertyMetadata(String.Empty));

        public string VolumeControlCaptionTextLast
        {
            get => (string)GetValue(VolumeControlCaptionTextLastProperty);
            set => SetValue(VolumeControlCaptionTextLastProperty, value);
        }

        // VolumeLevelText
        public static readonly DependencyProperty VolumeLevelTextProperty =
            DependencyProperty.Register("VolumeLevelText", typeof(string), typeof(VolumeSlider), new PropertyMetadata(String.Empty));

        public string VolumeLevelText
        {
            get => (string)GetValue(VolumeLevelTextProperty);
            set => SetValue(VolumeLevelTextProperty, value);
        }

        #endregion

        #region Event Handlers

        private static void OnVolumePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VolumeSlider slider)
            {
                var newValue = (double)e.NewValue;

                if (Double.IsNaN(newValue)) { return; }

                newValue = Math.Max(SLIDER_MINIMUM, newValue);
                newValue = Math.Min(SLIDER_MAXIMUM, newValue);

                slider.UpdateThumb(newValue);

                slider.RaiseValueChangedEvent(slider, new VolumeSliderEventArgs() { NewValue = newValue, OldValue = (double)e.OldValue });
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
                    // get the position of the tap in relation to the ContainerCanvas
                    GeneralTransform ttv = grid.TransformToVisual(this.ContainerCanvas);
                    Point p = ttv.TransformPoint(tapPoint);

                    // get the tap x
                    double Y = p.Y;

                    // snap to the destination
                    this.MoveTo(Y);
                }
            }
        }

        private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateThumb(GetPositionFromVolume(this.Volume));
        }

        private void Thumb_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // get where we are
            double currentPosition = this.ManipulationTranslation.Y;

            // get where they want to go
            double offset = e.Delta.Translation.Y;

            // calculate where we will actually go
            double nextPosition = currentPosition + offset;

            // update the thumb
            UpdateThumb(nextPosition, true);

            // set the volume level
            AudioHelper.SetVolumeTo(GetVolumeFromPosition(currentPosition));

            // update volume level caption
            UpdateVolumeLevelText(this.Volume);

            // change the volume level indicator
            SetCurrentVolumeUI();

            // set our new value
            Volume = GetVolumeFromPosition(nextPosition);
        }

        private void Thumb_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // once the manipulation ends, we need to snap to one of the stop points

            // get the current X position; this will have been updated by Thumb_ManipulationDelta
            double newYPosition = this.ManipulationTranslation.Y;

            // snap to the destination
            MoveTo(newYPosition);
        }

        #endregion

        #region Public Events

        // ValueChanged
        public delegate void ValueChangedEventHandler(object sender, VolumeSliderEventArgs e);

        public event ValueChangedEventHandler ValueChanged;

        private void RaiseValueChangedEvent(VolumeSlider volumeSlider, VolumeSliderEventArgs e)
        {
            ValueChanged?.Invoke(volumeSlider, e);
        }

        private void RaiseValueChangedEvent(VolumeSlider volumeSlider, double oldValue, double newValue)
        {
            VolumeSliderEventArgs args = new VolumeSliderEventArgs()
            {
                OldValue = oldValue,
                NewValue = newValue
            };

            this.RaiseValueChangedEvent(volumeSlider, args);
        }

        // Moved
        public delegate void MovedEventHandler(object sender, VolumeSliderEventArgs e);

        public event MovedEventHandler Snapped;

        private void RaiseMovedEvent(VolumeSlider daySlider, VolumeSliderEventArgs e)
        {
            Snapped?.Invoke(daySlider, e);
        }

        private void RaiseMovedEvent(VolumeSlider daySlider, double newValue)
        {
            VolumeSliderEventArgs args = new VolumeSliderEventArgs()
            {
                NewValue = newValue
            };

            this.RaiseMovedEvent(daySlider, args);
        }

        #endregion

        #region Public Methods

        public void MoveTo(double YPosition)
        {
            double normalizedYPosition = GetVolumeFromPosition(YPosition);

            // save the volume level as a percentage
            this.Volume = normalizedYPosition;

            // update the thumb
            UpdateThumb(YPosition, true);

            // change the volume level indicator
            SetCurrentVolumeUI();

            // update volume level caption
            UpdateVolumeLevelText(normalizedYPosition);

            // set the volume level
            AudioHelper.SetVolumeTo(normalizedYPosition);

            // raise the snapped event
            this.RaiseMovedEvent(this, normalizedYPosition);

            // telemetry
            //TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_PRODUCTROTATION, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion

        #region UI Methods

        private void UpdateVolumeLevelText(double volume)
        {
            double volumeLevel = volume * 100;
            this.VolumeCaption.Text = volumeLevel.ToString("0");
        }

        private void UpdateThumb(double position, bool update = false)
        {
            position = Math.Min(this.SliderLine.ActualHeight, position);
            position = Math.Max(0.0d, position);

            if (this.ContainerCanvas != null)
            {
                if (update || !Thumb.IsDragging)
                {
                    this.ManipulationTranslation.Y = position;
                    this.BlueSliderLine.Height = this.SliderLine.ActualHeight - position;
                }
            }
        }

        private void SetCurrentVolumeUI()
        {
            double volume = Math.Round(100 * this.Volume);

            // calculate stops
            double volumeLowPoint = 0;
            double volumeMidPoint = 32;
            double volumeMaxPoint = 65;

            if (volume > volumeMaxPoint)
            {
                VolumeImage.UriSource = VOLUME_MAX_IMAGE_URI;
                return;
            }
            else if (volume > volumeMidPoint)
            {
                VolumeImage.UriSource = VOLUME_MID_IMAGE_URI;
                return;
            }
            else if (volume > volumeLowPoint)
            {
                VolumeImage.UriSource = VOLUME_LOW_IMAGE_URI;
                return;
            }
        }

        private double GetPositionFromVolume(double volume)
        {
            volume = Math.Min(1.0d, volume);
            volume = Math.Max(0.0d, volume);

            return ((1 - volume) * SliderLine.ActualHeight);
        }

        private double GetVolumeFromPosition(double position)
        {
            position = Math.Min(this.SliderLine.ActualHeight, position);
            position = Math.Max(0.0d, position);

            return ((SliderLine.ActualHeight - position)/ SliderLine.ActualHeight);
        }

        #endregion
    }
}
