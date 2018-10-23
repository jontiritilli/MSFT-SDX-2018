using System;
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

namespace YogaC930AudioDemo.Controls
{
    public enum InteractionTypes
    {
        Keyboard,
        Mouse,
        Touch,
        Pen
    }

    public sealed class InteractedEventArgs
    {
        public InteractionTypes InteractionType;
    }

    public sealed class AttractorLoopPlayer : Control
    {

        #region Private Members

        private Border _border = null;
        private MediaPlayerElement _mediaPlayerElement = null;

        #endregion


        #region Construction

        public AttractorLoopPlayer()
        {
            this.DefaultStyleKey = typeof(AttractorLoopPlayer);

            this.Loaded += OnLoaded;
            this.KeyUp += AttractorLoopPlayer_KeyUp;
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
            DependencyProperty.Register("MediaSourceUri", typeof(Uri), typeof(AttractorLoopPlayer), new PropertyMetadata(null, OnMediaSourceFileChanged));

        public Uri MediaSourceUri
        {
            get { return (Uri)GetValue(MediaSourceUriProperty); }
            set { SetValue(MediaSourceUriProperty, value); }
        }

        // MediaSourceStorageFile
        public static readonly DependencyProperty MediaSourceStorageFileProperty =
            DependencyProperty.Register("MediaSourceStorageFile", typeof(StorageFile), typeof(AttractorLoopPlayer), new PropertyMetadata(null, OnMediaSourceFileChanged));

        public StorageFile MediaSourceStorageFile
        {
            get { return (StorageFile)GetValue(MediaSourceStorageFileProperty); }
            set { SetValue(MediaSourceStorageFileProperty, value); }
        }

        // AutoPlay
        public static readonly DependencyProperty AutoPlayProperty =
        DependencyProperty.Register("AutoPlay", typeof(bool), typeof(AttractorLoopPlayer), new PropertyMetadata(true, OnAutoPlayChanged));

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
                    _mediaPlayerElement.IsFullWindow = true;
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

        private void RaiseInteractionEvent(AttractorLoopPlayer attractorLoop, InteractedEventArgs args)
        {
            Interacted?.Invoke(attractorLoop, args);
        }

        private void RaiseInteractionEvent(AttractorLoopPlayer attractorLoop, InteractionTypes interactionType)
        {
            RaiseInteractionEvent(attractorLoop, new InteractedEventArgs() { InteractionType = interactionType });
        }

        // OnReady
        public delegate void ReadyEvent(object sender, object args);

        public event ReadyEvent Ready;

        private void RaiseReadyEvent(AttractorLoopPlayer attractorLoop, object args)
        {
            Ready?.Invoke(attractorLoop, args);
        }

        private void RaiseReadyEvent(AttractorLoopPlayer attractorLoop)
        {
            RaiseReadyEvent(attractorLoop, null);
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.StartPlayer();
        }

        private static void OnMediaSourceFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void MediaPlayer_KeyboardInteractionOccurred(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = HandleKey(e.Key);
        }

        private void AttractorLoopPlayer_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = HandleKey(e.Key);
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            e.Handled = HandleKey(e.Key);

            base.OnKeyUp(e);
        }

        private bool HandleKey(VirtualKey key)
        { 
            ResetPlayer();

            RaiseInteractionEvent(this, InteractionTypes.Keyboard);

            return true;
        }

        private void MediaPlayer_PointerInteractionOccurred(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = HandlePointer(e.Pointer.PointerDeviceType);
        }

        private bool HandlePointer(PointerDeviceType pointerDeviceType)
        { 
            ResetPlayer();

            InteractionTypes interactionType = InteractionTypes.Mouse;

            switch(pointerDeviceType)
            {
                case PointerDeviceType.Mouse:
                    interactionType = InteractionTypes.Mouse;
                    break;

                case PointerDeviceType.Pen:
                    interactionType = InteractionTypes.Pen;
                    break;

                case PointerDeviceType.Touch:
                    interactionType = InteractionTypes.Touch;
                    break;

            }

            RaiseInteractionEvent(this, interactionType);

            return true;
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

            // create the player
            _mediaPlayerElement = new MediaPlayerElement()
            {
                Name = "AttractorMediaPlayer",
                //Source = MediaSource.CreateFromUri(this.MediaSourceUri),
                AutoPlay = this.AutoPlay,
                AreTransportControlsEnabled = false,
                IsFullWindow = true,
                Stretch = Stretch.Uniform
            };
            if (null != this.MediaSourceStorageFile)
            {
                _mediaPlayerElement.Source = MediaSource.CreateFromStorageFile(this.MediaSourceStorageFile);
            }
            else
            {
                _mediaPlayerElement.Source = MediaSource.CreateFromUri(this.MediaSourceUri);
            }

            // set media player event handlers
            _mediaPlayerElement.MediaPlayer.MediaOpened += this.MediaPlayer_MediaOpened;
            _mediaPlayerElement.MediaPlayer.MediaFailed += this.MediaPlayer_MediaFailed;
            _mediaPlayerElement.MediaPlayer.MediaEnded += this.MediaPlayer_MediaEnded;

            // set interaction event handlers
            _mediaPlayerElement.PointerPressed += this.MediaPlayer_PointerInteractionOccurred;
            _mediaPlayerElement.KeyUp += this.MediaPlayer_KeyboardInteractionOccurred;

            _border.Child = _mediaPlayerElement;
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
