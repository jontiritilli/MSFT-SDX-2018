using System;

using SurfaceTestBed.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class Page4Page : Page
    {
        private Page4ViewModel ViewModel
        {
            get { return DataContext as Page4ViewModel; }
        }

        public Page4Page()
        {
            InitializeComponent();
        }
    }
}
