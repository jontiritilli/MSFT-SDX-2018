using System;

using SurfaceTestBed.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class Page3Page : Page
    {
        private Page3ViewModel ViewModel
        {
            get { return DataContext as Page3ViewModel; }
        }

        public Page3Page()
        {
            InitializeComponent();
        }
    }
}
