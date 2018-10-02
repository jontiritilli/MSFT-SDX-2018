using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceQuietPage : Page, INavigate
    {
        #region Private Members

        private ExperienceQuietViewModel ViewModel
        {
            get { return DataContext as ExperienceQuietViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceQuietPage()
        {
            InitializeComponent();

            this.LeftLegal.SetOpacity(0);

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;
            rBtnRight.PopupChild = PopRight;
            this.LeftLegal.SetOpacity(0);
            this.Loaded += ExperienceQuietPage_Loaded;
        }

        private void ExperienceQuietPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        #endregion

        #region Private Methods

        private void PopLeft_Opened(object sender, object e)
        {
            this.LeftLegal.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LeftLegal.SetOpacity(0);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            
            this.rBtnLeft.StartEntranceAnimation();
            this.rBtnLeft.StartRadiateAnimation();

            this.rBtnTop.StartEntranceAnimation();
            this.rBtnTop.StartRadiateAnimation();

            this.rBtnRight.StartEntranceAnimation();
            this.rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rBtnTop.ResetEntranceAnimation();
            this.rBtnTop.ResetRadiateAnimation();

            this.rBtnLeft.ResetEntranceAnimation();
            this.rBtnLeft.ResetRadiateAnimation();

            this.rBtnRight.ResetEntranceAnimation();
            this.rBtnRight.ResetRadiateAnimation();
        }

        #endregion

    }
}
