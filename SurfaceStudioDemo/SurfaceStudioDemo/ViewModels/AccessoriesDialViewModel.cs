using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceStudioDemo.Services;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace SurfaceStudioDemo.ViewModels
{
    public class AccessoriesDialViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";
        
        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/sketchbook.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/sketchbook.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/sketchbook.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Experience/sketchbook.png";
        private const string URI_IMAGESELECTOR_IMAGE_5 = "ms-appx:///Assets/Experience/sketchbook.png";

        private const string URI_APPSELECTOR_APP_1 = "ms-appx:///Assets/Icons/AppIcons/premiere.png";
        private const string URI_APPSELECTOR_APP_2 = "ms-appx:///Assets/Icons/AppIcons/maya.png";
        private const string URI_APPSELECTOR_APP_3 = "ms-appx:///Assets/Icons/AppIcons/sketchbook.png";
        private const string URI_APPSELECTOR_APP_4 = "ms-appx:///Assets/Icons/AppIcons/bluebeam.png";
        private const string URI_APPSELECTOR_APP_5 = "ms-appx:///Assets/Icons/AppIcons/psd.png";

        private const string URI_APPSELECTOR_APP_SELECTED_1 = "ms-appx:///Assets/Icons/AppIcons/premiere.png";
        private const string URI_APPSELECTOR_APP_SELECTED_2 = "ms-appx:///Assets/Icons/AppIcons/maya.png";
        private const string URI_APPSELECTOR_APP_SELECTED_3 = "ms-appx:///Assets/Icons/AppIcons/sketchbook.png";
        private const string URI_APPSELECTOR_APP_SELECTED_4 = "ms-appx:///Assets/Icons/AppIcons/bluebeam.png";
        private const string URI_APPSELECTOR_APP_SELECTED_5 = "ms-appx:///Assets/Icons/AppIcons/psd.png";

        private static readonly Size WINDOW_BOUNDS = WindowHelper.GetViewSizeInfo();
        private static readonly double CANVAS_X = WINDOW_BOUNDS.Width;
        private static readonly double CANVAS_Y = WINDOW_BOUNDS.Height;

        private const int APPSELECTOR_BUTTON_WIDTH = 60;
        private const int APPSELECTOR_BUTTON_HEIGHT = 60;

        private const double SELECTORIMAGE_IMAGEHEIGHT = 1200;
        private const double SELECTORIMAGE_IMAGEWIDTH = 1134.5;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public double ImageSelectorImageWidth;
        public double ImageSelectorImageHeight;


        public string Headline;
        public string Lede;

        public string Legal;

        public List<AppSelectorData> URIS = new List<AppSelectorData>();
        public List<AppSelectorImageURI> ImageURIS = new List<AppSelectorImageURI>();

        public GridLength AppCloseWidth
        {
            get
            {
                return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.AppCloseWidth));
            }
        }

        public double AppSelectorButtonWidth;
        public double AppSelectorButtonHeight;
        #endregion


        #region Construction

        public AccessoriesDialViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();
            this.AppSelectorButtonWidth = StyleHelper.GetApplicationDouble(LayoutSizes.AppSelectorButtonWidth);
            this.AppSelectorButtonHeight = StyleHelper.GetApplicationDouble(LayoutSizes.AppSelectorButtonHeight);
            this.ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            this.ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;
            URIS.Add(new AppSelectorData() {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_1,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_1                
            });

            URIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_2,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_2
            });

            URIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_3,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_3
            });

            URIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_4,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_4
            });

            URIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_5,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_5
            });

            ImageURIS.Add(new AppSelectorImageURI() {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            ImageURIS.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            ImageURIS.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            ImageURIS.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_4,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            ImageURIS.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_5,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesDialViewModel(this);
            }
        }

        #endregion
    }
}
