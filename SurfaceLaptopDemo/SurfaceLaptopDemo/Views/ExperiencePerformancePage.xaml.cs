using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
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
