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

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/caprock_adobecc.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/caprock_sketchbook.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/caprock_bluebeam.png";

        private const string URI_APPSELECTOR_APP_1 = "ms-appx:///Assets/Icons/AppIcons/adobecc.png";
        private const string URI_APPSELECTOR_APP_2 = "ms-appx:///Assets/Icons/AppIcons/autodesk.png";
        private const string URI_APPSELECTOR_APP_3 = "ms-appx:///Assets/Icons/AppIcons/bluebeam.png";

        private const string URI_APPSELECTOR_APP_SELECTED_1 = "ms-appx:///Assets/Icons/AppIcons/adobecc.png";
        private const string URI_APPSELECTOR_APP_SELECTED_2 = "ms-appx:///Assets/Icons/AppIcons/autodesk.png";
        private const string URI_APPSELECTOR_APP_SELECTED_3 = "ms-appx:///Assets/Icons/AppIcons/bluebeam.png";

        private const double SELECTORIMAGE_IMAGEHEIGHT = 1400;
        private const double SELECTORIMAGE_IMAGEWIDTH = 1300;

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
        public double closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight);

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
