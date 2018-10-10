using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;
namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayRelaxPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayRelaxViewModel ViewModel
        {
            get { return DataContext as ExperienceDayRelaxViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Static Properties

        public static ExperienceDayRelaxPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceDayRelaxPage()
        {
            InitializeComponent();
            ExperienceDayRelaxPage.Current = this;
            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            this.Loaded += ExperienceDayRelaxPage_Loaded;

        }

        private void ExperienceDayRelaxPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceDayRelaxPage.Current.HasLoaded = true;
            if (ExperienceDayRelaxPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }
        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }

        private void ClosePopupsOnExit()
        {
            if(null != this.PopLeft && this.PopLeft.IsOpen)
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
            if (ExperienceDayRelaxPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceDayRelaxPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
