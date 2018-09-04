using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;


namespace SurfaceLaptopDemo.ViewModels
{
    public class BestOfMicrosoftViewModel : ViewModelBase
    {
        public string BackgroundUri;

        public string Headline;
        public string BulletOneCTA;
        public string BulletTwoCTA;
        public string BulletThreeCTA;
        public string BulletFourCTA;
        public string Legal;
        public ListItem[] LeftItemList = new ListItem[3];
        public ListItem[] RightItemList = new ListItem[2];
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
