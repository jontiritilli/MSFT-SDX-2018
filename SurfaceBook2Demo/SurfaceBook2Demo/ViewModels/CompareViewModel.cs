using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class CompareViewModel : ViewModelBase
    {


        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";
        //NOTE TO DEV- THE WIDTHS ARE NOT RECONCILED WITH THE ASSETS. AND THE 13 TO 15 DISTINCTION IS ALSO NOT MADE
        // THIS CHECK IN WAS MADE FOR THE BETA AND NEEDS TO BE FIXED AFTER TY
        private const string URI_FAMILY_13 = "ms-appx:///Assets/Comparison/comparisonFamily_13.png";
        private const int IMG_FAMILY_WIDTH_13 = 1500;//3000
        private const int IMG_FAMILY_HEIGHT_13 = 534;//1068

        private const string URI_FAMILY_15 = "ms-appx:///Assets/Comparison/comparisonFamily_15.png";
        private const int IMG_FAMILY_WIDTH_15 = 1620;//3240
        private const int IMG_FAMILY_HEIGHT_15 = 582;//1164

        private const string URI_PRO = "ms-appx:///Assets/Comparison/comparisonCruz.png";
        private const int IMG_PRO_WIDTH = 702;//1404
        private const int IMG_PRO_HEIGHT = 630;//1260

        private const string URI_BOOK = "ms-appx:///Assets/Comparison/comparisonSb2.png";
        private const int IMG_BOOK_WIDTH = 702;//1404
        private const int IMG_BOOK_HEIGHT = 630;//1260

        private const string URI_STUDIO = "ms-appx:///Assets/Comparison/comparisonCaprock.png";
        private const int IMG_STUDIO_WIDTH = 702;//1404
        private const int IMG_STUDIO_HEIGHT = 630;//1260

        private const string URI_LAPTOP = "ms-appx:///Assets/Comparison/comparisonFoxburg.png";
        private const int IMG_LAPTOP_WIDTH = 702;//1404
        private const int IMG_LAPTOP_HEIGHT = 630;//1260

        private const string URI_GO = "ms-appx:///Assets/Comparison/comparisonGo.png";
        private const int IMG_GO_WIDTH = 702;//1404
        private const int IMG_GO_HEIGHT = 630;//1260
        #endregion

        #region Public Properties

        public string Headline;
        public string Lede;

        public string BackgroundUri = URI_BACKGROUND;

        public string FamilyUri;
        public int FamilyHeight;
        public int FamilyWidth;

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
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.FamilyUri = URI_FAMILY_15;
                    this.FamilyHeight = IMG_FAMILY_HEIGHT_15;
                    this.FamilyWidth = IMG_FAMILY_WIDTH_15;
                    break;
                case DeviceType.Book13:
                    this.FamilyUri = URI_FAMILY_13;
                    this.FamilyHeight = IMG_FAMILY_HEIGHT_13;
                    this.FamilyWidth = IMG_FAMILY_WIDTH_13;
                    break;
                default:
                    break;
            }
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
