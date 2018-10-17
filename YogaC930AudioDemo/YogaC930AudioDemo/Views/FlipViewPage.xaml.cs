using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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
            PlayerPopupPage.Current.CloseButton_Clicked += Close_Player_Clicked;
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
