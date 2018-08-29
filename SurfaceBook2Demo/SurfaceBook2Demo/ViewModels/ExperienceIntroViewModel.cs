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

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/background_sb2_dark.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/background_sb2_dark.jpg";

        private const string URI_BOOK15_15 = "ms-appx:///Assets/Experience/sb2_hero_sb15.png";
        private const string URI_BOOK15_13 = "ms-appx:///Assets/Experience/sb2_hero_sb15.png";
        private const string URI_BOOK13_15 = "ms-appx:///Assets/Experience/sb2_hero_sb13.png";
        private const string URI_BOOK13_13 = "ms-appx:///Assets/Experience/sb2_hero_sb13.png";
        private const string URI_DIAL_15 = "ms-appx:///Assets/Experience/sb2_hero_dial.png";
        private const string URI_DIAL_13 = "ms-appx:///Assets/Experience/sb2_hero_dial.png";
        private const string URI_PEN_15 = "ms-appx:///Assets/Experience/sb2_hero_pen.png";
        private const string URI_PEN_13 = "ms-appx:///Assets/Experience/sb2_hero_pen.png";

        private const string URI_PIXELSENSE = "ms-appx:///Assets/Experience/sb2_hero_insetPixelsense.png";
        private const string URI_COMPARE = "ms-appx:///Assets/Experience/sb2_hero_insetCompare.png";

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
        public string PopupPixelSenseInsetImage = URI_PIXELSENSE;
        public string PopupCompareHeadline;
        public string PopupCompareLede;
        public string PopupCompareLegal;
        public string PopupCompareInsetImage = URI_COMPARE;

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
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.Book15Uri = URI_BOOK15_13;
                    this.Book13Uri = URI_BOOK13_13;
                    this.DialUri = URI_DIAL_13;
                    this.PenUri = URI_PEN_13;
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
