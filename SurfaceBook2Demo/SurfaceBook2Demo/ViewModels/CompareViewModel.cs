using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class CompareViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        #endregion


        #region Construction

        public CompareViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadCompareViewModel(this);
            }
        }

        #endregion
    }
}
