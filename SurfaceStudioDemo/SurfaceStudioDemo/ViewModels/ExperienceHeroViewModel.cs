using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class ExperienceHeroViewModel : ViewModelBase
    {
        #region Constants
        
        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string HeroText;
        public int RowCount;
        public string SwipeText;

        #endregion

        #region Construction

        public ExperienceHeroViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceHeroViewModel(this);
            }
        }

        #endregion
    }
}
