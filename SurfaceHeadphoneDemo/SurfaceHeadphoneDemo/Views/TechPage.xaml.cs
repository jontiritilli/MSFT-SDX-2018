﻿using System;

using Windows.UI.Xaml.Controls;

using SurfaceHeadphoneDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class TechPage : Page, INavigate
    {
        #region Private Members

        private TechViewModel ViewModel
        {
            get { return DataContext as TechViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Construction

        public TechPage()
        {
            InitializeComponent();
            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            rBtnTop.PopupChild = PopTop;
            this.Loaded += TechPage_Loaded;
            this.BatteryLegal.SetOpacity(0);
        }

        private void TechPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
        }

        private void BatteryPop_Opened(object sender, object e)
        {
            this.BatteryLegal.SetOpacity(1);
        }

        private void BatteryPop_Closed(object sender, object e)
        {
            this.BatteryLegal.SetOpacity(0);
        }

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
            AnimationHelper.PerformPageExitAnimation(this);

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
