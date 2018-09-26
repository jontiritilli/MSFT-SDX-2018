using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;


namespace SurfaceJackDemo.Views
{
    public sealed partial class SpecsPage : Page, INavigate
    {
        #region Private Members

        private SpecsViewModel ViewModel
        {
            get { return DataContext as SpecsViewModel; }
        }

        #endregion


        #region Construction

        public SpecsPage()
        {
            InitializeComponent();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
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
