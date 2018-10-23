using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class HingeDesignPopupPage : Page
    {
        public RoutedEventHandler CloseButton_Clicked;
        private HingeDesignPopupViewModel ViewModel
        {
            get { return DataContext as HingeDesignPopupViewModel; }
        }
        #region Public Static Properties

        public static HingeDesignPopupPage Current { get; private set; }

        #endregion
        public HingeDesignPopupPage()
        {
            InitializeComponent();
            HingeDesignPopupPage.Current = this;
        }

        private void btnClose_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
    }
}
