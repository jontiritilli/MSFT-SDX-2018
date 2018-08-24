using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class DesignPage : Page
    {
        private DesignViewModel ViewModel
        {
            get { return DataContext as DesignViewModel; }
        }

        public DesignPage()
        {
            InitializeComponent();
        }
    }
}
