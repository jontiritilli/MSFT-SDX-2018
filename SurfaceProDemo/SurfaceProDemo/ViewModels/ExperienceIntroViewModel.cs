using System;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;
using SDX.Toolkit.Helpers;

namespace SurfaceProDemo.ViewModels
{
    public class ExperienceIntroViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";
        private const string URI_BACKGROUND_BLACK = "ms-appx:///Assets/Backgrounds/brand-bg.png";
        private const string URI_IMAGE_HERO_FRONT = "ms-appx:///Assets/Experience/platinum_hero_front.png";
        private const string URI_IMAGE_HERO_BACK = "ms-appx:///Assets/Experience/platinum_hero_back.png";
        private const string URI_IMAGE_HERO_FRONT_BLACK = "ms-appx:///Assets/Experience/black_hero_front.png";
        private const string URI_IMAGE_HERO_BACK_BLACK = "ms-appx:///Assets/Experience/black_hero_back.png";
        private const double WIDTH_HERO_FRONT = 1200;
        private const double WIDTH_HERO_BACK = 650;

        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string Lede;
        public string SwipeText;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftLegal;
        public string PopTopHeadline;
        public string PopTopLede;
        public string PopTopLegal;

        public string PopRightHeadline;
        public string PopRightLede;

        public string HeroFrontURI = URI_IMAGE_HERO_FRONT;
        public string HeroBackURI = URI_IMAGE_HERO_BACK;        

        public double HeroFrontWidth = WIDTH_HERO_FRONT;
        public double HeroBackWidth = WIDTH_HERO_BACK;

        public TextStyles HeadlineStyle = TextStyles.PageHeadline;
        public TextStyles LedeStyle = TextStyles.PageLede;

        #endregion


        #region Construction

        public ExperienceIntroViewModel()
        {
            if (ConfigurationService.Current.GetIsBlackSchemeEnabled())
            {
                HeadlineStyle = TextStyles.PageHeadlineDark;
                LedeStyle = TextStyles.PageLedeDark;
                BackgroundUri = URI_BACKGROUND_BLACK;
                HeroFrontURI = URI_IMAGE_HERO_FRONT_BLACK;
                HeroBackURI = URI_IMAGE_HERO_BACK_BLACK;
            }

            // get the localization service         
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceIntroViewModel(this);
            }
        }

        #endregion
    }
}
