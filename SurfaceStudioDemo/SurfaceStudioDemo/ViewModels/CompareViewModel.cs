using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class CompareViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";

        private const string URI_FAMILY = "ms-appx:///Assets/Comparison/comparisonFamily.png";
        private const int IMG_FAMILY_WIDTH = 2250; //4500/2

        #endregion

        #region Public Properties

        public string Headline;
        public string Lede;
        public string Legal;

        public string BackgroundUri = URI_BACKGROUND;

        public string FamilyUri = URI_FAMILY;
        public int FamilyWidth = IMG_FAMILY_WIDTH;

        public string ProTitle;
        public string BookTitle;
        public string StudioTitle;
        public string LaptopTitle;
        public string GoTitle;

        #endregion

        #region Construction

        public CompareViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadCompareViewModel(this);
            }
        }

        #endregion
    }
}
