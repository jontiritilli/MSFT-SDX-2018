using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesMousePage : Page, INavigate
    {

        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);

        #region Private Members

        private AccessoriesMouseViewModel ViewModel
        {
            get { return DataContext as AccessoriesMouseViewModel; }
        }

        #endregion

        #region Public Members

        public static AccessoriesMousePage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesMousePage()
        {
            InitializeComponent();

            // connect the accessories page color selections
            AccessoriesMousePage.Current = this;
            this.AppSelectorImageAccLeft.AppSelector = this.AppSelectorAccLeft;
            this.AppSelectorAccLeft.SelectedIDChanged += SelectedIDChanged;

            this.rBtnLeftMouse.PopupChild = PopLeft;
            this.rBtnRightMouse.PopupChild = PopRight;

        }

        public void SelectedIDChanged(object sender, EventArgs e)
        {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;
            AccessoriesPenPage.Current?.SetID(appSelector.SelectedID);
        }
        public void SetID(int ID)
        {
            // check if this.appselector isnull and do not set again if already matching
            if (this.AppSelectorAccLeft != null && this.AppSelectorAccLeft.SelectedID != ID)
            {
                this.AppSelectorAccLeft.SelectedID = ID;
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);

            rBtnLeftMouse.StartEntranceAnimation();
            rBtnLeftMouse.StartRadiateAnimation();

            rBtnCenterMouse.StartEntranceAnimation();
            rBtnCenterMouse.StartRadiateAnimation();

            rBtnRightMouse.StartEntranceAnimation();
            rBtnRightMouse.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);

            rBtnLeftMouse.ResetEntranceAnimation();
            rBtnLeftMouse.ResetRadiateAnimation();

            rBtnCenterMouse.ResetEntranceAnimation();
            rBtnCenterMouse.ResetRadiateAnimation();

            rBtnRightMouse.ResetEntranceAnimation();
            rBtnRightMouse.ResetRadiateAnimation();
        }

        #endregion
    }
}
