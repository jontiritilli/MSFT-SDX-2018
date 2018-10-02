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

        #endregion

        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();
            this.PopDialLegal.SetOpacity(0.0d);
            this.rBtnRightAccLeft.PopupChild = this.PopRight;
            this.Loaded += AccessoriesDialPage_Loaded;
        }

        private void AccessoriesDialPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        #endregion

        #region Private Methods

        private void PopDial_Opened(object sender, object e)
        {
            this.PopDialLegal.SetOpacity(1);
        }
        
        private void PopDial_Closed(object sender, object e)
        {
            this.PopDialLegal.SetOpacity(0);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnRightAccLeft.StartEntranceAnimation();
            rBtnRightAccLeft.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            rBtnRightAccLeft.ResetEntranceAnimation();
            rBtnRightAccLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
