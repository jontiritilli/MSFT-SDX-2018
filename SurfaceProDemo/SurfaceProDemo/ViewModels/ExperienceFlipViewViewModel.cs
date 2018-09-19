using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Helpers;
using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ExperienceFlipViewViewModel : ViewModelBase
    {
        #region Constants

        #endregion


        #region Public Properties

        public string StudioCaptionText;
        public string LaptopCaptionText;
        public string TabletCaptionText;
        
        #endregion


        #region Construction

        public ExperienceFlipViewViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceFlipViewViewModel(this);
            }
        }

        #endregion
    }
}
