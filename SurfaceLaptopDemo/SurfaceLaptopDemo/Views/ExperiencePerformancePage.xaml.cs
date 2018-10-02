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
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static ExperiencePerformancePage Current { get; private set; }

        #endregion

        #region Construction

        public ExperiencePerformancePage()
        {
            InitializeComponent();
            ExperiencePerformancePage.Current = this;
            this.AppSelectorImagePerf.AppSelector = this.AppSelectorPerf;
            this.AppSelectorImageMinorPerf.AppSelector = this.AppSelectorPerf;
            this.Loaded += ExperiencePerformancePage_Loaded;
        }

        private void ExperiencePerformancePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperiencePerformancePage.Current.HasLoaded = true;
            if (ExperiencePerformancePage.Current.HasNavigatedTo)
            {
                SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (ExperiencePerformancePage.Current.HasLoaded)
            {
                SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            }
            else
            {
                ExperiencePerformancePage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }
        #endregion
    }
}
