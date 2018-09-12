using System;

using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesDialPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesDialViewModel ViewModel
        {
            get { return DataContext as AccessoriesDialViewModel; }
        }
        public RoutedEventHandler CloseButton_Clicked;

        #region Public Static Properties

        public static AccessoriesDialPage Current { get; private set; }

        #endregion

        #endregion


        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();
            AccessoriesDialPage.Current = this;
            this.AppSelectorImageKB.AppSelector = this.AppSelectorKB;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
        #endregion
    }
}
