using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ExperienceHeroViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";
        private const string URI_BLACKBACKGROUND = "ms-appx:///Assets/Backgrounds/brand-bg.png";

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

            if (ConfigurationService.Current.GetIsBlackSchemeEnabled())
            {
                BackgroundUri = URI_BLACKBACKGROUND;
            }

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
