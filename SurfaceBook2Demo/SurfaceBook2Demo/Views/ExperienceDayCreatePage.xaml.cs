using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;
namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayCreatePage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayCreateViewModel ViewModel
        {
            get { return DataContext as ExperienceDayCreateViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion
        #region Public Static Properties

        public static ExperienceDayCreatePage Current { get; private set; }

        #endregion


        #region Construction

        public ExperienceDayCreatePage()
        {
            InitializeComponent();
            ExperienceDayCreatePage.Current = this;
            this.LegalCompare.SetOpacity(0.0d);

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;
            rBtnRight.PopupChild = PopRight;

            this.Loaded += ExperienceDayCreatePage_Loaded;

        }

        private void ExperienceDayCreatePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceDayCreatePage.Current.HasLoaded = true;
            if (ExperienceDayCreatePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }
        #endregion

        #region Private Methods

        private void PopRight_Opened(object sender, object e)
        {
            this.LegalCompare.SetOpacity(1);
        }

        private void PopRight_Closed(object sender, object e)
        {
            this.LegalCompare.SetOpacity(0);
        }

        private void PopLeft_Opened(object sender, object e)
        {
            this.LegalCompare.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LegalCompare.SetOpacity(0);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperienceDayCreatePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceDayCreatePage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
