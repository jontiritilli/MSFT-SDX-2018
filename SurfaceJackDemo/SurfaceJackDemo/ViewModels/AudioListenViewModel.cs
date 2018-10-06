using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Models;
using SurfaceJackDemo.Services;


namespace SurfaceJackDemo.ViewModels
{
    public class AudioListenViewModel : ViewModelBase
    {

        #region Constants

        //private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/gradient-bg.jpg";
        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/LancasterFrog.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string Headline;
        public string Lede;
        public string Legal;
        public Playlist Playlist = null;
        public ObservableCollection<PlaylistTrack> Tracks;

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
