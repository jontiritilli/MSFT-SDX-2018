using System;

using SurfaceBook2Demo.ViewModels;
using SDX.Toolkit.Helpers;

using Windows.UI.Xaml;
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

        private void ExperienceDayPlayPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceDayPlayPage.Current.HasLoaded = true;
            if (ExperienceDayPlayPage.Current.HasNavigatedTo)
            {
                AnimationHelper.PerformPageEntranceAnimation(this);
                this.ForzaPlayer.StartPlayer();
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperienceDayPlayPage.Current.HasLoaded)
            {
                AnimationHelper.PerformPageEntranceAnimation(this);
                this.ForzaPlayer.StartPlayer();
            }
            else
            {
                ExperienceDayPlayPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);
            this.ForzaPlayer.ResetPlayer(1000);
        }

        #endregion
    }
}
