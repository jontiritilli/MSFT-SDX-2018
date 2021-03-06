﻿using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesMousePage : Page, INavigate
    {

        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        #region Private Members

        private AccessoriesMouseViewModel ViewModel
        {
            get { return DataContext as AccessoriesMouseViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static AccessoriesMousePage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();

            // connect the accessories page color selections
            AccessoriesMousePage.Current = this;
            this.AppSelectorImageAccLeft.AppSelector = this.AppSelectorAccLeft;
            this.AppSelectorAccLeft.SelectedIDChanged += SelectedIDChanged;

            this.rBtnLeftMouse.PopupChild = PopLeft;
            this.rBtnCenterMouse.PopupChild = PopCenter;
            this.rBtnRightMouse.PopupChild = PopRight;

            this.Loaded += AccessoriesMousePage_Loaded;

        }

        private void AccessoriesMousePage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            AccessoriesMousePage.Current.HasLoaded = true;
            if (AccessoriesMousePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnLeftMouse.StartEntranceAnimation();
            rBtnLeftMouse.StartRadiateAnimation();

            rBtnCenterMouse.StartEntranceAnimation();
            rBtnCenterMouse.StartRadiateAnimation();

            rBtnRightMouse.StartEntranceAnimation();
            rBtnRightMouse.StartRadiateAnimation();
        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;
            AccessoriesPenPage.Current?.SetID(appSelector.SelectedID);
        }
        public void SetID(int ID)
        {
            // check if this.appselector isnull and do not set again if already matching
            if (this.AppSelectorAccLeft != null && this.AppSelectorAccLeft.SelectedID != ID)
            {
                this.AppSelectorAccLeft.SelectedID = ID;
            }
        }

        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }

            if (null != this.PopCenter && this.PopCenter.IsOpen)
            {
                this.PopCenter.IsOpen = false;
            }

            if (null != this.PopRight && this.PopRight.IsOpen)
            {
                this.PopRight.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (AccessoriesMousePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                AccessoriesMousePage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnLeftMouse.ResetEntranceAnimation();
            rBtnLeftMouse.ResetRadiateAnimation();

            rBtnCenterMouse.ResetEntranceAnimation();
            rBtnCenterMouse.ResetRadiateAnimation();

            rBtnRightMouse.ResetEntranceAnimation();
            rBtnRightMouse.ResetRadiateAnimation();
        }

        #endregion
    }
}
