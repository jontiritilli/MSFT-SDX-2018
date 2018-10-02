using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperiencePerformanceViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Performance/foxburg_powerpoint.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Performance/foxburg_word.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Performance/foxburg_excel.png";

        private const string URI_MINOR_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Performance/foxburg_books.png";
        private const string URI_MINOR_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Performance/foxburg_plant.png";
        private const string URI_MINOR_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Performance/foxburg_graphs.png";

        private const string URI_APPSELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Performance/Icons/ppt.png";
        private const string URI_APPSELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Performance/Icons/word.png";
        private const string URI_APPSELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Performance/Icons/excel.png";

        private const string URI_APPSELECTOR_IMAGE_1_SELECTED = "ms-appx:///Assets/Experience/Performance/Icons/ppt.png";
        private const string URI_APPSELECTOR_IMAGE_2_SELECTED = "ms-appx:///Assets/Experience/Performance/Icons/word.png";
        private const string URI_APPSELECTOR_IMAGE_3_SELECTED = "ms-appx:///Assets/Experience/Performance/Icons/excel.png";
        
        private const double SELECTORIMAGE_IMAGEWIDTH = 1150;
        private const double SELECTORIMAGE_IMAGEHEIGHT = 766.667;

        private const double SELECTORIMAGEMINOR_IMAGEWIDTH = 600;
        private const double SELECTORIMAGEMINOR_IMAGEHEIGHT = 400;

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;

        public string BackgroundUri = URI_BACKGROUND;

        public double ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
        public double ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;

        // complementary asset image sizes
        public double ImageSelectorMinorImageWidth = SELECTORIMAGEMINOR_IMAGEWIDTH;
        public double ImageSelectorMinorImageHeight = SELECTORIMAGEMINOR_IMAGEHEIGHT;

        public List<AppSelectorData> performanceSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> performanceSelectorImageURIs = new List<AppSelectorImageURI>();
        public List<AppSelectorImageURI> performanceSelectorMinorImageURIs = new List<AppSelectorImageURI>();

        #endregion

        public ExperiencePerformanceViewModel()
        {
            // list of app icons
            this.performanceSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_1,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_1_SELECTED
            });
            this.performanceSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_2,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_2_SELECTED
            });
            this.performanceSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_3,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_3_SELECTED
            });

            // list of associated app images to display
            this.performanceSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.performanceSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.performanceSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            // list of associated app complementary assets to display
            this.performanceSelectorMinorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_MINOR_IMAGESELECTOR_IMAGE_1,
                Width = SELECTORIMAGEMINOR_IMAGEWIDTH
            });
            this.performanceSelectorMinorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_MINOR_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGEMINOR_IMAGEWIDTH
            });
            this.performanceSelectorMinorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_MINOR_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGEMINOR_IMAGEWIDTH
            });

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperiencePerformanceViewModel(this);
            }
        }
    }
}
