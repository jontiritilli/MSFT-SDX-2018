using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceColorsViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;
        public string BackgroundUri;

        #endregion

        public ExperienceColorsViewModel()
        {
            BackgroundUri = URI_BACKGROUND;

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceColorsViewModel(this);
            }
        }
    }
}
