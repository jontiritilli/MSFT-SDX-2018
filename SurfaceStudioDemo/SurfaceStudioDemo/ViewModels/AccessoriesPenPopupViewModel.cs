using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Windows.UI.Xaml.Media;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.Services;

namespace SurfaceStudioDemo.ViewModels
{
    public class AccessoriesPenPopupViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic_bg.jpg";
        private const string URI_READY = "ms-appx:///Assets/Experience/appsDesktop.png";
        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        public double READY_IMAGE_WIDTH = 1000;

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string ReadyUri = URI_READY;

        public string Headline;
        public string ButtonText;
        public SolidColorBrush ReadyBoxBorderColor = RadiatingButton.GetSolidColorBrush("#FF0078D4");

        #endregion

        #region Construction

        public AccessoriesPenPopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesPenPopupViewModel(this);
            }
        }

        #endregion
    }
}
