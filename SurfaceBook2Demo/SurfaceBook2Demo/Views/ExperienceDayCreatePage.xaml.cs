using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayCreatePage : Page
    {
        private ExperienceDayCreateViewModel ViewModel
        {
            get { return DataContext as ExperienceDayCreateViewModel; }
        }

        public ExperienceDayCreatePage()
        {
            InitializeComponent();
        }
    }
}
