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

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Static Properties

        public static ExperienceDayWorkPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceDayWorkPage()
        {
            InitializeComponent();
            ExperienceDayWorkPage.Current = this;
            this.LegalBatteryLife.SetOpacity(0.0d);
            this.LegalConnections.SetOpacity(0.0d);
            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rBtnTop.PopupChild = FlipViewPage.Current.GetExperienceDayWorkPagePopup();
                ExperienceDayWorkPopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };
            this.LegalBatteryLife.SetOpacity(0);
            this.LegalConnections.SetOpacity(0);
            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

            this.Loaded += ExperienceDayWorkPage_Loaded;

        }

        private void ExperienceDayWorkPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceDayWorkPage.Current.HasLoaded = true;
            if (ExperienceDayWorkPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();

            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        #endregion

        #region Private Methods

        private void ImageBrush_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            int x = 0;
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rBtnTop.HandleClick();
        }

        private void PopLeft_Opened(object sender, object e)
        {
            this.LegalBatteryLife.SetOpacity(1);
        }

        private void PopRight_Opened(object sender, object e)
        {
            this.LegalConnections.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LegalBatteryLife.SetOpacity(0);
        }

        private void PopRight_Closed(object sender, object e)
        {
            this.LegalConnections.SetOpacity(0);
        }

        private void ClosePopupsOnExit()
        {
            if (null != rBtnTop.PopupChild && rBtnTop.PopupChild.IsOpen)
            {
                rBtnTop.PopupChild.IsOpen = false;
            }

            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }

            if (null != this.PopRight && this.PopRight.IsOpen)
            {
                this.PopRight.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperienceDayWorkPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceDayWorkPage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            // animations out            
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

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
