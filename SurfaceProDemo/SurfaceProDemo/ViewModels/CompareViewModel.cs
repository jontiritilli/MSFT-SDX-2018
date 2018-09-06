using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class CompareViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";

        private const string URI_FAMILY = "ms-appx:///Assets/Comparison/comparisonFamily.png";
        private const int IMG_FAMILY_WIDTH = 1368;//2736
        private const int IMG_FAMILY_HEIGHT = 492;//984

        private const string URI_PRO = "ms-appx:///Assets/Comparison/comparisonCruz.png";
        private const int IMG_PRO_WIDTH = 702;//1404
        private const int IMG_PRO_HEIGHT = 630;//1260

        private const string URI_BOOK = "ms-appx:///Assets/Comparison/cruz_comparisonSb2.png";
        private const int IMG_BOOK_WIDTH = 702;//1404
        private const int IMG_BOOK_HEIGHT = 630;//1260

        private const string URI_STUDIO = "ms-appx:///Assets/Comparison/comparisonCaprock.png";
        private const int IMG_STUDIO_WIDTH = 702;//1404
        private const int IMG_STUDIO_HEIGHT = 630;//1260

        private const string URI_LAPTOP = "ms-appx:///Assets/Comparison/comparisonFoxburg.png";
        private const int IMG_LAPTOP_WIDTH = 702;//1404
        private const int IMG_LAPTOP_HEIGHT = 630;//1260

        private const string URI_GO = "ms-appx:///Assets/Comparison/cruz_comparisonGo.png";
        private const int IMG_GO_WIDTH = 702;//1404
        private const int IMG_GO_HEIGHT = 630;//1260
        #endregion


        #region Public Properties

        public string Headline;
        public string Lede;    

        public string BackgroundUri = URI_BACKGROUND;

        public string FamilyUri = URI_FAMILY;
        public int FamilyHeight = IMG_FAMILY_HEIGHT;
        public int FamilyWidth = IMG_FAMILY_WIDTH;

        public string ProUri = URI_PRO;
        public int ProHeight = IMG_PRO_HEIGHT;
        public int ProWidth = IMG_PRO_WIDTH;

        public string BookUri = URI_BOOK;
        public int BookHeight = IMG_BOOK_HEIGHT;
        public int BookWidth = IMG_BOOK_WIDTH;
        
        public string StudioUri = URI_STUDIO;
        public int StudioHeight = IMG_STUDIO_HEIGHT;
        public int StudioWidth = IMG_STUDIO_WIDTH;
        
        public string LaptopUri = URI_LAPTOP;
        public int LaptopHeight = IMG_LAPTOP_HEIGHT;
        public int LaptopWidth = IMG_LAPTOP_WIDTH;
        
        public string GoUri = URI_GO;
        public int GoHeight = IMG_GO_HEIGHT;
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
