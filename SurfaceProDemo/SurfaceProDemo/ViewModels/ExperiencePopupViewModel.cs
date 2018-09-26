using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceProDemo.Services;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SurfaceProDemo.ViewModels
{
    public class ExperiencePopupViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/laptop_premiere.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/laptop_maya.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/laptop_sketchbook.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Experience/laptop_bluebeam.png";
        private const string URI_IMAGESELECTOR_IMAGE_5 = "ms-appx:///Assets/Experience/laptop_photoshop.png";

        private const string URI_APPSELECTOR_COLOR_1 = "ms-appx:///Assets/Icons/AppIcons/icon_adobePremiereCC.png";
        private const string URI_APPSELECTOR_COLOR_2 = "ms-appx:///Assets/Icons/AppIcons/icon_autodeskMaya2018.png";
        private const string URI_APPSELECTOR_COLOR_3 = "ms-appx:///Assets/Icons/AppIcons/icon_autodeskSketchbook.png";
        private const string URI_APPSELECTOR_COLOR_4 = "ms-appx:///Assets/Icons/AppIcons/icon_blueBeam.png";
        private const string URI_APPSELECTOR_COLOR_5 = "ms-appx:///Assets/Icons/AppIcons/icon_adobePhotoshopCC.png";

        private const string URI_APPSELECTOR_COLOR_1_SELECTED = "ms-appx:///Assets/Icons/AppIcons/icon_adobePremiereCC.png";
        private const string URI_APPSELECTOR_COLOR_2_SELECTED = "ms-appx:///Assets/Icons/AppIcons/icon_autodeskMaya2018.png";
        private const string URI_APPSELECTOR_COLOR_3_SELECTED = "ms-appx:///Assets/Icons/AppIcons/icon_autodeskSketchbook.png";
        private const string URI_APPSELECTOR_COLOR_4_SELECTED = "ms-appx:///Assets/Icons/AppIcons/icon_blueBeam.png";
        private const string URI_APPSELECTOR_COLOR_5_SELECTED = "ms-appx:///Assets/Icons/AppIcons/icon_adobePhotoshopCC.png";

        private const int APPSELECTOR_BUTTON_WIDTH = 60;
        private const int APPSELECTOR_BUTTON_HEIGHT = 60;

        private const int SELECTORIMAGE_IMAGEHEIGHT = 588;
        private const int SELECTORIMAGE_IMAGEWIDTH = 900;

        private readonly double _radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        private readonly double _closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight) / 2;
        private readonly double _maxImageWidth = StyleHelper.GetApplicationDouble("CanvasWidth");
        private readonly double _maxImageHeight = StyleHelper.GetApplicationDouble("CanvasHeight");
        private readonly double _closeEllipseRightMargin = StyleHelper.GetApplicationDouble("CloseButtonRightMargin");
        private readonly double _closeEllipseTopMargin = StyleHelper.GetApplicationDouble("CloseButtonTopMargin");
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;


        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;


        public string Headline;
        public string Lede;

        public string PopLeftLegal;

        public List<AppSelectorData> appSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();

        public int AppSelectorButtonWidth;
        public int AppSelectorButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        public double radiatingButtonRadius;
        public double closeIconHeight;

        public string CloseButtonXURI;
        public double ellipseGridCanvasSetLeft;
        public double closeEllipseTopMargin;

        public SolidColorBrush ellipseStroke;

        public GridLength AppCloseWidth
        {
            get
            {
                return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.AppCloseWidth));
            }
        }
        #endregion


        #region Construction

        public ExperiencePopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();
            // set the header and lede and colors list for app selector
            // use the event to handle color changed

            AppSelectorButtonWidth = APPSELECTOR_BUTTON_WIDTH;
            AppSelectorButtonHeight = APPSELECTOR_BUTTON_HEIGHT;
            this.ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            this.ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;
            CloseButtonXURI = URI_X_IMAGE;
            closeEllipseTopMargin = _closeEllipseTopMargin;
            ellipseGridCanvasSetLeft = _maxImageWidth - _closeEllipseRightMargin - _radiatingButtonRadius;
            radiatingButtonRadius = _radiatingButtonRadius;
            closeIconHeight = _closeIconHeight;
            ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");
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

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_5,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_5_SELECTED
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
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_5,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperiencePopupViewModel(this);
            }
        }

        #endregion
    }
}
