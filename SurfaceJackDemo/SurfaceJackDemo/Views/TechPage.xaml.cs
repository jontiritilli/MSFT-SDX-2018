using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class TechPage : Page
    {
        private TechViewModel ViewModel
        {
            get { return DataContext as TechViewModel; }
        }

        public TechPage()
        {
            InitializeComponent();
        }
    }
}
