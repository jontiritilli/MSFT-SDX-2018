using System;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

namespace SurfaceBook2Demo.Views
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
            
            rBtnLeft.PopupChild = PopLeft;
        }

        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
