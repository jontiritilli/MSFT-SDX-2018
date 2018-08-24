using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
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
