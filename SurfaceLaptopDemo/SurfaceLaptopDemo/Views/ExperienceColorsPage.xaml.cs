using System;

using SurfaceLaptopDemo.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
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
            this.AppSelectorExpColors.SelectedIDChanged += SelectedIDChanged;
        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;
            this.PageHeader.HeadlineStyle = (appSelector.SelectedID == 0) ? TextStyles.PageHeadline : TextStyles.PageHeadlineDark;
            this.PageHeader.LedeStyle = (appSelector.SelectedID == 0) ? TextStyles.PageLede : TextStyles.PageLedeDark;
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
