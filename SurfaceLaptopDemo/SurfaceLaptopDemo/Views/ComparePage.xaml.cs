using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ComparePage : Page
    {
        private CompareViewModel ViewModel
        {
            get { return DataContext as CompareViewModel; }
        }

        public ComparePage()
        {
            InitializeComponent();
        }
    }
}
