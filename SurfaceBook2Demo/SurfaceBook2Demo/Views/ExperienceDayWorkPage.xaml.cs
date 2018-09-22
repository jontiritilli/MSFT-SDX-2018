using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayWorkPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayWorkViewModel ViewModel
        {
            get { return DataContext as ExperienceDayWorkViewModel; }
        }

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


            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rBtnTop.HandleClick();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in            
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
            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion

        private void ImageBrush_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            int x = 0;
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
    }
}
