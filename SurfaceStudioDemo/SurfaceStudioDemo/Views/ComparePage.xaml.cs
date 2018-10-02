using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;
using SDX.Toolkit.Helpers;
using SDX.Telemetry.Services;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class ComparePage : Page, INavigate
    {
        #region Private Members

        private CompareViewModel ViewModel
        {
            get { return DataContext as CompareViewModel; }
        }

        #endregion

        #region Construction

        public ComparePage()
        {
            InitializeComponent();

            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1500) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rBtnPro.PopupChild = FlipViewPage.Current.GetComparePagePopupPro(); ;
                ComparePagePopupPro.Current.CloseButton_Clicked += Close_Pro_Clicked;

                this.rBtnBook.PopupChild = FlipViewPage.Current.GetComparePagePopupBook(); ;
                ComparePagePopupBook.Current.CloseButton_Clicked += Close_Book_Clicked;

                this.rBtnStudio.PopupChild = FlipViewPage.Current.GetComparePagePopupStudio();
                ComparePagePopupStudio.Current.CloseButton_Clicked += Close_Studio_Clicked;

                this.rBtnLaptop.PopupChild = FlipViewPage.Current.GetComparePagePopupLaptop();
                ComparePagePopupLaptop.Current.CloseButton_Clicked += Close_Laptop_Clicked;

                this.rBtnGo.PopupChild = FlipViewPage.Current.GetComparePagePopupGo();
                ComparePagePopupGo.Current.CloseButton_Clicked += Close_Go_Clicked;
            };

            this.Loaded += ComparePage_Loaded;
        }

        private void ComparePage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        #endregion

        #region Private Methods

        private void Close_Pro_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnPro.HandleClick();
            FlipViewPage.Current.ShowAppClose();
        }

        private void Close_Book_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnBook.HandleClick();
            FlipViewPage.Current.ShowAppClose();
        }

        private void Close_Studio_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnStudio.HandleClick();
            FlipViewPage.Current.ShowAppClose();
        }

        private void Close_Laptop_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnLaptop.HandleClick();
            FlipViewPage.Current.ShowAppClose();
        }

        private void Close_Go_Clicked(object sender, RoutedEventArgs e)
        {
            rBtnGo.HandleClick();
            FlipViewPage.Current.ShowAppClose();
        }

        private void RadiatingButton_Clicked(object sender, EventArgs e)
        {
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ComparisonHot);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Family, this.img_Family.TranslateDirection, 100, 500, 0);

            rBtnPro.StartEntranceAnimation();
            rBtnPro.StartRadiateAnimation();

            rBtnBook.StartEntranceAnimation();
            rBtnBook.StartRadiateAnimation();

            rBtnStudio.StartEntranceAnimation();
            rBtnStudio.StartRadiateAnimation();

            rBtnLaptop.StartEntranceAnimation();
            rBtnLaptop.StartRadiateAnimation();

            rBtnGo.StartEntranceAnimation();
            rBtnGo.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnPro.ResetEntranceAnimation();
            rBtnPro.ResetRadiateAnimation();

            rBtnBook.ResetEntranceAnimation();
            rBtnBook.ResetRadiateAnimation();

            rBtnStudio.ResetEntranceAnimation();
            rBtnStudio.ResetRadiateAnimation();

            rBtnLaptop.ResetEntranceAnimation();
            rBtnLaptop.ResetRadiateAnimation();

            rBtnGo.ResetEntranceAnimation();
            rBtnGo.ResetRadiateAnimation();
        }

        #endregion
    }
}
