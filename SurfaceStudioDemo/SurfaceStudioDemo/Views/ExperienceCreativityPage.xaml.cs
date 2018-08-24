using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperienceCreativityPage : Page
    {
        private ExperienceCreativityViewModel ViewModel
        {
            get { return DataContext as ExperienceCreativityViewModel; }
        }

        public ExperienceCreativityPage()
        {
            InitializeComponent();
        }
    }
}
