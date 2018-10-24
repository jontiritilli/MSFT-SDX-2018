using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class FeaturesPage : Page, INavigate
    {
        #region Private Properties

        private FeaturesViewModel ViewModel
        {
            get { return DataContext as FeaturesViewModel; }
        }

        #endregion


        #region Construction / Initialization

        public FeaturesPage()
        {
            InitializeComponent();

            this.Loaded += this.FeaturesPage_Loaded;
        }

        private void FeaturesPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // disable the system back button
            SystemNavigationManager mgr = SystemNavigationManager.GetForCurrentView();
            if (null != mgr)
            {
                mgr.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            // prepare for animation
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
