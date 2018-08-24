using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayWorkPage : Page
    {
        private ExperienceDayWorkViewModel ViewModel
        {
            get { return DataContext as ExperienceDayWorkViewModel; }
        }

        public ExperienceDayWorkPage()
        {
            InitializeComponent();
        }
    }
}
