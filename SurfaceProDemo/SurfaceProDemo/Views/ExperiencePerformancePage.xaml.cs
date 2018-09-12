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
                this.rButtonOne.PopupChild = FlipViewPage.Current.GetExperiencePagePopup();
                ExperiencePopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };           
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rButtonOne.HandleClick();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Laptop, this.img_Laptop.TranslateDirection, 100, 500, 0);
            this.rButtonOne.StartEntranceAnimation();
            this.rButtonOne.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rButtonOne.ResetEntranceAnimation();
            this.rButtonOne.ResetRadiateAnimation();


        }

        #endregion
    }
}
