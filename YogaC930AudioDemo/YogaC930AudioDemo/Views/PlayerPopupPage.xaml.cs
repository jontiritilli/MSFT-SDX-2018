using System;

using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class PlayerPopupPage : Page
    {
        private PlayerPopupViewModel ViewModel
        {
            get { return DataContext as PlayerPopupViewModel; }
        }

        public PlayerPopupPage()
        {
            InitializeComponent();
        }
    }
}
