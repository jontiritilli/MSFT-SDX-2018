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
                this.rButtonOne.PopupChild = FlipViewPage.Current.GetExperienceDayWorkPagePopup();
                ExperienceDayWorkPopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
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
