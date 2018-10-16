using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class SpeedsAndFeedsPage : Page
    {
        private SpeedsAndFeedsViewModel ViewModel
        {
            get { return DataContext as SpeedsAndFeedsViewModel; }
        }

        public SpeedsAndFeedsPage()
        {
            InitializeComponent();
        }
    }
}
