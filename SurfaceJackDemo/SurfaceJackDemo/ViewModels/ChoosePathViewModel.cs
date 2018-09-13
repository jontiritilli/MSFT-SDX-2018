using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SurfaceJackDemo.Services;


namespace SurfaceJackDemo.ViewModels
{
    public class ChoosePathViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB213 = "ms-appx:///Assets/Backgrounds/sb2_generic_bg.jpg";
        private const string URI_BACKGROUND_SB215 = "ms-appx:///Assets/Backgrounds/sb2_15_generic_bg.jpg";

        #endregion

        #region Public Properties

        public string BackgroundUri;
        public string DeviceOneTitle;
        public string DeviceOneCTA;
        public string DeviceTwoTitle;
        public string DeviceTwoCTA;

        #endregion

        #region Construction

        public ChoosePathViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadChoosePathViewModel(this);
            }

            // determine background
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Pro:
                case DeviceType.Unknown:
                case DeviceType.Go:
                default:
                    this.BackgroundUri = URI_BACKGROUND_CRUZ;
                    break;

                case DeviceType.Laptop:
                    this.BackgroundUri = URI_BACKGROUND_FOXBURG;
                    break;

                case DeviceType.Studio:
                    this.BackgroundUri = URI_BACKGROUND_CAPROCK;
                    break;

                case DeviceType.Book13:
                    this.BackgroundUri = URI_BACKGROUND_SB213;
                    break;

                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND_SB215;
                    break;
            }
        }

        #endregion


    }
}
