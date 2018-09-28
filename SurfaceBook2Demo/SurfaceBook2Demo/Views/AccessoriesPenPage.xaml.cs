using System;
using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AccessoriesPenPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion

        #region Construction

        public AccessoriesPenPage()
        {
            InitializeComponent();
            Canvas.SetTop(rBtnCenter, _canvasHeight * .40);
            Canvas.SetLeft(rBtnCenter, _canvasWidth * .50);
            this.rBtnCenter.Clicked += OnPenTryItClicked;

            this.ColoringBook.OnPenScreenContacted += OnPenScreenContacted;
        }

        #endregion

        #region Private Methods

        private void OnPenTryItClicked(object sender, EventArgs e)
        {
            this.ColoringBook.FadeInColoringImage();
        }

        private void OnPenScreenContacted(object sender, EventArgs e)
        {
            this.rBtnCenter.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            this.rBtnCenter.StartEntranceAnimation();
            this.rBtnCenter.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rBtnCenter.ResetEntranceAnimation();
            this.rBtnCenter.ResetRadiateAnimation();
        }

        #endregion
    }
}
