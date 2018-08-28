using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayCreatePage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayCreateViewModel ViewModel
        {
            get { return DataContext as ExperienceDayCreateViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceDayCreatePage()
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
