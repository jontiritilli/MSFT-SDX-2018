using System;

using Windows.UI.Xaml.Controls;

using SurfaceHeadphoneDemo.ViewModels;


namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class SpecsPage : Page, INavigate
    {
        #region Private Members

        private SpecsViewModel ViewModel
        {
            get { return DataContext as SpecsViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion


        #region Construction

        public SpecsPage()
        {
            InitializeComponent();
            this.Loaded += SpecsPage_Loaded;
        }

        private void SpecsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            this.HasLoaded = true;
            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
        }
        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (this.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                this.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
