using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

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
        }

        #endregion
    }
}
