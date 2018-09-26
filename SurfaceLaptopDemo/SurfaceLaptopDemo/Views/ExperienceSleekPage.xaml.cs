﻿using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSleekPage : Page, INavigate
    {

        #region Private Members

        private ExperienceSleekViewModel ViewModel
        {
            get { return DataContext as ExperienceSleekViewModel; }
        }

        #endregion

        #region Construction

        public ExperienceSleekPage()
        {
            InitializeComponent();
            this.rBtnBottomPerformance.PopupChild = PopBottom;
            this.rBtnLeftPerformance.PopupChild = PopLeft;
            this.rBtnTopPerformance.PopupChild = PopTop;
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnTopPerformance.StartEntranceAnimation();
            rBtnTopPerformance.StartRadiateAnimation();

            rBtnLeftPerformance.StartEntranceAnimation();
            rBtnLeftPerformance.StartRadiateAnimation();

            rBtnBottomPerformance.StartEntranceAnimation();
            rBtnBottomPerformance.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);

            rBtnTopPerformance.ResetEntranceAnimation();
            rBtnTopPerformance.ResetRadiateAnimation();

            rBtnLeftPerformance.ResetEntranceAnimation();
            rBtnLeftPerformance.ResetRadiateAnimation();

            rBtnBottomPerformance.ResetEntranceAnimation();
            rBtnBottomPerformance.ResetRadiateAnimation();
        }

        #endregion
    }
}
