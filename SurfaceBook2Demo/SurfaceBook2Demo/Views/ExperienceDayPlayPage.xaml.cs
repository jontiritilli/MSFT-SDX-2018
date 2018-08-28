using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayPlayPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayPlayViewModel ViewModel
        {
            get { return DataContext as ExperienceDayPlayViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceDayPlayPage()
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
