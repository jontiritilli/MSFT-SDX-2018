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

        #endregion


        #region Construction

        public ExperienceDayPlayPage()
        {
            InitializeComponent();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);                        
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);            
        }

        #endregion
    }
}
