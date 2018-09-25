using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceInnovationViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_DESIGN = "ms-appx:///Assets/Experience/Design/foxburg_design.png";
        private const string URI_POPLEFT_MEDIA = "ms-appx:///Assets/Experience/Design/foxburg_alcantara.png";
        private const double WIDTH_MEDIA = 1136d;
        private const double WIDTH_POPUP_MEDIA = 325d;
        #endregion

        #region Public Members

        public string Headline;
        public string Lede;

        public string TryIt;
        public string TryIt_Headline;
        public string TryIt_Lede;

        public string PopTop_Headline;
        public string PopTop_Lede;

        public string PopRight_Headline;
        public string PopRight_Lede;

        public string DesignUri = URI_DESIGN;
        public string BackgroundUri = URI_BACKGROUND;
        public string PopupMediaTryIt = URI_POPLEFT_MEDIA;

        public double MediaWidth;
        public double PopupMediaWidth;

        #endregion

        public ExperienceInnovationViewModel()
        {
            MediaWidth = WIDTH_MEDIA;
            PopupMediaWidth = WIDTH_POPUP_MEDIA;

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceInnovationViewModel(this);
            }
        }
    }
}
