using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesMousePage : Page
    {
        private AccessoriesMouseViewModel ViewModel
        {
            get { return DataContext as AccessoriesMouseViewModel; }
        }

        public AccessoriesMousePage()
        {
            InitializeComponent();
        }
    }
}
