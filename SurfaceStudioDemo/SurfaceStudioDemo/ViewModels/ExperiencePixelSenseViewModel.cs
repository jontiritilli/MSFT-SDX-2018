﻿using System;
using System.Collections.Generic;

using SurfaceStudioDemo.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceStudioDemo.ViewModels
{
    public class ExperiencePixelSenseViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";
        private const string URI_LEFT = "ms-appx:///Assets/Experience/appsDesktop.png";
        private const string URI_RIGHT = "ms-appx:///Assets/Experience/appsStudio.png";

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string Left_URI = URI_LEFT;
        public string Right_URI = URI_RIGHT;

        public double Left_Width = 950;
        public double Right_Width = 950;

        public string Headline;
        public string Lede;        

        public string PopLeft_Headline;
        public string PopLeft_Lede;
        public string PopLeft_Legal;

        public string PopBottom_Headline;
        public string PopBottom_Lede;
        public string PopBottom_Legal;

        #endregion

        #region Construction

        public ExperiencePixelSenseViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperiencePixelSenseViewModel(this);
            }
        }

        #endregion
    }
}
