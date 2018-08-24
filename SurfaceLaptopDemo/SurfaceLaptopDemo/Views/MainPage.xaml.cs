using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
