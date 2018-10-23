using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using YogaC930AudioDemo.Controls;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class PlayerPopupPage : Page
    {
        public RoutedEventHandler CloseButton_Clicked;

        private PlayerPopupViewModel ViewModel
        {
            get { return DataContext as PlayerPopupViewModel; }
        }

        #region Public Static Properties

        public static PlayerPopupPage Current { get; private set; }

        #endregion

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

        private void CloseButtonImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
            this.LoopPlayer.ResetPlayer();
            this.VolumeControl.VolumeToDefault();
        }

        public VolumeSlider GetVolumeControl()
        {
            return this.VolumeControl;
        }
    }
}
