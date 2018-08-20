using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Services;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class KeyFramePlayer : Control
    {
        #region Private Constants

        private const double FRAME_WIDTH = 3560d;
        private const double FRAME_HEIGHT = 2000d;
        private const double PLAYER_WIDTH = 1600d;
        private const double PLAYER_HEIGHT = (PLAYER_WIDTH * FRAME_HEIGHT) / FRAME_WIDTH;
        private const double SLIDER_WIDTH = (PLAYER_WIDTH / 3) * 2;
        private const double SLIDER_HEIGHT = 60d;

        private const double SLIDER_START = 0;
        private const double SLIDER_RANGE = 1000;
        private const double SLIDER_MIDPOINT = SLIDER_RANGE / 2;
        private const int TABLET_REST_FRAMEINDEX = 0;
        private const int STUDIO_REST_FRAMEINDEX = 151;
        private const int LAPTOP_REST_FRAMEINDEX = 210;

        private readonly Uri PRODUCT_ROTATION_URI = new Uri("ms-appx:///Assets/SeekPlayer/productrotation.mp4");
        private readonly Uri SLIDER_STOP_URI = new Uri("ms-appx:///Assets/SeekPlayer/slider-stop.png");
        private readonly Uri SLIDER_POSITION_URI = new Uri("ms-appx:///Assets/SeekPlayer/slider-position.png");

        #endregion

        #region Private Members

        private int _currentFrame = 0;
        private int _frameCount = 0;
        private int _frameMax = -1;

        private Grid _layoutRoot = null;
        private List<BitmapImage> _frameRack = null;
        private Image _imageBack = null;
        private Image _imageFront = null;
        private Slider _slider = null;
        private TextBlock _tablet = null;
        private TextBlock _studio = null;
        private TextBlock _laptop = null;

        #endregion


        #region Construction

        public KeyFramePlayer()
        {
            this.DefaultStyleKey = typeof(KeyFramePlayer);

            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion


        #region Dependency Properties

        // TabletStop
        public static readonly DependencyProperty TabletStopProperty =
            DependencyProperty.Register("TabletStop", typeof(string), typeof(SeekPlayer), new PropertyMetadata(null, OnTabletStopChanged));

        public string TabletStop
        {
            get { return (string)GetValue(TabletStopProperty); }
            set { SetValue(TabletStopProperty, value); }
        }

        // StudioStop
        public static readonly DependencyProperty StudioStopProperty =
            DependencyProperty.Register("StudioStop", typeof(string), typeof(SeekPlayer), new PropertyMetadata(null, OnStudioStopChanged));

        public string StudioStop
        {
            get { return (string)GetValue(StudioStopProperty); }
            set { SetValue(StudioStopProperty, value); }
        }

        // LaptopStop
        public static readonly DependencyProperty LaptopStopProperty =
            DependencyProperty.Register("LaptopStop", typeof(string), typeof(SeekPlayer), new PropertyMetadata(null, OnLaptopStopChanged));

        public string LaptopStop
        {
            get { return (string)GetValue(LaptopStopProperty); }
            set { SetValue(LaptopStopProperty, value); }
        }

        // TabletChild
        public static readonly DependencyProperty TabletChildProperty =
            DependencyProperty.Register("TabletChild", typeof(FadeInHeader), typeof(SeekPlayer), new PropertyMetadata(null, OnTabletChildChanged));

        public FadeInHeader TabletChild
        {
            get { return (FadeInHeader)GetValue(TabletChildProperty); }
            set { SetValue(TabletChildProperty, value); }
        }

        // StudioChild
        public static readonly DependencyProperty StudioChildProperty =
            DependencyProperty.Register("StudioChild", typeof(FadeInHeader), typeof(SeekPlayer), new PropertyMetadata(null, OnStudioChildChanged));

        public FadeInHeader StudioChild
        {
            get { return (FadeInHeader)GetValue(StudioChildProperty); }
            set { SetValue(StudioChildProperty, value); }
        }

        // LaptopChild
        public static readonly DependencyProperty LaptopChildProperty =
            DependencyProperty.Register("LaptopChild", typeof(FadeInHeader), typeof(SeekPlayer), new PropertyMetadata(null, OnLaptopChildChanged));

        public FadeInHeader LaptopChild
        {
            get { return (FadeInHeader)GetValue(LaptopChildProperty); }
            set { SetValue(LaptopChildProperty, value); }
        }

        #endregion


        #region Public Methods

        public void SetSliderPosition(double position)
        {
            // return if out of range
            if ((position < 0.0) || (position > SLIDER_RANGE)) { return; }

            // otherwise, set the position
            if (null != _slider)
            {
                _slider.Value = position;
            }
        }

        public void StartAnimation()
        {

        }

        public void ResetAnimation()
        {

        }

        #endregion


        #region Custom Events

        // OnReady
        public delegate void ReadyEvent(object sender, object args);

        public event ReadyEvent Ready;

        private void RaiseReadyEvent(SeekPlayer seekPlayer, object args)
        {
            Ready?.Invoke(seekPlayer, args);
        }

        private void RaiseReadyEvent(SeekPlayer seekPlayer)
        {
            RaiseReadyEvent(seekPlayer, null);
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnTabletStopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnStudioStopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnLaptopStopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnTabletChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnStudioChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnLaptopChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private void MediaPlayer_MediaOpened(Windows.Media.Playback.MediaPlayer sender, object args)
        {
        }

        private void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.UpdateUI();
        }

        #endregion


        #region Render UI

        private void RenderUI()
        {

            // get the layout base (a border here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // clear its row/col definitions and children
            _layoutRoot.RowDefinitions.Clear();
            _layoutRoot.ColumnDefinitions.Clear();
            _layoutRoot.Children.Clear();

            // set layout properties
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.Padding = new Thickness(0);
            _layoutRoot.RowSpacing = 10d;

            // create row/col definitions
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(PLAYER_HEIGHT) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SLIDER_HEIGHT) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((PLAYER_WIDTH - SLIDER_WIDTH) / 2) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SLIDER_WIDTH / 3) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SLIDER_WIDTH / 3) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SLIDER_WIDTH / 3) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((PLAYER_WIDTH - SLIDER_WIDTH) / 2) });

            // set starting frame
            _currentFrame = 0;

            // get the key frame service
            KeyFrameService keyFrameService = (KeyFrameService)SimpleIoc.Default.GetInstance<KeyFrameService>();
            if (null != keyFrameService)
            {
                // if it's loaded
                if (keyFrameService.IsLoaded)
                {
                    // grab the images
                    _frameRack = keyFrameService.BitmapImages;

                    if (null != _frameRack)
                    {
                        _frameCount = _frameRack.Count();
                        _frameMax = _frameCount - 1;
                    }
                }
            }

            // create the Back Image
            _imageBack = new Image()
            {
                Name = "ImageBack",
                Width = PLAYER_WIDTH,
                Height = PLAYER_HEIGHT,
                Opacity = 0.0,
            };
            _imageBack.ImageOpened += ImageBack_ImageOpened;
            Grid.SetRow(_imageBack, 0);
            Grid.SetColumn(_imageBack, 0);
            Grid.SetColumnSpan(_imageBack, 5);
            _layoutRoot.Children.Add(_imageBack);

            // create the Front Image
            _imageFront = new Image()
            {
                Name = "ImageFront",
                Width = PLAYER_WIDTH,
                Height = PLAYER_HEIGHT,
                Opacity = 0.0
            };
            _imageFront.ImageOpened += ImageFront_ImageOpened;
            Grid.SetRow(_imageFront, 0);
            Grid.SetColumn(_imageFront, 0);
            Grid.SetColumnSpan(_imageFront, 5);
            _layoutRoot.Children.Add(_imageFront);

            // create the slider
            _slider = new Slider()
            {
                Name = "SeekerSlider",
                Minimum = 0,
                Maximum = SLIDER_RANGE,
                //StepFrequency = 1, //SLIDER_MIDPOINT,
                TickFrequency = SLIDER_MIDPOINT,
                TickPlacement = TickPlacement.Inline,
                //SnapsTo = SliderSnapsTo.StepValues,
                Orientation = Orientation.Horizontal,
                Width = SLIDER_WIDTH,
                Height = SLIDER_HEIGHT,
                Opacity = 1.0
            };
            Grid.SetRow(_slider, 1);
            Grid.SetColumn(_slider, 1);
            Grid.SetColumnSpan(_slider, 3);
            _layoutRoot.Children.Add(_slider);

            // add events for slider
            _slider.ValueChanged += Slider_ValueChanged;

            // TODO: CHANGE STYLE OF SLIDER TO USER OUR IMAGES (FROM Files for dev / Intro / Intro Slider)

            // create the Tablet textblock
            _tablet = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Opacity = 1.0
            };
            StyleHelper.SetFontCharacteristics(_tablet, ControlStyles.SliderText);
            _tablet.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("TabletStop"), Mode = BindingMode.OneWay });
            Grid.SetRow(_tablet, 2);
            Grid.SetColumn(_tablet, 1);
            Grid.SetColumnSpan(_tablet, 1);
            _layoutRoot.Children.Add(_tablet);

            // create the Studio textblock
            _studio = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Opacity = 1.0
            };
            StyleHelper.SetFontCharacteristics(_studio, ControlStyles.SliderText);
            _studio.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("StudioStop"), Mode = BindingMode.OneWay });
            Grid.SetRow(_studio, 2);
            Grid.SetColumn(_studio, 2);
            Grid.SetColumnSpan(_studio, 1);
            _layoutRoot.Children.Add(_studio);

            // create the Laptop textblock
            _laptop = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Opacity = 1.0
            };
            StyleHelper.SetFontCharacteristics(_laptop, ControlStyles.SliderText);
            _laptop.SetBinding(TextBlock.TextProperty,
                    new Binding() { Source = this, Path = new PropertyPath("LaptopStop"), Mode = BindingMode.OneWay });
            Grid.SetRow(_laptop, 2);
            Grid.SetColumn(_laptop, 3);
            Grid.SetColumnSpan(_laptop, 1);
            _layoutRoot.Children.Add(_laptop);

            // set up entrance and exit animations

            // update the UI for start
            UpdateUI();
        }

        private void UpdateUI()
        {
            // where is the slider now
            double position = _slider.Value;
            double positionRatio = 0.0;

            // matching frameRack index
            int positionFrameIndex = 0;

            // opacity of children
            double quarterRange = SLIDER_RANGE / 4;
            double opacityLeft = 0.0;
            double opacityRight = 0.0;

            // calculate the video position in milliseconds and the child opacity values
            if (position < SLIDER_MIDPOINT)
            {
                // position
                positionRatio = (position / SLIDER_MIDPOINT);
                positionFrameIndex = (int)(positionRatio * STUDIO_REST_FRAMEINDEX);

                // opacity
                if (position < quarterRange)
                {
                    opacityLeft = 1 - (position / quarterRange);
                    opacityRight = 0.0;
                }
                else
                {
                    opacityLeft = 0.0;
                    opacityRight = (position - quarterRange) / quarterRange;
                }
                if (null != this.TabletChild) { this.TabletChild.SetOpacity(opacityLeft); }
                if (null != this.StudioChild) { this.StudioChild.SetOpacity(opacityRight); }
                if (null != this.LaptopChild) { this.LaptopChild.SetOpacity(0.0); }
            }
            else
            {
                // position
                positionRatio = ((position - SLIDER_MIDPOINT) / (SLIDER_RANGE - SLIDER_MIDPOINT));
                positionFrameIndex = (int)((positionRatio * (LAPTOP_REST_FRAMEINDEX - STUDIO_REST_FRAMEINDEX)) + STUDIO_REST_FRAMEINDEX);

                // opacity
                if (position < (SLIDER_MIDPOINT + quarterRange))
                {
                    opacityLeft = 1 - ((position - SLIDER_MIDPOINT) / quarterRange);
                    opacityRight = 0.0;
                }
                else
                {
                    opacityLeft = 0.0;
                    opacityRight = (position - quarterRange - SLIDER_MIDPOINT) / quarterRange;
                }
                if (null != this.TabletChild) { this.TabletChild.SetOpacity(0.0); }
                if (null != this.StudioChild) { this.StudioChild.SetOpacity(opacityLeft); }
                if (null != this.LaptopChild) { this.LaptopChild.SetOpacity(opacityRight); }
            }

            // seek to the frame
            this.SeekTo(positionFrameIndex);
        }

        #endregion


        #region UI Helpers

        private void SeekTo(int frameIndex)
        {
            // if we have no frame rack or no bitmaps in the rack, exit
            if ((null == _frameRack) || (0 == _frameRack.Count)) { return; }

            // ignore out of range values or current index
            if ((frameIndex < 0) || (frameIndex > _frameMax) || (frameIndex == _currentFrame)) { return; }

            // if we have image controls
            if ((null != _imageBack) && (null != _imageFront))
            {
                // are we moving to higher frames or to lower frames?
                if (_currentFrame < frameIndex)
                {
                    // moving to higher frames; start with the 'next' frame
                    for (int i = (_currentFrame + 1); i <= frameIndex; i++)
                    {
                        this.ShowFrame(i);
                    }
                }
                else
                {
                    // moving to lower frames; start with the 'previous' frame
                    for (int i = (_currentFrame - 1); i >= frameIndex; i--)
                    {
                        this.ShowFrame(i);
                    }
                }
            }
        }

        private void ShowFrame(int frameIndex)
        {
            // if we have no frame rack or no bitmaps in the rack, exit
            if ((null == _frameRack) || (0 == _frameRack.Count)) { return; }

            // ignore out of range values or current index
            if ((frameIndex < 0) || (frameIndex > _frameMax) || (frameIndex == _currentFrame)) { return; }

            // if we have image controls
            if ((null != _imageBack) && (null != _imageFront))
            {
                // update the current frame
                _currentFrame = frameIndex;

                // make sure back image is invisible
                _imageBack.Opacity = 0.01;

                // set the back image to the new frame
                _imageBack.Source = _frameRack[frameIndex];

                // remaining code is in ImageOpened handlers
            }
        }

        private void ImageBack_ImageOpened(object sender, RoutedEventArgs e)
        {
            // back image is loaded, so show it
            _imageBack.Opacity = 1.0;

            // make the front image invisible
            _imageFront.Opacity = 0.01;

            // set the front image to the current frame
            _imageFront.Source = _frameRack[_currentFrame];
        }

        private void ImageFront_ImageOpened(object sender, RoutedEventArgs e)
        {
            // make the front image visible
            _imageFront.Opacity = 1.0;

            // make the back image invisible
            _imageBack.Opacity = 0.01;
        }

        #endregion


        #region Code Helpers

        #endregion
    }
}
