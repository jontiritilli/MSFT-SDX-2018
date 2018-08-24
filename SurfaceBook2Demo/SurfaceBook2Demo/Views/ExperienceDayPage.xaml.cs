using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayPage : Page
    {
        private ExperienceDayViewModel ViewModel
        {
            get { return DataContext as ExperienceDayViewModel; }
        }

        public ExperienceDayPage()
        {
            InitializeComponent();
        }
    }
}
