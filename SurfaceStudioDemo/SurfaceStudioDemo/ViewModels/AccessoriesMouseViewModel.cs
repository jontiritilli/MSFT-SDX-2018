using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;

namespace SurfaceStudioDemo.ViewModels
{
    public class AccessoriesMouseViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";
        private const string URI_ACCESSORIES_PRIMARY = "ms-appx:///Assets/Accessories/accessoriesRight.png";

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

        public string PopTopHeadline;
        public string PopTopLede;
        public string PopTopLegal;

        #endregion


        #region Construction

        public AccessoriesMouseViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesMouseViewModel(this);
            }
        }

        #endregion
    }
}
