using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;


namespace SurfaceJackDemo.Views
{
    public sealed partial class DesignPage : Page, INavigate
    {
        #region Private Members

        private DesignViewModel ViewModel
        {
            get { return DataContext as DesignViewModel; }
        }

        #endregion


        #region Construction

        public DesignPage()
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
