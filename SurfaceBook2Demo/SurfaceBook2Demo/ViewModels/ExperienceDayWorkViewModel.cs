using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayWorkViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_work.png";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_work.png";

        private const string URI_HERO = "ms-appx:///Assets/Experience/sb2_work_sb15.png";

        private const string URI_CONNECTIONS = "ms-appx:///Assets/Experience/sb2_work_insetConnections.png";

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string HeroUri;

        public string Headline;
        public string Lede;
        public string PopupBatteryLifeHeadline;
        public string PopupBatteryLifeLede;
        public string PopupBatteryLifeHours;
        public string PopupBatteryLifeLegal;
        public string PopupConnectionsHeadline;
        public string PopupConnectionsLede;
        public string PopupConnectionsLegal;
        public string PopupConnectionsInsetImage = URI_CONNECTIONS;

        #endregion


        #region Construction

        public ExperienceDayWorkViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.HeroUri = URI_HERO;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.HeroUri = URI_HERO;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayWorkViewModel(this);
            }
        }

        #endregion
    }
}
