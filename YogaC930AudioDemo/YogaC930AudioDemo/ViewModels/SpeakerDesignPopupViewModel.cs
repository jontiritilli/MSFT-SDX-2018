using System;

using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Windows.UI.Xaml.Media;
using Windows.UI;

using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.ViewModels
{
    public class SpeakerDesignPopupViewModel : ViewModelBase
    {
        #region Public Properties

        public string Headline;
        public string Body;
        public SolidColorBrush BackgroundColor;
        public string PopupBgHex = "#E6082454";

        #endregion

        #region Construction

        public SpeakerDesignPopupViewModel()
        {
            BackgroundColor = ColorHexConverter.GetSolidColorBrush(PopupBgHex);

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadSpeakerDesignPopupViewModel(this);
            }
        }

        #endregion
    }
}
