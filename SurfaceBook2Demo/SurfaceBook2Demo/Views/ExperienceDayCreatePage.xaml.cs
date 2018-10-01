using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;
namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayCreatePage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayCreateViewModel ViewModel
        {
            get { return DataContext as ExperienceDayCreateViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceDayCreatePage()
        {
            InitializeComponent();

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;
            rBtnRight.PopupChild = PopRight;

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
