using System;

using SurfaceTestBed.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class FlipViewPage : Page
    {
        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        public FlipViewPage()
        {
            InitializeComponent();
        }
    }
}
