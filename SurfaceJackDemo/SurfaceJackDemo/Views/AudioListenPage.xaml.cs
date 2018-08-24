using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class AudioListenPage : Page
    {
        private AudioListenViewModel ViewModel
        {
            get { return DataContext as AudioListenViewModel; }
        }

        public AudioListenPage()
        {
            InitializeComponent();
        }
    }
}
