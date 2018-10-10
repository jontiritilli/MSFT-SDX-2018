using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SurfaceJackDemo.Views
{
    public sealed partial class AudioListenPage : Page, INavigate
    {
        #region Private Members

        private AudioListenViewModel ViewModel
        {
            get { return DataContext as AudioListenViewModel; }
        }
        private bool HasInteracted = false;
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        private ListView PlayerListView;
        #endregion
        #region public members
        public Popup ReadyScreen;
        #endregion


        #region static members
        public static AudioListenPage Current = null;
        #endregion

        #region Construction

        public AudioListenPage()
        {
            InitializeComponent();
            this.Loaded += AudioListenPage_Loaded;
            AudioListenPage.Current = this;
            this.PlayerListView = this.itemListView;
            rBtnLeft.PopupChild = PopLeft;

            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                this.rBtnLeft.PopupChild = FlipViewPage.Current.GetHowToPagePopup();
                HowToPage.Current.CloseButton_Clicked += CloseButton_Clicked;                
            };
        }

        #endregion

        #region Public Methods

        public void ChangeSelectedTrack(int Index)
        {
            if (null != this.PlayerListView)
            {
                this.PlayerListView.SelectedIndex = Index;
            }            
        }

        public void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        #endregion

        #region Private Methods

        private void AudioListenPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.itemListView.Background = StyleHelper.GetAcrylicBrush("Dark");
            this.OverlayGrid.Background = StyleHelper.GetAcrylicBrush("Dark");
            NavigateFromPage();
            this.HasLoaded = true;
            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rBtnLeft.HandleClick();
        }

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView itemListView)
            {
                FlipViewPage.Current.SelectTrack(itemListView.SelectedIndex);
            }
        }

        private void ClosePopupsOnExit()
        {
            if (null != rBtnLeft.PopupChild && rBtnLeft.PopupChild.IsOpen)
            {
                rBtnLeft.PopupChild.IsOpen = false;
            }
        }

        private void AudioTryItClose_Button_Clicked(object sender, RoutedEventArgs e)
        {
            HidePopup();
            AnimatePageEntrance();
            HasInteracted = true;
        }

        private void ShowPopup()
        {
            if (null != ReadyScreen && !HasInteracted)
            {
                ReadyScreen.IsOpen = true;
            }
        }

        private void HidePopup()
        {
            if (null != ReadyScreen && ReadyScreen.IsOpen)
            {
                ReadyScreen.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in            
            if (AudioListenPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                this.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();
        }

        #endregion

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView itemListView)
            {
                FlipViewPage.Current.SelectTrack(itemListView.SelectedIndex);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OverlayGrid.Visibility = Visibility.Collapsed;
        }
    }
}
