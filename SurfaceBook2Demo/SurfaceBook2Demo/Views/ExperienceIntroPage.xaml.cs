using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
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
