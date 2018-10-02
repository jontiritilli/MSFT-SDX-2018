using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

namespace SurfaceBook2Demo.Views
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
        #region Public Static Properties

        public static AccessoriesDialPage Current { get; private set; }

        #endregion


        #region Construction

        public AccessoriesDialPage()
        {
            InitializeComponent();
            AccessoriesDialPage.Current = this;
            this.LegalDial.SetOpacity(0.0d);
            this.LegalPen.SetOpacity(0.0d);

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;

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
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();
        }
        #endregion

        #region Event Handlers
        private void PopDial_Opened(object sender, object e)
        {
            this.LegalDial.SetOpacity(1);
        }

        private void PopPen_Opened(object sender, object e)
        {
            this.LegalPen.SetOpacity(1);
        }

        private void PopDial_Closed(object sender, object e)
        {
            this.LegalDial.SetOpacity(0);
        }

        private void PopPen_Closed(object sender, object e)
        {
            this.LegalPen.SetOpacity(0);
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
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnTop.ResetEntranceAnimation();
            rBtnTop.ResetRadiateAnimation();

            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();
        }

        #endregion
    }
}
