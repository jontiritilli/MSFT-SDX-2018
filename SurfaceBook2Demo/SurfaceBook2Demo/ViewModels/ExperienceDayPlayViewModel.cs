using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayPlayViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_play.png";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_play.png";

        private const string URI_HERO_13 = "ms-appx:///Assets/Experience/sb2_play_sb13.png";
        private const string URI_HERO_15 = "ms-appx:///Assets/Experience/sb2_play_sb15.png";

        private const string URI_GAME_DEMO = "ms-appx:///Assets/Experience/forza_demo.mp4";

        private const double VIDEO_WIDTH_13 = 480;
        private const double VIDEO_HEIGHT_13 = 372;
        private const double HERO_IMAGE_WIDTH_13 = 1000;

        private const double VIDEO_WIDTH_15 = 565;
        private const double VIDEO_HEIGHT_15 = 437.875;
        private const double HERO_IMAGE_WIDTH_15 = 1200;

        private const double VIDEO_SET_TOP_15 = .25;
        private const double VIDEO_SET_LEFT_15 = .33;

        private const double VIDEO_SET_TOP_13 = .27;
        private const double VIDEO_SET_LEFT_13 = .34;

        #endregion


        #region Public Properties

        public double CanvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        public double CanvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        public string BackgroundUri;

        public string HeroUri;

        public string DemoUri;
        public double VideoSetLeft;
        public double VideoSetTop;

        public string Headline;
        public string Lede;
        public string Legal;

        public double VideoWidth;
        public double VideoHeight;
        public double HeroImageWidth;

        #endregion


        #region Construction

        public ExperienceDayPlayViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.HeroUri = URI_HERO_15;
                    this.VideoWidth = VIDEO_WIDTH_15;
                    this.VideoHeight = VIDEO_HEIGHT_15;
                    this.HeroImageWidth = HERO_IMAGE_WIDTH_15;
                    this.VideoSetLeft = VIDEO_SET_LEFT_15 * CanvasWidth;
                    this.VideoSetTop = VIDEO_SET_TOP_15 * CanvasHeight;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.HeroUri = URI_HERO_15;
                    this.VideoWidth = VIDEO_WIDTH_13;
                    this.VideoHeight = VIDEO_HEIGHT_13;
                    this.HeroImageWidth = HERO_IMAGE_WIDTH_13;
                    this.VideoSetLeft = VIDEO_SET_LEFT_13 * CanvasWidth;
                    this.VideoSetTop = VIDEO_SET_TOP_13 * CanvasHeight;
                    break;
            }
            DemoUri = URI_GAME_DEMO;
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayPlayViewModel(this);
            }
        }

        #endregion
    }
}
