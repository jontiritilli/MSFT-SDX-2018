using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceProDemo.Views
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

        #region Public Members
        public static ExperienceIntroPage Current { get; private set; }
        #endregion

        #region Construction

        public ExperienceIntroPage()
        {
            InitializeComponent();
            ExperienceIntroPage.Current = this;
            this.LeftLegal.SetOpacity(0.0d);
            this.TopLegal.SetOpacity(0.0d);
            rBtnRight.PopupChild = PopRight;
            //rBtnTop.PopupChild = PopTop;
            rBtnLeft.PopupChild = PopLeft;
            this.TopLegal.SetOpacity(0);
            this.TopLegal.SetOpacity(0);
            if (Services.ConfigurationService.Current.GetIsBlackSchemeEnabled())
            {
                rBtnRight.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            this.Loaded += ExperienceIntroPage_Loaded;
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

        #endregion

        #region Private Methods

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();

            //rBtnTop.StartEntranceAnimation();
            //rBtnTop.StartRadiateAnimation();
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LeftLegal.SetOpacity(0);
        }

        private void PopLeft_Opened(object sender, object e)
        {
            this.LeftLegal.SetOpacity(1);
        }

        //private void PopTop_Opened(object sender, object e)
        //{
        //    this.TopLegal.SetOpacity(1);
        //}

        //private void PopTop_Closed(object sender, object e)
        //{
        //    this.TopLegal.SetOpacity(0);
        //}

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

            //rBtnTop.ResetEntranceAnimation();
            //rBtnTop.ResetRadiateAnimation();
        }

        #endregion

    }
}
