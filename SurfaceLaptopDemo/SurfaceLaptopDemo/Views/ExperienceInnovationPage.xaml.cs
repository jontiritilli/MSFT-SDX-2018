using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceInnovationPage : Page, INavigate
    {
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        #region Private Members

        private ExperienceInnovationViewModel ViewModel
        {
            get { return DataContext as ExperienceInnovationViewModel; }
        }

        #endregion

        #region Construction

        public ExperienceInnovationPage()
        {
            InitializeComponent();
            Canvas.SetTop(rBtnTopInnovation, _canvasHeight * .18);
            Canvas.SetLeft(rBtnTopInnovation, _canvasWidth * .37);

            Canvas.SetTop(rBtnRightInnovation, _canvasHeight * .50);
            Canvas.SetLeft(rBtnRightInnovation, _canvasWidth * .74);

            Canvas.SetTop(rBtnTryItInnovation, _canvasHeight * .63);
            Canvas.SetLeft(rBtnTryItInnovation, _canvasWidth * .18);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTopInnovation.StartEntranceAnimation();
            rBtnTopInnovation.StartRadiateAnimation();

            rBtnRightInnovation.StartEntranceAnimation();
            rBtnRightInnovation.StartRadiateAnimation();

            rBtnTryItInnovation.StartEntranceAnimation();
            rBtnTryItInnovation.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnTopInnovation.ResetEntranceAnimation();
            rBtnTopInnovation.ResetRadiateAnimation();

            rBtnRightInnovation.ResetEntranceAnimation();
            rBtnRightInnovation.ResetRadiateAnimation();

            rBtnTryItInnovation.ResetEntranceAnimation();
            rBtnTryItInnovation.ResetRadiateAnimation();
        }

        #endregion
    }
}
