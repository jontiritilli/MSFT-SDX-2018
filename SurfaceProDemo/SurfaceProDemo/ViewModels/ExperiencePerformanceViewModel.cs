using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ExperiencePerformanceViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";
        private const string URI_LAPTOP= "ms-appx:///Assets/Experience/laptop.png";

        private const int IMG_LAPTOP_WIDTH = 1122;
        private const int IMG_LAPTOP_HEIGHT = 402;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;        
        public string LaptopUri = URI_LAPTOP;
        public int LaptopHeight = IMG_LAPTOP_HEIGHT;
        public int LaptopWidth = IMG_LAPTOP_WIDTH;


        public string Headline;
        public string Lede;

        public string PopTryIt;
        public string PopTryItHeadline;
        public string PopTryItLede;

        public string PopCenterHeadline;
        public string PopCenterLede;

        public string PopCenterBulletOne;
        public string PopCenterBulletTwo;
        public string PopCenterBulletThree;
        public string PopCenterBulletFour;
        public string PopCenterFive;

        public string PopRightHeadline;
        public string PopRightLede;
        #endregion

        #region Construction

        public ExperiencePerformanceViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperiencePerformanceViewModel(this);
            }
        }

        #endregion
    }
}
