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
        private const string URI_ZOOM_IMAGE = "ms-appx:///Assets/Lifestyle/foxburg_burgundy_lifestyle.png";

        private const double ZOOM_IMAGE_HEIGHT = 500d;
        private const double ZOOM_IMAGE_WIDTH = 500d;

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;

        public string TryIt_Headline;
        public string TryIt_Lede;

        public string DesignUri;
        public string BackgroundUri;
        public string ZoomImageUri;
        public double ZoomImageHeight;
        public double ZoomImageWidth;

        #endregion

        public AccessoriesTouchViewModel()
        {
            DesignUri = URI_DESIGN;
            BackgroundUri = URI_BACKGROUND;
            ZoomImageUri = URI_ZOOM_IMAGE;
            ZoomImageHeight = ZOOM_IMAGE_HEIGHT;
            ZoomImageWidth = ZOOM_IMAGE_WIDTH;

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
