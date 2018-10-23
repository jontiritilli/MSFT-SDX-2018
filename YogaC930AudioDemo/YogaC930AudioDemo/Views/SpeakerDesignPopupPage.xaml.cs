using System;
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
        }

        private void CloseButtonImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
    }
}
