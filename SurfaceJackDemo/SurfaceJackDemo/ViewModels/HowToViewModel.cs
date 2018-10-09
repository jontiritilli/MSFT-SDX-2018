using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceJackDemo.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SurfaceJackDemo.ViewModels
{
    public class HowToViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Controls/controls_imageFPO1.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Controls/controls_imageFPO2.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Controls/controls_imageFPO3.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Controls/controls_imageFPO4.png";
        private const string URI_IMAGESELECTOR_IMAGE_5 = "ms-appx:///Assets/Controls/controls_imageFPO5.png";

        private const string URI_APPSELECTOR_ICON_1 = "ms-appx:///Assets/Controls/overview.png";
        private const string URI_APPSELECTOR_ICON_2 = "ms-appx:///Assets/Controls/playback.png";
        private const string URI_APPSELECTOR_ICON_3 = "ms-appx:///Assets/Controls/volume.png";
        private const string URI_APPSELECTOR_ICON_4 = "ms-appx:///Assets/Controls/cortana.png";
        private const string URI_APPSELECTOR_ICON_5 = "ms-appx:///Assets/Controls/noiseCancellation.png";
        private const string URI_APPSELECTOR_ICON_6 = "ms-appx:///Assets/Controls/mic.png";

        private const string URI_APPSELECTOR_ICON_1_SELECTED = "ms-appx:///Assets/Controls/overview_selected.png";
        private const string URI_APPSELECTOR_ICON_2_SELECTED = "ms-appx:///Assets/Controls/playback_selected.png";
        private const string URI_APPSELECTOR_ICON_3_SELECTED = "ms-appx:///Assets/Controls/volume_selected.png";
        private const string URI_APPSELECTOR_ICON_4_SELECTED = "ms-appx:///Assets/Controls/cortana_selected.png";
        private const string URI_APPSELECTOR_ICON_5_SELECTED = "ms-appx:///Assets/Controls/noiseCancellation_selected.png";
        private const string URI_APPSELECTOR_ICON_6_SELECTED = "ms-appx:///Assets/Controls/mic_selected.png";


        //private const string URI_IMAGE = "ms-appx:///Assets/Experience/joplin_design.png";
        private const double WIDTH_IMAGE = 1728;
        private const double HEIGHT_IMAGE = 1047;
        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/cruz_close_app_button.png";
        #endregion


        #region Public Properties
        public string Headline;
        public string Legal;
        public string x_ImageURI = URI_X_IMAGE;
        public string BackgroundUri;
        //public string ImageUri = URI_IMAGE;
        public double ImageWidth;
        public double radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        public SolidColorBrush ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");
        public double closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonCloseIconWidth);
        public double EllipseGridCanvasSetLeft;
        public double CloseEllipseMargin = StyleHelper.GetApplicationDouble("CompareCloseMargin");
        public double PageWidth = StyleHelper.GetApplicationDouble("ScreenWidth");
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();
        public double ImageSelectorImageWidth;
        public double ImageSelectorImageHeight;
        public List<AppSelectorData> ListItems = new List<AppSelectorData>();
        #endregion

        #region Construction

        public HowToViewModel()
        {
            EllipseGridCanvasSetLeft = PageWidth - CloseEllipseMargin - radiatingButtonRadius;
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    BackgroundUri = URI_BACKGROUND_FOXBURG;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 3 * 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 3 * 2;
                    break;
                case DeviceType.Studio:
                    BackgroundUri = URI_BACKGROUND_CAPROCK;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    break;
                case DeviceType.Book15:
                    BackgroundUri = URI_BACKGROUND_SB2_15;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    break;
                case DeviceType.Book13:
                    BackgroundUri = URI_BACKGROUND_SB2_13;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    break;
                case DeviceType.Pro:
                    BackgroundUri = URI_BACKGROUND_CRUZ;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    break;
                default:
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    break;
            }

            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = WIDTH_IMAGE
            });

            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = WIDTH_IMAGE
            });

            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = WIDTH_IMAGE
            });

            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_4,
                Width = WIDTH_IMAGE
            });

            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_5,
                Width = WIDTH_IMAGE
            });
            this.ListItems.Add(new AppSelectorData() {                
                Source_NotSelectedImage = URI_APPSELECTOR_ICON_1,
                Source_SelectedImage = URI_APPSELECTOR_ICON_1_SELECTED
            });
            this.ListItems.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_ICON_2,
                Source_SelectedImage = URI_APPSELECTOR_ICON_2_SELECTED
            });
            this.ListItems.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_ICON_3,
                Source_SelectedImage = URI_APPSELECTOR_ICON_3_SELECTED
            });
            this.ListItems.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_ICON_4,
                Source_SelectedImage = URI_APPSELECTOR_ICON_4_SELECTED
            });
            this.ListItems.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_ICON_5,
                Source_SelectedImage = URI_APPSELECTOR_ICON_5_SELECTED
            });
            this.ListItems.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_ICON_6,
                Source_SelectedImage = URI_APPSELECTOR_ICON_6_SELECTED
            });
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadHowToViewModel(this);
            }
        }

        #endregion
    }

    //class CustomStyleSelecter : StyleSelector
    //{
    //    protected override Style SelectStyleCore(object item, DependencyObject container)
    //    {
    //        SolidColorBrush highlight = (item as AppSelectorData).Highlight;
    //        Style style = new Style(typeof(ListViewItem));
    //        style.Setters.Add(new Setter(Control.BackgroundProperty, highlight));
    //        return style;
    //    }
    //}
    
}
