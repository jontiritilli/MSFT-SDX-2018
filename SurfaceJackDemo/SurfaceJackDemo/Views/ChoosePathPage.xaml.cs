using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
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
