using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        private void CloseButtonImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
    }
}
