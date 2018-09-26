using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class BestOfMicrosoftPage : Page, INavigate
    {
        #region Private Members

        private BestOfMicrosoftViewModel ViewModel
        {
            get { return DataContext as BestOfMicrosoftViewModel; }
        }

        #endregion


        #region Construction

        public BestOfMicrosoftPage()
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
