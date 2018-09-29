using System;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
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

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

            this.Loaded += this.ExperienceIntroPage_Loaded;
        }

        private void ExperienceIntroPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
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

            // TEST - To prove that .IsNavigationEnabled works.
            //if (null != FlipViewPage.GetNavigationBar())
            //{
            //    FlipViewPage.GetNavigationBar().IsNavigationEnabled = false;
            //}
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion


        #region Event Handlers

        private void PopLeft_Opened(object sender, object e)
        {
            this.LegalPixelSense.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LegalPixelSense.SetOpacity(0);
        }

        private void PopRight_Opened(object sender, object e)
        {
         
        }

        private void PopRight_Closed(object sender, object e)
        {
         
        }

        #endregion
    }
}
