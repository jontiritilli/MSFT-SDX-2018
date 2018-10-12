using System;
using System.Collections.Generic;

using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.UI.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SDX.Toolkit.Controls
{
    public enum PlayerStatii
    {
        NotStarted,
        Playing,
        Paused,
    }

    public enum PlayerInteractions
    {
        Play,
        Pause,
        PreviousTrack,
        NextTrack,
        Scrub,
        ArtistName,
        TrackTitle,
        Equalizer,
    }

    public class PlayerInteractionEventArgs : EventArgs
    {
        #region Constructors

        public PlayerInteractionEventArgs()
        {
        }

        public PlayerInteractionEventArgs(PlayerInteractions playerInteraction, string artistName, string trackTitle)
        {
            this.PlayerInteraction = playerInteraction;
            this.ArtistName = artistName;
            this.TrackTitle = trackTitle;
        }

        #endregion


        #region Public Properties

        public PlayerInteractions PlayerInteraction { get; set; }

        public string ArtistName { get; set; }

        public string TrackTitle { get; set; }

        #endregion
    }

    public sealed partial class MusicBar : UserControl
    {
        #region Private Constants

        private const string URI_PREVIOUSTRACK = "ms-appx:///Assets/MusicBar/joplin_player_prev.png";
        private const string URI_NEXTTRACK = "ms-appx:///Assets/MusicBar/joplin_player_next.png";
        private const string URI_PLAY = "ms-appx:///Assets/MusicBar/joplin_player_play.png";
        private const string URI_PAUSE = "ms-appx:///Assets/MusicBar/joplin_player_pause.png";
        private const string URI_EQUALIZER_00 = "ms-appx:///Assets/MusicBar/joplin_player_eq0.png";
        private const string URI_EQUALIZER_01 = "ms-appx:///Assets/MusicBar/joplin_player_eq1.png";
        private const string URI_EQUALIZER_02 = "ms-appx:///Assets/MusicBar/joplin_player_eq2.png";
        private const string URI_EQUALIZER_03 = "ms-appx:///Assets/MusicBar/joplin_player_eq3.png";
        private const string URI_EQUALIZER_04 = "ms-appx:///Assets/MusicBar/joplin_player_eq4.png";
        private const string URI_EQUALIZER_05 = "ms-appx:///Assets/MusicBar/joplin_player_eq5.png";

        #endregion


        #region Private Members

        private MediaPlayer mediaPlayer;
        private MediaPlaybackList mediaPlaybackList;

        #endregion


        #region Construction/Destruction

        public MusicBar()
        {
            this.InitializeComponent();

            // no focus visual indications
            this.UseSystemFocusVisuals = false;

            // event handlers
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            this.KeyUp += MusicBar_OnKeyUp;
            

            // load the playlist
            this.LoadPlaylist();

            // update the ui
            this.UpdateUI();

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }

        #endregion


        #region Public Properties

        // these properties are to expose GridLengths of Size styles that the control needs.
        // why do we need this? because it's UWP, and we can't use a Converter. FYM$
        public GridLength PlayerHeight { get { return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerHeight)); } }
        public GridLength PlayerLeftMargin { get { return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerLeftMargin)); } }
        public GridLength PlayerRightMargin { get { return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerRightMargin)); } }
        public GridLength PlayerTrackSpacer { get { return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerTrackSpacer)); } }
        public GridLength PlayerButtonWidth { get { return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerButtonWidth)); } }
        public GridLength PlayerButtonSpacer { get { return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerButtonSpacer)); } }

        public AcrylicBrush BackgroundBrush {  get { return StyleHelper.GetAcrylicBrush("Light"); } }

        #endregion


        #region Dependency Properties

        // PlayerPlaylist
        public static readonly DependencyProperty PlayerPlaylistProperty =
            DependencyProperty.Register("PlayerPlaylist", typeof(Playlist), typeof(MusicBar), new PropertyMetadata(null, OnPlayerPlaylistChanged));

        public Playlist PlayerPlaylist
        {
            get { return (Playlist)GetValue(PlayerPlaylistProperty); }
            set { SetValue(PlayerPlaylistProperty, value); }
        }

        // PlayerStatus
        public static readonly DependencyProperty PlayerStatusProperty =
            DependencyProperty.Register("PlayerStatus", typeof(PlayerStatii), typeof(MusicBar), new PropertyMetadata(PlayerStatii.NotStarted, OnPlayerStatusChanged));

        public PlayerStatii PlayerStatus
        {
            get => (PlayerStatii)GetValue(PlayerStatusProperty);
            set => SetValue(PlayerStatusProperty, value);
        }

        // AutoPlay
        public static readonly DependencyProperty AutoPlayProperty =
            DependencyProperty.Register("AutoPlay", typeof(bool), typeof(MusicBar), new PropertyMetadata(true));

        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty);
            set => SetValue(AutoPlayProperty, value);
        }

        // PreviousTrackIconUri
        public static readonly DependencyProperty PreviousTrackIconUriProperty =
            DependencyProperty.Register("PreviousTrackIconUri", typeof(string), typeof(MusicBar), new PropertyMetadata(URI_PREVIOUSTRACK));

        public string PreviousTrackIconUri
        {
            get => (string)GetValue(PreviousTrackIconUriProperty);
            set => SetValue(PreviousTrackIconUriProperty, value);
        }

        // NextTrackIconUri
        public static readonly DependencyProperty NextTrackIconUriProperty =
            DependencyProperty.Register("NextTrackIconUri", typeof(string), typeof(MusicBar), new PropertyMetadata(URI_NEXTTRACK));

        public string NextTrackIconUri
        {
            get => (string)GetValue(NextTrackIconUriProperty);
            set => SetValue(NextTrackIconUriProperty, value);
        }

        // PlayIconUri
        public static readonly DependencyProperty PlayIconUriProperty =
            DependencyProperty.Register("PlayIconUri", typeof(string), typeof(MusicBar), new PropertyMetadata(URI_PLAY));

        public string PlayIconUri
        {
            get => (string)GetValue(PlayIconUriProperty);
            set => SetValue(PlayIconUriProperty, value);
        }

        // PauseIconUri
        public static readonly DependencyProperty PauseIconUriProperty =
            DependencyProperty.Register("PauseIconUri", typeof(string), typeof(MusicBar), new PropertyMetadata(URI_PAUSE));

        public string PauseIconUri
        {
            get => (string)GetValue(PauseIconUriProperty);
            set => SetValue(PauseIconUriProperty, value);
        }

        // EqualizerUris
        public static readonly DependencyProperty EqualizerUrisProperty =
            DependencyProperty.Register("EqualizerUris", typeof(List<string>), typeof(MusicBar), 
                new PropertyMetadata(new List<string>() {URI_EQUALIZER_00, URI_EQUALIZER_01, URI_EQUALIZER_02, URI_EQUALIZER_03,
                                                            URI_EQUALIZER_04, URI_EQUALIZER_05}));

        public List<string> EqualizerUris
        {
            get => (List<string>)GetValue(EqualizerUrisProperty);
            set => SetValue(EqualizerUrisProperty, value);
        }

        // Volume
        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(double), typeof(MusicBar),new PropertyMetadata(0.0d));

        public double Volume
        {
            get => (double)GetValue(VolumeProperty);
            set => SetValue(VolumeProperty, value);
        }
        #endregion


        #region Custom Events

        public delegate void InteractedEvent(object sender, PlayerInteractionEventArgs e);

        public event InteractedEvent Interacted;

        private void RaiseInteractedEvent(MusicBar musicBar, PlayerInteractionEventArgs e)
        {
            this.Interacted?.Invoke(musicBar, e);
        }

        private void RaiseInteractedEvent(MusicBar musicBar, PlayerInteractions playerInteraction, string artistName, string trackTitle)
        {
            PlayerInteractionEventArgs args = new PlayerInteractionEventArgs(playerInteraction, artistName, trackTitle);

            this.RaiseInteractedEvent(musicBar, args);
        }


               
        public delegate void OnSelectionChangedEvent(object sender, EventArgs e);

        public event OnSelectionChangedEvent SelectionChanged;

        private void RaiseSelectionChangedEvent(MusicBar sender, EventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        private void RaiseSelectionChangedEvent(MusicBar sender)
        {
            this.RaiseSelectionChangedEvent(sender, new EventArgs());
        }

        
        #endregion


        #region Player Methods

        private void LoadPlaylist()
        {
            // do we have a list?
            if (this.IsPlaylistValid)
            {
                // if we have a media player already, stop it
                if (this.IsPlayerValid)
                {
                    this.mediaPlayer.Pause();
                }

                // create the media playback list
                this.mediaPlaybackList = new MediaPlaybackList()
                {
                    AutoRepeatEnabled = true,
                    ShuffleEnabled = false,
                };

                // add event handlers
                this.mediaPlaybackList.CurrentItemChanged += this.MediaPlaybackList_CurrentItemChanged;

                // first item to play
                MediaPlaybackItem firstItem = null;

                // loop through our tracks
                foreach (PlaylistTrack track in this.PlayerPlaylist.Tracks)
                {
                    MediaPlaybackItem item = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(track.MediaSourceUri)));

                    // create a MediaPlaybackItem and add it to the playback list
                    this.mediaPlaybackList.Items.Add(item);

                    // have we saved the first item yet?
                    if (null == firstItem)
                    {
                        // no, so save this item
                        firstItem = item;
                    }
                }

                // set the first item
                this.mediaPlaybackList.StartingItem = firstItem;
                this.PlayerPlaylist.SelectedIndex = 0;

                // create the media player (every time)
                this.mediaPlayer = new MediaPlayer()
                {
                    AutoPlay = false,
                    IsLoopingEnabled = true,
                    AudioCategory = MediaPlayerAudioCategory.Media,
                    AudioDeviceType = MediaPlayerAudioDeviceType.Multimedia,
                    //Volume = 0.5    // TODO: Use AudioHelper to set volume instead
                };
                SDX.Toolkit.Helpers.AudioHelper.SetVolumeTo(this.Volume);

                // add event handlers
                this.mediaPlayer.PlaybackSession.PositionChanged += this.PlaybackSession_PositionChanged;

                // add the playback list to the mediaplayer
                this.mediaPlayer.Source = this.mediaPlaybackList;

                // if we're on autoplay, start playing
                if (this.AutoPlay)
                {
                    this.Play();
                }
            }
        }

        public bool CanGoBack
        {
            get
            {
                bool canGoBack = false;

                if ((this.IsPlaylistValid) && (0 < this.PlayerPlaylist.SelectedIndex))
                {
                    canGoBack = true;
                }

                return canGoBack;
            }
        }

        public bool CanGoForward
        {
            get
            {
                bool canGoForward = false;

                if ((this.IsPlaylistValid) && (this.PlayerPlaylist.SelectedIndex < (this.PlayerPlaylist.Tracks.Count - 1)))
                {
                    canGoForward = true;
                }

                return canGoForward;
            }
        }

        public bool IsPlayerValid
        {
            get
            {
                bool isPlayerValid = false;

                if ((null != this.mediaPlayer) && (null != this.mediaPlaybackList) && (this.mediaPlaybackList.Items.Count > 0))
                {
                    isPlayerValid = true;
                }

                return isPlayerValid;
            }
        }

        public bool IsPlaylistValid
        {
            get
            {
                bool isPlaylistValid = false;

                if ((null != this.PlayerPlaylist) && (null != this.PlayerPlaylist.Tracks) && (this.PlayerPlaylist.Tracks.Count > 0))
                {
                    isPlaylistValid = true;
                }

                return isPlaylistValid;
            }
        }

        public void Play()
        {
            // if player and playlist are valid
            if ((this.IsPlayerValid) && (this.IsPlaylistValid))
            {
                // play
                this.mediaPlayer.Play();

                // update player status
                this.PlayerStatus = PlayerStatii.Playing;

                // update the ui
                this.UpdateUI();

                // raise event
                PlaylistTrack track = this.PlayerPlaylist.Tracks[this.PlayerPlaylist.SelectedIndex];
                RaiseInteractedEvent(this, PlayerInteractions.Play, track.ArtistName, track.TrackTitle);
            }
        }

        public void Pause()
        {
            // if player and playlist are valid
            if ((this.IsPlayerValid) && (this.IsPlaylistValid))
            {
                // pause
                this.mediaPlayer.Pause();

                // update player status
                this.PlayerStatus = PlayerStatii.Paused;

                // update the ui
                this.UpdateUI();

                // raise event
                PlaylistTrack track = this.PlayerPlaylist.Tracks[this.PlayerPlaylist.SelectedIndex];
                RaiseInteractedEvent(this, PlayerInteractions.Pause, track.ArtistName, track.TrackTitle);
            }
        }

        public void MoveToNextTrack()
        {
            // if we have a player and playlist
            if ((this.IsPlayerValid) && (this.IsPlaylistValid))
            {
                // can we go forward?
                if (this.CanGoForward)
                {
                    // update the media player
                    this.mediaPlaybackList.MoveNext();

                    // update our selected index
                    this.PlayerPlaylist.SelectedIndex++;

                    // update our UI
                    this.UpdateUI();

                    // raise event
                    this.RaiseSelectionChangedEvent(this);
                    PlaylistTrack track = this.PlayerPlaylist.Tracks[this.PlayerPlaylist.SelectedIndex];
                    RaiseInteractedEvent(this, PlayerInteractions.NextTrack, track.ArtistName, track.TrackTitle);
                }
            }
        }

        public void MoveToPreviousTrack()
        {
            // if we have a player and a playlist
            if ((this.IsPlayerValid) && (this.IsPlaylistValid))
            {
                // can we go back?
                if (this.CanGoBack)
                {
                    // update the media player
                    this.mediaPlaybackList.MovePrevious();

                    // update our selected index
                    this.PlayerPlaylist.SelectedIndex--;

                    // update our UI
                    this.UpdateUI();

                    // raise event
                    this.RaiseSelectionChangedEvent(this);
                    PlaylistTrack track = this.PlayerPlaylist.Tracks[this.PlayerPlaylist.SelectedIndex];
                    RaiseInteractedEvent(this, PlayerInteractions.PreviousTrack, track.ArtistName, track.TrackTitle);
                }
            }
        }

        public void MoveToTrack(int Index)
        {
            // if we have a player and a playlist
            if ((this.IsPlayerValid) && (this.IsPlaylistValid))
            {
                // update the media player
                uint uIndex = (uint)Index;
                // update our selected index
                this.PlayerPlaylist.SelectedIndex = Index;
                this.mediaPlaybackList.MoveTo(uIndex);

                // update our UI
                this.UpdateUI();

                // raise event
                PlaylistTrack track = this.PlayerPlaylist.Tracks[this.PlayerPlaylist.SelectedIndex];
                RaiseInteractedEvent(this, PlayerInteractions.PreviousTrack, track.ArtistName, track.TrackTitle);
            }
        }
        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 1, 3, Colors.Red);
            //TestHelper.AddGridCellBorders(this.TrackInfo, 1, 5, Colors.Orange);
            //TestHelper.AddGridCellBorders(this.PlayerButtons, 1, 9, Colors.Yellow);
        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            ((MusicBar)sender).UpdateUI();
        }

        private static void OnPlayerPlaylistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MusicBar player)
            {
                // load the playlist
                player.LoadPlaylist();

                // update the UI to match
                player.UpdateUI();
            }
        }

        private static void OnPlayerStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void PreviousTrackButton_Click(object sender, RoutedEventArgs e)
        {
            this.MoveToPreviousTrack();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Pause();
        }

        private void NextTrackButton_Click(object sender, RoutedEventArgs e)
        {
            this.MoveToNextTrack();
        }

        private void EqualizerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
#pragma warning disable CS4014
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // is our playlist valid?
                if (this.IsPlaylistValid)
                {
                    // NOTE: No real reason to do this since MediaPlaybackList just keeps 
                    // fucking up the number it gives us. Our index is updated in our Move* methods already.
                    ////// update our selected index
                    ////this.PlayerPlaylist.SelectedIndex = (int)sender.CurrentItemIndex;

                    ////// the player does not always give a correct number for item 0
                    ////if (-1 == this.PlayerPlaylist.SelectedIndex)
                    ////{
                    ////    this.PlayerPlaylist.SelectedIndex = 0;
                    ////}

                    // update the UI
                    this.UpdateUI();
                }
            });
#pragma warning restore CS4014
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            // our position changed, so update the UI
            this.UpdateTrackPosition(sender.Position, sender.NaturalDuration);
        }

        private void MusicBar_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = HandleKey(e.Key);
        }

        public bool HandleKey(VirtualKey key)
        {
            bool handled = false;

            switch (key)
            {
                case VirtualKey.Right:
                    if (this.CanGoForward)
                    {
                        MoveToNextTrack();
                        handled = true;
                    }
                    break;

                case VirtualKey.Left:
                    if (this.CanGoBack)
                    {
                        MoveToPreviousTrack();
                        handled = true;
                    }
                    break;

                case VirtualKey.Enter:
                    this.Play();
                    break;

                case VirtualKey.Escape:

                    ApplicationView view = ApplicationView.GetForCurrentView();

                    if (null != view)
                    {
                        if (view.IsFullScreenMode)
                        {
                            view.ExitFullScreenMode();
                        }
                    }
                    handled = true;
                    break;
            }

            return handled;
        }

        #endregion


        #region UI Methods

        private void UpdateUI()
        {
#pragma warning disable CS4014
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (this.IsPlaylistValid)
                {
                    // get the current track
                    PlaylistTrack track = this.PlayerPlaylist.Tracks[this.PlayerPlaylist.SelectedIndex];

                    // if we have a track
                    if (null != track)
                    {
                        // update artist name
                        this.ArtistName.Text = track.ArtistName;

                        // update track title
                        this.TrackTitle.Text = track.TrackTitle;

                        // update progress bar
                        // this is updated separately

                        // update previous track button
                        this.PreviousTrackIcon.Opacity = (this.CanGoBack) ? 1.0 : 0.6;
                        this.PreviousTrackButton.IsEnabled = (this.CanGoBack);

                        // update play/pause/equalizer
                        if ((PlayerStatii.NotStarted == this.PlayerStatus) || (PlayerStatii.Paused == this.PlayerStatus))
                        {
                            this.PlayButton.Visibility = Visibility.Visible;
                            this.PauseButton.Visibility = Visibility.Collapsed;
                            // stop equalizer
                        }
                        else
                        {
                            this.PlayButton.Visibility = Visibility.Collapsed;
                            this.PauseButton.Visibility = Visibility.Visible;
                            // start equalizer
                        }

                        //  update next track
                        this.NextTrackIcon.Opacity = (this.CanGoForward) ? 1.0 : 0.6;
                        this.NextTrackButton.IsEnabled = (this.CanGoForward);

                        // update equalizer
                        if (null != this.EqualizerIcon)
                        {
                            switch(this.PlayerStatus)
                            {
                                case PlayerStatii.NotStarted:
                                case PlayerStatii.Paused:
                                default:
                                    this.EqualizerIcon.Stop();
                                    break;

                                case PlayerStatii.Playing:
                                    this.EqualizerIcon.Start();
                                    break;
                            }
                        }
                    }
                }
            });
#pragma warning restore CS4014
        }

        private void UpdateTrackPosition(TimeSpan position, TimeSpan duration)
        {
#pragma warning disable CS4014
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (null != this.Scrub)
                {
                    double val = Double.IsNaN(position.TotalMilliseconds) ? 0 : position.TotalMilliseconds;
                    double max = (Double.IsNaN(duration.TotalMilliseconds) || (0 == duration.TotalMilliseconds)) ? 1 : duration.TotalMilliseconds;

                    this.Scrub.Value = (val / max);
                }
            });
#pragma warning restore CS4014
        }



        #endregion


        #region UI Helpers

        #endregion


    }
}
