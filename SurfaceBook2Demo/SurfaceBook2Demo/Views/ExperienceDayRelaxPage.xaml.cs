using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;
namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayRelaxPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayRelaxViewModel ViewModel
        {
            get { return DataContext as ExperienceDayRelaxViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public ExperienceDayRelaxPage()
        {
            InitializeComponent();
            Canvas.SetLeft(rBtnLeft, _canvasWidth * .40);
            Canvas.SetTop(rBtnLeft, _canvasHeight * .70);

            Canvas.SetLeft(rBtnRight, _canvasWidth * .55);
            Canvas.SetTop(rBtnRight, _canvasHeight * .30);



            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

            //PopLeft.HorizontalOffset = _canvasWidth * .40;
            //PopLeft.VerticalOffset = _canvasHeight * .70;

            //PopRight.HorizontalOffset = _canvasWidth * .55;
            //PopRight.VerticalOffset = _canvasHeight * .30;

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
