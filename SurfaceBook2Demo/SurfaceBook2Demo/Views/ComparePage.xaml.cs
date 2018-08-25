using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
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
