using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class SpeedsAndFeedsPage : Page, INavigate
    {
        #region Private Properties

        private SpeedsAndFeedsViewModel ViewModel
        {
            get { return DataContext as SpeedsAndFeedsViewModel; }
        }

        #endregion


        #region Construction / Initialization

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

            // prepare for page animation
            AnimationHelper.PrepForPageAnimation(this);
        }

        #endregion


        #region INavigate

        public void NavigateToPage()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
