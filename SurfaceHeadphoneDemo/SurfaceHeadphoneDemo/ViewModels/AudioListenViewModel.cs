using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;
using SurfaceHeadphoneDemo.Services;
using Windows.UI.Xaml.Media;

namespace SurfaceHeadphoneDemo.ViewModels
{
    public class AudioListenViewModel : ViewModelBase
    {
        #region Constants

        //private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/gradient-bg.jpg";
        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/LancasterFrog.jpg";
        private const string URI_IMAGE = "ms-appx:///Assets/Experience/audio_headphones.png";
        private const double WIDTH_IMAGE = 2040;
        private const string URI_SPOTIFYIMAGE = "ms-appx:///Assets/Experience/audio_spotify.png";
        private const double WIDTH_SPOTIFYIMAGE = 432;

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string Headline;
        public string Lede;
        public string BulletListTitle;
        public string Legal;
        public string TryIt;
        public string ImageUri = URI_IMAGE;
        public double ImageWidth;
        public string ImageSpotifyUri = URI_SPOTIFYIMAGE;
        public double ImageSpotifyWidth;
        public Playlist Playlist = null;
        public ObservableCollection<PlaylistTrack> Tracks;
        
        public string ButtonText;
        public SolidColorBrush ReadyBoxBorderColor = RadiatingButton.GetSolidColorBrush("#FF0078D4");

        #endregion

        #region Construction

        public AudioListenViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAudioListenViewModel(this);
            }

            // load the playlist
            if ((null != PlaylistService.Current) && (PlaylistService.Current.IsLoaded))
            {
                this.Playlist = PlaylistService.Current.DefaultPlaylist;
                this.Tracks = this.Playlist.Tracks;
            }

            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    ImageWidth = WIDTH_IMAGE / 3 * 2;
                    ImageSpotifyWidth = WIDTH_SPOTIFYIMAGE / 3 * 2;
                    break;
                case DeviceType.Studio:
                case DeviceType.Book15:
                case DeviceType.Book13:
                case DeviceType.Pro:
                default:
                    ImageWidth = WIDTH_IMAGE / 2;
                    ImageSpotifyWidth = WIDTH_SPOTIFYIMAGE / 2;
                    break;
            }
        }

        #endregion
    }
}
