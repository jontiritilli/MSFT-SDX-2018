using System;

using SurfaceTestBed.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class Page1Page : Page
    {
        private Page1ViewModel ViewModel
        {
            get { return DataContext as Page1ViewModel; }
        }

        public Page1Page()
        {
            InitializeComponent();
        }
    }
}
