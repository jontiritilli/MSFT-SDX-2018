using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSleekPage : Page, INavigate
    {

        #region Private Members

        private ExperienceSleekViewModel ViewModel
        {
            get { return DataContext as ExperienceSleekViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static ExperienceSleekPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceSleekPage()
        {
            InitializeComponent();
            ExperienceSleekPage.Current = this;
            this.PopBottomLegal.SetOpacity(0.0d);
            this.rBtnBottomPerformance.PopupChild = PopBottom;
            this.rBtnLeftPerformance.PopupChild = PopLeft;
            this.rBtnTopPerformance.PopupChild = PopTop;
            this.PopBottomLegal.SetOpacity(0);
            this.Loaded += ExperienceSleekPage_Loaded;
        }

        private void ExperienceSleekPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceSleekPage.Current.HasLoaded = true;
            if (ExperienceSleekPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnTopPerformance.StartEntranceAnimation();
            rBtnTopPerformance.StartRadiateAnimation();

            rBtnLeftPerformance.StartEntranceAnimation();
            rBtnLeftPerformance.StartRadiateAnimation();

            rBtnBottomPerformance.StartEntranceAnimation();
            rBtnBottomPerformance.StartRadiateAnimation();
        }
        #endregion

        #region Private Methods

        private void PopBottom_Opened(object sender, object e)
        {
            this.PopBottomLegal.SetOpacity(1);
        }

        private void PopBottom_Closed(object sender, object e)
        {
            this.PopBottomLegal.SetOpacity(0);

        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (ExperienceSleekPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                ExperienceSleekPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);

            rBtnTopPerformance.ResetEntranceAnimation();
            rBtnTopPerformance.ResetRadiateAnimation();

            rBtnLeftPerformance.ResetEntranceAnimation();
            rBtnLeftPerformance.ResetRadiateAnimation();

            rBtnBottomPerformance.ResetEntranceAnimation();
            rBtnBottomPerformance.ResetRadiateAnimation();
        }

        #endregion
    }
}
