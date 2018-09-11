using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayWorkPopupViewModel
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/gradient-bg.png";

        private readonly BitmapImage BITMAPIMAGE_IMAGESELECTOR_IMAGE_1 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupImage_1);
        private readonly BitmapImage BITMAPIMAGE_IMAGESELECTOR_IMAGE_2 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupImage_2);
        private readonly BitmapImage BITMAPIMAGE_IMAGESELECTOR_IMAGE_3 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupImage_3);
        private readonly BitmapImage BITMAPIMAGE_IMAGESELECTOR_IMAGE_4 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupImage_4);
        private readonly BitmapImage BITMAPIMAGE_IMAGESELECTOR_IMAGE_5 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupImage_5);

        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_1 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_1);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_2 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_2);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_3 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_3);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_4 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_4);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_5 = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_5);

        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_1_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_Selected_1);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_2_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_Selected_2);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_3_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_Selected_3);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_4_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_Selected_4);
        private readonly BitmapImage BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED = StyleHelper.GetApplicationBitmapImage(BitmapImages.PagePopupAppIcon_Selected_5);

        private const int APPSELECTOR_BUTTON_WIDTH = 60;
        private const int APPSELECTOR_BUTTON_HEIGHT = 60;

        private const int SELECTORIMAGE_IMAGEHEIGHT = 588;
        private const int SELECTORIMAGE_IMAGEWIDTH = 1176;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;


        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;


        public string Headline;
        public string Lede;

        public string Legal;

        public Dictionary<int, ImagePair> ImagePairs;
        public List<BitmapImage> BMImages = new List<BitmapImage>();
        public List<AppSelectorData> appSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();        

        public int AppSelectorButtonWidth;
        public int AppSelectorButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        #endregion


        #region Construction

        public ExperienceDayWorkPopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();
            // set the header and lede and colors list for app selector
            // use the event to handle color changed

            AppSelectorButtonWidth = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED.DecodePixelWidth;
            AppSelectorButtonHeight = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED.DecodePixelHeight;
            ICON_WIDTH = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED.DecodePixelWidth;
            this.ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            this.ImageSelectorImageHeight = SELECTORIMAGE_IMAGEHEIGHT;
            this.ImagePairs = new Dictionary<int, ImagePair>();

            this.ImagePairs.Add(0, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_1,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_1.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_1_SELECTED,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_1_SELECTED.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this.ImagePairs.Add(1, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_2,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_2.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_2_SELECTED,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_2_SELECTED.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this.ImagePairs.Add(2, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_3,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_3.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_3_SELECTED,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_3_SELECTED.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this.ImagePairs.Add(3, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_4,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_4.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_4_SELECTED,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_4_SELECTED.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this.ImagePairs.Add(4, new ImagePair()
            {
                NotSelected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_5,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_5.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                Selected = new Image()
                {
                    Source = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED,
                    Width = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED.DecodePixelWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            });

            this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_1);
            this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_2);
            this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_3);
            this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_4);
            this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_5);           

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadExperienceDayWorkPopupViewModel(this);
            }
        }

        #endregion
    }
}
