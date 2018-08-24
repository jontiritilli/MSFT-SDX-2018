using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class SpecsPage : Page
    {
        private SpecsViewModel ViewModel
        {
            get { return DataContext as SpecsViewModel; }
        }

        public SpecsPage()
        {
            InitializeComponent();
        }
    }
}
