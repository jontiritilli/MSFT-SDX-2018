using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using Windows.Foundation;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Controls;


namespace SurfaceLaptopDemo.ViewModels
{
    public class ExperienceColorsViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_brand-bg.png";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Experience/Colors/foxburg_black_lifestyle.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Experience/Colors/foxburg_burgundy_lifestyle.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Experience/Colors/foxburg_cobalt_lifestyle.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Experience/Colors/foxburg_platinum_lifestyle.png";

        private const string URI_APPSELECTOR_COLOR_1 = "ms-appx:///Assets/Experience/Colors/Icons/fox_black.png";
        private const string URI_APPSELECTOR_COLOR_2 = "ms-appx:///Assets/Experience/Colors/Icons/fox_burgundy.png";
        private const string URI_APPSELECTOR_COLOR_3 = "ms-appx:///Assets/Experience/Colors/Icons/fox_cobalt.png";
        private const string URI_APPSELECTOR_COLOR_4 = "ms-appx:///Assets/Experience/Colors/Icons/fox_platinum.png";
        //private const string URI_APPSELECTOR_COLOR_5 = "ms-appx:///Assets/Colors/fox_coral.png";

        private const string URI_APPSELECTOR_COLOR_1_SELECTED = "ms-appx:///Assets/Experience/Colors/Icons/fox_black_active.png";
        private const string URI_APPSELECTOR_COLOR_2_SELECTED = "ms-appx:///Assets/Experience/Colors/Icons/fox_burgundy_active.png";
        private const string URI_APPSELECTOR_COLOR_3_SELECTED = "ms-appx:///Assets/Experience/Colors/Icons/fox_cobalt_active.png";
        private const string URI_APPSELECTOR_COLOR_4_SELECTED = "ms-appx:///Assets/Experience/Colors/Icons/fox_platinum_active.png";
        //private const string URI_APPSELECTOR_COLOR_5_SELECTED = "ms-appx:///Assets/Colors/fox_coral_active.png";

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;

        public string BackgroundUri;

        public double ImageSelectorImageWidth = StyleHelper.GetApplicationDouble("CanvasWidth");

        public List<AppSelectorData> lifeStyleColorSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> lifeStyleColorSelectorImageURIs = new List<AppSelectorImageURI>();

        #endregion

        public ExperienceColorsViewModel()
        {

            // list of color swatches
            this.lifeStyleColorSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_1,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_1_SELECTED
            });
            this.lifeStyleColorSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_2,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_2_SELECTED
            });
            this.lifeStyleColorSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_3,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_3_SELECTED
            });
            this.lifeStyleColorSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_4,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_4_SELECTED
            });
            //this.lifeStyleColorSelectorData.Add(new AppSelectorData()
            //{
            //    Source_NotSelectedImage = URI_APPSELECTOR_COLOR_5,
            //    Source_SelectedImage = URI_APPSELECTOR_COLOR_5_SELECTED
            //});

            // list of associated lifestyle images to display
            this.lifeStyleColorSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = ImageSelectorImageWidth
            });
            this.lifeStyleColorSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = ImageSelectorImageWidth
            });
            this.lifeStyleColorSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = ImageSelectorImageWidth
            });
            this.lifeStyleColorSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_4,
                Width = ImageSelectorImageWidth
            });
            //this.lifeStyleColorSelectorImageURIs.Add(new AppSelectorImageURI()
            //{
            //    URI = URI_IMAGESELECTOR_IMAGE_5,
            //    Width = ImageSelectorImageWidth
            //});

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceColorsViewModel(this);
            }
        }
    }
}
