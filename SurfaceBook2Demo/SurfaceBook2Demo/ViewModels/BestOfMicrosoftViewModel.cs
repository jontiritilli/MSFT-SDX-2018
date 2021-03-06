﻿using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
{
    public class BestOfMicrosoftViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_light.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string Headline;
        public string BulletOneCTA;
        public string BulletTwoCTA;
        public string BulletThreeCTA;
        public string BulletFourCTA;        
        public string LegalBulletOne;
        public string LegalBulletTwo;
        public string LegalBulletThree;
        public string LegalBulletFour;
        public string LegalBulletFive;
    public List<ListItem> ItemList = new List<ListItem>();        
        public double ICON_WIDTH = 60d;

        #endregion


        #region Construction

        public BestOfMicrosoftViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    break;
            }

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadBestOfMicrosoftViewModel(this);
            }
        }

        #endregion
    }
}
