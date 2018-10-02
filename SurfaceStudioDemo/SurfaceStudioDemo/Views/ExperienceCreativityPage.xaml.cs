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

        #endregion

        #region Construction

        public ExperienceCreativityPage()
        {
            InitializeComponent();

            this.Loaded += ExperienceCreativityPage_Loaded;
        }

        private void ExperienceCreativityPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
            AnimationHelper.PerformFadeIn(this.backgroundImage2, 1100d, 3000d);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
            AnimationHelper.PerformFadeOut(this.backgroundImage2, 0d, 0d);
        }

        #endregion
    }
}
