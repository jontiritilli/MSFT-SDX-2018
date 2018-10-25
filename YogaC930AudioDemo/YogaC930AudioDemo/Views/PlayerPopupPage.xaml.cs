using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using YogaC930AudioDemo.Controls;
using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class PlayerPopupPage : Page, INavigate
    {
        #region Private Properties

        private PlayerPopupViewModel ViewModel
        {
            get { return DataContext as PlayerPopupViewModel; }
        }

        #endregion


        #region Public Properties

        public RoutedEventHandler CloseButton_Clicked;

        #endregion


        #region Public Static Properties

        public static PlayerPopupPage Current { get; private set; }

        #endregion


        #region Construction / Initialization

        public PlayerPopupPage()
        {
            InitializeComponent();
            
            PlayerPopupPage.Current = this;

            this.Loaded += this.PlayerPopupPage_Loaded;
        }

        private void PlayerPopupPage_Loaded(object sender, RoutedEventArgs e)
        {
            // disable the system back button
            SystemNavigationManager mgr = SystemNavigationManager.GetForCurrentView();
            if (null != mgr)
            {
                mgr.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        #endregion


        #region Public Methods

        public VolumeSlider GetVolumeControl()
        {
            return this.VolumeControl;
        }

        #endregion


        #region Event Handlers

        private void LoopPlayer_PlaybackEnded(object sender, EventArgs args)
        {
            CloseAndExit();
        }

        private void CloseButtonImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CloseAndExit();
        }

        private void CloseAndExit()
        {
            var trash = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CloseButton_Clicked(this, new RoutedEventArgs());
                this.LoopPlayer.ResetPlayer();
                this.VolumeControl.VolumeToDefault();
            });
        }

        #endregion


        #region INavigate

        public void NavigateToPage()
        {
            // NOTHING TO ANIMATE
            //AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            // NOTHING TO ANIMATE
            //AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
