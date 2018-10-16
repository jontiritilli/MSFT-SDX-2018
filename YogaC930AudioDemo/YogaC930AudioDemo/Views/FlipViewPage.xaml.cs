using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class FlipViewPage : Page
    {
        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        public FlipViewPage()
        {
            InitializeComponent();
        }
    }
}
