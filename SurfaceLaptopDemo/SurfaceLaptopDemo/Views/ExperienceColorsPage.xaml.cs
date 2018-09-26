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
            this.rBtnLeft.PopupChild = this.PopLeft;
        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;

            // change headline and lead style if the black option is chosen
            this.PageHeader.HeadlineStyle = (appSelector.SelectedID == 1) ? TextStyles.PageHeadline : TextStyles.PageHeadlineDark;
            this.PageHeader.LedeStyle = (appSelector.SelectedID == 1) ? TextStyles.PageLede : TextStyles.PageLedeDark;

            // show the radiating button if the black option is chosen
            this.rBtnLeft.Opacity = (appSelector.SelectedID == 1) ? 1 : 0;
        }
        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            this.rBtnLeft.StartEntranceAnimation();
            this.rBtnLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rBtnLeft.ResetEntranceAnimation();
            this.rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
