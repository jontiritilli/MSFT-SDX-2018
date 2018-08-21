using System;

using SurfaceTestBed.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class Page2Page : Page
    {
        private Page2ViewModel ViewModel
        {
            get { return DataContext as Page2ViewModel; }
        }

        public Page2Page()
        {
            InitializeComponent();
        }
    }
}
