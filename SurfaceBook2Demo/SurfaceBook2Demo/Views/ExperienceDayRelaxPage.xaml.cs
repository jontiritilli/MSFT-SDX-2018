using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;
namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayRelaxPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayRelaxViewModel ViewModel
        {
            get { return DataContext as ExperienceDayRelaxViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public ExperienceDayRelaxPage()
        {
            InitializeComponent();

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
