using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class AudioPage : Page, INavigate
    {
        #region Private Properties

        private AudioViewModel ViewModel
        {
            get { return DataContext as AudioViewModel; }
        }

        #endregion


        #region Public Properties

        public Popup HingeDesignPopup;
        public Popup SpeakerDesignPagePopup;

        #endregion


        #region Construction / Initialization

        public AudioPage()
        {
            InitializeComponent();
            this.Loaded += AudioPage_Loaded;

        }

        private void AudioPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.HingeDesignPopup = FlipViewPage.Current.GetHingDesignPopupPagePopup();
            HingeDesignPopupPage.Current.CloseButton_Clicked+= CloseButtonLeft_Clicked;

            this.SpeakerDesignPagePopup = FlipViewPage.Current.GetSpeakerPopupPagePopup();
            SpeakerDesignPopupPage.Current.CloseButton_Clicked += CloseButtonRight_Clicked;

            //YogaC930AudioDemo.Helpers.TestHelper.AddGridCellBorders(this.LayoutRoot, 3, 3, Windows.UI.Colors.AliceBlue);

            // disable the system back button
            SystemNavigationManager mgr = SystemNavigationManager.GetForCurrentView();
            if (null != mgr)
            {
                mgr.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        #endregion


        #region Event Handlers

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (this.SpeakerDesignPagePopup.IsOpen == false)
            {
                this.SpeakerDesignPagePopup.IsOpen = true;
                AnimationHelper.PerformPageEntranceAnimation((Page)this.SpeakerDesignPagePopup.Child);
            }
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            if (this.HingeDesignPopup.IsOpen == false)
            {
                this.HingeDesignPopup.IsOpen = true;
                AnimationHelper.PerformPageEntranceAnimation((Page)this.HingeDesignPopup.Child);
            }
        }

        private void CloseButtonLeft_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.HingeDesignPopup.IsOpen == true)
            {
                AnimationHelper.PerformPageExitAnimation((Page)this.HingeDesignPopup.Child);
                this.HingeDesignPopup.IsOpen = false;
            }
        }

        private void CloseButtonRight_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.SpeakerDesignPagePopup.IsOpen == true)
            {
                AnimationHelper.PerformPageExitAnimation((Page)this.SpeakerDesignPagePopup.Child);
                this.SpeakerDesignPagePopup.IsOpen = false;
            }
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
