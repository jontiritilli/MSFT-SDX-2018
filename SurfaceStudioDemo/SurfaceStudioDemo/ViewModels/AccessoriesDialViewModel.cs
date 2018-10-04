using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class AccessoriesDialViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";
        private const string URI_ACCESSORIES_PRIMARY = "ms-appx:///Assets/Accessories/accessoriesLeft.png";
        private const string URI_POPBOTTOM_VIDEO = "ms-appx:///Assets/Experience/accDialPopupVideo.mp4";

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string PrimaryImage = URI_ACCESSORIES_PRIMARY;

        public string Headline;
        public string Lede;

        public string PopDialHeadline;
        public string PopDialLede;
        public string PopDialLegal;

        public string PopDialVideo = URI_POPBOTTOM_VIDEO;

        #endregion

        #region Construction

        public AccessoriesDialViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesDialViewModel(this);
            }
        }

        #endregion
    }
}
