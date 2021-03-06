﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using Windows.Foundation;
using Windows.Storage;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum PopupTypes
    {
        None,
        Text,
        Image,
        Video,
        Battery
    }

    public sealed class PopupMedia : Control
    {
        #region Private Members

        // ui elements to track
        Grid _layoutRoot = null;
        Header _header = null;
        ImageEx _image = null;
        LoopPlayer _player = null;
        PopupContentBatteryLife _batteryLife = null;

        Storyboard _storyboard = null;

        #endregion

        #region Construction/Destruction

        public PopupMedia()
        {
            this.DefaultStyleKey = typeof(PopupMedia);

            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Dependency Properties

        // Headline
        public static readonly DependencyProperty HeadlineProperty =
            DependencyProperty.Register("Headline", typeof(string), typeof(PopupMedia), new PropertyMetadata(String.Empty, OnHeadlineChanged));

        public string Headline
        {
            get => (string)GetValue(HeadlineProperty);
            set => SetValue(HeadlineProperty, value);
        }

        // Lede
        public static readonly DependencyProperty LedeProperty =
            DependencyProperty.Register("Lede", typeof(string), typeof(PopupMedia), new PropertyMetadata(String.Empty, OnLedeChanged));

        public string Lede
        {
            get => (string)GetValue(LedeProperty);
            set => SetValue(LedeProperty, value);
        }

        // HourText
        public static readonly DependencyProperty HourTextProperty =
            DependencyProperty.Register("HourText", typeof(string), typeof(PopupMedia), new PropertyMetadata("hrs"));

        public string HourText
        {
            get => (string)GetValue(HourTextProperty);
            set => SetValue(HourTextProperty, value);
        }

        // HourIntegerMax
        public static readonly DependencyProperty HourIntegerMaxProperty =
            DependencyProperty.Register("HourIntegerMax", typeof(double), typeof(PopupMedia), new PropertyMetadata(0d));

        public double HourIntegerMax
        {
            get => (double)GetValue(HourIntegerMaxProperty);
            set => SetValue(HourIntegerMaxProperty, value);
        }

        // PopupType
        public static readonly DependencyProperty PopupTypeProperty =
            DependencyProperty.Register("PopupType", typeof(bool), typeof(PopupMedia), new PropertyMetadata(true, OnPopupTypeChanged));

        public PopupTypes PopupType
        {
            get => (PopupTypes)GetValue(PopupTypeProperty);
            set => SetValue(PopupTypeProperty, value);
        }

        // PlaybackRate
        public static readonly DependencyProperty PlaybackRateProperty =
        DependencyProperty.Register("PlaybackRate", typeof(double), typeof(RadiatingButton), new PropertyMetadata(1d, OnPlaybackRateChanged));

        public double PlaybackRate
        {
            get { return (double)GetValue(PlaybackRateProperty); }
            set { SetValue(PlaybackRateProperty, value); }
        }

        //// MediaSourceStorageFile
        //public static readonly DependencyProperty MediaSourceStorageFileProperty =
        //    DependencyProperty.Register("MediaSourceStorageFile", typeof(StorageFile), typeof(RadiatingButton), new PropertyMetadata(new StorageFile, OnMediaSourceStorageFileChanged));


        //public StorageFile MediaSourceStorageFile
        //{
        //    get => (StorageFile)GetValue(MediaSourceStorageFileProperty);
        //    set => SetValue(MediaSourceStorageFileProperty, value);
        //}

        //// MediaSourceUri
        public static readonly DependencyProperty MediaSourceUriProperty =
            DependencyProperty.Register("MediaSourceUri", typeof(Uri), typeof(PopupMedia), new PropertyMetadata(null, OnMediaSourceUriChanged));

        public Uri MediaSourceUri
        {
            get => (Uri)GetValue(MediaSourceUriProperty);
            set => SetValue(MediaSourceUriProperty, value);
        }

        public static readonly DependencyProperty MediaHeightProperty =
    DependencyProperty.Register("MediaHeight", typeof(double), typeof(PopupMedia), new PropertyMetadata(0d));

        public double MediaHeight
        {
            get => (double)GetValue(MediaHeightProperty);
            set => SetValue(MediaHeightProperty, value);
        }


        public static readonly DependencyProperty MediaWidthProperty =
    DependencyProperty.Register("MediaWidth", typeof(double), typeof(PopupMedia), new PropertyMetadata(0d));

        public double MediaWidth
        {
            get => (double)GetValue(MediaWidthProperty);
            set => SetValue(MediaWidthProperty, value);
        }


        public static readonly DependencyProperty MediaHasMarginProperty =
    DependencyProperty.Register("MediaHasMargin", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true));

        public bool MediaHasMargin
        {
            get => (bool)GetValue(MediaHasMarginProperty);
            set => SetValue(MediaHasMarginProperty, value);
        }


        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false, OnAutoStartChanged));

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }

        #endregion

        #region Public Methods

        public void StartAnimation()
        {
            if (null != _storyboard)
            {
                _storyboard.Begin();
            }
        }

        public void ResetAnimation()
        {
            if (null != _storyboard)
            {
                _storyboard.Stop();
            }
        }

        public LoopPlayer GetPopupChildPlayer()
        {
            if (null != _player)
            {
                return _player;
            }

            return new LoopPlayer();
        }

        #endregion

        #region Custom Events


        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartAnimation();
            }
        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            ((PopupMedia)sender).UpdateUI();
        }

        private static void OnHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is PopupMedia popup) && (null != popup._header))
            {
                if (e.NewValue is string newValue)
                {
                    popup._header.Headline = newValue;
                }
            }
        }

        private static void OnLedeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is PopupMedia popup) && (null != popup._header))
            {
                if (e.NewValue is string newValue)
                {
                    popup._header.Lede = newValue;
                }
            }
        }

        private static void OnHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPopupTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnMediaSourceStorageFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnMediaSourceUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPlaybackRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is PopupMedia popup) && (null != popup._player))
            {
                if (e.NewValue is double newValue)
                {
                    popup._player.PlaybackRate = newValue;
                }                
            }
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // we can't work without it, so return if that failed
            if (null == _layoutRoot) { return; }

            // set the background/border of the grid
            _layoutRoot.Background = StyleHelper.GetAcrylicBrush();
            _layoutRoot.BorderBrush = new SolidColorBrush(Colors.White);
            _layoutRoot.BorderThickness = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupBorder);

            // add rows to the popup; only need to do this if we have an image or video
            if ((PopupTypes.Image == this.PopupType) || (PopupTypes.Video == this.PopupType))
            {
                // header
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // spacer
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PopupSpacer))});

                // image/video/battery
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
            // add rows to the popup; only need to do this if we have an image or video
            if (PopupTypes.Battery == this.PopupType)
            {
                // header
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // spacer
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PopupSpacer)) });

                // battery
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(150) });
            }

            // set our Padding
            if ((this.PopupType == PopupTypes.Image || this.PopupType == PopupTypes.Video) && !this.MediaHasMargin)
            {
                // set our width
                this.Width = this.MediaWidth;
                _layoutRoot.Width = this.MediaWidth;
                
            }
            else
            {
                _layoutRoot.Padding = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupPadding);
                // set our width
                _layoutRoot.Width = this.Width;
            }

            // figure out our width
            // if the control width is infinity or not a number
            if (Double.IsInfinity(this.Width) || Double.IsNaN(this.Width))
            {
                // use the default popup width
                this.Width = StyleHelper.GetApplicationDouble(LayoutSizes.PopupDefaultWidth);
            }



            // if we autostart, set our opacity to 0 to prepare for that
            if (this.AutoStart) { _layoutRoot.Opacity = 0.0; }

            // create the header
            _header = new Header()
            {
                Headline = this.Headline,
                Lede = this.Lede,
                HeadlineStyle = TextStyles.PopupHeadline,
                LedeStyle = TextStyles.PopupLede,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = this.Width - _layoutRoot.Padding.Left - _layoutRoot.Padding.Right,
            };
            if ((this.PopupType == PopupTypes.Image || this.PopupType == PopupTypes.Video) && !this.MediaHasMargin)
            {
                Thickness Padding = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupPadding);
                _header.Width = this.Width - Padding.Left - Padding.Right;
                _header.Margin = new Thickness() {
                    Left = Padding.Left,
                    Right = Padding.Right,
                    Top = Padding.Top
                };                
            }


            // add it to the layout
            Grid.SetRow(_header, 0);
            _layoutRoot.Children.Add(_header);

            // if this is a popup with an image
            if (this.PopupType == PopupTypes.Image)
            {
                // create the image
                _image = new ImageEx()
                {
                    ImageSource = this.MediaSourceUri.OriginalString,
                    ImageWidth = this.MediaWidth
                };

                // add to the grid
                Grid.SetRow(_image, 2);
                _layoutRoot.Children.Add(_image);
            }
            // if this is a popup with a video
            else if (this.PopupType == PopupTypes.Video)
            {
                // create the loop player and add it to the grid
                _player = new LoopPlayer()
                {
                    MediaSourceUri = this.MediaSourceUri,
                    VideoWidth = this.MediaWidth,
                    VideoHeight = this.MediaHeight,
                    PlaybackRate = this.PlaybackRate
                };
                
                // add to the grid
                Grid.SetRow(_player, 2);
                _layoutRoot.Children.Add(_player);
            }
            // if this is a popup with a battery life
            else if (this.PopupType == PopupTypes.Battery)
            {
                // create the loop player and add it to the grid
                _batteryLife = new PopupContentBatteryLife()
                {
                    Name = "BatteryLife",
                    HourText = this.HourText,
                    HourIntegerMax = this.HourIntegerMax,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = this.Width - _layoutRoot.Padding.Left - _layoutRoot.Padding.Right,
                    DurationInMilliseconds = 2000d,
                    StaggerDelayInMilliseconds = 200d,
                };

                // add to the grid
                Grid.SetRow(_batteryLife, 2);
                _layoutRoot.Children.Add(_batteryLife);
            }

        }

        private void UpdateUI()
        {

        }

        #endregion

        #region Code Helpers

        #endregion

        #region UI Helpers



        #endregion
    }
}
