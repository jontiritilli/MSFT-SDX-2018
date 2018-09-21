using System;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperienceHeroPage : Page, INavigate
    {
        #region Private Members

        private ExperienceHeroViewModel ViewModel
        {
            get { return DataContext as ExperienceHeroViewModel; }
        }

        #endregion

        #region Construction

        public ExperienceHeroPage()
        {
            InitializeComponent();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
