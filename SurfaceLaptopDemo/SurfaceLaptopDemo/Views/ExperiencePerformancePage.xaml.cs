using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
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
            this.AppSelectorImagePerf.AppSelector = this.AppSelectorPerf;
            this.AppSelectorImageMinorPerf.AppSelector = this.AppSelectorPerf;
            this.Loaded += ExperiencePerformancePage_Loaded;
        }

        private void ExperiencePerformancePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }
        #endregion
    }
}
