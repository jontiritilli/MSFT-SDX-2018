using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.ViewModels
{
    public class ExperienceQuietViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";
        private const string URI_TABLET= "ms-appx:///Assets/Experience/tablet.png";

        private const int IMG_TABLET_WIDTH = 1122;
        private const int IMG_TABLET_HEIGHT = 402;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;
        public string TabletUri = URI_TABLET;
        public int TabletHeight = IMG_TABLET_HEIGHT;
        public int TabletWidth = IMG_TABLET_WIDTH;


        public string Headline;
        public string Lede;

        public string PopLeftHeadline;
        public string PopLeftLede;

        public string PopTopHeadline;
        public string PopTopLede;

        public string PopCenterBulletOne;
        public string PopCenterBulletTwo;
        public string PopCenterBulletThree;
        public string PopCenterBulletFour;
        public string PopCenterFive;

        public string PopRightHeadline;
        public string PopRightLede;
        #endregion

        #region Construction

        public ExperienceQuietViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceQuietViewModel(this);
            }
        }

        #endregion
    }
}
