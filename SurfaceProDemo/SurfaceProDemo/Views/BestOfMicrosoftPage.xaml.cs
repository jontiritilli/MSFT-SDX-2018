using System;

using SurfaceProDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceProDemo.Views
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
