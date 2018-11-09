using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceHeadphoneDemo.Services;


namespace SurfaceHeadphoneDemo.ViewModels
{
    public class FirmwareUpdateViewModel : ViewModelBase
    {
        #region Private Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/gradient-bg.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string SearchMessage;
        public string UpdateMessage;

        #endregion


        #region Construction

        public FirmwareUpdateViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadFirmwareUpdateViewModel(this);
            }
        }

        #endregion
    }
}
