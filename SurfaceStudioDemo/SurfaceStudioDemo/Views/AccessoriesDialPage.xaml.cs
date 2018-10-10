using System;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesDialPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesDialViewModel ViewModel
        {
            get { return DataContext as AccessoriesDialViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Members

        public static AccessoriesDialPage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();
            AccessoriesDialPage.Current = this;
            this.PopDialLegal.SetOpacity(0.0d);
            this.rBtnRightAccLeft.PopupChild = this.PopRight;
            this.Loaded += AccessoriesDialPage_Loaded;
        }

        private void AccessoriesDialPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            AccessoriesDialPage.Current.HasLoaded = true;
            if (AccessoriesDialPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnRightAccLeft.StartEntranceAnimation();
            rBtnRightAccLeft.StartRadiateAnimation();
        }

        #endregion

        #region Private Methods

        private void ClosePopupsOnExit()
        {
            if (null != this.PopRight && this.PopRight.IsOpen)
            {
                this.PopRight.IsOpen = false;
            }
        }

        #endregion

        #region Private Methods

        private void PopDial_Opened(object sender, object e)
        {
            this.PopDialPlayer.GetPopupChildPlayer().StartPlayer();
            this.PopDialLegal.SetOpacity(1);
        }
        
        private void PopDial_Closed(object sender, object e)
        {
            this.PopDialLegal.SetOpacity(0);
            this.PopDialPlayer.GetPopupChildPlayer().ResetPlayer();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (AccessoriesDialPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                AccessoriesDialPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();

            rBtnRightAccLeft.ResetEntranceAnimation();
            rBtnRightAccLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
