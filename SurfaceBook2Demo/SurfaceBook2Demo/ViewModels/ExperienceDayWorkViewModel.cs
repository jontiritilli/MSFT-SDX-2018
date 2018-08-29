using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayWorkViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string Headline;
        public string Lede;
        public string SliderBatteryCopy;
        public string BatteryPopupHeadline;
        public string BatteryPopupCopy;
        public string BatteryPopupHours;

        #endregion


        #region Construction

        public ExperienceDayWorkViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayWorkViewModel(this);
            }
        }

        #endregion
    }
}
