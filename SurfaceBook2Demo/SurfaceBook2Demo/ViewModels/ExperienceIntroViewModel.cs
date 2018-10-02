using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;

using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceIntroViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_dark.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_dark.jpg";

        private const string URI_BOOK15_15 = "ms-appx:///Assets/Experience/sb2_hero_sb15.png";
        private const string URI_BOOK15_13 = "ms-appx:///Assets/Experience/sb2_hero_sb15.png";
        private const string URI_BOOK13_15 = "ms-appx:///Assets/Experience/sb2_hero_sb13.png";
        private const string URI_BOOK13_13 = "ms-appx:///Assets/Experience/sb2_hero_sb13.png";
        private const string URI_DIAL_15 = "ms-appx:///Assets/Experience/sb2_hero_dial.png";
        private const string URI_DIAL_13 = "ms-appx:///Assets/Experience/sb2_hero_dial.png";
        private const string URI_PEN_15 = "ms-appx:///Assets/Experience/sb2_hero_pen.png";
        private const string URI_PEN_13 = "ms-appx:///Assets/Experience/sb2_hero_pen.png";

        private const string URI_PIXELSENSE_13 = "ms-appx:///Assets/Experience/sb2_hero_insetPixelsense_sb13.png";
        private const string URI_COMPARE_13 = "ms-appx:///Assets/Experience/sb2_hero_insetCompare_sb13.png";


        private const string URI_PIXELSENSE_15 = "ms-appx:///Assets/Experience/sb2_hero_insetPixelsense_sb15.png";
        private const string URI_COMPARE_15 = "ms-appx:///Assets/Experience/sb2_hero_insetCompare_sb15.png";

        private const double WIDTH_PIXELSENSE_IMAGE_13 = 354;
        private const double WIDTH_COMPARE_IMAGE_13 = 354;

        private const double WIDTH_PIXELSENSE_IMAGE_15 = 354;
        private const double WIDTH_COMPARE_IMAGE_15 = 354;

        #endregion


        #region Public Properties

        public string BackgroundUri;
        public string Book15Uri;
        public string Book13Uri;
        public string DialUri;
        public string PenUri;

        public string Headline;
        public string Lede;

        public string PopupPixelSenseHeadline;
        public string PopupPixelSenseLede;
        public string PopupPixelSenseLegal;
        public string PopupPixelSenseInsetImage;
        public double PopupPixelSenseImageWidth;

        public string PopupCompareHeadline;
        public string PopupCompareLede;
        public string PopupCompareLegal;
        public string PopupCompareInsetImage;
        public double PopupCompareImageWidth;

        #endregion

        #region Construction

        public ExperienceIntroViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.Book15Uri = URI_BOOK15_15;
                    this.Book13Uri = URI_BOOK13_15;
                    this.DialUri = URI_DIAL_15;
                    this.PenUri = URI_PEN_15;
                    this.PopupCompareInsetImage = URI_COMPARE_15;
                    this.PopupPixelSenseInsetImage = URI_PIXELSENSE_15;
                    this.PopupPixelSenseImageWidth = WIDTH_PIXELSENSE_IMAGE_15;
                    this.PopupCompareImageWidth = WIDTH_COMPARE_IMAGE_15;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.Book15Uri = URI_BOOK15_13;
                    this.Book13Uri = URI_BOOK13_13;
                    this.DialUri = URI_DIAL_13;
                    this.PenUri = URI_PEN_13;
                    this.PopupCompareInsetImage = URI_COMPARE_13;
                    this.PopupPixelSenseInsetImage = URI_PIXELSENSE_13;
                    this.PopupPixelSenseImageWidth = WIDTH_PIXELSENSE_IMAGE_13;
                    this.PopupCompareImageWidth = WIDTH_COMPARE_IMAGE_13;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceIntroViewModel(this);
            }
        }

        #endregion
    }
}
