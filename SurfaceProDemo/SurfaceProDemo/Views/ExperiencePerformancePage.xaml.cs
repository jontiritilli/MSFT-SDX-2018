using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperiencePerformancePage : Page
    {
        private ExperiencePerformanceViewModel ViewModel
        {
            get { return DataContext as ExperiencePerformanceViewModel; }
        }

        public ExperiencePerformancePage()
        {
            InitializeComponent();
        }
    }
}
