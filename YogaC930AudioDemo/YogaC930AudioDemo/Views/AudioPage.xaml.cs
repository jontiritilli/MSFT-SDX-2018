using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class AudioPage : Page
    {
        public Popup HingeDesignPopup;
        public Popup SpeakerDesignPagePopup;
        private AudioViewModel ViewModel
        {
            get { return DataContext as AudioViewModel; }
        }

        public AudioPage()
        {
            InitializeComponent();
            this.Loaded += AudioPage_Loaded;

        }

        private void AudioPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.HingeDesignPopup = FlipViewPage.Current.GetHingDesignPopupPagePopup();
            HingeDesignPopupPage.Current.CloseButton_Clicked+= Close_Left_Clicked;
            this.SpeakerDesignPagePopup = FlipViewPage.Current.GetSpeakerPopupPagePopup();
            SpeakerDesignPopupPage.Current.CloseButton_Clicked += Close_Right_Clicked;

        }

        private void btnLeft_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.HingeDesignPopup.IsOpen == false)
            {
                this.HingeDesignPopup.IsOpen = true;
            }
        }

        private void btnRight_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.SpeakerDesignPagePopup.IsOpen == false)
            {
                this.SpeakerDesignPagePopup.IsOpen = true;
            }
        }

        private void Close_Left_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.HingeDesignPopup.IsOpen == true)
            {
                this.HingeDesignPopup.IsOpen = false;
            }
        }

        private void Close_Right_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.SpeakerDesignPagePopup.IsOpen == true)
            {
                this.SpeakerDesignPagePopup.IsOpen = false;
            }
        }
    }
}
