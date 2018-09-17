using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;
namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayCreatePage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayCreateViewModel ViewModel
        {
            get { return DataContext as ExperienceDayCreateViewModel; }
        }
        //double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        //double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public ExperienceDayCreatePage()
        {
            InitializeComponent();
            //Canvas.SetTop(rBtnLeft, _canvasHeight * .70);
            //Canvas.SetLeft(rBtnLeft, _canvasWidth * .20);

            //Canvas.SetTop(rBtnTop, _canvasHeight * .40);
            //Canvas.SetLeft(rBtnTop, _canvasWidth * .50);

            //Canvas.SetTop(rBtnRight, _canvasHeight * .55);
            //Canvas.SetLeft(rBtnRight, _canvasWidth * .60);

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;
            rBtnRight.PopupChild = PopRight;

            //PopLeft.VerticalOffset = _canvasHeight * .70;
            //PopLeft.HorizontalOffset = _canvasWidth * .20;

            //PopTop.VerticalOffset = _canvasHeight * .40;
            //PopTop.HorizontalOffset = _canvasWidth * .50;

            //PopRight.VerticalOffset = _canvasHeight * .55;
            //PopRight.HorizontalOffset = _canvasWidth * .60;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
