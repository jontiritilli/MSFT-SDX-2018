﻿using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;


namespace SurfaceLaptopDemo.ViewModels
{
    public class BestOfMicrosoftViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";

        #endregion

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string Legal;

        public string BulletOneCTA;
        public string BulletTwoCTA;
        public string BulletThreeCTA;
        public string BulletFourCTA;

        public List<ListItem> LeftItemList = new List<ListItem>();
        public List<ListItem> RightItemList = new List<ListItem>();

        public double ICON_WIDTH = 60d;

        public BestOfMicrosoftViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadBestOfMicrosoftViewModel(this);
            }
        }
    }
}
