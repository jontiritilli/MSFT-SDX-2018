﻿using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceStudioDemo.ViewModels
{
    public class BestOfMicrosoftViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string BulletOneLegal;
        public string BulletTwoLegal;
        public string BulletThreeLegal;
        public string BulletFourLegal;

        public List<ListItem> LeftItemList = new List<ListItem>();
        public List<ListItem> RightItemList = new List<ListItem>();

        public double ICON_WIDTH = StyleHelper.GetApplicationDouble("BestOfMicrosoftListIconWidth");

        #endregion

        #region Construction

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

        #endregion
    }
}
