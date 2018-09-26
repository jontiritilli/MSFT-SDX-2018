using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class ExperienceCreativityViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND1 = "ms-appx:///Assets/Experience/hero01.jpg";
        private const string URI_BACKGROUND2 = "ms-appx:///Assets/Experience/hero02.jpg";

        #endregion

        #region Public Properties

        public string Headline;
        public string Lede;
        public string BackgroundOneUri = URI_BACKGROUND1;
        public string BackgroundTwoUri = URI_BACKGROUND2;

        #endregion

        #region Construction

        public ExperienceCreativityViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceCreativityViewModel(this);
            }
        }

        #endregion
    }
}
