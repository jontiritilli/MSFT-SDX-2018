using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class SpeakerDesignPopupPage : Page
    {
        public RoutedEventHandler CloseButton_Clicked;

        private SpeakerDesignPopupViewModel ViewModel
        {
            get { return DataContext as SpeakerDesignPopupViewModel; }
        }

        #region Public Static Properties

        public static SpeakerDesignPopupPage Current { get; private set; }

        #endregion

        public SpeakerDesignPopupPage()
        {
            InitializeComponent();

            SpeakerDesignPopupPage.Current = this;

            this.Loaded += this.SpeakerDesignPopupPage_Loaded;
        }

        private void SpeakerDesignPopupPage_Loaded(object sender, RoutedEventArgs e)
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
        }
    }
}
