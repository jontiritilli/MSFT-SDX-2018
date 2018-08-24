using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceIntroPage : Page
    {
        private ExperienceIntroViewModel ViewModel
        {
            get { return DataContext as ExperienceIntroViewModel; }
        }

        public ExperienceIntroPage()
        {
            InitializeComponent();
        }
    }
}
