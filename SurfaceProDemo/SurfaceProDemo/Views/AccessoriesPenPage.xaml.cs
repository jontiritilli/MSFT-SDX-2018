using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
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
