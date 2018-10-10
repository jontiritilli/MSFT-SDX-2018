using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Controls;
namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceTransformPage : Page, INavigate
    {
        #region Private Members

        private ExperienceTransformViewModel ViewModel
        {
            get { return DataContext as ExperienceTransformViewModel; }
        }

        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Members

        public static ExperienceTransformPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceTransformPage()
        {
            InitializeComponent();
            ExperienceTransformPage.Current = this;
            rBtnRight.PopupChild = PopRight;
            this.Loaded += ExperienceTransformPage_Loaded;
        }

        private void ExperienceTransformPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceTransformPage.Current.HasLoaded = true;
            if (ExperienceTransformPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (ExperienceTransformPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceTransformPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
