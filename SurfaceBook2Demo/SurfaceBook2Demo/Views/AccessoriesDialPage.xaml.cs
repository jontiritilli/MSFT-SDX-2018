using System;

using SurfaceBook2Demo.ViewModels;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AccessoriesDialPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesDialViewModel ViewModel
        {
            get { return DataContext as AccessoriesDialViewModel; }
        }
        private ImageEx _bestOfImage;
        #endregion


        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();
            this.ContentArea.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.CadetBlue);
            this.ContentArea.Background = new SolidColorBrush(Colors.CadetBlue);

            // add the lifestyle image

            BitmapImage bitmapImage = new BitmapImage() { UriSource = new Uri(@"ms-appx:///Assets/ColoringBook/GeishaTattoo.png"), DecodePixelWidth = 768 };
            _bestOfImage = new ImageEx()
            {
                Opacity = 0,
                Name = "BestOfImage",
                BitmapImage = bitmapImage,
                //ImageSource = @"ms-appx:///Assets/ColoringBook/GeishaTattoo.png",
                ImageWidth = 768,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                DurationInMilliseconds = 200d,
                StaggerDelayInMilliseconds = 200d,
                AutoStart = false
            };
            Grid.SetRow(_bestOfImage, 3);
            Grid.SetColumn(_bestOfImage, 1);
            this.ContentArea.Children.Add(_bestOfImage);

            //ColoringBook _ColoringBook = new ColoringBook()
            //{
            //    HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right,
            //    ControlStyle = ControlStyles.TouchHere,
            //    AutoStart = false,
            //    ImageURI = "ms-appx:///Assets/ColoringBook/GeishaTattoo.png",
            //    ImageHeight = 1041,
            //    ImageWidth = 768,
            //    Colors = new System.Collections.Generic.List<ColoringBookColor>
            //    {
            //        new ColoringBookColor
            //        {
            //            URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/black.png",
            //            URI_SelectedImage ="ms-appx:///Assets/ColorPanel/black-selected.png",
            //            Color = Color.FromArgb(10, 187,187,187)
            //        },
            //        new ColoringBookColor
            //        {
            //            URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/burgundy.png",
            //            URI_SelectedImage ="ms-appx:///Assets/ColorPanel/burgundy-selected.png",
            //            Color = Color.FromArgb(10, 218,194,196)
            //        },
            //        new ColoringBookColor
            //        {
            //            URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/cobalt.png",
            //            URI_SelectedImage ="ms-appx:///Assets/ColorPanel/cobalt-selected.png",
            //            Color = Color.FromArgb(10, 176,196,206)
            //        },
            //        new ColoringBookColor
            //        {
            //            URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/silver.png",
            //            URI_SelectedImage ="ms-appx:///Assets/ColorPanel/silver-selected.png",
            //            Color = Color.FromArgb(10, 206,206,206)
            //        }

            //    }
            //};
            //Grid.SetRow(_ColoringBook, 0);
            //Grid.SetRowSpan(_ColoringBook, 4);
            //Grid.SetColumn(_ColoringBook, 0);
            //Grid.SetColumnSpan(_ColoringBook, 3);
            //this.ContentArea.Children.Add(_ColoringBook);

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            AnimationHelper.PerformPageEntranceAnimation(this);
            // specialized handling for stuff you dont want to slide and fade in normally. handle it your own damn self
            AnimationHelper.PerformFadeIn(this._bestOfImage, 400d, 0d);
            AnimationHelper.PerformTranslateIn(this._bestOfImage,TranslateDirection.Right,100d, 400d,0.0d);
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);
            AnimationHelper.PerformFadeOut(this._bestOfImage, 400d, 0d);
        }

        #endregion
    }
}
