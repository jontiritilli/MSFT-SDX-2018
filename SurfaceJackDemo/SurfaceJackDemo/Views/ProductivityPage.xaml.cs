using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;


namespace SurfaceJackDemo.Views
{
    public sealed partial class ProductivityPage : Page, INavigate
    {
        #region Private Members

        private ProductivityViewModel ViewModel
        {
            get { return DataContext as ProductivityViewModel; }
        }

        #endregion


        #region Construction

        public ProductivityPage()
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
