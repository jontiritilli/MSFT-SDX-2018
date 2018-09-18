using System;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesTryItPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesTryItViewModel ViewModel
        {
            get { return DataContext as AccessoriesTryItViewModel; }
        }

        #endregion

        #region Construction

        public AccessoriesTryItPage()
        {
            InitializeComponent();
            this.rBtnLeftAccLeft.PopupChild = PopLeft;
            this.rBtnRightAccLeft.PopupChild = PopRight;
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnLeftAccLeft.StartEntranceAnimation();
            rBtnLeftAccLeft.StartRadiateAnimation();

            rBtnRightAccLeft.StartEntranceAnimation();
            rBtnRightAccLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
            rBtnLeftAccLeft.ResetEntranceAnimation();
            rBtnLeftAccLeft.ResetRadiateAnimation();

            rBtnRightAccLeft.ResetEntranceAnimation();
            rBtnRightAccLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
