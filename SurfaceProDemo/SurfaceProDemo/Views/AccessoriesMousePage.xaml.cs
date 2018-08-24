using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
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
