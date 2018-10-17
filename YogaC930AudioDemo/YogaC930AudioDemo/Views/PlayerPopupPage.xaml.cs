using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class PlayerPopupPage : Page
    {
        public RoutedEventHandler CloseButton_Clicked;
        private PlayerPopupViewModel ViewModel
        {
            get { return DataContext as PlayerPopupViewModel; }
        }
        #region Public Static Properties

        public static PlayerPopupPage Current { get; private set; }

        #endregion
        public PlayerPopupPage()
        {
            InitializeComponent();
            PlayerPopupPage.Current = this;
        }

        private void btnClose_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
    }
}
