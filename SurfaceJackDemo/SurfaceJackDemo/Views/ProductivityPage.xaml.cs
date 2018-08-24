using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class ProductivityPage : Page
    {
        private ProductivityViewModel ViewModel
        {
            get { return DataContext as ProductivityViewModel; }
        }

        public ProductivityPage()
        {
            InitializeComponent();
        }
    }
}
