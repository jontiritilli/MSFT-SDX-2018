using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class AccessoriesTouchViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_DESIGN = "ms-appx:///Assets/Experience/Design/foxburg_design.png";
        private const string URI_ZOOM_IMAGE = "ms-appx:///Assets/Accessories/foxburg_aerial.jpg";

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;

        public string TryIt_Headline;
        public string TryIt_Lede;

        public string DesignUri = URI_DESIGN;
        public string BackgroundUri = URI_BACKGROUND;
        public string PinchZoomImageURI = URI_ZOOM_IMAGE;

        #endregion

        public AccessoriesTouchViewModel()
        {
             // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesTouchViewModel(this);
            }
        }
    }
}
