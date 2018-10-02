using System;

using SurfaceBook2Demo.ViewModels;
using SDX.Toolkit.Helpers;

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

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion
        #region Public Static Properties

        public static ExperienceDayPlayPage Current { get; private set; }

        #endregion


        #region Construction

        public ExperienceDayPlayPage()
        {
            InitializeComponent();
            ExperienceDayPlayPage.Current = this;
            this.Loaded += ExperienceDayPlayPage_Loaded;
        }

        private void ExperienceDayPlayPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceDayPlayPage.Current.HasLoaded = true;
            if (ExperienceDayPlayPage.Current.HasNavigatedTo)
            {
                SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            }
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperienceDayPlayPage.Current.HasLoaded)
            {
                SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            }
            else
            {
                ExperienceDayPlayPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);            
        }

        #endregion
    }
}
