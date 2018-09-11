using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;
using Windows.Media.Playback;
using Windows.Media.Core;


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


        #endregion


        #region Dependency Properties

        // PlayerPlaylist
        public static readonly DependencyProperty PlayerPlaylistProperty =
            DependencyProperty.Register("Playlist", typeof(Playlist), typeof(MusicBar), new PropertyMetadata(null, OnPlayerPlaylistChanged));

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

        // PreviousTrackIconUri
        public static readonly DependencyProperty PreviousTrackIconUriProperty =
            DependencyProperty.Register("PreviousTrackIconUri", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_PREVIOUSTRACK));

        public bool PreviousTrackIconUri
        {
            get => (bool)GetValue(PreviousTrackIconUriProperty);
            set => SetValue(PreviousTrackIconUriProperty, value);
        }

        // NextTrackIconUri
        public static readonly DependencyProperty NextTrackIconUriProperty =
            DependencyProperty.Register("NextTrackIconUri", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_NEXTTRACK));

        public bool NextTrackIconUri
        {
            get => (bool)GetValue(NextTrackIconUriProperty);
            set => SetValue(NextTrackIconUriProperty, value);
        }

        // PlayIconUri
        public static readonly DependencyProperty PlayIconUriProperty =
            DependencyProperty.Register("PlayIconUri", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_PLAY));

        public bool PlayIconUri
        {
            get => (bool)GetValue(PlayIconUriProperty);
            set => SetValue(PlayIconUriProperty, value);
        }

        // PauseIconUri
        public static readonly DependencyProperty PauseIconUriProperty =
            DependencyProperty.Register("PauseIconUri", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_PAUSE));

        public bool PauseIconUri
        {
            get => (bool)GetValue(PauseIconUriProperty);
            set => SetValue(PauseIconUriProperty, value);
        }

        // EqualizerIconUri0
        public static readonly DependencyProperty EqualizerIconUri0Property =
            DependencyProperty.Register("EqualizerIconUri0", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_EQUALIZER_00));

        public bool EqualizerIconUri0
        {
            get => (bool)GetValue(EqualizerIconUri0Property);
            set => SetValue(EqualizerIconUri0Property, value);
        }

        // EqualizerIconUri1
        public static readonly DependencyProperty EqualizerIconUri1Property =
            DependencyProperty.Register("EqualizerIconUri1", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_EQUALIZER_01));

        public bool EqualizerIconUri1
        {
            get => (bool)GetValue(EqualizerIconUri1Property);
            set => SetValue(EqualizerIconUri1Property, value);
        }

        // EqualizerIconUri2
        public static readonly DependencyProperty EqualizerIconUri2Property =
            DependencyProperty.Register("EqualizerIconUri2", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_EQUALIZER_02));

        public bool EqualizerIconUri2
        {
            get => (bool)GetValue(EqualizerIconUri2Property);
            set => SetValue(EqualizerIconUri2Property, value);
        }

        // EqualizerIconUri3
        public static readonly DependencyProperty EqualizerIconUri3Property =
            DependencyProperty.Register("EqualizerIconUri3", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_EQUALIZER_03));

        public bool EqualizerIconUri3
        {
            get => (bool)GetValue(EqualizerIconUri3Property);
            set => SetValue(EqualizerIconUri3Property, value);
        }

        // EqualizerIconUri4
        public static readonly DependencyProperty EqualizerIconUri4Property =
            DependencyProperty.Register("EqualizerIconUri4", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_EQUALIZER_04));

        public bool EqualizerIconUri4
        {
            get => (bool)GetValue(EqualizerIconUri4Property);
            set => SetValue(EqualizerIconUri4Property, value);
        }

        // EqualizerIconUri5
        public static readonly DependencyProperty EqualizerIconUri5Property =
            DependencyProperty.Register("EqualizerIconUri5", typeof(string), typeof(NavigationBar), new PropertyMetadata(URI_EQUALIZER_05));

        public bool EqualizerIconUri5
        {
            get => (bool)GetValue(EqualizerIconUri5Property);
            set => SetValue(EqualizerIconUri5Property, value);
        }


        #endregion

        #region Custom Events

        public delegate void InteractedEvent(object sender, PlayerInteractionEventArgs e);

        public event InteractedEvent Interacted;

        private void RaiseInteractedEvent(MusicBar musicBar, PlayerInteractionEventArgs e)
        {
            Interacted?.Invoke(musicBar, e);
        }

        private void RaiseInteractedEvent(MusicBar musicBar, PlayerInteractions playerInteraction, string artistName, string trackTitle)
        {
            PlayerInteractionEventArgs args = new PlayerInteractionEventArgs(playerInteraction, artistName, trackTitle);

            RaiseInteractedEvent(musicBar, args);
        }

        #endregion


        #region Player Methods

        private void LoadPlaylist()
        {
            // do we have a list?
            if ((null != this.PlayerPlaylist) && (null != this.PlayerPlaylist.Tracks) && (this.PlayerPlaylist.Tracks.Count > 0))
            {
                // create the media playback list
                this.mediaPlaybackList = new MediaPlaybackList()
                {
                    AutoRepeatEnabled = true,
                    ShuffleEnabled = false,
                };

                // add event handlers
                this.mediaPlaybackList.CurrentItemChanged += this.MediaPlaybackList_CurrentItemChanged;

                // loop through our tracks
                foreach (PlaylistTrack track in this.PlayerPlaylist.Tracks)
                {
                    // create a MediaPlaybackItem and add it to the playback list
                    this.mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(track.MediaSourceUri))));
                }

                // create the media player (every time)
                this.mediaPlayer = new MediaPlayer()
                {
                    AutoPlay = false,
                    IsLoopingEnabled = true,
                    AudioCategory = MediaPlayerAudioCategory.Media
                };

                // add event handlers
                this.mediaPlayer.PlaybackSession.PositionChanged += this.PlaybackSession_PositionChanged;

                // add the playback list to the mediaplayer
                this.mediaPlayer.Source = this.mediaPlaybackList;
            }
        }

        public bool CanGoBack
        {
            get
            {
                bool canGoBack = false;

                if (0 < this.PlayerPlaylist.SelectedIndex)
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

                if (this.PlayerPlaylist.SelectedIndex < (this.PlayerPlaylist.Tracks.Count - 1))
                {
                    canGoForward = true;
                }

                return canGoForward;
            }
        }

        public void Play()
        {

        }

        public void Pause()
        {

        }

        public void MoveToNextTrack()
        {

        }

        public void MoveToPreviousTrack()
        {

        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

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
            if (d is MusicBar player)
            {
                // TODO: Implement this.
            }
        }

        private void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            // update our selected index
            this.PlayerPlaylist.SelectedIndex = (int)sender.CurrentItemIndex;

            // update the UI
            this.UpdateUI();
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

            //// i hate this, but App is not getting these keys
            //App.Current.HandleKeyUp(key);

            //return handled;

            return false;
        }

        #endregion

        #region UI Methods

        private void UpdateUI()
        {
            // update artist name

            // update track title

            // update progress bar

            // update button states
            //  - previous track
            //  - play/pause
            //  - next track

            // update equalizer
        }

        private void UpdateTrackPosition(TimeSpan position, TimeSpan duration)
        {
            if (null != this.Scrub)
            {
                double val = position.TotalMilliseconds;
                double max = duration.TotalMilliseconds;

                this.Scrub.Value = (val / max);
            }
        }

        #endregion



        #region UI Helpers

        #endregion

    }
}
