using System;

using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesTryItPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesTryItViewModel ViewModel
        {
            get { return DataContext as AccessoriesTryItViewModel; }
        }

        #endregion


        #region Construction

        public AccessoriesTryItPage()
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
