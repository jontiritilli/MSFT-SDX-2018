using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class BestOfMicrosoftViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string BulletOneCTA;
        public string BulletTwoCTA;
        public string BulletThreeCTA;
        public string BulletFourCTA;
        public string Legal;
        public ListItem[] LeftItemList = new ListItem[3];
        public ListItem[] RightItemList = new ListItem[2];
        public double ICON_WIDTH = 60d;
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
