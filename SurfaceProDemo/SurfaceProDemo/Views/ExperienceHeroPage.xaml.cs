using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceHeroPage : Page
    {
        private ExperienceHeroViewModel ViewModel
        {
            get { return DataContext as ExperienceHeroViewModel; }
        }

        public ExperienceHeroPage()
        {
            InitializeComponent();
        }
    }
}
