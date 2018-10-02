using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using Windows.UI.Xaml;


namespace SurfaceProDemo.Views
{
    public sealed partial class ExperiencePerformancePage : Page, INavigate
    {
        #region Private Members

        private ExperiencePerformanceViewModel ViewModel
        {
            get { return DataContext as ExperiencePerformanceViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members
        public static ExperiencePerformancePage Current { get; private set; }
        #endregion

        #region Construction

        public ExperiencePerformancePage()
        {
            InitializeComponent();
            ExperiencePerformancePage.Current = this;
            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rBtnTop.PopupChild = FlipViewPage.Current.GetExperiencePagePopup();
                ExperiencePopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            this.Loaded += ExperiencePerformancePage_Loaded;
        }

        private void ExperiencePerformancePage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperiencePerformancePage.Current.HasLoaded = true;
            if (ExperiencePerformancePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rBtnTop.HandleClick();
        }
        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);

            this.rBtnLeft.StartEntranceAnimation();
            this.rBtnLeft.StartRadiateAnimation();

            this.rBtnTop.StartEntranceAnimation();
            this.rBtnTop.StartRadiateAnimation();

            this.rBtnRight.StartEntranceAnimation();
            this.rBtnRight.StartRadiateAnimation();
        }
        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperiencePerformancePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperiencePerformancePage.Current.HasNavigatedTo = true;
            }
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
