using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ExperienceIntroViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";
        private const string URI_IMAGEHERO_FRONT = "ms-appx:///Assets/Experience/black_hero_front.png";
        private const string URI_IMAGEHERO_BACK = "ms-appx:///Assets/Experience/black_hero_back.png";        
        private const double IMAGEHERO_FRONT_WIDTH = 1194;
        private const double IMAGEHERO_BACK_WIDTH = 610;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string Lede;
        public string SwipeText;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftLegal;
        public string PopTopHeadline;
        public string PopTopLede;
        public string PopTopLegal;

        public string HeroFrontURI = URI_IMAGEHERO_FRONT;
        public string HeroBackURI = URI_IMAGEHERO_BACK;        

        public double HeroFrontWidth = IMAGEHERO_FRONT_WIDTH;
        public double HeroBackWidth = IMAGEHERO_BACK_WIDTH;
        #endregion

        #region Construction

        public ExperienceIntroViewModel()
        {
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
