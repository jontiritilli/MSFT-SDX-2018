using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceHeadphoneDemo.Services;


namespace SurfaceHeadphoneDemo.ViewModels
{
    public class SpecsViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";

        private const string URI_IMAGE = "ms-appx:///Assets/Specs/specs-device.png";
        private const double WIDTH_IMAGE = 2880;
        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string ImageUri = URI_IMAGE;
        public string Headline;
        public string BulletOneCTA;
        public string BulletTwoCTA;
        public string BulletThreeCTA;
        public string BulletFourCTA;
        public string LegalBulletOne;
        public string LegalBulletTwo;
        public string LegalBulletThree;
        public string LegalBulletFour;
        public string LegalBulletFive;
        public string LegalBulletSix;
        public string LegalBulletSeven;
        public double ICON_WIDTH = 25d;
        public List<ListItem> ItemList = new List<ListItem>();
        #endregion

        #region Construction

        public SpecsViewModel()
        {
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    BackgroundUri = URI_BACKGROUND_FOXBURG;
                    break;
                case DeviceType.Studio:
                    BackgroundUri = URI_BACKGROUND_CAPROCK;
                    break;
                case DeviceType.Book15:
                    BackgroundUri = URI_BACKGROUND_SB2_15;
                    break;
                case DeviceType.Book13:
                    BackgroundUri = URI_BACKGROUND_SB2_13;
                    break;
                case DeviceType.Pro:
                default:
                    BackgroundUri = URI_BACKGROUND_CRUZ;
                    break;
            }
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadSpecsViewModel(this);
            }
        }

        #endregion
    }
}
