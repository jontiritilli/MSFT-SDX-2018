using System;

using Windows.UI.Xaml.Controls;

using SurfaceHeadphoneDemo.ViewModels;


namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class DesignPage : Page, INavigate
    {
        #region Private Members

        private DesignViewModel ViewModel
        {
            get { return DataContext as DesignViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion


        #region Construction

        public DesignPage()
        {
            InitializeComponent();
            this.Loaded += DesignPage_Loaded;
            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            rBtnTop.PopupChild = PopTop;
        }

        private void DesignPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            this.HasLoaded = true;
            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }
        public void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();

            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != rBtnLeft.PopupChild && rBtnLeft.PopupChild.IsOpen)
            {
                rBtnLeft.PopupChild.IsOpen = false;
            }
            if (null != rBtnRight.PopupChild && rBtnRight.PopupChild.IsOpen)
            {
                rBtnRight.PopupChild.IsOpen = false;
            }
            if (null != rBtnTop.PopupChild && rBtnTop.PopupChild.IsOpen)
            {
                rBtnTop.PopupChild.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (this.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                this.HasNavigatedTo = true;
            }
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

            ClosePopupsOnExit();
        }

        #endregion
    }
}
