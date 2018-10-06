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
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
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
