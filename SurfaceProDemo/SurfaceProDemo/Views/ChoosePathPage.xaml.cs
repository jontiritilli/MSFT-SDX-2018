using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ChoosePathPage : Page
    {
        private ChoosePathViewModel ViewModel
        {
            get { return DataContext as ChoosePathViewModel; }
        }

        public ChoosePathPage()
        {
            InitializeComponent();
        }
    }
}
