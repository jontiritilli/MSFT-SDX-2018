using System;

using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.ViewModels
{
    public class FeaturesViewModel : ViewModelBase
    {
        #region Public Properties

        public string PenHeadline;
        public string PenBodyFirst;
        public string PenBodyBold;
        public string PenBodyLast;

        public string InkHeadline;
        public string InkBodyFirst;
        public string InkBodyBold;
        public string InkBodyLast;

        public string WebCamHeadline;
        public string WebCamBodyFirst;
        public string WebCamBodyBold;
        public string WebCamBodyLast;

        #endregion


        #region Construction

        public FeaturesViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadFeaturesViewModel(this);
            }
        }

        #endregion
    }
}
