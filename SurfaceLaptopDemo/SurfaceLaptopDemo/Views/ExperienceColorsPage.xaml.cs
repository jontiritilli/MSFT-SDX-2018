using System;

using SurfaceLaptopDemo.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;
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

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static ExperienceColorsPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceColorsPage()
        {
            InitializeComponent();
            ExperienceColorsPage.Current = this;
            this.AppSelectorImageExpColors.AppSelector = this.AppSelectorExpColors;
            ViewModel.BackgroundUri = ViewModel.lifeStyleColorSelectorImageURIs[AppSelectorImageExpColors.SelectedID].URI;
            this.AppSelectorExpColors.SelectedIDChanged += SelectedIDChanged;
            this.rBtnLeft.PopupChild = this.PopLeft;
            isBlackEnabled = ConfigurationService.Current.GetIsBlackSchemeEnabled();
            this.Loaded += ExperienceColorsPage_Loaded;
        }

        private void ExperienceColorsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceColorsPage.Current.HasLoaded = true;
            if (ExperienceColorsPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            this.rBtnLeft.Visibility = Visibility.Collapsed;
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
                this.rBtnLeft.Visibility = Visibility.Visible;
                this.rBtnLeft.StartEntranceAnimation();
                this.rBtnLeft.StartRadiateAnimation();
            }
            else
            {
                this.PageHeader.HeadlineStyle = TextStyles.PageHeadlineDark;
                this.PageHeader.LedeStyle = TextStyles.PageLedeDark;
                this.PageHeader.SetOpacity(1d);

                // show the radiating button if the black option is chosen
                this.rBtnLeft.Visibility = Visibility.Collapsed;
                this.rBtnLeft.ResetEntranceAnimation();
                this.rBtnLeft.ResetRadiateAnimation();
            }

        }

        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (ExperienceColorsPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceColorsPage.Current.HasNavigatedTo = true;
            }


        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            this.rBtnLeft.ResetEntranceAnimation();
            this.rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
