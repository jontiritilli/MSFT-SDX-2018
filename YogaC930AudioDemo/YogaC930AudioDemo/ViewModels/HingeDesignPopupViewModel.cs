﻿using System;

using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.ViewModels
{
    public class HingeDesignPopupViewModel : ViewModelBase
    {
        #region Public Properties

        public string Headline;
        public string BodyFirst;
        public string BodyBold;
        public string BodyLast;

        #endregion


        #region Construction

        public HingeDesignPopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadHingeDesignPopupViewModel(this);
            }
        }

        #endregion
    }
}
