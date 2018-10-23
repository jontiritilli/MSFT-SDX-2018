using System;
using Windows.UI.Core;
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

            this.Loaded += this.SpeedsAndFeedsPage_Loaded;
        }

        private void SpeedsAndFeedsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // disable the system back button
            SystemNavigationManager mgr = SystemNavigationManager.GetForCurrentView();
            if (null != mgr)
            {
                mgr.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
