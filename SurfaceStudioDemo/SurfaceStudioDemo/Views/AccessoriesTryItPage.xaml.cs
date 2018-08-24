using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesTryItPage : Page
    {
        private AccessoriesTryItViewModel ViewModel
        {
            get { return DataContext as AccessoriesTryItViewModel; }
        }

        public AccessoriesTryItPage()
        {
            InitializeComponent();
        }
    }
}
