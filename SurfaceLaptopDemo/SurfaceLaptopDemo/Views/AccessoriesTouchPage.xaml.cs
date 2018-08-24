using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesTouchPage : Page
    {
        private AccessoriesTouchViewModel ViewModel
        {
            get { return DataContext as AccessoriesTouchViewModel; }
        }

        public AccessoriesTouchPage()
        {
            InitializeComponent();
        }
    }
}
