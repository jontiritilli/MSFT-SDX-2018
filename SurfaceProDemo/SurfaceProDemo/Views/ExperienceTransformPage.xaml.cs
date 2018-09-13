using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceTransformPage : Page, INavigate
    {
        #region Private Members

        private ExperienceTransformViewModel ViewModel
        {
            get { return DataContext as ExperienceTransformViewModel; }
        }

        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public ExperienceTransformPage()
        {
            InitializeComponent();
            Canvas.SetTop(rBtnRight, _canvasHeight * .55);
            Canvas.SetLeft(rBtnRight, _canvasWidth * .81);
            rBtnRight.PopupChild = PopRight;
            PopRight.HorizontalOffset = _canvasWidth * .81;
            PopRight.VerticalOffset = _canvasHeight * .55;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Studio, this.img_Studio.TranslateDirection, 100, 500, 0);
            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
