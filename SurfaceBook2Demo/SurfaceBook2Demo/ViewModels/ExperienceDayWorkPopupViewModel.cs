using System;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;


namespace SurfaceBook2Demo.ViewModels
{
    public class ExperienceDayWorkPopupViewModel
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";

        private const string URI_X_IMAGE = @"ms-appx:///Assets/Universal/close-icon.png";

        private const string URI_IMAGESELECTOR_IMAGE_1_13 = "ms-appx:///Assets/Experience/sb2_work_appScreen_premiere_13.png";
        private const string URI_IMAGESELECTOR_IMAGE_2_13 = "ms-appx:///Assets/Experience/sb2_work_appScreen_maya_13.png";
        private const string URI_IMAGESELECTOR_IMAGE_3_13 = "ms-appx:///Assets/Experience/sb2_work_appScreen_sketchbook_13.png";
        private const string URI_IMAGESELECTOR_IMAGE_4_13 = "ms-appx:///Assets/Experience/sb2_work_appScreen_bluebeam_13.png";
        private const string URI_IMAGESELECTOR_IMAGE_5_13 = "ms-appx:///Assets/Experience/sb2_work_appScreen_photoshop_13.png";

        private const string URI_IMAGESELECTOR_IMAGE_1_15 = "ms-appx:///Assets/Experience/sb2_work_appScreen_premiere_15.png";
        private const string URI_IMAGESELECTOR_IMAGE_2_15 = "ms-appx:///Assets/Experience/sb2_work_appScreen_maya_15.png";
        private const string URI_IMAGESELECTOR_IMAGE_3_15 = "ms-appx:///Assets/Experience/sb2_work_appScreen_sketchbook_15.png";
        private const string URI_IMAGESELECTOR_IMAGE_4_15 = "ms-appx:///Assets/Experience/sb2_work_appScreen_bluebeam_15.png";
        private const string URI_IMAGESELECTOR_IMAGE_5_15 = "ms-appx:///Assets/Experience/sb2_work_appScreen_photoshop_15.png";

        private const string URI_APPSELECTOR_APP_1_13 = "ms-appx:///Assets/Experience/sb2_13_premiere.png";
        private const string URI_APPSELECTOR_APP_2_13 = "ms-appx:///Assets/Experience/sb2_13_maya.png";
        private const string URI_APPSELECTOR_APP_3_13 = "ms-appx:///Assets/Experience/sb2_13_sketchbook.png";
        private const string URI_APPSELECTOR_APP_4_13 = "ms-appx:///Assets/Experience/sb2_13_bluebeam.png";
        private const string URI_APPSELECTOR_APP_5_13 = "ms-appx:///Assets/Experience/sb2_13_photoshop.png";

        private const string URI_APPSELECTOR_APP_1_15 = "ms-appx:///Assets/Experience/sb2_15_premiere.png";
        private const string URI_APPSELECTOR_APP_2_15 = "ms-appx:///Assets/Experience/sb2_15_maya.png";
        private const string URI_APPSELECTOR_APP_3_15 = "ms-appx:///Assets/Experience/sb2_15_sketchbook.png";
        private const string URI_APPSELECTOR_APP_4_15 = "ms-appx:///Assets/Experience/sb2_15_bluebeam.png";
        private const string URI_APPSELECTOR_APP_5_15 = "ms-appx:///Assets/Experience/sb2_15_photoshop.png";

        private const int IMAGESELECTORIMAGE_WIDTH_13 = 1098;
        private const int IMAGESELECTORIMAGE_WIDTH_15 = 1176;

        private const int IMAGESELECTORIMAGE_HEIGHT_13 = 744;
        private const int IMAGESELECTORIMAGE_HEIGHT_15 = 798;

        private const int APPSELECTORIMAGE_WIDTH_13 = 60;
        private const int APPSELECTORIMAGE_WIDTH_15 = 60;

        private readonly double _radiatingButtonRadius = StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius);
        private readonly double _closeIconHeight = StyleHelper.GetApplicationDouble(LayoutSizes.TryItIconHeight) / 2;
        private readonly double _maxImageWidth = StyleHelper.GetApplicationDouble("CanvasWidth");
        private readonly double _maxImageHeight = StyleHelper.GetApplicationDouble("CanvasHeight");
        private readonly double _closeEllipseRightMargin = StyleHelper.GetApplicationDouble("CloseButtonRightMargin");
        private readonly double _closeEllipseTopMargin = StyleHelper.GetApplicationDouble("CloseButtonTopMargin");

        private readonly int APPSELECTOR_BUTTON_WIDTH = System.Convert.ToInt32(Math.Round((double)Application.Current.Resources["AppSelectorButtonWidth"], 0)); 
        private readonly int APPSELECTOR_BUTTON_HEIGHT = System.Convert.ToInt32(Math.Round((double)Application.Current.Resources["AppSelectorButtonWidth"], 0));
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

        public GridLength AppCloseWidth
        {
            get
            {
                return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.AppCloseWidth));
            }
        }

        public double radiatingButtonRadius;
        public double closeIconHeight;

        public string CloseButtonXURI;
        public double ellipseGridCanvasSetLeft;
        public double closeEllipseTopMargin;

        public SolidColorBrush ellipseStroke;
        #endregion


        #region Construction

        public ExperienceDayWorkPopupViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();
            // set the header and lede and colors list for app selector
            // use the event to handle color changed


            CloseButtonXURI = URI_X_IMAGE;
            closeEllipseTopMargin = _closeEllipseTopMargin;
            ellipseGridCanvasSetLeft = _maxImageWidth - _closeEllipseRightMargin - _radiatingButtonRadius;
            radiatingButtonRadius = _radiatingButtonRadius;
            closeIconHeight = _closeIconHeight;
            ellipseStroke = RadiatingButton.GetSolidColorBrush("#FFD2D2D2");

            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:

                    AppSelectorButtonWidth = APPSELECTORIMAGE_WIDTH_15;
                    AppSelectorButtonHeight = APPSELECTORIMAGE_WIDTH_15;
                    this.ImageSelectorImageWidth = IMAGESELECTORIMAGE_WIDTH_15;
                    this.ImageSelectorImageHeight = IMAGESELECTORIMAGE_HEIGHT_15;

                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_1_15,
                        Source_SelectedImage = URI_APPSELECTOR_APP_1_15
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_2_15,
                        Source_SelectedImage = URI_APPSELECTOR_APP_2_15
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_3_15,
                        Source_SelectedImage = URI_APPSELECTOR_APP_3_15
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_4_15,
                        Source_SelectedImage = URI_APPSELECTOR_APP_4_15
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_5_15,
                        Source_SelectedImage = URI_APPSELECTOR_APP_5_15
                    });

                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_1_15,
                        Width = IMAGESELECTORIMAGE_WIDTH_15
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_2_15,
                        Width = IMAGESELECTORIMAGE_WIDTH_15
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_3_15,
                        Width = IMAGESELECTORIMAGE_WIDTH_15
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_4_15,
                        Width = IMAGESELECTORIMAGE_WIDTH_15
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_5_15,
                        Width = IMAGESELECTORIMAGE_WIDTH_15
                    });

                    break;

                case DeviceType.Book13:

                    AppSelectorButtonWidth = APPSELECTORIMAGE_WIDTH_13;
                    AppSelectorButtonHeight = APPSELECTORIMAGE_WIDTH_13;
                    this.ImageSelectorImageWidth = IMAGESELECTORIMAGE_WIDTH_13;
                    this.ImageSelectorImageHeight = IMAGESELECTORIMAGE_HEIGHT_13;

                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_1_13,
                        Source_SelectedImage = URI_APPSELECTOR_APP_1_13
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_2_13,
                        Source_SelectedImage = URI_APPSELECTOR_APP_2_13
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_3_13,
                        Source_SelectedImage = URI_APPSELECTOR_APP_3_13
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_4_13,
                        Source_SelectedImage = URI_APPSELECTOR_APP_4_13
                    });
                    this.appSelectorData.Add(new AppSelectorData()
                    {
                        Source_NotSelectedImage = URI_APPSELECTOR_APP_5_13,
                        Source_SelectedImage = URI_APPSELECTOR_APP_5_13
                    });

                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_1_13,
                        Width = IMAGESELECTORIMAGE_WIDTH_13
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_2_13,
                        Width = IMAGESELECTORIMAGE_WIDTH_13
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_3_13,
                        Width = IMAGESELECTORIMAGE_WIDTH_13
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_4_13,
                        Width = IMAGESELECTORIMAGE_WIDTH_13
                    });
                    this.appSelectorImageURIs.Add(new AppSelectorImageURI()
                    {
                        URI = URI_IMAGESELECTOR_IMAGE_5_13,
                        Width = IMAGESELECTORIMAGE_WIDTH_13
                    });
                    break;
                default:
                    break;
            }

            //        this.ImagePairs = new Dictionary<int, ImagePair>();

            //this.ImagePairs.Add(0, new ImagePair()
            //{
            //    NotSelected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_1,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_1.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    },
            //    Selected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_1_SELECTED,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_1_SELECTED.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    }
            //});

            //this.ImagePairs.Add(1, new ImagePair()
            //{
            //    NotSelected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_2,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_2.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    },
            //    Selected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_2_SELECTED,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_2_SELECTED.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    }
            //});

            //this.ImagePairs.Add(2, new ImagePair()
            //{
            //    NotSelected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_3,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_3.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    },
            //    Selected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_3_SELECTED,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_3_SELECTED.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    }
            //});

            //this.ImagePairs.Add(3, new ImagePair()
            //{
            //    NotSelected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_4,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_4.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    },
            //    Selected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_4_SELECTED,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_4_SELECTED.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    }
            //});

            //this.ImagePairs.Add(4, new ImagePair()
            //{
            //    NotSelected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_5,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_5.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    },
            //    Selected = new Image()
            //    {
            //        Source = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED,
            //        Width = BITMAPIMAGE_APPSELECTOR_APP_5_SELECTED.DecodePixelWidth,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //        VerticalAlignment = VerticalAlignment.Center,
            //        Opacity = 1.0
            //    }
            //});

            //this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_1);
            //this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_2);
            //this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_3);
            //this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_4);
            //this.BMImages.Add(BITMAPIMAGE_IMAGESELECTOR_IMAGE_5);           

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
