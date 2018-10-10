using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceInnovationPage : Page, INavigate
    {
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        #region Private Members

        private ExperienceInnovationViewModel ViewModel
        {
            get { return DataContext as ExperienceInnovationViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static ExperienceInnovationPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceInnovationPage()
        {
            InitializeComponent();
            ExperienceInnovationPage.Current = this;
            this.rBtnTryItInnovation.PopupChild = PopLeft;
            this.rBtnTopInnovation.PopupChild = PopTop;
            this.rBtnRightInnovation.PopupChild = PopRight;
            this.Loaded += ExperienceInnovationPage_Loaded;
        }

        private void ExperienceInnovationPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceInnovationPage.Current.HasLoaded = true;
            if (ExperienceInnovationPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTopInnovation.StartEntranceAnimation();
            rBtnTopInnovation.StartRadiateAnimation();

            rBtnRightInnovation.StartEntranceAnimation();
            rBtnRightInnovation.StartRadiateAnimation();

            rBtnTryItInnovation.StartEntranceAnimation();
            rBtnTryItInnovation.StartRadiateAnimation();
        }
        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != this.PopLeft && this.PopLeft.IsOpen)
            {
                this.PopLeft.IsOpen = false;
            }

            if (null != this.PopTop && this.PopTop.IsOpen)
            {
                this.PopTop.IsOpen = false;
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
            if (ExperienceInnovationPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceInnovationPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnTopInnovation.ResetEntranceAnimation();
            rBtnTopInnovation.ResetRadiateAnimation();

            rBtnRightInnovation.ResetEntranceAnimation();
            rBtnRightInnovation.ResetRadiateAnimation();

            rBtnTryItInnovation.ResetEntranceAnimation();
            rBtnTryItInnovation.ResetRadiateAnimation();
        }

        #endregion
    }
}
