using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
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
