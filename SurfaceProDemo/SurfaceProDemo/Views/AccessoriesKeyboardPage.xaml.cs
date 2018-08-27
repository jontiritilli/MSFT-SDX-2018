using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class AccessoriesKeyboardPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesKeyboardViewModel ViewModel
        {
            get { return DataContext as AccessoriesKeyboardViewModel; }
        }

        #endregion


        #region Construction

        public AccessoriesKeyboardPage()
        {
            InitializeComponent();
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

        #endregion
    }
}
