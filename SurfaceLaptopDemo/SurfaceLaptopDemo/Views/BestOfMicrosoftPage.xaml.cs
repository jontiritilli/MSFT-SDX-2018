using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class BestOfMicrosoftPage : Page
    {
        private BestOfMicrosoftViewModel ViewModel
        {
            get { return DataContext as BestOfMicrosoftViewModel; }
        }

        public BestOfMicrosoftPage()
        {
            InitializeComponent();
        }
    }
}
