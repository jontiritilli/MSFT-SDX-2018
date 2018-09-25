using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using SurfaceBook2Demo.Services;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceBook2Demo.ViewModels
{
    public class ComparePopupStudioViewModel
    {
        #region Constants

        private const string URI_PRIMARYIMAGE_13 = "ms-appx:///Assets/Comparison/comparisonStudio_13.png";
        private const string URI_PRIMARYIMAGE_15 = "ms-appx:///Assets/Comparison/comparisonStudio_15.png";

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        private const double PRIMARY_IMAGEWIDTH_13 = 798;
        private const double PRIMARY_IMAGEWIDTH_15 = 876;

        #endregion

        #region Public Properties

        public string PrimaryImageURI;
        public double PrimaryImageWidth;
        public string Headline;

        public string SubHead;
        public string Lede;
        public string Legal;

        public string BulletOneLegal;
        public string BulletTwoLegal;
        public string BulletThreeLegal;
        public string BulletFourLegal;
        public string BulletFiveLegal;

        public double ICON_WIDTH = StyleHelper.GetApplicationDouble(LayoutSizes.CompareListIconWidth);

        public string x_ImageURI = URI_X_IMAGE;
        public double radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        public SolidColorBrush ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");
        public double closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight);
        public double EllipseGridCanvasSetLeft;
        public double CloseEllipseMargin = StyleHelper.GetApplicationDouble("CompareCloseMargin");

        public double PageWidth = StyleHelper.GetApplicationDouble("ScreenWidth");

        public List<ListItem> CompareListItems = new List<ListItem>();

        #endregion

        #region Construction

        public ComparePopupStudioViewModel()
        {
            EllipseGridCanvasSetLeft = PageWidth - CloseEllipseMargin - radiatingButtonRadius;

            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.PrimaryImageURI = URI_PRIMARYIMAGE_15;
                    this.PrimaryImageWidth = PRIMARY_IMAGEWIDTH_15;
                    break;
                case DeviceType.Book13:
                    this.PrimaryImageURI = URI_PRIMARYIMAGE_13;
                    this.PrimaryImageWidth = PRIMARY_IMAGEWIDTH_13;
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
                localizationService.LoadComparePopupStudioViewModel(this);
            }
        }

        #endregion
    }
}
