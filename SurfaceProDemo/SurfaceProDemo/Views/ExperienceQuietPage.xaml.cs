using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceQuietPage : Page
    {
        private ExperienceQuietViewModel ViewModel
        {
            get { return DataContext as ExperienceQuietViewModel; }
        }

        public ExperienceQuietPage()
        {
            InitializeComponent();
        }
    }
}
