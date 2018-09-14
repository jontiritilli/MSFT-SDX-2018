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
