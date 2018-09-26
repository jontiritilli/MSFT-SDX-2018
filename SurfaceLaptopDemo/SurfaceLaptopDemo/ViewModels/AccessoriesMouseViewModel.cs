using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class AccessoriesMouseViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Accessories/foxburg_platinum_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Accessories/foxburg_black_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Accessories/foxburg_cobalt_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Accessories/foxburg_burgundy_left.png";

        private const string URI_APPSELECTOR_IMAGE_1 = "ms-appx:///Assets/Accessories/Icons/fox_accs_platinum.png";
        private const string URI_APPSELECTOR_IMAGE_2 = "ms-appx:///Assets/Accessories/Icons/fox_accs_black.png";
        private const string URI_APPSELECTOR_IMAGE_3 = "ms-appx:///Assets/Accessories/Icons/fox_accs_cobalt.png";
        private const string URI_APPSELECTOR_IMAGE_4 = "ms-appx:///Assets/Accessories/Icons/fox_accs_burgundy.png";

        private const string URI_APPSELECTOR_IMAGE_1_SELECTED = "ms-appx:///Assets/Accessories/Icons/fox_accs_platinum_active.png";
        private const string URI_APPSELECTOR_IMAGE_2_SELECTED = "ms-appx:///Assets/Accessories/Icons/fox_accs_black_active.png";
        private const string URI_APPSELECTOR_IMAGE_3_SELECTED = "ms-appx:///Assets/Accessories/Icons/fox_accs_cobalt_active.png";
        private const string URI_APPSELECTOR_IMAGE_4_SELECTED = "ms-appx:///Assets/Accessories/Icons/fox_accs_burgundy_active.png";

        #endregion

        #region Public Members

        public string Headline;
        public string Lede;
        public string Legal;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftLegal;

        public string PopCenterHeadline;
        public string PopCenterLede;

        public string PopRightHeadline;
        public string PopRightLede;

        public string BackgroundUri = URI_BACKGROUND;

        public double ImageSelectorImageWidth = StyleHelper.GetApplicationDouble("AccessoryPrimaryImageWidth");

        public List<AppSelectorData> accLeftSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> accLeftSelectorImageURIs = new List<AppSelectorImageURI>();

        #endregion

        public AccessoriesMouseViewModel()
        {

            // list of app icons
            this.accLeftSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_1,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_1_SELECTED
            });
            this.accLeftSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_2,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_2_SELECTED
            });
            this.accLeftSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_3,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_3_SELECTED
            });
            this.accLeftSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_IMAGE_4,
                Source_SelectedImage = URI_APPSELECTOR_IMAGE_4_SELECTED
            });

            // list of associated app images to display
            this.accLeftSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_1,
                Width = ImageSelectorImageWidth
            });
            this.accLeftSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = ImageSelectorImageWidth
            });
            this.accLeftSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = ImageSelectorImageWidth
            });
            this.accLeftSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_4,
                Width = ImageSelectorImageWidth
            });

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesMouseViewModel(this);
            }
        }
    }
}
