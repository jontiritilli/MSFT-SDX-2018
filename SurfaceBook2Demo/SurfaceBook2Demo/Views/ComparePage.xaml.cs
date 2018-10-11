using System;

using SurfaceBook2Demo.ViewModels;
using SurfaceBook2Demo.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;
using SDX.Telemetry.Services;


namespace SurfaceBook2Demo.Views
{
    public sealed partial class ComparePage : Page, INavigate
    {
        #region Private Members

        private CompareViewModel ViewModel
        {
            get { return DataContext as CompareViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Static Properties

        public static ComparePage Current { get; private set; }

        #endregion

        #region Construction

        public ComparePage()
        {
            InitializeComponent();
            ComparePage.Current = this;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1500) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();

                this.CompareLegal.Visibility = ConfigurationService.Current.GetIsStudioCompareLegalEnabled() ? Visibility.Visible : Visibility.Collapsed;

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
            ComparePage.Current.HasLoaded = true;
            if (ComparePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);            

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

        private void ClosePopupsOnExit()
        {
            if (null != rBtnPro.PopupChild && rBtnPro.PopupChild.IsOpen)
            {
                rBtnPro.PopupChild.IsOpen = false;
            }
            if (null != rBtnBook.PopupChild && rBtnBook.PopupChild.IsOpen)
            {
                rBtnBook.PopupChild.IsOpen = false;
            }
            if (null != rBtnStudio.PopupChild && rBtnStudio.PopupChild.IsOpen)
            {
                rBtnStudio.PopupChild.IsOpen = false;
            }
            if (null != rBtnPro.PopupChild && rBtnLaptop.PopupChild.IsOpen)
            {
                rBtnLaptop.PopupChild.IsOpen = false;
            }
            if (null != rBtnGo.PopupChild && rBtnGo.PopupChild.IsOpen)
            {
                rBtnGo.PopupChild.IsOpen = false;
            }
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ComparePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ComparePage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

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
