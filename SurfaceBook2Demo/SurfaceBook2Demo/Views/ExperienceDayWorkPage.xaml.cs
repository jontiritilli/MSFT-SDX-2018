using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayWorkPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayWorkViewModel ViewModel
        {
            get { return DataContext as ExperienceDayWorkViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceDayWorkPage()
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
