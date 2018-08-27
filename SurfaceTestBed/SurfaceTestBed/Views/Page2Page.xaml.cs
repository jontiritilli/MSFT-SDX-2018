using System;

using Windows.UI;
using Windows.UI.Xaml.Controls;
using SDX.Toolkit;
using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class Page2Page : Page
    {
        private Page2ViewModel ViewModel
        {
            get { return DataContext as Page2ViewModel; }
        }

        public Page2Page()
        {
            InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.Purple);
            //this.LayoutRoot.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            ColoringBook _ColoringBook = new ColoringBook()
            {
                //Width = CANVAS_X,
                //Height = CANVAS_Y,
                //Width = WINDOW_BOUNDS.Width,
                //Height = WINDOW_BOUNDS.Height - 80,
                Caption = "Giggity",
                ControlStyle = ControlStyles.TouchHere,
                AutoStart = false,
                ImageURI = "ms-appx:///Assets/ColoringBook/GeishaTattoo.png",
                ImageHeight = 1041,
                ImageWidth = 768,
                Colors = new System.Collections.Generic.List<ColoringBookColor>
                {
                    new ColoringBookColor
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/black.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/black-selected.png",
                        Color = Color.FromArgb(10, 187,187,187)
                    },
                    new ColoringBookColor
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/burgundy.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/burgundy-selected.png",
                        Color = Color.FromArgb(10, 218,194,196)
                    },
                    new ColoringBookColor
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/cobalt.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/cobalt-selected.png",
                        Color = Color.FromArgb(10, 176,196,206)
                    },
                    new ColoringBookColor
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/silver.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/silver-selected.png",
                        Color = Color.FromArgb(10, 206,206,206)
                    }

                }
            };
            Grid.SetRow(_ColoringBook, 0);
            Grid.SetRowSpan(_ColoringBook, 8);
            Grid.SetColumn(_ColoringBook, 0);
            Grid.SetColumnSpan(_ColoringBook, 3);
            this.LayoutRoot.Children.Add(_ColoringBook);
        }        
    }
}
