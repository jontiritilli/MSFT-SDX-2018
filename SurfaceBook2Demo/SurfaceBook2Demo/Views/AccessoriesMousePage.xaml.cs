using System;
using SDX.Toolkit.Helpers;
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
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();
            Canvas.SetTop(rBtnLeft, _canvasHeight * .60);
            Canvas.SetLeft(rBtnLeft, _canvasWidth * .40);

            rBtnLeft.PopupChild = PopLeft;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
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
