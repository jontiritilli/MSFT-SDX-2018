using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
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
