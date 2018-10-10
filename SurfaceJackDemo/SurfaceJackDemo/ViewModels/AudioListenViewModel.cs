﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;
using SurfaceJackDemo.Services;
using Windows.UI.Xaml.Media;

namespace SurfaceJackDemo.ViewModels
{
    public class AudioListenViewModel : ViewModelBase
    {

        #region Constants

        //private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/gradient-bg.jpg";
        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/LancasterFrog.jpg";
        private const string URI_IMAGE = "ms-appx:///Assets/Experience/audio_headphones.png";
        private const double WIDTH_IMAGE = 2040;
        private const string URI_READY = "ms-appx:///Assets/Experience/joplin_gateway.png";
        private double READY_IMAGE_WIDTH = 1392;

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string Headline;
        public string Lede;
        public string OverlayHeadline;
        public string OverlayLede;
        public string Legal;
        public string OverlayCTA;
        public string ImageUri = URI_IMAGE;
        public double ImageWidth;
        public Playlist Playlist = null;
        public ObservableCollection<PlaylistTrack> Tracks;
        public double ReadyWidth;
        public string ReadyUri = URI_READY;
        
        public string ButtonText;
        public SolidColorBrush ReadyBoxBorderColor = RadiatingButton.GetSolidColorBrush("#FF0078D4");

        #endregion


        #region Construction

        public AudioListenViewModel()
        {

            // load the playlist
            if ((null != PlaylistService.Current) && (PlaylistService.Current.IsLoaded))
            {
                this.Playlist = PlaylistService.Current.DefaultPlaylist;
                this.Tracks = this.Playlist.Tracks;
            }
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    ImageWidth = WIDTH_IMAGE / 3 * 2;
                    ReadyWidth = READY_IMAGE_WIDTH / 3 * 2;
                    break;
                case DeviceType.Studio:
                case DeviceType.Book15:
                case DeviceType.Book13:
                case DeviceType.Pro:
                default:
                    ImageWidth = WIDTH_IMAGE / 2;
                    ReadyWidth = READY_IMAGE_WIDTH / 2;
                    break;
            }

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAudioListenViewModel(this);
            }
        }

        #endregion
    }
}
