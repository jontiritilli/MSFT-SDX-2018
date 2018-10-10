using System;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceIntroPage : Page, INavigate
    {
        #region Private Members

        private ExperienceIntroViewModel ViewModel
        {
            get { return DataContext as ExperienceIntroViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Static Properties

        public static ExperienceIntroPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceIntroPage()
        {
            InitializeComponent();
            ExperienceIntroPage.Current = this;
            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            this.LegalCompare.SetOpacity(0);
            this.LegalPixelSense.SetOpacity(0);

            this.Loaded += this.ExperienceIntroPage_Loaded;
        }

        private void ExperienceIntroPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceIntroPage.Current.HasLoaded = true;
            if (ExperienceIntroPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        #endregion

        #region Event Handlers

        private void PopLeft_Opened(object sender, object e)
        {
            this.LegalPixelSense.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LegalPixelSense.SetOpacity(0);
        }

        private void PopRight_Opened(object sender, object e)
        {
            this.LegalCompare.SetOpacity(1);

        }

        private void PopRight_Closed(object sender, object e)
        {
            this.LegalCompare.SetOpacity(0);

        }

        private void ClosePopupsOnExit()
        {

            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }

            if (null != this.PopRight && this.PopRight.IsOpen)
            {
                this.PopRight.IsOpen = false;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperienceIntroPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceIntroPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion

    }
}
