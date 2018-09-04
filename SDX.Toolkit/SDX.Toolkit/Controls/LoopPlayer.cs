﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    //public enum InteractionTypes
    //{
    //    Keyboard,
    //    Mouse,
    //    Touch,
    //    Pen
    //}

    //public sealed class InteractedEventArgs
    //{
    //    public InteractionTypes InteractionType;
    //}

    public sealed class LoopPlayer : Control
    {

        #region Private Members

        private Border _border = null;
        private MediaPlayerElement _mediaPlayerElement = null;

        #endregion


        #region Construction

        public LoopPlayer()
        {
            this.DefaultStyleKey = typeof(LoopPlayer);

            this.Loaded += OnLoaded;            
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion


        #region Dependency Properties

        // MediaSourceUri
        public static readonly DependencyProperty MediaSourceUriProperty =
            DependencyProperty.Register("MediaSourceUri", typeof(Uri), typeof(LoopPlayer), new PropertyMetadata(null, OnMediaSourceFileChanged));

        public Uri MediaSourceUri
        {
            get { return (Uri)GetValue(MediaSourceUriProperty); }
            set { SetValue(MediaSourceUriProperty, value); }
        }

        // MediaSourceStorageFile
        public static readonly DependencyProperty MediaSourceStorageFileProperty =
            DependencyProperty.Register("MediaSourceStorageFile", typeof(StorageFile), typeof(LoopPlayer), new PropertyMetadata(null, OnMediaSourceFileChanged));

        public StorageFile MediaSourceStorageFile
        {
            get { return (StorageFile)GetValue(MediaSourceStorageFileProperty); }
            set { SetValue(MediaSourceStorageFileProperty, value); }
        }

        // AutoPlay
        public static readonly DependencyProperty AutoPlayProperty =
        DependencyProperty.Register("AutoPlay", typeof(bool), typeof(LoopPlayer), new PropertyMetadata(true, OnAutoPlayChanged));

        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        #endregion


        #region Public Methods

        public void StartPlayer()
        {
            if (null != _mediaPlayerElement)
            {
                var trash = _mediaPlayerElement.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {                    
                    _mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(1);
                    _mediaPlayerElement.MediaPlayer.Play();
                });
            }
        }

        public void ResetPlayer()
        {
            if (null != _mediaPlayerElement)
            {
                var trash = _mediaPlayerElement.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _mediaPlayerElement.MediaPlayer.Pause();
                    _mediaPlayerElement.MediaPlayer.PlaybackSession.Position = TimeSpan.FromMilliseconds(1);
                    _mediaPlayerElement.IsFullWindow = false;
                });
            }
        }

        #endregion


        #region Custom Events

        // OnInteraction
        public delegate void InteractedEvent(object sender, InteractedEventArgs args);

        public event InteractedEvent Interacted;

        private void RaiseInteractionEvent(LoopPlayer attractorLoop, InteractedEventArgs args)
        {
            Interacted?.Invoke(attractorLoop, args);
        }

        private void RaiseInteractionEvent(LoopPlayer attractorLoop, InteractionTypes interactionType)
        {
            RaiseInteractionEvent(attractorLoop, new InteractedEventArgs() { InteractionType = interactionType });
        }

        // OnReady
        public delegate void ReadyEvent(object sender, object args);

        public event ReadyEvent Ready;

        private void RaiseReadyEvent(LoopPlayer attractorLoop, object args)
        {
            Ready?.Invoke(attractorLoop, args);
        }

        private void RaiseReadyEvent(LoopPlayer attractorLoop)
        {
            RaiseReadyEvent(attractorLoop, null);
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnMediaSourceFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }       

        private void MediaPlayer_MediaOpened(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            //RaiseReadyEvent(this, args);
            this.StartPlayer();
        }

        private void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            if (null != sender)
            {
                sender.Pause();
                sender.PlaybackSession.Position = TimeSpan.FromMilliseconds(1);
                sender.Play();
            }
        }

        #endregion


        #region Render UI

        private void RenderUI()
        {

            // get the layout base (a border here)
            _border = (Border)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _border)
            {
                return;
            }

            // if the player exists, we don't need to do anything
            if (null != _mediaPlayerElement)
            {
                return;
            }

            IMediaPlaybackSource Source;
            if (null != this.MediaSourceStorageFile)
            {
                Source = MediaSource.CreateFromStorageFile(this.MediaSourceStorageFile);
            }
            else
            {
                Source = MediaSource.CreateFromUri(this.MediaSourceUri);
            }

            // create the player
            _mediaPlayerElement = new MediaPlayerElement()
            {
                Name = "MediaPlayer",
                Source = Source,
                AutoPlay = this.AutoPlay,
                AreTransportControlsEnabled = false,
                Width = 400,
                Height = 400
            };


            // set media player event handlers
            _mediaPlayerElement.MediaPlayer.MediaOpened += this.MediaPlayer_MediaOpened;
            _mediaPlayerElement.MediaPlayer.MediaFailed += this.MediaPlayer_MediaFailed;
            _mediaPlayerElement.MediaPlayer.MediaEnded += this.MediaPlayer_MediaEnded;

            Grid.SetRow(_mediaPlayerElement, 0);
            Grid.SetColumn(_mediaPlayerElement, 0);
            _border.Child = _mediaPlayerElement;
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
