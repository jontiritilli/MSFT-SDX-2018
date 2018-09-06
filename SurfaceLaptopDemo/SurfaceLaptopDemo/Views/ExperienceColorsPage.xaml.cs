using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceColorsPage : Page, INavigate
    {
        #region Private Members

        private ExperienceColorsViewModel ViewModel
        {
            get { return DataContext as ExperienceColorsViewModel; }
        }

        #endregion

        #region Construction

        public ExperienceColorsPage()
        {
            InitializeComponent();
            this.AppSelectorImageExpColors.AppSelector = this.AppSelectorExpColors;
            ViewModel.BackgroundUri = ViewModel.lifeStyleColorSelectorImageURIs[AppSelectorImageExpColors.SelectedID].URI;
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
