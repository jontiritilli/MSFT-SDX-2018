using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class AccessoriesMouseViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_light.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";

        private const string URI_HERO13 = "ms-appx:///Assets/Accessories/sb2_13_accessories_right.png";
        private const string URI_HERO15 = "ms-appx:///Assets/Accessories/sb2_15_accessories_right.png";

        private const string URI_MOUSEINSETIMAGE_13 = "ms-appx:///Assets/Accessories/sb2_13_accessories_right_insetScrollWheel.png";
        private const string URI_MOUSEINSETIMAGE_15 = "ms-appx:///Assets/Accessories/sb2_15_accessories_right_insetScrollWheel.png";

        private const double HEROIMAGE_WIDTH_13 = 1000;
        private const double HEROIMAGE_WIDTH_15 = 1200;

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string HeroUri;
        public double ImageWidth;

        public string Headline;
        public string Lede;

        public string PopupMouseHeadline;
        public string PopupMouseLede;
        public string PopupMouseLegal;
        public string PopupMouseTryItTitle;
        public string PopupMouseTryItCaption;
        public string PopupMouseInsetImage;
        public double PopupMouseImageWidth;

        #endregion


        #region Construction

        public AccessoriesMouseViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.HeroUri = URI_HERO15;
                    this.ImageWidth = HEROIMAGE_WIDTH_13;
                    this.PopupMouseInsetImage = URI_MOUSEINSETIMAGE_15;
                    this.PopupMouseImageWidth = 378;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.HeroUri = URI_HERO13;
                    this.ImageWidth = HEROIMAGE_WIDTH_13;
                    this.PopupMouseInsetImage = URI_MOUSEINSETIMAGE_13;
                    this.PopupMouseImageWidth = 354;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesMouseViewModel(this);
            }
        }

        #endregion
    }
}
