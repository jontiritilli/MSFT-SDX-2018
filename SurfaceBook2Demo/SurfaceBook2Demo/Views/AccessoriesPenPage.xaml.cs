using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AccessoriesPenPage : Page
    {
        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }

        public AccessoriesPenPage()
        {
            InitializeComponent();
        }
    }
}
