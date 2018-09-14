using System;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class ExperienceCraftedPage : Page, INavigate
    {
        #region Private Members

        private ExperienceCraftedViewModel ViewModel
        {
            get { return DataContext as ExperienceCraftedViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceCraftedPage()
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
