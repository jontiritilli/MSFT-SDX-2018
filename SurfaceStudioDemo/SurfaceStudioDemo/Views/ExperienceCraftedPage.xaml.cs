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

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members
        public static ExperienceCraftedPage Current { get; private set; }
        #endregion


        #region Construction

        public ExperienceCraftedPage()
        {
            InitializeComponent();
            ExperienceCraftedPage.Current = this;
            this.rBtnLeftCrafted.PopupChild = PopLeft;
            this.rBtnRightCrafted.PopupChild = PopRight;
            this.Loaded += ExperienceCraftedPage_Loaded;
        }

        private void ExperienceCraftedPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceCraftedPage.Current.HasLoaded = true;
            if (ExperienceCraftedPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnRightCrafted.StartEntranceAnimation();
            rBtnRightCrafted.StartRadiateAnimation();

            rBtnLeftCrafted.StartEntranceAnimation();
            rBtnLeftCrafted.StartRadiateAnimation();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (ExperienceCraftedPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceCraftedPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            rBtnRightCrafted.ResetEntranceAnimation();
            rBtnRightCrafted.ResetRadiateAnimation();

            rBtnLeftCrafted.ResetEntranceAnimation();
            rBtnLeftCrafted.ResetRadiateAnimation();
        }

        #endregion
    }
}
