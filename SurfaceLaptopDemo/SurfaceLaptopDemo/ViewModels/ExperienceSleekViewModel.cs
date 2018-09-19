using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceSleekViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_DESIGN = "ms-appx:///Assets/Experience/Sleek/foxburg_morepower.png";

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        
        public string PopRight_Headline;
        public string PopRight_Lede;

        public string PopLeft_Headline;
        public string PopLeft_Lede;

        public string PopBottom_Headline;
        public string PopBottom_Lede;
        public string PopBottom_Legal;

        public string DesignUri;
        public string BackgroundUri;

        #endregion

        public ExperienceSleekViewModel()
        {
            DesignUri = URI_DESIGN;
            BackgroundUri = URI_BACKGROUND;

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceSleekViewModel(this);
            }
        }
    }
}
