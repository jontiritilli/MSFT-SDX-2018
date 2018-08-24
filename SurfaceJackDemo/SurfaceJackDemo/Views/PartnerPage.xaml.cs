using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class PartnerPage : Page
    {
        private PartnerViewModel ViewModel
        {
            get { return DataContext as PartnerViewModel; }
        }

        public PartnerPage()
        {
            InitializeComponent();
        }
    }
}
