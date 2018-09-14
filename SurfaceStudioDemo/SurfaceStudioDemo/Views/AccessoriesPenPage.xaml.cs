using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesPenPage : Page, INavigate
    {
        #region Private Members

        Popup ReadyScreen;
        public bool Visited = false;

        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }

        #endregion

        #region Construction

        public AccessoriesPenPage()
        {
            InitializeComponent();

            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                ReadyScreen = FlipViewPage.Current.GetAccessoriesPenPopup();
                AccessoriesPenPopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;
            };
        }

        #endregion

        #region Private Methods

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.ReadyScreen.IsOpen = false;
        }

        private void ShowPopup()
        {
            if (!Visited)
            {
                ReadyScreen.IsOpen = true;
                Visited = true;
            }
        }

        private void HidePopup()
        {
            if (ReadyScreen.IsOpen)
            {
                ReadyScreen.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            AnimationHelper.PerformPageEntranceAnimation(this);
            ShowPopup();
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);
            HidePopup();
        }

        #endregion
    }
}
