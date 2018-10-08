using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class SurroundSoundPage : Page
    {
        private SurroundSoundViewModel ViewModel
        {
            get { return DataContext as SurroundSoundViewModel; }
        }

        public SurroundSoundPage()
        {
            InitializeComponent();
        }
    }
}
