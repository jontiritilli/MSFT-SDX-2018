using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;


namespace SurfaceStudioDemo.ViewModels
{
    public class CompareViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";

        private const string URI_FAMILY = "ms-appx:///Assets/Comparison/comparisonFamily.png";
        private const int IMG_FAMILY_WIDTH = 2400;//4500

        //NOTE TO FUTURE DEV- THE WIDTHS BELOW ARE NOT CORRECTED FOR ACTUAL ASSETS. PLS CHECK WITH ASSETS
        private const string URI_PRO = "ms-appx:///Assets/Comparison/comparisonPro.png";
        private const int IMG_PRO_WIDTH = 702;//1404

        private const string URI_BOOK = "ms-appx:///Assets/Comparison/comparisonSb2.png";
        private const int IMG_BOOK_WIDTH = 702;//1404

        private const string URI_STUDIO = "ms-appx:///Assets/Comparison/comparisonStudio.png";
        private const int IMG_STUDIO_WIDTH = 702;//1404

        private const string URI_LAPTOP = "ms-appx:///Assets/Comparison/comparisonLaptop.png";
        private const int IMG_LAPTOP_WIDTH = 702;//1404

        private const string URI_GO = "ms-appx:///Assets/Comparison/comparisonGo.png";
        private const int IMG_GO_WIDTH = 702;//1404
        #endregion

        #region Public Properties

        public string Headline;
        public string Lede;

        public string BackgroundUri = URI_BACKGROUND;

        public string FamilyUri = URI_FAMILY;
        public int FamilyWidth = IMG_FAMILY_WIDTH;

        public string ProUri = URI_PRO;
        public int ProWidth = IMG_PRO_WIDTH;

        public string BookUri = URI_BOOK;
        public int BookWidth = IMG_BOOK_WIDTH;

        public string StudioUri = URI_STUDIO;
        public int StudioWidth = IMG_STUDIO_WIDTH;

        public string LaptopUri = URI_LAPTOP;
        public int LaptopWidth = IMG_LAPTOP_WIDTH;

        public string GoUri = URI_GO;
        public int GoWidth = IMG_GO_WIDTH;

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
