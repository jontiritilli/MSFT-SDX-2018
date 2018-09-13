using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SDX.Toolkit.Helpers;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayWorkPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayWorkViewModel ViewModel
        {
            get { return DataContext as ExperienceDayWorkViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        double _rbLeftSetTop;
        double _rbLeftSetLeft;
        #endregion


        #region Construction

        public ExperienceDayWorkPage()
        {
            InitializeComponent();
            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rBtnTop.PopupChild = FlipViewPage.Current.GetExperienceDayWorkPagePopup();
                ExperienceDayWorkPopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };

            _rbLeftSetTop = _canvasHeight * .7;
            _rbLeftSetLeft = _canvasWidth * .20;
            Canvas.SetTop(rBtnLeft, _rbLeftSetTop);
            Canvas.SetLeft(rBtnLeft, _rbLeftSetLeft);


            Canvas.SetTop(rBtnRight, _canvasHeight * .65);
            Canvas.SetLeft(rBtnRight, _canvasWidth * .60);

            Canvas.SetTop(rBtnTop, _canvasHeight * .35);
            Canvas.SetLeft(rBtnTop, _canvasWidth * .25);

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            rBtnLeft.PopupChild.HorizontalOffset = 20;
            //PopLeft.VerticalOffset = _rbLeftSetTop;
            //PopLeft.HorizontalOffset = _rbLeftSetLeft + (StyleHelper.GetApplicationDouble(LayoutSizes.RadiatingButtonEllipseRadius) * 2) + 10;

            //PopRight.VerticalOffset = _canvasHeight * .45;
            //PopRight.HorizontalOffset = _canvasWidth * .60;

        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rBtnTop.HandleClick();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
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
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
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
