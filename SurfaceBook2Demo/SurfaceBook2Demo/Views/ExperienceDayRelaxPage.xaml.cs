using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayRelaxPage : Page
    {
        private ExperienceDayRelaxViewModel ViewModel
        {
            get { return DataContext as ExperienceDayRelaxViewModel; }
        }

        public ExperienceDayRelaxPage()
        {
            InitializeComponent();
        }
    }
}
