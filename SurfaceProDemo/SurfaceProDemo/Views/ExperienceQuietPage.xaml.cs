using System;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

using SurfaceProDemo.ViewModels;

namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceQuietPage : Page, INavigate
    {
        #region Private Members

        private ExperienceQuietViewModel ViewModel
        {
            get { return DataContext as ExperienceQuietViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Members

        public static ExperienceQuietPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceQuietPage()
        {
            InitializeComponent();
            ExperienceQuietPage.Current = this;
            this.LeftLegal.SetOpacity(0);

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;
            rBtnRight.PopupChild = PopRight;
            this.LeftLegal.SetOpacity(0);
            this.Loaded += ExperienceQuietPage_Loaded;
        }

        private void ExperienceQuietPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceQuietPage.Current.HasLoaded = true;
            if (ExperienceQuietPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            this.rBtnLeft.StartEntranceAnimation();
            this.rBtnLeft.StartRadiateAnimation();

            this.rBtnTop.StartEntranceAnimation();
            this.rBtnTop.StartRadiateAnimation();

            this.rBtnRight.StartEntranceAnimation();
            this.rBtnRight.StartRadiateAnimation();
        }
        #endregion

        #region Private Methods

        private void PopLeft_Opened(object sender, object e)
        {
            this.LeftLegal.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LeftLegal.SetOpacity(0);
        }

        private void ClosePopupsOnExit()
        {
            if (null != this.PopTop && this.PopTop.IsOpen)
            {
                this.PopTop.IsOpen = false;
            }
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
            if (ExperienceQuietPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceQuietPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            this.rBtnTop.ResetEntranceAnimation();
            this.rBtnTop.ResetRadiateAnimation();

            this.rBtnLeft.ResetEntranceAnimation();
            this.rBtnLeft.ResetRadiateAnimation();

            this.rBtnRight.ResetEntranceAnimation();
            this.rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
