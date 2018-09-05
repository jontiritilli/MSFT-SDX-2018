using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceColorsViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";

        private const string URI_APPSELECTOR_COLOR_1 = "ms-appx:///Assets/Icons/AccessoriesColors/cobalt_swatch.png";
        private const string URI_APPSELECTOR_COLOR_2 = "ms-appx:///Assets/Icons/AccessoriesColors/black_swatch.png";
        private const string URI_APPSELECTOR_COLOR_3 = "ms-appx:///Assets/Icons/AccessoriesColors/platinum_swatch.png";
        private const string URI_APPSELECTOR_COLOR_4 = "ms-appx:///Assets/Icons/AccessoriesColors/burgundy_swatch.png";

        private const string URI_APPSELECTOR_COLOR_1_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/cobalt_swatch.png";
        private const string URI_APPSELECTOR_COLOR_2_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/black_swatch.png";
        private const string URI_APPSELECTOR_COLOR_3_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/platinum_swatch.png";
        private const string URI_APPSELECTOR_COLOR_4_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/burgundy_swatch.png";

        private const int APPSELECTOR_BUTTON_WIDTH = 40;
        private const int APPSELECTOR_BUTTON_HEIGHT = 40;

        private const int SELECTORIMAGE_IMAGEHEIGHT = 648;
        private const int SELECTORIMAGE_IMAGEWIDTH = 764;

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;

        public string BackgroundUri;

        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;

        public List<AppSelectorData> appSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();

        #endregion

        public ExperienceColorsViewModel()
        {
            BackgroundUri = URI_BACKGROUND;

            this.ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            this.ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_1,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_1_SELECTED
            });
            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_2,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_2_SELECTED
            });
            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_3,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_3_SELECTED
            });
            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_4,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_4_SELECTED
            });

            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_4,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceColorsViewModel(this);
            }
        }
    }
}
