using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;


namespace SurfaceJackDemo.Views
{
    public sealed partial class TechPage : Page, INavigate
    {
        #region Private Members

        private TechViewModel ViewModel
        {
            get { return DataContext as TechViewModel; }
        }

        #endregion


        #region Construction

        public TechPage()
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
