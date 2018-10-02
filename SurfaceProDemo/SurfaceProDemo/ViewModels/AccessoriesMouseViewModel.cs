using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class AccessoriesMouseViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Accessories/Images/cobalt-right.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Accessories/Images/black-right.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Accessories/Images/platinum-right.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Accessories/Images/bugundy-right.png";

        private const string URI_APPSELECTOR_COLOR_1 = "ms-appx:///Assets/Icons/AccessoriesColors/cobalt.png";
        private const string URI_APPSELECTOR_COLOR_2 = "ms-appx:///Assets/Icons/AccessoriesColors/black.png";
        private const string URI_APPSELECTOR_COLOR_3 = "ms-appx:///Assets/Icons/AccessoriesColors/platinum.png";
        private const string URI_APPSELECTOR_COLOR_4 = "ms-appx:///Assets/Icons/AccessoriesColors/burgundy.png";

        private const string URI_APPSELECTOR_COLOR_1_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/cobalt_active.png";
        private const string URI_APPSELECTOR_COLOR_2_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/black_active.png";
        private const string URI_APPSELECTOR_COLOR_3_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/platinum_active.png";
        private const string URI_APPSELECTOR_COLOR_4_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/burgundy_active.png";

        private const int APPSELECTOR_BUTTON_WIDTH = 40;
        private const int APPSELECTOR_BUTTON_HEIGHT = 40;

        private const int SELECTORIMAGE_IMAGEHEIGHT = 912;
        private const int SELECTORIMAGE_IMAGEWIDTH = 1368;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;


        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;


        public string Headline;
        public string Lede;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftLegal;
        public string PopRightHeadline;
        public string PopRightLede;
        public string PopRightLegal;

        public List<AppSelectorData> appSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();

        public int ColoringBookButtonWidth;
        public int ColoringBookButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        #endregion


        #region Construction

        public AccessoriesMouseViewModel()
        {
            this.ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            this.ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_1,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_1_SELECTED
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_2,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_2_SELECTED
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_3,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_3_SELECTED
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_4,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_4_SELECTED
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
                localizationService.LoadAccessoriesMouseViewModel(this);
            }
        }

        #endregion
    }
}
