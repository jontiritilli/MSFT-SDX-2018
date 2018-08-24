using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceTransformPage : Page
    {
        private ExperienceTransformViewModel ViewModel
        {
            get { return DataContext as ExperienceTransformViewModel; }
        }

        public ExperienceTransformPage()
        {
            InitializeComponent();
        }
    }
}
