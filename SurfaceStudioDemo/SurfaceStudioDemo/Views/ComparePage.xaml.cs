using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
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
