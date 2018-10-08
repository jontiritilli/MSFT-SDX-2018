using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceJackDemo.Views
{
    public sealed partial class AudioListenPage : Page, INavigate
    {
        #region Private Members

        private AudioListenViewModel ViewModel
        {
            get { return DataContext as AudioListenViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        private ListView PlayerListView;
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
        }

        public void ChangeSelectedTrack(int Index)
        {
            if (null != this.PlayerListView)
            {
                this.PlayerListView.SelectedIndex = Index;
            }            
        }

        private void AudioListenPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.itemListView.Background = StyleHelper.GetAcrylicBrush("Dark");
            NavigateFromPage();
            this.HasLoaded = true;
            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        public void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
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
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView itemListView)
            {
                FlipViewPage.Current.SelectTrack(itemListView.SelectedIndex);
            }
            
        }
    }
}
