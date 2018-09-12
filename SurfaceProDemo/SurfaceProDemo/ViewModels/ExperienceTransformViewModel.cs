using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ExperienceTransformViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";        
        private const string URI_STUDIO = "ms-appx:///Assets/Experience/studio.png";

        private const int IMG_STUDIO_WIDTH = 1122;
        private const int IMG_STUDIO_HEIGHT = 402;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string StudioUri = URI_STUDIO;
        public int StudioHeight = IMG_STUDIO_HEIGHT;
        public int StudioWidth = IMG_STUDIO_WIDTH;


        public string Headline;
        public string Lede;

        public string PopHeadline;
        public string PopLede;
        public string PopTryItCaption;
        #endregion

        #region Construction

        public ExperienceTransformViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceTransformViewModel(this);
            }
        }

        #endregion
    }
}
