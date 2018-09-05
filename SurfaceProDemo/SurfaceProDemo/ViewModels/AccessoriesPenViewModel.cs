using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SurfaceProDemo.Services;
using System.Collections.Generic;
using Windows.UI;

namespace SurfaceProDemo.ViewModels
{
    public class AccessoriesPenViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";           
       
        private const string URI_COLORING_BOOK_IMAGE = "ms-appx:///Assets/Accessories/peacock.png";

        // private consts for widths of the coloring book image 
        private const int COLORING_BOOK_IMAGEWIDTH = 700;

        private const int COLORING_BOOK_IMAGEHEIGHT = 700;

        //consts for colors and the rgbs        
        private Color COLORING_BOOK_COLOR_1 = Color.FromArgb(100, 229, 48, 97);
        private Color COLORING_BOOK_COLOR_2 = Color.FromArgb(100, 12, 98, 144);
        private Color COLORING_BOOK_COLOR_3 = Color.FromArgb(100, 152, 216, 216);
        private Color COLORING_BOOK_COLOR_4 = Color.FromArgb(100, 255, 179, 103);
        private Color COLORING_BOOK_COLOR_5 = Color.FromArgb(100, 151, 101, 245);

        private const string URI_COLORING_BOOK_COLOR_1 = "ms-appx:///Assets/Accessories/ArtworkColors/red.png";
        private const string URI_COLORING_BOOK_COLOR_2 = "ms-appx:///Assets/Accessories/ArtworkColors/blue.png";
        private const string URI_COLORING_BOOK_COLOR_3 = "ms-appx:///Assets/Accessories/ArtworkColors/skyblue.png";
        private const string URI_COLORING_BOOK_COLOR_4 = "ms-appx:///Assets/Accessories/ArtworkColors/yellow.png";
        private const string URI_COLORING_BOOK_COLOR_5 = "ms-appx:///Assets/Accessories/ArtworkColors/purple.png";

        private const string URI_COLORING_BOOK_COLOR_1_SELECTED = "ms-appx:///Assets/Accessories/ArtworkColors/red.png";
        private const string URI_COLORING_BOOK_COLOR_2_SELECTED = "ms-appx:///Assets/Accessories/ArtworkColors/blue.png";
        private const string URI_COLORING_BOOK_COLOR_3_SELECTED = "ms-appx:///Assets/Accessories/ArtworkColors/skyblue.png";
        private const string URI_COLORING_BOOK_COLOR_4_SELECTED = "ms-appx:///Assets/Accessories/ArtworkColors/yellow.png";
        private const string URI_COLORING_BOOK_COLOR_5_SELECTED = "ms-appx:///Assets/Accessories/ArtworkColors/purple.png";

        private const string URI_COLORING_BOOK_CLEAR_BUTTON = "ms-appx:///Assets/Accessories/ArtworkColors/reset.png";


        private const int COLORING_BOOK_BUTTON_WIDTH = 40;
        private const int COLORING_BOOK_BUTTON_HEIGHT = 40;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;        

        public string ColoringBookUri;

        public int ColoringBookImageWidth;
        public int ColoringBookImageHeight;


        public string Headline;
        public string Lede;

        public string TryItTitle;
        public string TryItLede;
        public ListItem[] ListItems = new ListItem[4];
        public List<ColoringBookColor> Colors = new List<ColoringBookColor>();

        public int ColoringBookButtonWidth;
        public int ColoringBookButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        #endregion

        #region Construction

        public AccessoriesPenViewModel()
        {

            this.BackgroundUri = URI_BACKGROUND;
            this.ColoringBookUri = URI_COLORING_BOOK_IMAGE;
            this.ColoringBookImageHeight = COLORING_BOOK_IMAGEHEIGHT;
            this.ColoringBookImageWidth = COLORING_BOOK_IMAGEWIDTH;
            this.ColoringBookButtonHeight = COLORING_BOOK_BUTTON_HEIGHT;
            this.ColoringBookButtonWidth = COLORING_BOOK_BUTTON_WIDTH;
            this.ColoringBookClearButtonURI = URI_COLORING_BOOK_CLEAR_BUTTON;
            this.Colors.Add(new ColoringBookColor()
            {
                URI_NotSelectedImage = URI_COLORING_BOOK_COLOR_1,
                URI_SelectedImage = URI_COLORING_BOOK_COLOR_1_SELECTED,
                Color = COLORING_BOOK_COLOR_1
            });

            this.Colors.Add(new ColoringBookColor()
            {
                URI_NotSelectedImage = URI_COLORING_BOOK_COLOR_2,
                URI_SelectedImage = URI_COLORING_BOOK_COLOR_2_SELECTED,
                Color = COLORING_BOOK_COLOR_2
            });

            this.Colors.Add(new ColoringBookColor()
            {
                URI_NotSelectedImage = URI_COLORING_BOOK_COLOR_3,
                URI_SelectedImage = URI_COLORING_BOOK_COLOR_3_SELECTED,
                Color = COLORING_BOOK_COLOR_3
            });

            this.Colors.Add(new ColoringBookColor()
            {
                URI_NotSelectedImage = URI_COLORING_BOOK_COLOR_4,
                URI_SelectedImage = URI_COLORING_BOOK_COLOR_4_SELECTED,
                Color = COLORING_BOOK_COLOR_4
            });

            this.Colors.Add(new ColoringBookColor()
            {
                URI_NotSelectedImage = URI_COLORING_BOOK_COLOR_5,
                URI_SelectedImage = URI_COLORING_BOOK_COLOR_5_SELECTED,
                Color = COLORING_BOOK_COLOR_5
            });


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
