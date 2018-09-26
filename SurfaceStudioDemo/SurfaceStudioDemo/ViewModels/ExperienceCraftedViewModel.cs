using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class ExperienceCraftedViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic_bg.jpg";
        private const string URI_CRAFTMANSHIP = "ms-appx:///Assets/Experience/craftmanship.png";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string DesignUri = URI_CRAFTMANSHIP;
        public string Headline;
        public string Lede;
        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopRightHeadline;
        public string PopRightLede;

        #endregion


        #region Construction

        public ExperienceCraftedViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceCraftedViewModel(this);
            }
        }

        #endregion
    }
}
