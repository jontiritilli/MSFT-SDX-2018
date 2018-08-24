using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
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
