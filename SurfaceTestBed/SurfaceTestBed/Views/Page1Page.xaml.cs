using System;
using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using SDX.Toolkit.Controls;

namespace SurfaceTestBed.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1Page : Page
    {
        private Page1ViewModel ViewModel
        {
            get { return DataContext as Page1ViewModel; }
        }

        public Page1Page()
        {
            InitializeComponent();
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.AliceBlue);
            this.LayoutRoot.Background = new SolidColorBrush(Colors.CadetBlue);
            this.Loaded += OnLoaded;
            AppSelector _tstSelector = new AppSelector()
            {//                
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                DurationInMilliseconds = 200d,
                StaggerDelayInMilliseconds = 200d,
                AutoStart = false,
                Orientation = Orientation.Vertical,
                ShowMessages = true,
                URIs = new List<AppSelectorData>
                {
                    new AppSelectorData
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/black.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/black-selected.png",
                        Message= "first"
                    },
                    new AppSelectorData
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/burgundy.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/burgundy-selected.png",
                        Message= "second"
                    },
                    new AppSelectorData
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/cobalt.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/cobalt-selected.png",
                        Message= "third"
                    },
                    new AppSelectorData
                    {
                        URI_NotSelectedImage="ms-appx:///Assets/ColorPanel/silver.png",
                        URI_SelectedImage ="ms-appx:///Assets/ColorPanel/silver-selected.png",
                        Message= "fourth"
                    }

                }
            };
            // add the test selector here so it's after the color selector image
            Grid.SetRow(_tstSelector, 3);
            Grid.SetColumn(_tstSelector, 1);
            this.LayoutRoot.Children.Add(_tstSelector);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }
    }
}
