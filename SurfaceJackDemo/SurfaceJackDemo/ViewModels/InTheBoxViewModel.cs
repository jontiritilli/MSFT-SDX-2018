using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceJackDemo.Services;


namespace SurfaceJackDemo.ViewModels
{
    public class InTheBoxViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";

        private const string URI_IMAGEHEADPHONES = "ms-appx:///Assets/InTheBox/inthebox_headphones.png";
        private const double WIDTH_IMAGEHEADPHONES = 744;
        private const string URI_IMAGEBOX = "ms-appx:///Assets/InTheBox/inthebox_carrycaseFPO.png";
        private const double WIDTH_IMAGEBOX = 720;
        private const string URI_IMAGEHPCORD = "ms-appx:///Assets/InTheBox/inthebox_stereoFPO.png";
        private const double WIDTH_IMAGEHPCORD = 432;
        private const string URI_IMAGEUSBCORD = "ms-appx:///Assets/InTheBox/inthebox_usbFPO.png";
        private const double WIDTH_IMAGEUSBCORD = 432;


        #endregion


        #region Public Properties

        public string BackgroundUri;
        public string Headline;
        public string Lede;

        public string ImageHeadphonesUri = URI_IMAGEHEADPHONES;
        public string ImageBoxUri = URI_IMAGEBOX;
        public string ImageHPCordUri = URI_IMAGEHPCORD;
        public string ImageUSBCordUri = URI_IMAGEUSBCORD;

        public double ImageHeadphonesWidth;
        public double ImageBoxWidth;
        public double ImageHPCordWidth;
        public double ImageUSBCordWidth;

        public List<ListItem> ListItems = new List<ListItem>();
        public double ICON_WIDTH = StyleHelper.GetApplicationDouble(LayoutSizes.AccessoriesPenListIconWidth);

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
                    ImageHeadphonesWidth = WIDTH_IMAGEHEADPHONES / 3 * 2;
                    ImageBoxWidth = WIDTH_IMAGEBOX / 3 * 2;
                    ImageHPCordWidth = WIDTH_IMAGEHPCORD / 3 * 2;
                    ImageUSBCordWidth = WIDTH_IMAGEUSBCORD / 3 * 2;
                    break;
                case DeviceType.Studio:
                    BackgroundUri = URI_BACKGROUND_CAPROCK;
                    ImageHeadphonesWidth = WIDTH_IMAGEHEADPHONES / 2;
                    ImageBoxWidth = WIDTH_IMAGEBOX / 2;
                    ImageHPCordWidth = WIDTH_IMAGEHPCORD / 2;
                    ImageUSBCordWidth = WIDTH_IMAGEUSBCORD / 2;
                    break;
                case DeviceType.Book15:
                    BackgroundUri = URI_BACKGROUND_SB2_15;
                    ImageHeadphonesWidth = WIDTH_IMAGEHEADPHONES / 2;
                    ImageBoxWidth = WIDTH_IMAGEBOX / 2;
                    ImageHPCordWidth = WIDTH_IMAGEHPCORD / 2;
                    ImageUSBCordWidth = WIDTH_IMAGEUSBCORD / 2;
                    break;
                case DeviceType.Book13:
                    BackgroundUri = URI_BACKGROUND_SB2_13;
                    ImageHeadphonesWidth = WIDTH_IMAGEHEADPHONES / 2;
                    ImageBoxWidth = WIDTH_IMAGEBOX / 2;
                    ImageHPCordWidth = WIDTH_IMAGEHPCORD / 2;
                    ImageUSBCordWidth = WIDTH_IMAGEUSBCORD / 2;
                    break;
                case DeviceType.Pro:
                    BackgroundUri = URI_BACKGROUND_CRUZ;
                    ImageHeadphonesWidth = WIDTH_IMAGEHEADPHONES / 2;
                    ImageBoxWidth = WIDTH_IMAGEBOX / 2;
                    ImageHPCordWidth = WIDTH_IMAGEHPCORD / 2;
                    ImageUSBCordWidth = WIDTH_IMAGEUSBCORD / 2;
                    break;
                default:
                    ImageHeadphonesWidth = WIDTH_IMAGEHEADPHONES / 2;
                    ImageBoxWidth = WIDTH_IMAGEBOX / 2;
                    ImageHPCordWidth = WIDTH_IMAGEHPCORD / 2;
                    ImageUSBCordWidth = WIDTH_IMAGEUSBCORD / 2;
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
