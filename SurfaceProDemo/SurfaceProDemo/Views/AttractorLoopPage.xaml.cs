using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class AttractorLoopPage : Page
    {
        private AttractorLoopViewModel ViewModel
        {
            get { return DataContext as AttractorLoopViewModel; }
        }

        public AttractorLoopPage()
        {
            InitializeComponent();
        }
    }
}
