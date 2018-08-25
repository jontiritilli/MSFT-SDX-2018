using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayPlayPage : Page
    {
        private ExperienceDayPlayViewModel ViewModel
        {
            get { return DataContext as ExperienceDayPlayViewModel; }
        }

        public ExperienceDayPlayPage()
        {
            InitializeComponent();
        }
    }
}
