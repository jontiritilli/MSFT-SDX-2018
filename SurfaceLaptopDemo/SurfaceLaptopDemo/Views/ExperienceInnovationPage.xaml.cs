using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceInnovationPage : Page
    {
        private ExperienceInnovationViewModel ViewModel
        {
            get { return DataContext as ExperienceInnovationViewModel; }
        }

        public ExperienceInnovationPage()
        {
            InitializeComponent();
        }
    }
}
