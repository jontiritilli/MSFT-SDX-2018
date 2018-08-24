using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperiencePixelSensePage : Page
    {
        private ExperiencePixelSenseViewModel ViewModel
        {
            get { return DataContext as ExperiencePixelSenseViewModel; }
        }

        public ExperiencePixelSensePage()
        {
            InitializeComponent();
        }
    }
}
