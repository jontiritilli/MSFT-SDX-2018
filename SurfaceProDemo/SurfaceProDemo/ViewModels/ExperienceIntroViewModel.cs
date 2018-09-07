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
        private const string URI_BLACKBACKGROUND = "ms-appx:///Assets/Backgrounds/brand-bg.png";
        private const string URI_IMAGEHERO_FRONT = "ms-appx:///Assets/Experience/platinum_hero_front.png";
        private const string URI_IMAGEHERO_BACK = "ms-appx:///Assets/Experience/platinum_hero_back.png";
        private const string URI_IMAGEBLACKHERO_FRONT = "ms-appx:///Assets/Experience/black_hero_front.png";
        private const string URI_IMAGEBLACKHERO_BACK = "ms-appx:///Assets/Experience/black_hero_back.png";
        private const double IMAGEHERO_FRONT_WIDTH = 1194;
        private const double IMAGEHERO_BACK_WIDTH = 610;
        private const TextStyles PAGEHEADER_HEADLINE_STYLE = TextStyles.PageHeadline;
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

        public string HeroFrontURI = URI_IMAGEHERO_FRONT;
        public string HeroBackURI = URI_IMAGEHERO_BACK;        

        public double HeroFrontWidth = IMAGEHERO_FRONT_WIDTH;
        public double HeroBackWidth = IMAGEHERO_BACK_WIDTH;

        public TextStyles HeadlineStyle = PAGEHEADER_HEADLINE_STYLE;
        #endregion

        #region Construction

        public ExperienceIntroViewModel()
        {
            if (ConfigurationService.Current.GetIsBlackSchemeEnabled())
            {
                HeadlineStyle = TextStyles.PageHeadlineDark;
                BackgroundUri = URI_BLACKBACKGROUND;
                HeroFrontURI = URI_IMAGEBLACKHERO_FRONT;
                HeroBackURI = URI_IMAGEBLACKHERO_BACK;
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
