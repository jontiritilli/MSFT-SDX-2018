using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using Windows.UI.Xaml;
using SDX.Toolkit.Helpers;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperiencePerformancePage : Page, INavigate
    {
        #region Private Members

        private ExperiencePerformanceViewModel ViewModel
        {
            get { return DataContext as ExperiencePerformanceViewModel; }
        }

        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion


        #region Construction

        public ExperiencePerformancePage()
        {
            InitializeComponent();
            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rBtnTop.PopupChild = FlipViewPage.Current.GetExperiencePagePopup();
                ExperiencePopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };
            Canvas.SetTop(rBtnLeft, _canvasHeight * .50);
            Canvas.SetLeft(rBtnLeft, _canvasWidth * .45);

            Canvas.SetTop(rBtnTop, _canvasHeight * .30);
            Canvas.SetLeft(rBtnTop, _canvasWidth * .55);

            Canvas.SetTop(rBtnRight, _canvasHeight * .30);
            Canvas.SetLeft(rBtnRight, _canvasWidth * .85);

            rBtnLeft.PopupChild = PopLeft;
            PopLeft.VerticalOffset = _canvasHeight * .50;
            PopLeft.HorizontalOffset = _canvasWidth * .45;
            
            rBtnRight.PopupChild = PopRight;
            PopRight.VerticalOffset = _canvasHeight * .30;
            PopRight.HorizontalOffset = _canvasWidth * .85;           
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
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Laptop, this.img_Laptop.TranslateDirection, 100, 500, 0);
            this.rBtnLeft.StartEntranceAnimation();
            this.rBtnLeft.StartRadiateAnimation();

            this.rBtnTop.StartEntranceAnimation();
            this.rBtnTop.StartRadiateAnimation();

            this.rBtnRight.StartEntranceAnimation();
            this.rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rBtnTop.ResetEntranceAnimation();
            this.rBtnTop.ResetRadiateAnimation();

            this.rBtnLeft.ResetEntranceAnimation();
            this.rBtnLeft.ResetRadiateAnimation();

            this.rBtnRight.ResetEntranceAnimation();
            this.rBtnRight.ResetRadiateAnimation();


        }

        #endregion
    }
}
