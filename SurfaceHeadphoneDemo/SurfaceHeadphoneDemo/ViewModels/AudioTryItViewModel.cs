using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceHeadphoneDemo.Services;
using Windows.UI.Xaml.Media;

namespace SurfaceHeadphoneDemo.ViewModels
{
    public class AudioTryItViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB2_15 = "ms-appx:///Assets/Backgrounds/sb2_15_Generic_BG.jpg";
        private const string URI_BACKGROUND_SB2_13 = "ms-appx:///Assets/Backgrounds/sb2_Generic_BG.jpg";
        
        private const string URI_READY = "ms-appx:///Assets/Experience/joplin_gateway.png";
        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        public double READY_IMAGE_WIDTH = 1350;

        #endregion


        #region Public Properties

        public string BackgroundUri;
        public string OverlayHeadline;
        public string OverlayLede;
        public string CTA;

        public string ReadyUri = URI_READY;

        public string ButtonText;
        public SolidColorBrush ReadyBoxBorderColor = RadiatingButton.GetSolidColorBrush("#FF0078D4");
        public double ImageWidth;
        #endregion


        #region Construction

        public AudioTryItViewModel()
        {
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    BackgroundUri = URI_BACKGROUND_FOXBURG;
                    ImageWidth = READY_IMAGE_WIDTH / 3 * 2;
                    break;
                case DeviceType.Studio:
                    BackgroundUri = URI_BACKGROUND_CAPROCK;
                    ImageWidth = READY_IMAGE_WIDTH / 2;
                    break;
                case DeviceType.Book15:
                    BackgroundUri = URI_BACKGROUND_SB2_15;
                    ImageWidth = READY_IMAGE_WIDTH / 2;
                    break;
                case DeviceType.Book13:
                    BackgroundUri = URI_BACKGROUND_SB2_13;
                    ImageWidth = READY_IMAGE_WIDTH / 2;
                    break;
                case DeviceType.Pro:
                    BackgroundUri = URI_BACKGROUND_CRUZ;
                    ImageWidth = READY_IMAGE_WIDTH / 2;
                    break;
                default:
                    ImageWidth = READY_IMAGE_WIDTH / 2;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAudioTryItViewModel(this);
            }
        }

        #endregion
    }
}
