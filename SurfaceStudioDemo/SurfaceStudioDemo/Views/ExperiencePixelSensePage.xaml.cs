using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperiencePixelSensePage : Page, INavigate
    {
        #region Private Members

        private ExperiencePixelSenseViewModel ViewModel
        {
            get { return DataContext as ExperiencePixelSenseViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Static Properties

        public static ExperiencePixelSensePage Current { get; private set; }

        #endregion

        #region Construction

        public ExperiencePixelSensePage()
        {
            InitializeComponent();
            ExperiencePixelSensePage.Current = this;
            this.PopBottomLegal.SetOpacity(0.0d);
            this.PopLeftLegal.SetOpacity(0.0d);
            this.rBtnBottomPixelSense.PopupChild = PopBottom;
            this.rBtnLeftPixelSense.PopupChild = PopLeft;

            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                rBtnRightPixelSense.PopupChild = FlipViewPage.Current.GetExperiencePixelSensePopup();
                ExperiencePixelSensePopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };

            this.Loaded += ExperiencePixelSensePage_Loaded;
        }

        private void ExperiencePixelSensePage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperiencePixelSensePage.Current.HasLoaded = true;
            if (ExperiencePixelSensePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnRightPixelSense.StartEntranceAnimation();
            rBtnRightPixelSense.StartRadiateAnimation();

            rBtnLeftPixelSense.StartEntranceAnimation();
            rBtnLeftPixelSense.StartRadiateAnimation();

            rBtnBottomPixelSense.StartEntranceAnimation();
            rBtnBottomPixelSense.StartRadiateAnimation();
        }
        #endregion

        #region Private Methods

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnRightPixelSense.HandleClick();
            FlipViewPage.Current.ShowAppClose();
        }

        private void PopLeft_Opened(object sender, object e)
        {
            this.PopLeftLegal.SetOpacity(1);
        }

        private void PopBottom_Opened(object sender, object e)
        {
            this.PopBottomLegal.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.PopLeftLegal.SetOpacity(0);
        }

        private void PopBottom_Closed(object sender, object e)
        {
            this.PopBottomLegal.SetOpacity(0);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperiencePixelSensePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperiencePixelSensePage.Current.HasNavigatedTo = true;
            }
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
