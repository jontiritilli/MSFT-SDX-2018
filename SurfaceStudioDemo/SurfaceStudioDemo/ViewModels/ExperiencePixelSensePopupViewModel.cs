using System;
using System.Collections.Generic;

using SurfaceStudioDemo.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceStudioDemo.ViewModels
{
    public class ExperiencePixelSensePopupViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";
        private const string URI_LEFT = "ms-appx:///Assets/Experience/appsDesktop.png";
        private const string URI_RIGHT = "ms-appx:///Assets/Experience/appsStudio.png";
        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";


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

        private const int APPSELECTOR_BUTTON_WIDTH = 60;
        private const int APPSELECTOR_BUTTON_HEIGHT = 60;

        private const double SELECTORIMAGE_IMAGEHEIGHT = 1200;
        private const double SELECTORIMAGE_IMAGEWIDTH = 1134.5;

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string Lede;
        public string Legal;

        public double ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
        public double ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;

        public List<AppSelectorData> IconURIS = new List<AppSelectorData>();
        public List<AppSelectorImageURI> ImageURIS = new List<AppSelectorImageURI>();

        public double radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        public double closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight) / 2;

        public double MaxImageWidth = StyleHelper.GetApplicationDouble("ScreenWidth");
        public double MaxImageHeight = StyleHelper.GetApplicationDouble("ScreenHeight");

        public string x_ImageURI = URI_X_IMAGE;
        public double EllipseGridCanvasSetLeft;
        public double CloseEllipseTopMargin = StyleHelper.GetApplicationDouble("CloseButtonTopMargin");
        public double CloseEllipseRightMargin = StyleHelper.GetApplicationDouble("CloseButtonRightMargin");

        public SolidColorBrush ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");

        #endregion

        #region Construction

        public ExperiencePixelSensePopupViewModel()
        {
            EllipseGridCanvasSetLeft = MaxImageWidth - CloseEllipseRightMargin - radiatingButtonRadius;

            IconURIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_1,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_1
            });

            IconURIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_2,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_2
            });

            IconURIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_3,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_3
            });

            IconURIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_4,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_4
            });

            IconURIS.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_APP_5,
                Source_SelectedImage = URI_APPSELECTOR_APP_SELECTED_5
            });

            ImageURIS.Add(new AppSelectorImageURI()
            {
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
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperiencePixelSensePopupViewModel(this);
            }
        }

        #endregion
    }
}
