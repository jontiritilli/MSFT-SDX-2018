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

            //// prepare for page animation
            //AnimationHelper.PrepForPageAnimation(this);
        }

        #endregion


        #region Public Methods

        public VolumeSlider GetVolumeControl()
        {
            return this.VolumeControl;
        }

        #endregion


        #region Event Handlers

        private void CloseButtonImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
            this.LoopPlayer.ResetPlayer();
            this.VolumeControl.VolumeToDefault();
        }

        #endregion


        #region INavigate

        public void NavigateToPage()
        {
            //AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            //AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
