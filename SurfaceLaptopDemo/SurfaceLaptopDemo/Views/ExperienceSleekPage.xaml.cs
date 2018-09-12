using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSleekPage : Page, INavigate
    {
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

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
            Canvas.SetTop(rBtnTopPerformance, _canvasHeight * .25);
            Canvas.SetLeft(rBtnTopPerformance, _canvasWidth * .82);

            Canvas.SetTop(rBtnLeftPerformance, _canvasHeight * .67);
            Canvas.SetLeft(rBtnLeftPerformance, _canvasWidth * .4);

            Canvas.SetTop(rBtnBottomPerformance, _canvasHeight * .75);
            Canvas.SetLeft(rBtnBottomPerformance, _canvasWidth * .8);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
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
