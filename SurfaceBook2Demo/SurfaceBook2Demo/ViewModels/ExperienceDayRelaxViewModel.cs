using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayRelaxViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_relax.png";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_relax.png";

        private const string URI_HERO = "ms-appx:///Assets/Experience/sb2_relax_sb15.png";

        private const string URI_HINGE_13 = "ms-appx:///Assets/Experience/sb2_relax_insetHinge_sb13.png";
        private const string URI_HINGE_15 = "ms-appx:///Assets/Experience/sb2_relax_insetHinge_sb15.png";

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string HeroUri;

        public string Headline;
        public string Lede;
        public string PopupHingeHeadline;
        public string PopupHingeLede;
        public string PopupDisplayHeadline;
        public string PopupDisplayLede;
        public string PopupDisplayInsetImage;
        public double PopupDisplayImageWidth;

        #endregion


        #region Construction

        public ExperienceDayRelaxViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.HeroUri = URI_HERO;
                    this.PopupDisplayInsetImage = URI_HINGE_15;
                    this.PopupDisplayImageWidth = 378;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.HeroUri = URI_HERO;
                    this.PopupDisplayInsetImage = URI_HINGE_13;
                    this.PopupDisplayImageWidth = 354;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayRelaxViewModel(this);
            }
        }

        #endregion
    }
}
