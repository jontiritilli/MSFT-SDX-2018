using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI;

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
        public Popup HowToScreen;

        #endregion

        #region Public Static Members

        public static AudioListenPage Current { get; private set; }

        #endregion

        #region Construction

        public AudioListenPage()
        {
            InitializeComponent();

            AudioListenPage.Current = this;

            this.PlayerListView = this.itemListView;

            this.Loaded += AudioListenPage_Loaded;
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
            ShowPopup();
            AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        #endregion

        #region Private Methods

        private void AudioListenPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.itemListView.Background = new SolidColorBrush(Colors.Black);

            NavigateFromPage();

            this.itemListView.SelectedIndex = 0;

            this.HasLoaded = true;

            // get the initial screen cover popup
            this.ReadyScreen = FlipViewPage.Current.GetAudioListenPopup();

            AudioListenPopupPage.Current.CloseButton_Clicked += AudioTryItClose_Button_Clicked;

            // get the howto screen popup
            this.HowToScreen = FlipViewPage.Current.GetHowToPagePopup();
            this.rBtnLeft.PopupChild = HowToScreen;

            HowToPage.Current.CloseButton_Clicked += HowToCloseButton_Clicked;

            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void HowToCloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.rBtnLeft.HandleClick();
        }

        private void AudioTryItClose_Button_Clicked(object sender, RoutedEventArgs e)
        {
            HasInteracted = true;
            HidePopup();
            AnimatePageEntrance();
        }

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView itemListView)
            {
                FlipViewPage.Current.SelectTrack(itemListView.SelectedIndex);
                // hack to force the controltemplates to change to use the selected icon and foreground
                // dont judge me
                this.itemListView.ScrollIntoView(this.itemListView.SelectedItem);
                foreach (var item in e.AddedItems)
                {
                    ListViewItem listViewItem = (ListViewItem)itemListView.ContainerFromItem(item);

                    listViewItem.ContentTemplate = (DataTemplate)this.Resources["SelectedPlayListViewItem"];
                }
                foreach (var item in e.RemovedItems)
                {
                    ListViewItem listViewItem = (ListViewItem)itemListView.ContainerFromItem(item);
                    listViewItem.ContentTemplate = (DataTemplate)this.Resources["PlayListViewItem"];
                }
            }
        }

        private void ClosePopupsOnExit()
        {
            if (null != rBtnLeft.PopupChild && rBtnLeft.PopupChild.IsOpen)
            {
                rBtnLeft.PopupChild.IsOpen = false;
            }
            HidePopup();
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
    }
}
