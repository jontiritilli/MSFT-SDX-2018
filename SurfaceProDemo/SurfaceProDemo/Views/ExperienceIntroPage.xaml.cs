using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;

using SDX.Toolkit.Controls;
namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceIntroPage : Page, INavigate
    {
        #region Private Members

        private ExperienceIntroViewModel ViewModel
        {
            get { return DataContext as ExperienceIntroViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceIntroPage()
        {
            InitializeComponent();      

            rBtnRight.PopupChild = PopRight;
            rBtnTop.PopupChild = PopTop;
            rBtnLeft.PopupChild = PopLeft;
            if (Services.ConfigurationService.Current.GetIsBlackSchemeEnabled())
            {
                rBtnRight.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();

            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();

            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();
        }

        #endregion

        private void PopLeft_Closed(object sender, object e)
        {
            this.LeftLegal.SetOpacity(0);
        }

        private void PopLeft_Opened(object sender, object e)
        {
            this.LeftLegal.SetOpacity(1);
        }

        private void PopTop_Opened(object sender, object e)
        {
            this.TopLegal.SetOpacity(1);
        }

        private void PopTop_Closed(object sender, object e)
        {
            this.TopLegal.SetOpacity(0);
        }
    }
}
