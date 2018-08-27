using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceQuietPage : Page, INavigate
    {
        #region Private Members

        private ExperienceQuietViewModel ViewModel
        {
            get { return DataContext as ExperienceQuietViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceQuietPage()
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
