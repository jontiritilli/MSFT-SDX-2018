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
                                                new BounceEase() { Bounces = 3, Bounciness = 1, EasingMode = EasingMode.EaseIn },
                                                new BounceEase() { Bounces = 3, Bounciness = 1, EasingMode = EasingMode.EaseIn },
                                                500, 500);
        }

        private void ContentFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != this.ContentFlipView)
            {
                switch (this.ContentFlipView.SelectedIndex)
                {
                    case 0:     // audio page
                        this.ColorScheme = ColorSchemes.Light;
                        break;

                    case 1:     // features page
                        this.ColorScheme = ColorSchemes.Dark;
                        break;

                    case 2:     // speeds and feeds page
                        this.ColorScheme = ColorSchemes.Light;
                        break;
                }

                // update play button


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
