using System;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperienceCreativityPage : Page, INavigate
    {
        #region Private Members

        private ExperienceCreativityViewModel ViewModel
        {
            get { return DataContext as ExperienceCreativityViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members
        public static ExperienceCreativityPage Current { get; private set; }
        #endregion

        #region Construction

        public ExperienceCreativityPage()
        {
            InitializeComponent();
            ExperienceCreativityPage.Current = this;
            this.Loaded += ExperienceCreativityPage_Loaded;
        }

        private void ExperienceCreativityPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceCreativityPage.Current.HasLoaded = true;
            if (ExperienceCreativityPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
            AnimationHelper.PerformFadeIn(this.backgroundImage2, 1100d, 3000d);
        }
        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (ExperienceCreativityPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceCreativityPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
            AnimationHelper.PerformFadeOut(this.backgroundImage2, 0d, 0d);
        }

        #endregion
    }
}
