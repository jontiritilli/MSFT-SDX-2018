using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Views
{
    public sealed partial class FlipViewPage : Page
    {
        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }
        #region public static
        public static FlipViewPage Current { get; private set; }
        #endregion
        public FlipViewPage()
        {
            InitializeComponent();
            FlipViewPage.Current = this;
            this.Loaded += FlipViewPage_Loaded;
        }

        private void FlipViewPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PlayerPopupPage.Current.CloseButton_Clicked += Close_Player_Clicked;
        }

        public Popup GetPlayerPopup()
        {
            return this.PlayerPopup;
        }

        public Popup GetSpeakerPopupPagePopup()
        {
            return this.SpeakerDesignPopupPagePopup;

        }

        public Popup GetHingDesignPopupPagePopup()
        {
            return this.HingeDesignPopup;

        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.PlayerPopup.IsOpen == false)
            {
                this.PlayerPopup.IsOpen = true;
            }
        }

        private void Close_Player_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.PlayerPopup.IsOpen == true)
            {
                this.PlayerPopup.IsOpen = false;
            }
        }
    }
}
