using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceHeadphoneDemo.Services;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SurfaceHeadphoneDemo.ViewModels
{
    public class HowToViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Controls/overview-device.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Controls/play-pause-device.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Controls/skip-device.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Controls/volume-device.png";
        private const string URI_IMAGESELECTOR_IMAGE_5 = "ms-appx:///Assets/Controls/noise-cancellation-device.png";
        private const string URI_IMAGESELECTOR_IMAGE_6 = "ms-appx:///Assets/Controls/answer-lifestyle.jpg";

        private const string URI_APPSELECTOR_ICON_1 = "ms-appx:///Assets/Controls/overview.png";
        private const string URI_APPSELECTOR_ICON_2 = "ms-appx:///Assets/Controls/play-pause.png";
        private const string URI_APPSELECTOR_ICON_3 = "ms-appx:///Assets/Controls/skip.png";
        private const string URI_APPSELECTOR_ICON_4 = "ms-appx:///Assets/Controls/volume.png";
        private const string URI_APPSELECTOR_ICON_5 = "ms-appx:///Assets/Controls/noise-cancellation.png";
        private const string URI_APPSELECTOR_ICON_6 = "ms-appx:///Assets/Controls/answer.png";

        private const string URI_APPSELECTOR_ICON_1_SELECTED = "ms-appx:///Assets/Controls/overview-selected.png";
        private const string URI_APPSELECTOR_ICON_2_SELECTED = "ms-appx:///Assets/Controls/play-pause-selected.png";
        private const string URI_APPSELECTOR_ICON_3_SELECTED = "ms-appx:///Assets/Controls/skip-selected.png";
        private const string URI_APPSELECTOR_ICON_4_SELECTED = "ms-appx:///Assets/Controls/volume-selected.png";
        private const string URI_APPSELECTOR_ICON_5_SELECTED = "ms-appx:///Assets/Controls/noise-cancellation-selected.png";
        private const string URI_APPSELECTOR_ICON_6_SELECTED = "ms-appx:///Assets/Controls/answer-selected.png";

        // illustration overlays
        private const string URI_ILLUSTRATION_1 = "ms-appx:///Assets/Controls/overview-device-illustration.png";
        private const string URI_ILLUSTRATION_2 = "ms-appx:///Assets/Controls/play-pause-illustration.png";
        private const string URI_ILLUSTRATION_3_INNER = "ms-appx:///Assets/Controls/skip-illustration-inner.png";
        private const string URI_ILLUSTRATION_3_OUTER = "ms-appx:///Assets/Controls/skip-illustration-outer.png";
        private const string URI_ILLUSTRATION_4 = "ms-appx:///Assets/Controls/volume-illustration.png";
        private const string URI_ILLUSTRATION_5 = "ms-appx:///Assets/Controls/noise-cancellation-illustration.png";

        // overview overlays
        private const string URI_OVERVIEW_1 = "ms-appx:///Assets/Controls/OverviewLines/Noise-cancellation.png";
        private const string URI_OVERVIEW_2 = "ms-appx:///Assets/Controls/OverviewLines/Power.png";
        private const string URI_OVERVIEW_3 = "ms-appx:///Assets/Controls/OverviewLines/Volume.png";
        private const string URI_OVERVIEW_4 = "ms-appx:///Assets/Controls/OverviewLines/3.5-connection.png";
        private const string URI_OVERVIEW_5 = "ms-appx:///Assets/Controls/OverviewLines/Usb-port.png";
        private const string URI_OVERVIEW_6 = "ms-appx:///Assets/Controls/OverviewLines/Mic-mute.png";

        private const double WIDTH_OVERVIEW_CAPTION = 110;

        //no height ? height == width
        private const double WIDTH_OVERVIEW_1 = 188;
        private const double WIDTH_OVERVIEW_2 = 550;
        private const double WIDTH_OVERVIEW_3 = 204;
        private const double WIDTH_OVERVIEW_4 = 319;
        private const double WIDTH_OVERVIEW_5 = 434;
        private const double WIDTH_OVERVIEW_6 = 70;

        private const double WIDTH_PLAY_PAUSE_ILLUSTRATION = 215;
        private const double WIDTH_SKIP_ILLUSTRATION = 303;

        private const double WIDTH_VOLUME_ILLUSTRATION = 574;
        private const double HEIGHT_VOLUME_ILLUSTRATION = 363;

        private const double WIDTH_OVERVIEW_ILLUSTRATION = 2016;
        private const double HEIGHT_OVERVIEW_ILLUSTRATION = 1640;

        private const double WIDTH_NOISE_ILLUSTRATION = 622;
        private const double HEIGHT_NOISE_ILLUSTRATION = 422;

        private const double WIDTH_IMAGE = 2016;
        private const double HEIGHT_IMAGE = 1640;
        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";
        private const string URI_AS_IMAGE_BACKGROUND = "ms-appx:///Assets/Controls/controls-background.jpg";

        #endregion

        #region Public Properties

        public string Headline;
        public string Legal;
        public string Caption2;
        public string Caption3;
        public string Caption4;
        public string Caption5;
        public string Caption6;

        public double OverviewCaptionWidth = WIDTH_OVERVIEW_CAPTION;
        public string OverviewCaption1;
        public string OverviewCaption2;
        public string OverviewCaption3;
        public string OverviewCaption4;
        public string OverviewCaption5;
        public string OverviewCaption6;

        public string x_ImageURI = URI_X_IMAGE;
        public string BackgroundUri;
        public string AppSelectorImageBackgroundUri = URI_AS_IMAGE_BACKGROUND;

        public SolidColorBrush SeletedBGColor = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));

        //public string ImageUri = URI_IMAGE;
        public double ImageWidth;
        public double radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        public SolidColorBrush ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");
        public double closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonCloseIconWidth);
        public double EllipseGridCanvasSetLeft;
        public double CloseEllipseMargin = StyleHelper.GetApplicationDouble("CompareCloseMargin");
        public double PageWidth = StyleHelper.GetApplicationDouble("ScreenWidth");
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();
        public List<AppSelectorImageURI> appSelectorIllustrationImageURIs = new List<AppSelectorImageURI>();
        public double ImageSelectorImageWidth;
        public double ImageSelectorImageHeight;
        public List<AppSelectorData> ListItems = new List<AppSelectorData>();
        public List<object> IllustrationScreenPositions = new List<object>();

        public string OverviewIllustrationURI = URI_ILLUSTRATION_1;
        public double Width_Overview = WIDTH_OVERVIEW_ILLUSTRATION;
        public double Height_Overview = HEIGHT_OVERVIEW_ILLUSTRATION;

        public string PlayIllustrationURI = URI_ILLUSTRATION_2;
        public double Width_Play_Pause = WIDTH_PLAY_PAUSE_ILLUSTRATION;

        public string SkipIllustrationInnerURI = URI_ILLUSTRATION_3_INNER;
        public string SkipIllustrationOuterURI = URI_ILLUSTRATION_3_OUTER;
        public double Width_Skip = WIDTH_SKIP_ILLUSTRATION;

        public string VolumeIllustrationURI = URI_ILLUSTRATION_4;
        public double Width_Volume = WIDTH_VOLUME_ILLUSTRATION;
        public double Height_Volume = HEIGHT_VOLUME_ILLUSTRATION;

        public string NoiseIllustrationURI = URI_ILLUSTRATION_5;
        public double Width_Noise = WIDTH_NOISE_ILLUSTRATION;
        public double Height_Noise = HEIGHT_NOISE_ILLUSTRATION;

        public double Width_Overview_1;
        public double Width_Overview_2;
        public double Width_Overview_3;
        public double Width_Overview_4;
        public double Width_Overview_5;
        public double Width_Overview_6;

        public string OverviewOverlayURI_1 = URI_OVERVIEW_1;
        public string OverviewOverlayURI_2 = URI_OVERVIEW_2;
        public string OverviewOverlayURI_3 = URI_OVERVIEW_3;
        public string OverviewOverlayURI_4 = URI_OVERVIEW_4;
        public string OverviewOverlayURI_5 = URI_OVERVIEW_5;
        public string OverviewOverlayURI_6 = URI_OVERVIEW_6;

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
                    ImageSelectorImageWidth = (WIDTH_IMAGE / 3) * 2;
                    ImageSelectorImageHeight = (HEIGHT_IMAGE / 3) * 2;
                    Width_Play_Pause = (WIDTH_PLAY_PAUSE_ILLUSTRATION / 3) *2;
                    Width_Skip = (WIDTH_SKIP_ILLUSTRATION / 3) *2;
                    Width_Volume = (WIDTH_VOLUME_ILLUSTRATION / 3) *2;
                    Width_Overview = (WIDTH_OVERVIEW_ILLUSTRATION / 3) *2;
                    Width_Noise = (WIDTH_NOISE_ILLUSTRATION / 3) *2;
                    Width_Overview_1 = (WIDTH_OVERVIEW_1 / 3) * 2;
                    Width_Overview_2 = (WIDTH_OVERVIEW_2 / 3) * 2;
                    Width_Overview_3 = (WIDTH_OVERVIEW_3 / 3) * 2;
                    Width_Overview_4 = (WIDTH_OVERVIEW_4 / 3) * 2;
                    Width_Overview_5 = (WIDTH_OVERVIEW_5 / 3) * 2;
                    Width_Overview_6 = (WIDTH_OVERVIEW_6 / 3) * 2;
                    break;
                case DeviceType.Studio:
                    BackgroundUri = URI_BACKGROUND_CAPROCK;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    Width_Play_Pause = WIDTH_PLAY_PAUSE_ILLUSTRATION / 2;
                    Width_Skip = WIDTH_SKIP_ILLUSTRATION / 2;
                    Width_Volume = WIDTH_VOLUME_ILLUSTRATION / 2;
                    Width_Overview = WIDTH_OVERVIEW_ILLUSTRATION / 2;
                    Width_Noise = WIDTH_NOISE_ILLUSTRATION / 2;
                    Width_Overview_1 = WIDTH_OVERVIEW_1 / 2;
                    Width_Overview_2 = WIDTH_OVERVIEW_2 / 2;
                    Width_Overview_3 = WIDTH_OVERVIEW_3 / 2;
                    Width_Overview_4 = WIDTH_OVERVIEW_4 / 2;
                    Width_Overview_5 = WIDTH_OVERVIEW_5 / 2;
                    Width_Overview_6 = WIDTH_OVERVIEW_6 / 2;
                    break;
                case DeviceType.Book15:
                    BackgroundUri = URI_BACKGROUND_SB2_15;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    Width_Play_Pause = WIDTH_PLAY_PAUSE_ILLUSTRATION / 2;
                    Width_Skip = WIDTH_SKIP_ILLUSTRATION / 2;
                    Width_Volume = WIDTH_VOLUME_ILLUSTRATION / 2;
                    Width_Overview = WIDTH_OVERVIEW_ILLUSTRATION / 2;
                    Width_Noise = WIDTH_NOISE_ILLUSTRATION / 2;
                    Width_Overview_1 = WIDTH_OVERVIEW_1 / 2;
                    Width_Overview_2 = WIDTH_OVERVIEW_2 / 2;
                    Width_Overview_3 = WIDTH_OVERVIEW_3 / 2;
                    Width_Overview_4 = WIDTH_OVERVIEW_4 / 2;
                    Width_Overview_5 = WIDTH_OVERVIEW_5 / 2;
                    Width_Overview_6 = WIDTH_OVERVIEW_6 / 2;
                    break;
                case DeviceType.Book13:
                    BackgroundUri = URI_BACKGROUND_SB2_13;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    Width_Play_Pause = WIDTH_PLAY_PAUSE_ILLUSTRATION / 2;
                    Width_Skip = WIDTH_SKIP_ILLUSTRATION / 2;
                    Width_Volume = WIDTH_VOLUME_ILLUSTRATION / 2;
                    Width_Overview = WIDTH_OVERVIEW_ILLUSTRATION / 2;
                    Width_Noise = WIDTH_NOISE_ILLUSTRATION / 2;
                    Width_Overview_1 = WIDTH_OVERVIEW_1 / 2;
                    Width_Overview_2 = WIDTH_OVERVIEW_2 / 2;
                    Width_Overview_3 = WIDTH_OVERVIEW_3 / 2;
                    Width_Overview_4 = WIDTH_OVERVIEW_4 / 2;
                    Width_Overview_5 = WIDTH_OVERVIEW_5 / 2;
                    Width_Overview_6 = WIDTH_OVERVIEW_6 / 2;
                    break;
                case DeviceType.Pro:
                case DeviceType.Unknown:
                default:
                    BackgroundUri = URI_BACKGROUND_CRUZ;
                    ImageSelectorImageWidth = WIDTH_IMAGE / 2;
                    ImageSelectorImageHeight = HEIGHT_IMAGE / 2;
                    Width_Play_Pause = WIDTH_PLAY_PAUSE_ILLUSTRATION / 2;
                    Width_Skip = WIDTH_SKIP_ILLUSTRATION / 2;
                    Width_Volume = WIDTH_VOLUME_ILLUSTRATION / 2;
                    Width_Overview = WIDTH_OVERVIEW_ILLUSTRATION / 2;
                    Width_Noise = WIDTH_NOISE_ILLUSTRATION / 2;
                    Height_Noise = HEIGHT_NOISE_ILLUSTRATION / 2;
                    Width_Overview_1 = WIDTH_OVERVIEW_1 / 2;
                    Width_Overview_2 = WIDTH_OVERVIEW_2 / 2;
                    Width_Overview_3 = WIDTH_OVERVIEW_3 / 2;
                    Width_Overview_4 = WIDTH_OVERVIEW_4 / 2;
                    Width_Overview_5 = WIDTH_OVERVIEW_5 / 2;
                    Width_Overview_6 = WIDTH_OVERVIEW_6 / 2;
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
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_6,
                Width = WIDTH_IMAGE
            });

            //Illustration Images List

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

    
}
