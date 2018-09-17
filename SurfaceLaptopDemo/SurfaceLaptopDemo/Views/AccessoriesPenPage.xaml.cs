﻿using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesPenPage : Page, INavigate
    {

        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        #region Private Members

        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }

        #endregion

        #region Public Members

        public static AccessoriesPenPage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesPenPage()
        {
            InitializeComponent();
            AccessoriesPenPage.Current = this;
            this.AppSelectorImageAccRight.AppSelector = this.AppSelectorAccRight;
            this.AppSelectorAccRight.SelectedIDChanged += SelectedIDChanged;

            this.rBtnPen.PopupChild = PopCenter;
        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;
            AccessoriesMousePage.Current?.SetID(appSelector.SelectedID);
        }
        public void SetID(int ID)
        {
            // check if this.appselector isnull and do not set again if already matching
            if (this.AppSelectorAccRight != null && this.AppSelectorAccRight.SelectedID != ID)
            {
                this.AppSelectorAccRight.SelectedID = ID;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnPen.StartEntranceAnimation();
            rBtnPen.StartRadiateAnimation();

        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);

            rBtnPen.ResetEntranceAnimation();
            rBtnPen.ResetRadiateAnimation();
            
        }

        #endregion
    }
}
