using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSpeakersPage : Page
    {
        private ExperienceSpeakersViewModel ViewModel
        {
            get { return DataContext as ExperienceSpeakersViewModel; }
        }

        public ExperienceSpeakersPage()
        {
            InitializeComponent();
        }
    }
}
