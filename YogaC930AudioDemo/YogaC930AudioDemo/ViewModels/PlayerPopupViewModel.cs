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

        public string BodyFirst;
        public string BodyLast;

        #endregion

        #region Constants

        public const string POPUP_VIDEO_URI = "ms-appx:///Assets/Player/Makeba_Yoga_30s_POS_1920x1080H264.png";
        //public const string POPUP_VIDEO_URI = "ms-appx:///Assets/Player/Makeba_Yoga_30s_POS_1920x1080.png";

        #endregion

        #region Public Members

        public string PopupVideo_URI = POPUP_VIDEO_URI;

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
