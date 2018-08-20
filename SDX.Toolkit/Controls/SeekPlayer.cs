using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using SDX.Toolkit.Helpers;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum PlayerModes
    {
        Static,
        Intro,
        Scrub
    }

    public sealed class SeekPlayer : Control
    {
        #region Private Constants

        // new device uses 1800x1200 screen resolution,
        // but default scaling is 150%,
        // so effective pixels are 1200x800
        // navbar takes 80 effective pixels, 120 raw pixels
        // so raw page size is 1800x1080 and
        // effective page size is 1200x720

        private static readonly Rect WINDOW_BOUNDS = ApplicationView.GetForCurrentView().VisibleBounds;

        private const double VIDEO_WIDTH = 1800d;
        private const double VIDEO_HEIGHT = 1080d;
        //private const double PLAYER_WIDTH = 1800d;
        private static readonly double PLAYER_WIDTH = WINDOW_BOUNDS.Width;
        private static readonly double PLAYER_HEIGHT = (PLAYER_WIDTH * VIDEO_HEIGHT) / VIDEO_WIDTH;
        private static readonly double SLIDER_WIDTH = (PLAYER_WIDTH / 3) * 2;
        private const double SLIDER_HEIGHT = 65d;
        private const double MARGIN_SPACER = 50d;

        private const double SLIDER_START = 0;
        private const double SLIDER_RANGE = 1000;
        private const double SLIDER_MIDPOINT = SLIDER_RANGE / 2;
        private const double STATIC_REST_MILLISECONDS = 0;
        private const double INTRO_REST_MILLISECONDS = 0;
        private const double TABLET_REST_MILLISECONDS = 1620;
        private const double STUDIO_REST_MILLISECONDS = 4060;
        private const double LAPTOP_REST_MILLSECONDS = 7000;

        private readonly Uri PRODUCT_ROTATION_URI = new Uri("ms-appx:///Assets/SeekPlayer/productrotation.mp4");

        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private MediaPlayerElement _mediaPlayerElement = null;

        private SurfaceSlider _slider = null;

        private Storyboard _introStoryboard = null;

        private PlayerModes PlayerMode = PlayerModes.Static;

        #endregion


        #region Construction

        public SeekPlayer()
        {
            this.DefaultStyleKey = typeof(SeekPlayer);

            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Properties

        public SurfaceSlider Slider
        {
            get { return _slider; }
            set
            {
                // remove previous event handler
                if (null != _slider)
                {
                    _slider.ValueChanged -= Slider_ValueChanged;
                }

                // save the new slider
                _slider = value;

                // add our event to it
                _slider.ValueChanged += Slider_ValueChanged;
            }
        }

        #endregion
        
        #region Dependency Properties

        // PlayerPositionMilliseconds
        public static readonly DependencyProperty PlayerPositionMillisecondsProperty =
            DependencyProperty.Register("PlayerPositionMilliseconds", typeof(double), typeof(SeekPlayer), new PropertyMetadata(0d, OnPlayerPositionMillisecondsChanged));

        public double PlayerPositionMilliseconds
        {
            get { return (double)GetValue(PlayerPositionMillisecondsProperty); }
            set { SetValue(PlayerPositionMillisecondsProperty, value); }
        }

        #endregion


        #region Public Methods

        public void StartStatic()
        {
            // set static mode
            this.PlayerMode = PlayerModes.Static;

            // set the slider value to 0; must do this although it will reset the video position
            if (null != _slider)
            {
                _slider.Value = 0;
            }

            this.SeekTo(STATIC_REST_MILLISECONDS + .01);
            //// set the video position - must do this because slider doesn't relate to this portion
            //if ((null != _mediaPlayerElement) && (null != _mediaPlayerElement.MediaPlayer) && (null != _mediaPlayerElement.MediaPlayer.PlaybackSession))
            //{
            //    _mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(STATIC_REST_MILLISECONDS + 0.01);
            //}

            // reset the slider, etc.
            this.ResetAnimation();
        }

        public void StartIntro()
        {
            // set intro mode
            this.PlayerMode = PlayerModes.Intro;

            //this.SeekTo(INTRO_REST_MILLISECONDS);
            //// set the video to the intro - slider doesn't know about this
            //if ((null != _mediaPlayerElement) && (null != _mediaPlayerElement.MediaPlayer) && (null != _mediaPlayerElement.MediaPlayer.PlaybackSession))
            //{
            //    _mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(INTRO_REST_MILLISECONDS + 0.01);
            //}

            // reset the slider, etc.
            this.ResetAnimation();

            // start animation
            if (null != _introStoryboard)
            {
                _introStoryboard.Begin();
            }
        }

        public void StartScrub()
        {
            // set scrub mode
            this.PlayerMode = PlayerModes.Scrub;

            // stop the intro storyboard
            if (null != _introStoryboard)
            {
                _introStoryboard.Stop();
            }

            // make sure we're on our starting point
            //this.SeekTo(TABLET_REST_MILLISECONDS);

            // animate the slider and components in
            if (null != _slider)
            {
                _slider.Visibility = Visibility.Visible;
                AnimationHelper.PerformFadeIn(_slider, 100d, 0d);
            }

            // update the UI
            this.UpdateUI();
        }

        public void SeekTo(double destinationPosition)
        {
            if ((null != _mediaPlayerElement) && (null != _mediaPlayerElement.MediaPlayer) && (null != _mediaPlayerElement.MediaPlayer.PlaybackSession))
            {
                TimeSpan timespanPosition = _mediaPlayerElement.MediaPlayer.PlaybackSession.Position;

                double currentPosition = timespanPosition.TotalMilliseconds;

                double duration = Math.Abs(destinationPosition - currentPosition);

                Storyboard storyboardSeek = AnimationHelper.CreateStandardAnimation(this, "PlayerPositionMilliseconds", currentPosition, currentPosition, destinationPosition, duration, 0d, false, false, new RepeatBehavior(1));
                storyboardSeek.Begin();
            }
        }

        public void ResetAnimation()
        {
            if (null != _slider)
            {
                if (AnimationHelper.IsVisible(_slider))
                {
                    AnimationHelper.PerformFadeOut(_slider, 100d, 0d);
                }
                _slider.Visibility = Visibility.Collapsed;
            }
        }

        #endregion


        #region Custom Events

        // IntroCompleted
        public delegate void IntroCompletedEvent(object sender, object args);

        public event IntroCompletedEvent IntroCompleted;

        private void RaiseIntroCompletedEvent(SeekPlayer seekPlayer, object args)
        {
            IntroCompleted?.Invoke(seekPlayer, args);
        }

        private void RaiseIntroCompletedEvent(SeekPlayer seekPlayer)
        {
            RaiseIntroCompletedEvent(seekPlayer, null);
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private static void OnPlayerPositionMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 
            if (d is SeekPlayer player)
            {
                if (null != player._mediaPlayerElement)
                {
                    player._mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds((double)e.NewValue);
                }
            }
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

        private void SeekPlayer_IntroStoryboardCompleted(object sender, object e)
        {
            this.RaiseIntroCompletedEvent(this);
        }

        private void Slider_ValueChanged(object sender, ValueEventArgs e)
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
            //_layoutRoot.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            //_layoutRoot.BorderThickness = new Thickness(3);

            // create row/col definitions
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SLIDER_HEIGHT) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(MARGIN_SPACER) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((PLAYER_WIDTH - SLIDER_WIDTH) / 2) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SLIDER_WIDTH / 3) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SLIDER_WIDTH / 3) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SLIDER_WIDTH / 3) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((PLAYER_WIDTH - SLIDER_WIDTH) / 2) });

            //// test - create a border
            //Border border = new Border()
            //{
            //    BorderBrush = new SolidColorBrush(Colors.DarkRed),
            //    BorderThickness = new Thickness(1),
            //};
            //Grid.SetRow(border, 0);
            //Grid.SetRowSpan(border, 4);
            //Grid.SetColumn(border, 0);
            //Grid.SetColumnSpan(border, 5);
            //_layoutRoot.Children.Add(border);

            // create the player
            _mediaPlayerElement = new MediaPlayerElement()
            {
                Name = "SeekerPlayer",
                Stretch = Stretch.UniformToFill,
                Margin = new Thickness(0),
                Padding = new Thickness(0),
                Source = MediaSource.CreateFromUri(PRODUCT_ROTATION_URI),
                Opacity = 1.0d,
                AreTransportControlsEnabled = false,
                IsFullWindow = false,
                AutoPlay = false
            };
            Grid.SetRow(_mediaPlayerElement, 0);
            Grid.SetRowSpan(_mediaPlayerElement, 4);
            Grid.SetColumn(_mediaPlayerElement, 0);
            Grid.SetColumnSpan(_mediaPlayerElement, 5);
            _layoutRoot.Children.Add(_mediaPlayerElement);
            //border.Child = _mediaPlayerElement;

            // set media player event handlers
            _mediaPlayerElement.MediaPlayer.MediaOpened += this.MediaPlayer_MediaOpened;
            _mediaPlayerElement.MediaPlayer.MediaFailed += this.MediaPlayer_MediaFailed;
            _mediaPlayerElement.MediaPlayer.MediaEnded += this.MediaPlayer_MediaEnded;

            // create the intro play animation
            _introStoryboard = AnimationHelper.CreateStandardAnimation(this, "PlayerPositionMilliseconds", INTRO_REST_MILLISECONDS, INTRO_REST_MILLISECONDS, TABLET_REST_MILLISECONDS, TABLET_REST_MILLISECONDS, 0d, false, false, new RepeatBehavior(1));
            _introStoryboard.Completed += SeekPlayer_IntroStoryboardCompleted;

            // test
            //TestHelper.AddGridCellBorders(_layoutRoot, 4, 5, Colors.AliceBlue);

            // update the UI for start
            UpdateUI();
        }

        private void UpdateUI()
        {
            if ((null != _slider) && (null != _mediaPlayerElement))
            {
                // are we in scrub mode? we only update slider in scrub mode
                if (PlayerModes.Scrub == this.PlayerMode)
                {
                    // where is the slider now
                    //double position = _slider.Value;
                    double value = _slider.Value;
                    double positionRatio = 0.0;

                    // matching video milliseconds
                    double positionMilliseconds = 0;

                    // calculate the video position in milliseconds and the child opacity values
                    if (value < SLIDER_MIDPOINT)
                    {
                        // position
                        positionRatio = (value / SLIDER_MIDPOINT);
                        positionMilliseconds = (positionRatio * (STUDIO_REST_MILLISECONDS - TABLET_REST_MILLISECONDS)) + TABLET_REST_MILLISECONDS;
                    }
                    else
                    {
                        // position
                        positionRatio = ((value - SLIDER_MIDPOINT) / (SLIDER_RANGE - SLIDER_MIDPOINT));
                        positionMilliseconds = (positionRatio * (LAPTOP_REST_MILLSECONDS - STUDIO_REST_MILLISECONDS)) + STUDIO_REST_MILLISECONDS;
                    }

                    // reposition the media player element
                    if ((null != _mediaPlayerElement) && (null != _mediaPlayerElement.MediaPlayer) && (_mediaPlayerElement.MediaPlayer.PlaybackSession.CanSeek))
                    {
                        TimeSpan seekTimeSpan = TimeSpan.FromMilliseconds(positionMilliseconds);
                        _mediaPlayerElement.MediaPlayer.PlaybackSession.Position = seekTimeSpan;
                    }
                }
            }
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
