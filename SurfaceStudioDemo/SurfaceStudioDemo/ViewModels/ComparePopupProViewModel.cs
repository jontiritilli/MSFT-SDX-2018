﻿using System;
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
using SurfaceStudioDemo.Services;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceStudioDemo.ViewModels
{
    public class ComparePopupProViewModel
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";
        private const string URI_PRIMARYIMAGE = "ms-appx:///Assets/Comparison/comparisonPro.png";
        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        private const double PRIMARY_IMAGEHEIGHT = 1200;
        private const double PRIMARY_IMAGEWIDTH = 1134.5;

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string PrimaryImageURI = URI_PRIMARYIMAGE;
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

        public double radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        public double closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight);

        public double MaxImageWidth = StyleHelper.GetApplicationDouble("ScreenWidth");
        public double MaxImageHeight = StyleHelper.GetApplicationDouble("ScreenHeight");

        public string x_ImageURI = URI_X_IMAGE;
        public double EllipseGridCanvasSetLeft;
        public double CloseEllipseTopMargin = StyleHelper.GetApplicationDouble("CloseButtonTopMargin");
        public double CloseEllipseRightMargin = StyleHelper.GetApplicationDouble("CloseButtonRightMargin");

        public List<ListItem> CompareListItems = new List<ListItem>();

        public SolidColorBrush ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");

        #endregion

        #region Construction

        public ComparePopupProViewModel()
        {
            EllipseGridCanvasSetLeft = MaxImageWidth - CloseEllipseRightMargin - radiatingButtonRadius;

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadComparePopupProViewModel(this);
            }
        }

        #endregion
    }
}
