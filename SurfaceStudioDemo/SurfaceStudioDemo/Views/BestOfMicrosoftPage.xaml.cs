using System;

using SurfaceStudioDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceStudioDemo.Views
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
