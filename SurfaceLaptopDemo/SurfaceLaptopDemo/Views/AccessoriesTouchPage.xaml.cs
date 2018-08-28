using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesTouchPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesTouchViewModel ViewModel
        {
            get { return DataContext as AccessoriesTouchViewModel; }
        }

        #endregion


        #region Construction

        public AccessoriesTouchPage()
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
