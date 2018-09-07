using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;
using Windows.UI;

namespace SurfaceBook2Demo.ViewModels
{
    public class AccessoriesPenViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_light.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";

        private const string URI_COLORING_BOOK_IMAGE_15 = "ms-appx:///Assets/Accessories/sb2_15_accessories_car.svg";
        private const string URI_COLORING_BOOK_IMAGE_13 = "ms-appx:///Assets/Accessories/sb2_13_accessories_car.svg";

        // private consts for widths of the coloring book image 
        private const int COLORING_BOOK_IMAGEWIDTH_15 = 400;
        private const int COLORING_BOOK_IMAGEWIDTH_13 = 400;

        private const int COLORING_BOOK_IMAGEHEIGHT_15 = 400;
        private const int COLORING_BOOK_IMAGEHEIGHT_13 = 400;

        //consts for colors and the rgbs
         // THESE ARE WRONG. GET THESE FRON THE ADOBE FILE   
        private Color COLORING_BOOK_COLOR_1 = Color.FromArgb(100, 229, 48, 97);
        private Color COLORING_BOOK_COLOR_2 = Color.FromArgb(100, 12, 98, 144);
        private Color COLORING_BOOK_COLOR_3 = Color.FromArgb(100, 152, 216, 216);
        private Color COLORING_BOOK_COLOR_4 = Color.FromArgb(100, 255, 179, 103);
        private Color COLORING_BOOK_COLOR_5 = Color.FromArgb(100, 151, 101, 245);        

        private const string URI_SVG_COLORING_BOOK_COLOR_1_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_2_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_3_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_4_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_5_13 = "ms-appx:///Assets/Universal/close-app-button.png";

        private const string URI_SVG_COLORING_BOOK_COLOR_1_SELECTED_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_2_SELECTED_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_3_SELECTED_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_4_SELECTED_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_5_SELECTED_13 = "ms-appx:///Assets/Universal/close-app-button.png";

        private const string URI_SVG_COLORING_BOOK_COLOR_1_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_2_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_3_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_4_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_5_15 = "ms-appx:///Assets/Universal/close-app-button.png";

        private const string URI_SVG_COLORING_BOOK_COLOR_1_SELECTED_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_2_SELECTED_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_3_SELECTED_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_4_SELECTED_15 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_COLOR_5_SELECTED_15 = "ms-appx:///Assets/Universal/close-app-button.png";

        private const string URI_SVG_COLORING_BOOK_CLEAR_BUTTON_13 = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_SVG_COLORING_BOOK_CLEAR_BUTTON_15 = "ms-appx:///Assets/Universal/close-app-button.png";


        private const int COLORING_BOOK_BUTTON_WIDTH_13 = 40;
        private const int COLORING_BOOK_BUTTON_HEIGHT_13 = 40;

        private const int COLORING_BOOK_BUTTON_WIDTH_15 = 40;
        private const int COLORING_BOOK_BUTTON_HEIGHT_15 = 40;
        #endregion


        #region Public Properties

        public string BackgroundUri;

        public string ColoringBookUri;

        public int ColoringBookImageWidth;
        public int ColoringBookImageHeight;
        

        public string Headline;
        public string Lede;

        public string TryItTitle;
        public string TryItLede;
        public List<ListItem> ListItems = new List<ListItem>();
        public List<ColoringBookColor> Colors = new List<ColoringBookColor>();

        public int ColoringBookButtonWidth;
        public int ColoringBookButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        // TODO: need a list of items here for the List control.        
        #endregion


        #region Construction

        public AccessoriesPenViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;
                    this.ColoringBookUri = URI_COLORING_BOOK_IMAGE_15;
                    this.ColoringBookImageHeight = COLORING_BOOK_IMAGEHEIGHT_15;
                    this.ColoringBookImageWidth = COLORING_BOOK_IMAGEWIDTH_15;
                    this.ColoringBookButtonHeight = COLORING_BOOK_BUTTON_HEIGHT_15;
                    this.ColoringBookButtonWidth = COLORING_BOOK_BUTTON_WIDTH_15;
                    this.ColoringBookClearButtonURI = URI_SVG_COLORING_BOOK_CLEAR_BUTTON_15;

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_1_15,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_1_SELECTED_15,
                        Color = COLORING_BOOK_COLOR_1
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_2_15,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_2_SELECTED_15,
                        Color = COLORING_BOOK_COLOR_2
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_3_15,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_3_SELECTED_15,
                        Color = COLORING_BOOK_COLOR_3
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_4_15,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_4_SELECTED_15,
                        Color = COLORING_BOOK_COLOR_4
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_5_15,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_5_SELECTED_15,
                        Color = COLORING_BOOK_COLOR_5
                    });
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;
                    this.ColoringBookUri = URI_COLORING_BOOK_IMAGE_13;
                    this.ColoringBookImageHeight = COLORING_BOOK_IMAGEHEIGHT_13;
                    this.ColoringBookImageWidth = COLORING_BOOK_IMAGEWIDTH_13;
                    this.ColoringBookButtonHeight = COLORING_BOOK_BUTTON_HEIGHT_13;
                    this.ColoringBookButtonWidth = COLORING_BOOK_BUTTON_WIDTH_13;
                    this.ColoringBookClearButtonURI = URI_SVG_COLORING_BOOK_CLEAR_BUTTON_13;
                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_1_13,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_1_SELECTED_13,
                        Color = COLORING_BOOK_COLOR_1
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_2_13,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_2_SELECTED_13,
                        Color = COLORING_BOOK_COLOR_2
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_3_13,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_3_SELECTED_13,
                        Color = COLORING_BOOK_COLOR_3
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_4_13,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_4_SELECTED_13,
                        Color = COLORING_BOOK_COLOR_4
                    });

                    this.Colors.Add(new ColoringBookColor()
                    {
                        URI_NotSelectedImage = URI_SVG_COLORING_BOOK_COLOR_5_13,
                        URI_SelectedImage = URI_SVG_COLORING_BOOK_COLOR_5_SELECTED_13,
                        Color = COLORING_BOOK_COLOR_5
                    });
                    break;
            }

          

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesPenViewModel(this);
            }
        }

        #endregion
    }
}
