using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSpeakersPage : Page, INavigate
    {
        #region Private Members

        private ExperienceSpeakersViewModel ViewModel
        {
            get { return DataContext as ExperienceSpeakersViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceSpeakersPage()
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
