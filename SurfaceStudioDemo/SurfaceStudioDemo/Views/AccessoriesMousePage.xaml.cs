using System;

using Windows.UI.Xaml.Controls;

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

        #endregion


        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();
            this.rBtnLeftAccRight.PopupChild = PopLeft;
            this.rBtnRightAccRight.PopupChild = PopRight;
            this.rBtnTopAccRight.PopupChild = PopTop;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeftAccRight.StartEntranceAnimation();
            rBtnLeftAccRight.StartRadiateAnimation();

            rBtnRightAccRight.StartEntranceAnimation();
            rBtnRightAccRight.StartRadiateAnimation();

            rBtnTopAccRight.StartEntranceAnimation();
            rBtnTopAccRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
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
