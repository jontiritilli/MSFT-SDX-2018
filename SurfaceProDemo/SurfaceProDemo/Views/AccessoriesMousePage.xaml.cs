using System;
using SDX.Toolkit.Controls;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;

namespace SurfaceProDemo.Views
{
    public sealed partial class AccessoriesMousePage : Page, INavigate
    {
        #region Private Members

        private AccessoriesMouseViewModel ViewModel
        {
            get { return DataContext as AccessoriesMouseViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion


        #region Public Members
        public static AccessoriesMousePage Current { get; private set; }
        #endregion


        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();
            AccessoriesMousePage.Current = this;
            this.PopLeftLegal.SetOpacity(0.0d);
            this.AppSelectorImageMouse.AppSelector = this.AppSelectorMouse;
            this.AppSelectorMouse.SelectedIDChanged += SelectedIDChanged;

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;
            this.Loaded += AccessoriesMousePage_Loaded;
        }

        private void AccessoriesMousePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            AccessoriesMousePage.Current.HasLoaded = true;
            if (AccessoriesMousePage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link
            //me parent accessorieskeyboardpage
            AppSelector appSelector = (AppSelector)sender;
            AccessoriesKeyboardPage.Current?.SetID(appSelector.SelectedID);
        }
        public void SetID(int ID)
        {
            // check if this.appselector isnull and do not set again if already matching
            if (this.AppSelectorMouse != null && this.AppSelectorMouse.SelectedID != ID)
            {
                this.AppSelectorMouse.SelectedID = ID;
            }
        }
        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (AccessoriesMousePage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                AccessoriesMousePage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        #endregion
    }
}
