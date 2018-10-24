using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class HingeDesignPopupPage : Page, INavigate
    {
        #region Private Properties

        private HingeDesignPopupViewModel ViewModel
        {
            get { return DataContext as HingeDesignPopupViewModel; }
        }

        #endregion


        #region Public Members

        public RoutedEventHandler CloseButton_Clicked;

        #endregion


        #region Public Static Properties

        public static HingeDesignPopupPage Current { get; private set; }

        #endregion


        #region Construction / Initialization

        public HingeDesignPopupPage()
        {
            InitializeComponent();

            HingeDesignPopupPage.Current = this;

            this.Loaded += this.HingeDesignPopupPage_Loaded;
        }

        private void HingeDesignPopupPage_Loaded(object sender, RoutedEventArgs e)
        {
            // disable the system back button
            SystemNavigationManager mgr = SystemNavigationManager.GetForCurrentView();
            if (null != mgr)
            {
                mgr.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            // prepare for animation
            AnimationHelper.PrepForPageAnimation(this);
        }

        #endregion


        #region Event Handlers

        private void CloseButtonImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        #endregion


        #region INavigate

        public void NavigateToPage()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
