using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
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
