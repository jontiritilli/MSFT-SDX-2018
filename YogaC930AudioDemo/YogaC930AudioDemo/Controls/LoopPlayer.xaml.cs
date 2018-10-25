using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YogaC930AudioDemo.Controls
{
    public sealed partial class LoopPlayer : UserControl
    {
        #region Initialization

        public LoopPlayer()
        {
            this.InitializeComponent();

            this.Loaded += this.LoopPlayer_Loaded;
        }

        private void LoopPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            // load the media
            if (null != this.MediaSourceStorageFile)
            {
                this.MediaLoopPlayer.Source = MediaSource.CreateFromStorageFile(this.MediaSourceStorageFile);
            }
            else
            {
                this.MediaLoopPlayer.Source = MediaSource.CreateFromUri(this.MediaSourceUri);
            }

            // set media player event handlers
            this.MediaLoopPlayer.MediaPlayer.MediaOpened += this.MediaPlayer_MediaOpened;
            this.MediaLoopPlayer.MediaPlayer.MediaFailed += this.MediaPlayer_MediaFailed;
            this.MediaLoopPlayer.MediaPlayer.MediaEnded += this.MediaPlayer_MediaEnded;
        }

        #endregion

               
        #region Dependency Properties

        // MediaSourceUri
        public static readonly DependencyProperty MediaSourceUriProperty =
            DependencyProperty.Register("MediaSourceUri", typeof(Uri), typeof(AttractorLoopPlayer), new PropertyMetadata(null));

        public Uri MediaSourceUri
        {
            get { return (Uri)GetValue(MediaSourceUriProperty); }
            set { SetValue(MediaSourceUriProperty, value); }
        }

        // MediaSourceStorageFile
        public static readonly DependencyProperty MediaSourceStorageFileProperty =
            DependencyProperty.Register("MediaSourceStorageFile", typeof(StorageFile), typeof(AttractorLoopPlayer), new PropertyMetadata(null));

        public StorageFile MediaSourceStorageFile
        {
            get { return (StorageFile)GetValue(MediaSourceStorageFileProperty); }
            set { SetValue(MediaSourceStorageFileProperty, value); }
        }

        // AutoPlay
        public static readonly DependencyProperty AutoPlayProperty =
        DependencyProperty.Register("AutoPlay", typeof(bool), typeof(AttractorLoopPlayer), new PropertyMetadata(true));

        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        #endregion


        #region Public Methods

        public void StartPlayer()
        {
            if (null != this.MediaLoopPlayer)
            {
                var trash = this.MediaLoopPlayer.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.MediaLoopPlayer.IsFullWindow = true;
                    this.MediaLoopPlayer.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(1);
                    this.MediaLoopPlayer.MediaPlayer.Play();
                });
            }
        }

        public void ResetPlayer()
        {
            if (null != this.MediaLoopPlayer)
            {
                var trash = this.MediaLoopPlayer.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.MediaLoopPlayer.MediaPlayer.Pause();
                    this.MediaLoopPlayer.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(1);
                    this.MediaLoopPlayer.IsFullWindow = false;
                });
            }
        }

        #endregion


        #region Public Events

        // PlaybackEnded
        public delegate void PlaybackEndedEvent(object sender, EventArgs args);

        public event PlaybackEndedEvent PlaybackEnded;

        private void RaisePlaybackEndedEvent()
        {
            RaisePlaybackEndedEvent(this, new EventArgs());
        }

        private void RaisePlaybackEndedEvent(LoopPlayer loopPlayer, EventArgs args)
        {
            PlaybackEnded?.Invoke(loopPlayer, args);
        }

        #endregion


        #region Event Handlers

        private void MediaPlayer_MediaOpened(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            // don't need this because MediaPlayerElement in XAML already auto starts if
            // our autostart property is true
            //if (this.AutoPlay)
            //{
            //    this.StartPlayer();
            //}
        }

        private void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            // no longer want video to repeat
            //if (null != sender)
            //{
            //    sender.Pause();
            //    sender.PlaybackSession.Position = TimeSpan.FromMilliseconds(1);
            //    sender.Play();
            //}

            // instead, we'll reset the player and raise an event
            this.ResetPlayer();
            this.RaisePlaybackEndedEvent();
        }

        #endregion
    }
}
