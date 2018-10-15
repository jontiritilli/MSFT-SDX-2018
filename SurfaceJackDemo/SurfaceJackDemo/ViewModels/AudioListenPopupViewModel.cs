using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Windows.UI.Xaml.Media;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

using SurfaceJackDemo.Services;

namespace SurfaceJackDemo.ViewModels
{
    public class AudioListenPopupViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_READY = "ms-appx:///Assets/Experience/joplin_gateway.png";
        private const double READY_IMAGE_WIDTH = 1350;

        #endregion

        #region Public Properties

        public string ReadyImageUri = URI_READY;
        public double ReadyWidth;

        public string Headline;
        public string Lede;
        public string ButtonCTA;

        public SolidColorBrush ReadyBoxBorderColor = RadiatingButton.GetSolidColorBrush("#FF0078D4");

        #endregion

        #region Construction

        public AudioListenPopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAudioListenPopupViewModel(this);
            }


            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();
            // need sizing handling
            switch (deviceType)
            {
                case DeviceType.Laptop:
                    ReadyWidth = READY_IMAGE_WIDTH / 3 * 2;
                    break;
                case DeviceType.Studio:
                case DeviceType.Book15:
                case DeviceType.Book13:
                case DeviceType.Pro:
                default:
                    ReadyWidth = READY_IMAGE_WIDTH / 2;
                    break;
            }
        }

        #endregion
    }
}
