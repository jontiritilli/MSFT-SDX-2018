using System;

using SurfaceTestBed.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceTestBed.Views
{
    public sealed partial class Page5Page : Page
    {
        private Page5ViewModel ViewModel
        {
            get { return DataContext as Page5ViewModel; }
        }

        public Page5Page()
        {
            InitializeComponent();
        }
    }
}
