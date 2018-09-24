using System;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesDialPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesDialViewModel ViewModel
        {
            get { return DataContext as AccessoriesDialViewModel; }
        }

        #endregion

        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();

            this.rBtnRightAccLeft.PopupChild = this.PopRight;
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnRightAccLeft.StartEntranceAnimation();
            rBtnRightAccLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            rBtnRightAccLeft.ResetEntranceAnimation();
            rBtnRightAccLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
