using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayCreateViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_create.png";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_experience_create.png";

        private const string URI_HERO = "ms-appx:///Assets/Experience/sb2_create_sb15.png";
        //"ms-appx:///Assets/Experience/transform.mp4";
        private const string URI_TRANSFORM_IMAGE_13 = "ms-appx:///Assets/Experience/sb2_transform_13.png"; 
        private const string URI_TRANSFORM_IMAGE_15 = "ms-appx:///Assets/Experience/sb2_transform_15.png"; 

        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string HeroUri;

        public string Headline;
        public string Lede;
        public string Legal;
        public string PopupDialHeadline;
        public string PopupDialLede;
        public string PopupPenHeadline;
        public string PopupPenLede;
        public string PopupTransformHeadline;
        public string PopupTransformLede;
        public string PopupTransformImageUri;
        public double PopupTransformImageWidth;        

        #endregion


        #region Construction

        public ExperienceDayCreateViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.PopupTransformImageUri = URI_TRANSFORM_IMAGE_15;
                    this.PopupTransformImageWidth = 480;
                    this.HeroUri = URI_HERO;                    
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.PopupTransformImageUri = URI_TRANSFORM_IMAGE_13;
                    this.PopupTransformImageWidth = 450;
                    this.HeroUri = URI_HERO;
                    break;
            }

            // video uri for transform popup
            
               

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayCreateViewModel(this);
            }
        }

        #endregion
    }
}
