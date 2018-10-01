using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AccessoriesDialPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesDialViewModel ViewModel
        {
            get { return DataContext as AccessoriesDialViewModel; }
        }
        //double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        //double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();
            this.LegalDial.SetOpacity(0.0d);
            this.LegalPen.SetOpacity(0.0d);
            //Canvas.SetTop(rBtnLeft, _canvasHeight * .70);
            //Canvas.SetLeft(rBtnLeft, _canvasWidth * .20);

            //Canvas.SetTop(rBtnTop, _canvasHeight * .40);
            //Canvas.SetLeft(rBtnTop, _canvasWidth * .50);

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;

            //PopLeft.VerticalOffset = _canvasHeight * .70;
            //PopLeft.HorizontalOffset = _canvasWidth * .20;

            //PopTop.VerticalOffset = _canvasHeight * .40;
            //PopTop.HorizontalOffset = _canvasWidth * .50;            
        }

        #endregion

        #region Event Handlers
        private void PopDial_Opened(object sender, object e)
        {
            this.LegalDial.SetOpacity(1);
        }

        private void PopPen_Opened(object sender, object e)
        {
            this.LegalPen.SetOpacity(1);
        }

        private void PopDial_Closed(object sender, object e)
        {
            this.LegalDial.SetOpacity(0);
        }

        private void PopPen_Closed(object sender, object e)
        {
            this.LegalPen.SetOpacity(0);
        }
        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
