using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class AccessoriesPenViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_light.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";

        private const string URI_COLORING_BOOK_IMAGE_15 = "ms-appx:///Assets/Accessories/sb2_15_accessories_car.svg";
        private const string URI_COLORING_BOOK_IMAGE_13 = "ms-appx:///Assets/Accessories/sb2_13_accessories_car.svg";

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string ColoringBookUri;

        // TODO: need coloring book colors

        public string Headline;
        public string Lede;

        public string TryItTitle;
        public string TryItLede;

        // TODO: need a list of items here for the List control.

        #endregion


        #region Construction

        public AccessoriesPenViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.ColoringBookUri = URI_COLORING_BOOK_IMAGE_15;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.ColoringBookUri = URI_COLORING_BOOK_IMAGE_13;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesPenViewModel(this);
            }
        }

        #endregion
    }
}
