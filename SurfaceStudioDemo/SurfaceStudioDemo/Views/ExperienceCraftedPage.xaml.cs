using System;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperienceCraftedPage : Page, INavigate
    {
        #region Private Members

        private ExperienceCraftedViewModel ViewModel
        {
            get { return DataContext as ExperienceCraftedViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceCraftedPage()
        {
            InitializeComponent();
            this.rBtnLeftCrafted.PopupChild = PopLeft;
            this.rBtnRightCrafted.PopupChild = PopRight;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnRightCrafted.StartEntranceAnimation();
            rBtnRightCrafted.StartRadiateAnimation();

            rBtnLeftCrafted.StartEntranceAnimation();
            rBtnLeftCrafted.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            rBtnRightCrafted.ResetEntranceAnimation();
            rBtnRightCrafted.ResetRadiateAnimation();

            rBtnLeftCrafted.ResetEntranceAnimation();
            rBtnLeftCrafted.ResetRadiateAnimation();
        }

        #endregion
    }
}
