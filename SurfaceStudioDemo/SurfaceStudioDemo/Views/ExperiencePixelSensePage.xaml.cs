using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperiencePixelSensePage : Page, INavigate
    {
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        #region Private Members

        private ExperiencePixelSenseViewModel ViewModel
        {
            get { return DataContext as ExperiencePixelSenseViewModel; }
        }

        #endregion

        #region Public Static Properties

        public static ExperiencePixelSensePage Current { get; private set; }

        #endregion

        #region Construction

        public ExperiencePixelSensePage()
        {
            InitializeComponent();

            Canvas.SetTop(rBtnLeftPixelSense, _canvasHeight * .47);
            Canvas.SetLeft(rBtnLeftPixelSense, _canvasWidth * .24);

            Canvas.SetTop(rBtnRightPixelSense, _canvasHeight * .63);
            Canvas.SetLeft(rBtnRightPixelSense, _canvasWidth * .65);

            Canvas.SetTop(rBtnBottomPixelSense, _canvasHeight * .83);
            Canvas.SetLeft(rBtnBottomPixelSense, _canvasWidth * .68);

            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                rBtnRightPixelSense.PopupChild = FlipViewPage.Current.GetExperiencePixelSensePopup();
                ExperiencePixelSensePopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };
        }

        #endregion

        #region Private Methods

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnRightPixelSense.HandleClick();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            AnimationHelper.PerformPageEntranceAnimation(this);
            AnimationHelper.PerformTranslateIn(this.img_Left, this.img_Left.TranslateDirection, 100, 500, 0);
            AnimationHelper.PerformTranslateIn(this.img_Right, this.img_Left.TranslateDirection, 100, 1000, 0);

            rBtnRightPixelSense.StartEntranceAnimation();
            rBtnRightPixelSense.StartRadiateAnimation();

            rBtnLeftPixelSense.StartEntranceAnimation();
            rBtnLeftPixelSense.StartRadiateAnimation();

            rBtnBottomPixelSense.StartEntranceAnimation();
            rBtnBottomPixelSense.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);
            rBtnRightPixelSense.ResetEntranceAnimation();
            rBtnRightPixelSense.ResetRadiateAnimation();

            rBtnLeftPixelSense.ResetEntranceAnimation();
            rBtnLeftPixelSense.ResetRadiateAnimation();

            rBtnBottomPixelSense.ResetEntranceAnimation();
            rBtnBottomPixelSense.ResetRadiateAnimation();
        }

        #endregion
    }
}
