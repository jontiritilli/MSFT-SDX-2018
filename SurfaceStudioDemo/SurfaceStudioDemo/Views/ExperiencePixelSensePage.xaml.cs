using System;

using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;
using Windows.UI.Xaml;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperiencePixelSensePage : Page, INavigate
    {
        #region Private Members

        private ExperiencePixelSenseViewModel ViewModel
        {
            get { return DataContext as ExperiencePixelSenseViewModel; }
        }

        #endregion


        #region Construction

        public ExperiencePixelSensePage()
        {
            InitializeComponent();
            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rButtonOne.PopupChild = FlipViewPage.Current.GetAccessoriesDialPopup();
                AccessoriesDialPage.Current.CloseButton_Clicked += CloseButton_Clicked;



            };
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Left, this.img_Left.TranslateDirection, 100, 500, 0);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Right, this.img_Left.TranslateDirection, 100, 1000, 0);
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

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rButtonOne.HandleClick();
        }

        #endregion
    }
}
