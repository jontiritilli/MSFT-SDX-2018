using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceHeadphoneDemo.Services;


namespace SurfaceHeadphoneDemo.ViewModels
{
    public class InTheBoxViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";

        private const string URI_IMAGE = "ms-appx:///Assets/InTheBox/whats-in-the-box.png";
        private const double WIDTH_IMAGE = 1776;


        #endregion


        #region Public Properties

        public string BackgroundUri;
        public string Headline;
        public string Lede;

        public string ImageUri = URI_IMAGE;

        public double ImageWidth;
        public double ImageBoxWidth;
        public double ImageHPCordWidth;
        public double ImageUSBCordWidth;

        public List<ListItem> ListItems = new List<ListItem>();
        public double ICON_WIDTH = StyleHelper.GetApplicationDouble(LayoutSizes.WITBListIconWidth);

        #endregion

        #region Construction

        public InTheBoxViewModel()
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
                localizationService.LoadPartnerViewModel(this);
            }
        }

        #endregion
    }
}
