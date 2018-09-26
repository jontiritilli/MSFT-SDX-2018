using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayPlayViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_play.png";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_play.png";

        private const string URI_HERO_13 = "ms-appx:///Assets/Experience/sb2_play_sb13.png";
        private const string URI_HERO_15 = "ms-appx:///Assets/Experience/sb2_play_sb15.png";

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string HeroUri;

        public string Headline;
        public string Lede;
        public string Legal;

        #endregion


        #region Construction

        public ExperienceDayPlayViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.HeroUri = URI_HERO_13;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.HeroUri = URI_HERO_15;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayPlayViewModel(this);
            }
        }

        #endregion
    }
}
