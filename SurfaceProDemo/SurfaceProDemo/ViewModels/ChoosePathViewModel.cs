﻿using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ChoosePathViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string ChooseSurfacePro;
        public string ChooseSurfaceJack;

        #endregion

        #region Construction

        public ChoosePathViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadChoosePathViewModel(this);
            }
        }

        #endregion


    }
}
