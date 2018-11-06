using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Helpers;
using SurfaceHeadphoneDemo.Services;


namespace SurfaceHeadphoneDemo.ViewModels
{
    public class TechViewModel : ViewModelBase
    {

        #region Constants
        
        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";

        private const string URI_IMAGE = "ms-appx:///Assets/Experience/joplin_tech.png";
        private const double WIDTH_IMAGE = 2736;
        #endregion


        #region Public Properties

        public string BackgroundUri;
        public string Headline;
        public string Lede;
        public string Legal;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftHR;
        public string PopRightHeadline;
        public string PopRightLede;
        public string PopTopHeadline;
        public string PopTopLede;
        
        public string ImageUri = URI_IMAGE;
        public double ImageWidth;
        #endregion

        #region Construction

        public TechViewModel()
        {
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    BackgroundUri = URI_BACKGROUND_FOXBURG;
                    ImageWidth = WIDTH_IMAGE / 3 * 2;
                    break;
                case DeviceType.Studio:
                    BackgroundUri = URI_BACKGROUND_CAPROCK;
                    ImageWidth = WIDTH_IMAGE / 2;
                    break;
                case DeviceType.Book15:
                    BackgroundUri = URI_BACKGROUND_SB2_15;
                    ImageWidth = WIDTH_IMAGE / 2;
                    break;
                case DeviceType.Book13:
                    BackgroundUri = URI_BACKGROUND_SB2_13;
                    ImageWidth = WIDTH_IMAGE / 2;
                    break;
                case DeviceType.Pro:
                    BackgroundUri = URI_BACKGROUND_CRUZ;
                    ImageWidth = WIDTH_IMAGE / 2;
                    break;
                default:
                    ImageWidth = WIDTH_IMAGE / 2;
                    break;
            }
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadTechViewModel(this);
            }
        }

        #endregion
    }
}
