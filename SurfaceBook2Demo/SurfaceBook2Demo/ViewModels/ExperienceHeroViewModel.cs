using System;

using Windows.UI.Xaml.Media.Imaging;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceHeroViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_dark.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_dark.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri;
        public string HeroText;
        public int RowCount;
        public string SwipeText;

        #endregion


        #region Construction

        public ExperienceHeroViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceHeroViewModel(this);
            }
        }

        #endregion
    }
}
