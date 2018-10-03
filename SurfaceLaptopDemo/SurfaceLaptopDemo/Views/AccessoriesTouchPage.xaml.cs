using System;

using SurfaceLaptopDemo.ViewModels;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesTouchPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesTouchViewModel ViewModel
        {
            get { return DataContext as AccessoriesTouchViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static AccessoriesTouchPage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesTouchPage()
        {
            InitializeComponent();
            AccessoriesTouchPage.Current = this;
            PinchZoomElement.ZoomEvent += HandleZoomChanged;
            this.Loaded += AccessoriesTouchPage_Loaded;
        }

        private void AccessoriesTouchPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            AccessoriesTouchPage.Current.HasLoaded = true;
            if (AccessoriesTouchPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }
        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            PinchZoomElement.ResetControl();
            rButtonOne.StartEntranceAnimation();
            rButtonOne.StartRadiateAnimation();
        }
        #endregion

        #region private Methods

        private void HandleZoomChanged(object sender, RoutedEventArgs e)
        {
            rButtonOne.FadeOutButton();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (AccessoriesTouchPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                AccessoriesTouchPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            PinchZoomElement.ResetControl();
            rButtonOne.ResetEntranceAnimation();
            rButtonOne.ResetRadiateAnimation();
        }

        #endregion
    }
}
