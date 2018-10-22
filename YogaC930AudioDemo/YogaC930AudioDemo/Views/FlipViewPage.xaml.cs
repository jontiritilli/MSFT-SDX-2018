using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using YogaC930AudioDemo.Controls;
using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class FlipViewPage : Page
    {
        #region Public Static Methods

        public static FlipViewPage Current { get; private set; }

        #endregion


        #region Public Properties

        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        private ColorSchemes ColorScheme = ColorSchemes.Light;

        #endregion


        #region Construction

        public FlipViewPage()
        {
            InitializeComponent();

            FlipViewPage.Current = this;

            this.Loaded += FlipViewPage_Loaded;
        }

        #endregion


        #region Event Handlers

        private void FlipViewPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // set player popup close handler
            PlayerPopupPage.Current.CloseButton_Clicked += Close_Player_Clicked;

            // animate in the play audio demo button
            AnimationHelper.PerformTranslateIn(this.PlayAudioDemoButton, TranslateAxis.Vertical,
                                                -87, -87, -4,
                                                new BounceEase() { Bounces = 3, Bounciness = 1, EasingMode = EasingMode.EaseIn},
                                                new BounceEase() { Bounces = 3, Bounciness = 1, EasingMode = EasingMode.EaseIn },
                                                2500, 2500);
        }

        private void ContentFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != this.ContentFlipView)
            {
                // based on the current page, set the scheme to light or dark
                switch (this.ContentFlipView.SelectedIndex)
                {
                    case 0:     // audio page
                        this.ColorScheme = ColorSchemes.Light;

                        if ((null != this.LeftChevronLight) && (null != LeftChevronDark) && (null != RightChevronLight) && (null != RightChevronDark))
                        {
                            // hide the left arrow
                            this.LeftChevronLight.Visibility = Visibility.Collapsed;
                            this.LeftChevronDark.Visibility = Visibility.Collapsed;

                            // show the right arrow dark
                            this.RightChevronLight.Visibility = Visibility.Collapsed;
                            this.RightChevronDark.Visibility = Visibility.Visible;
                        }
                        break;

                    case 1:     // features page
                        this.ColorScheme = ColorSchemes.Dark;

                        // show the left arrow light
                        if ((null != this.LeftChevronLight) && (null != LeftChevronDark) && (null != RightChevronLight) && (null != RightChevronDark))
                        {
                            this.LeftChevronLight.Visibility = Visibility.Visible;
                            this.LeftChevronDark.Visibility = Visibility.Collapsed;

                            // show the right arrow dark
                            this.RightChevronLight.Visibility = Visibility.Collapsed;
                            this.RightChevronDark.Visibility = Visibility.Visible;
                        }
                        break;

                    case 2:     // speeds and feeds page
                        this.ColorScheme = ColorSchemes.Light;

                        if ((null != this.LeftChevronLight) && (null != LeftChevronDark) && (null != RightChevronLight) && (null != RightChevronDark))
                        {
                            // show the left arrow dark
                            this.LeftChevronLight.Visibility = Visibility.Collapsed;
                            this.LeftChevronDark.Visibility = Visibility.Visible;

                            // hide the right arrow 
                            this.RightChevronLight.Visibility = Visibility.Collapsed;
                            this.RightChevronDark.Visibility = Visibility.Collapsed;
                        }
                        break;
                }

                // update play button
                // - play button has no color changes any longer

                // update navigation buttons
                if (null != this.NavigationBarExploreWindows) { this.NavigationBarExploreWindows.ColorScheme = this.ColorScheme; }
                if (null != this.NavigationBarGoToDesktop) { this.NavigationBarGoToDesktop.ColorScheme = this.ColorScheme; }
            }
        }

        private void NavigationBarExploreWindows_Click(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // do something here; launch an rdx uri?
        }

        private void NavigationBarGoToDesktop_Click(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // do something here; exit app or just minimize?
        }

        private void PlayAudioDemoBlueBorder_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (this.PlayerPopup.IsOpen == false)
            {
                this.PlayerPopup.IsOpen = true;
            }
        }

        private void AppCloseButtonBorder_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // ugly, but effective
            App.Current.Exit();
        }

        //private void PlayDemoButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        //{
        //    if (this.PlayerPopup.IsOpen == false)
        //    {
        //        this.PlayerPopup.IsOpen = true;
        //    }
        //}

        private void Close_Player_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.PlayerPopup.IsOpen == true)
            {
                this.PlayerPopup.IsOpen = false;
            }
        }

        #endregion


        #region Public Methods

        public Popup GetPlayerPopup()
        {
            return this.PlayerPopup;
        }

        public Popup GetSpeakerPopupPagePopup()
        {
            return this.SpeakerDesignPopup;

        }

        public Popup GetHingDesignPopupPagePopup()
        {
            return this.HingeDesignPopup;

        }

        #endregion
    }
}
