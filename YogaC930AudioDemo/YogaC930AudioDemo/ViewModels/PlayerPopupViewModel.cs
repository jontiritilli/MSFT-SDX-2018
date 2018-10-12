using System;

using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.ViewModels
{
    public class PlayerPopupViewModel : ViewModelBase
    {
        #region Public Properties

        public string Body;

        #endregion


        #region Construction

        public PlayerPopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadPlayerPopupViewModel(this);
            }
        }

        #endregion
    }
}
