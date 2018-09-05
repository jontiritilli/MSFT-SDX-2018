using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceSpeakersViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Speaker/foxburg_enjoy_music.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Speaker/foxburg_enjoy_movie.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Speaker/foxburg_enjoy_skype.png";

        private const string URI_APPSELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Icons/fox_music.png";
        private const string URI_APPSELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Icons/fox_movie.png";
        private const string URI_APPSELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Icons/fox_skype.png";

        private const string URI_APPSELECTOR_IMAGE_1_SELECTED = "ms-appx:///Assets/Experience/Icons/fox_music.png";
        private const string URI_APPSELECTOR_IMAGE_2_SELECTED = "ms-appx:///Assets/Experience/Icons/fox_movie.png";
        private const string URI_APPSELECTOR_IMAGE_3_SELECTED = "ms-appx:///Assets/Experience/Icons/fox_skype.png";

        private const int APPSELECTOR_BUTTON_WIDTH = 100;
        private const int APPSELECTOR_BUTTON_HEIGHT = 100;

        private const int SELECTORIMAGE_IMAGEWIDTH = 1200;
        private const int SELECTORIMAGE_IMAGEHEIGHT = 780;

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;

        public string BackgroundUri;

        public int AppSelectorButtonWidth;
        public int AppSelectorButtonHeight;

        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;

        public List<AppSelectorData> speakersSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> speakersSelectorImageURIs = new List<AppSelectorImageURI>();

        #endregion

        public ExperienceSpeakersViewModel()
        {

            BackgroundUri = URI_BACKGROUND;
            ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;

            // list of app icons
            this.speakersSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_1,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_1_SELECTED
            });
            this.speakersSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_2,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_2_SELECTED
            });
            this.speakersSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_3,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_3_SELECTED
            });

            // list of associated app images to display
            this.speakersSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.speakersSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.speakersSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceSpeakersViewModel(this);
            }
        }
    }
}
