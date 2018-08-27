using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class ExperiencePerformancePage : Page, INavigate
    {
        #region Private Members

        private ExperiencePerformanceViewModel ViewModel
        {
            get { return DataContext as ExperiencePerformanceViewModel; }
        }

        #endregion


        #region Construction

        public ExperiencePerformancePage()
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
