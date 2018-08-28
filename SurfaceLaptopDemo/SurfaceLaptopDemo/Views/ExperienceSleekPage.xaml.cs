using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSleekPage : Page, INavigate
    {
        #region Private Members

        private ExperienceSleekViewModel ViewModel
        {
            get { return DataContext as ExperienceSleekViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceSleekPage()
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
