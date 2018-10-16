using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class AudioPage : Page
    {
        private AudioViewModel ViewModel
        {
            get { return DataContext as AudioViewModel; }
        }

        public AudioPage()
        {
            InitializeComponent();
        }
    }
}
