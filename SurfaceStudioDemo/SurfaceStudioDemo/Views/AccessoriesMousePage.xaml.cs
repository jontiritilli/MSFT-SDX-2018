using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
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
