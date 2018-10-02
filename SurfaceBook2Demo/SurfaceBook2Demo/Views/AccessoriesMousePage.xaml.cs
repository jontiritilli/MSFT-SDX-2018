using System;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AccessoriesMousePage : Page, INavigate
    {
        #region Private Members

        private AccessoriesMouseViewModel ViewModel
        {
            get { return DataContext as AccessoriesMouseViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion


        #region Public Static Properties

        public static AccessoriesMousePage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();
            AccessoriesMousePage.Current = this;
            rBtnLeft.PopupChild = PopLeft;

            this.Loaded += AccessoriesMousePage_Loaded;
            this.Unloaded += AccessoriesMousePage_Unloaded;
        }

        private void AccessoriesMousePage_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        private void AccessoriesMousePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            AccessoriesMousePage.Current.HasLoaded = true;
            if (AccessoriesMousePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }            
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in

            if (AccessoriesMousePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                AccessoriesMousePage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
