using System;
using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceIntroPage : Page, INavigate
    {
        #region Private Members

        private ExperienceIntroViewModel ViewModel
        {
            get { return DataContext as ExperienceIntroViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public ExperienceIntroPage()
        {
            InitializeComponent();

            Canvas.SetTop(rBtnLeft, _canvasHeight * .30);
            Canvas.SetLeft(rBtnLeft, _canvasWidth * .20);


            Canvas.SetTop(rBtnRight, _canvasHeight * .30);
            Canvas.SetLeft(rBtnRight, _canvasWidth * .65);

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

            //PopLeft.VerticalOffset = _canvasHeight * .30;
            //PopLeft.HorizontalOffset = _canvasWidth * .20;

            //PopRight.VerticalOffset = _canvasHeight * .30;
            //PopRight.HorizontalOffset = _canvasWidth * .65;

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
