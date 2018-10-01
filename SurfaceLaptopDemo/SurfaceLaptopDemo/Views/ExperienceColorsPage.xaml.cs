using System;

using SurfaceLaptopDemo.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml.Controls;
using SurfaceLaptopDemo.Services;

namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceColorsPage : Page, INavigate
    {
        #region Private Members
        private bool isBlackEnabled;
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
            isBlackEnabled = ConfigurationService.Current.GetIsBlackSchemeEnabled();
        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;

            if (isBlackEnabled && appSelector.SelectedID == 1)
            {
                // change headline and lead style if the black option is chosen
                this.PageHeader.HeadlineStyle = TextStyles.PageHeadline;
                this.PageHeader.LedeStyle = TextStyles.PageLede;
                this.PageHeader.SetOpacity(1d);

                // show the radiating button if the black option is chosen
                this.rBtnLeft.Opacity = 1d;
            }
            else
            {
                this.PageHeader.HeadlineStyle = TextStyles.PageHeadlineDark;
                this.PageHeader.LedeStyle = TextStyles.PageLedeDark;
                this.PageHeader.SetOpacity(1d);

                // show the radiating button if the black option is chosen
                this.rBtnLeft.Opacity = 0.0d;
            }

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
