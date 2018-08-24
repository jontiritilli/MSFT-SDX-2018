using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperienceCraftedPage : Page
    {
        private ExperienceCraftedViewModel ViewModel
        {
            get { return DataContext as ExperienceCraftedViewModel; }
        }

        public ExperienceCraftedPage()
        {
            InitializeComponent();
        }
    }
}
