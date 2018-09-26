using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;


namespace SurfaceJackDemo.Views
{
    public sealed partial class PartnerPage : Page, INavigate
    {
        #region Private Members

        private PartnerViewModel ViewModel
        {
            get { return DataContext as PartnerViewModel; }
        }

        #endregion


        #region Construction

        public PartnerPage()
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
