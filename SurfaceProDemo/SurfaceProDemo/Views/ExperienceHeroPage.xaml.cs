using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using SurfaceProDemo.Services;
using Windows.UI.Xaml;

namespace SurfaceProDemo.Views
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
            if (ConfigurationService.Current.GetIsBlackSchemeEnabled()) {
                this.PageHero.Style = (Style)Application.Current.Resources["HeroDark"];
            }
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        #endregion
    }
}
