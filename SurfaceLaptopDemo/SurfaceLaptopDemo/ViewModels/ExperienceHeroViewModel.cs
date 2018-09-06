using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceHeroViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_brand-bg.png";

        #endregion

        #region Public Properties

        public string BackgroundUri;
        public string HeroText;
        public int RowCount;
        public string SwipeText;

        #endregion

        public ExperienceHeroViewModel()
        {
            BackgroundUri = URI_BACKGROUND;

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceHeroViewModel(this);
            }
        }
    }
}
