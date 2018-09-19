using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class AccessoriesTryItViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic_bg.jpg";
        private const string URI_ACCESSORIES_PRIMARY = "ms-appx:///Assets/Accessories/accessoriesLeft.png";

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string PrimaryImage = URI_ACCESSORIES_PRIMARY;

        public string Headline;
        public string Lede;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftLegal;

        public string PopRightHeadline;
        public string PopRightLede;
        public string PopRightLegal;

        #endregion

        #region Construction

        public AccessoriesTryItViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesTryItViewModel(this);
            }
        }

        #endregion
    }
}
