using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class SpeakerDesignPopupPage : Page
    {
        private SpeakerDesignPopupViewModel ViewModel
        {
            get { return DataContext as SpeakerDesignPopupViewModel; }
        }

        public SpeakerDesignPopupPage()
        {
            InitializeComponent();
        }
    }
}
