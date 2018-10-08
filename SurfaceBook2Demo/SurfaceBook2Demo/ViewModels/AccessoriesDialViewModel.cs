using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class AccessoriesDialViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_light.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";

        private const string URI_HERO13 = "ms-appx:///Assets/Accessories/sb2_13_accessories_left.png";
        private const string URI_HERO15 = "ms-appx:///Assets/Accessories/sb2_15_accessories_left.png";

        private const double HEROIMAGE_WIDTH_13 = 1100;
        private const double HEROIMAGE_WIDTH_15 = 1190

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string HeroUri;
        public double ImageWidth;

        public string Headline;
        public string Lede;

        public string PopupDialHeadline;
        public string PopupDialLede;
        public string PopupDialLegal;

        public string PopupPenHeadline;
        public string PopupPenLede;
        public string PopupPenLegal;
        
        #endregion


        #region Construction

        public AccessoriesDialViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.HeroUri = URI_HERO15;
                    this.ImageWidth = HEROIMAGE_WIDTH_15;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.HeroUri = URI_HERO13;
                    this.ImageWidth = HEROIMAGE_WIDTH_13;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesDialViewModel(this);
            }
        }

        #endregion
    }
}
