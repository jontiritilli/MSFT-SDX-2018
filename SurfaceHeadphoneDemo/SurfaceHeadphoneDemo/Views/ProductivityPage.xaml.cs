using System;

using Windows.UI.Xaml.Controls;

using SurfaceHeadphoneDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class ProductivityPage : Page, INavigate
    {
        #region Private Members

        private ProductivityViewModel ViewModel
        {
            get { return DataContext as ProductivityViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Construction

        public ProductivityPage()
        {
            InitializeComponent();
            rBtnTop.PopupChild = PopTop;
            rBtnCenter.PopupChild = PopCenter;
            rBtnBottom.PopupChild = PopBottom;
            this.PopBottomLegal.SetOpacity(0);
            this.PopTopLegal.SetOpacity(0);
            this.Loaded += ProductivityPage_Loaded;
        }

        private void ProductivityPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            this.HasLoaded = true;
            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
            rBtnCenter.StartEntranceAnimation();
            rBtnCenter.StartRadiateAnimation();
            rBtnBottom.StartEntranceAnimation();
            rBtnBottom.StartRadiateAnimation();
        }

        private void PopBottom_Opened(object sender, object e)
        {
            this.PopBottomLegal.SetOpacity(1);
        }

        private void PopBottom_Closed(object sender, object e)
        {
            this.PopBottomLegal.SetOpacity(0);
        }

        private void PopTop_Opened(object sender, object e)
        {
            this.PopTopLegal.SetOpacity(1);
        }

        private void PopTop_Closed(object sender, object e)
        {
            this.PopTopLegal.SetOpacity(0);
        }

        private void ClosePopupsOnExit()
        {
            if (null != rBtnBottom.PopupChild && rBtnBottom.PopupChild.IsOpen)
            {
                rBtnBottom.PopupChild.IsOpen = false;
            }
            if (null != rBtnCenter.PopupChild && rBtnCenter.PopupChild.IsOpen)
            {
                rBtnCenter.PopupChild.IsOpen = false;
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
            AnimationHelper.PerformPageExitAnimation(this);

            rBtnBottom.ResetEntranceAnimation();
            rBtnBottom.ResetRadiateAnimation();

            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnCenter.ResetEntranceAnimation();
            rBtnCenter.ResetRadiateAnimation();

            ClosePopupsOnExit();
        }

        #endregion
    }
}
