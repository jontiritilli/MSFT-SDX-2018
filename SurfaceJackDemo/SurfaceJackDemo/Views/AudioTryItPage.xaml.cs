using System;

using SurfaceJackDemo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceJackDemo.Views
{
    public sealed partial class AudioTryItPage : Page
    {
        private AudioTryItViewModel ViewModel
        {
            get { return DataContext as AudioTryItViewModel; }
        }

        public AudioTryItPage()
        {
            InitializeComponent();
        }
    }
}
