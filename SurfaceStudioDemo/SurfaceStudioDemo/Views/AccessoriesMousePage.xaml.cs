using System;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
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

        #region Public Members
        public static AccessoriesMousePage Current { get; private set; }
        #endregion

        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();
            AccessoriesMousePage.Current = this;
            this.rBtnLeftAccRight.PopupChild = PopLeft;
            this.rBtnRightAccRight.PopupChild = PopRight;
            this.rBtnTopAccRight.PopupChild = PopTop;
            this.Loaded += AccessoriesMousePage_Loaded;
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
            AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeftAccRight.StartEntranceAnimation();
            rBtnLeftAccRight.StartRadiateAnimation();

            rBtnRightAccRight.StartEntranceAnimation();
            rBtnRightAccRight.StartRadiateAnimation();

            rBtnTopAccRight.StartEntranceAnimation();
            rBtnTopAccRight.StartRadiateAnimation();
        }

        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }
            if (null != this.PopRight && this.PopRight.IsOpen)
            {
                this.PopRight.IsOpen = false;
            }
            if (null != this.PopTop && this.PopTop.IsOpen)
            {
                this.PopTop.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
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
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnLeftAccRight.ResetEntranceAnimation();
            rBtnLeftAccRight.ResetRadiateAnimation();

            rBtnRightAccRight.ResetEntranceAnimation();
            rBtnRightAccRight.ResetRadiateAnimation();

            rBtnTopAccRight.ResetEntranceAnimation();
            rBtnTopAccRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
