using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
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
