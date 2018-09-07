using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;
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

        private const string URI_APPSELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Icons/fox_ppt.png";
        private const string URI_APPSELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Icons/fox_word.png";
        private const string URI_APPSELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Icons/fox_excel.png";

        private const string URI_APPSELECTOR_IMAGE_1_SELECTED = "ms-appx:///Assets/Experience/Icons/fox_ppt.png";
        private const string URI_APPSELECTOR_IMAGE_2_SELECTED = "ms-appx:///Assets/Experience/Icons/fox_word.png";
        private const string URI_APPSELECTOR_IMAGE_3_SELECTED = "ms-appx:///Assets/Experience/Icons/fox_excel.png";

        private const int APPSELECTOR_BUTTON_WIDTH = 80;
        private const int APPSELECTOR_BUTTON_HEIGHT = 80;

        private const int SELECTORIMAGE_IMAGEWIDTH = 1100;
        private const int SELECTORIMAGE_IMAGEHEIGHT = 750;

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;

        public string BackgroundUri;

        public int AppSelectorButtonWidth;
        public int AppSelectorButtonHeight;

        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;

        public List<AppSelectorData> performanceSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> performanceSelectorImageURIs = new List<AppSelectorImageURI>();

        #endregion

        public ExperiencePerformanceViewModel()
        {

            BackgroundUri = URI_BACKGROUND;

            AppSelectorButtonWidth = APPSELECTOR_BUTTON_WIDTH;
            AppSelectorButtonHeight = APPSELECTOR_BUTTON_HEIGHT;

            ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;

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
