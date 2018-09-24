﻿using System;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AccessoriesMousePage : Page, INavigate
    {
        #region Private Members

        private AccessoriesMouseViewModel ViewModel
        {
            get { return DataContext as AccessoriesMouseViewModel; }
        }
        #endregion


        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();

            rBtnLeft.PopupChild = PopLeft;
        }

        #endregion

        #region Event Handlers
        private void PopMouse_Opened(object sender, object e)
        {
            this.LegalMouse.SetOpacity(1);
        }

        private void PopMouse_Closed(object sender, object e)
        {
            this.LegalMouse.SetOpacity(0);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
