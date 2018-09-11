using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceInnovationPage : Page, INavigate
    {
        #region Private Members

        private ExperienceInnovationViewModel ViewModel
        {
            get { return DataContext as ExperienceInnovationViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceInnovationPage()
        {
            InitializeComponent();
        }

        #endregion


        #region INavigate Interface


        public void NavigateToPage()
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
