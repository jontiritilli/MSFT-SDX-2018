using System;
using SDX.Toolkit.Controls;
using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceProDemo.Views
{
    public sealed partial class AccessoriesKeyboardPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesKeyboardViewModel ViewModel
        {
            get { return DataContext as AccessoriesKeyboardViewModel; }
        }
        double _canvasWidth = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasWidth);
        double _canvasHeight = StyleHelper.GetApplicationDouble(LayoutSizes.CanvasHeight);
        #endregion

        #region Public Members
        public static AccessoriesKeyboardPage Current { get; private set; }
        #endregion

        #region Construction

        public AccessoriesKeyboardPage()
        {
            InitializeComponent();
            AccessoriesKeyboardPage.Current = this;
            this.AppSelectorImageKB.AppSelector = this.AppSelectorKB;
            this.AppSelectorKB.SelectedIDChanged += SelectedIDChanged;

            Canvas.SetTop(rBtnTop, _canvasHeight * .20);
            Canvas.SetLeft(rBtnTop, _canvasWidth * .55);

            Canvas.SetTop(rBtnBottom, _canvasHeight * .80);
            Canvas.SetLeft(rBtnBottom, _canvasWidth * .55);

            rBtnTop.PopupChild = PopTop;
            PopTop.VerticalOffset = _canvasHeight * .20;
            PopTop.HorizontalOffset = _canvasWidth * .55;

            rBtnBottom.PopupChild = PopBottom;
            PopBottom.VerticalOffset = _canvasHeight * .80;
            PopBottom.HorizontalOffset = _canvasWidth * .55;


        }

        public void SelectedIDChanged(object sender, EventArgs e) {
            //capture selected changed event so we can pass the id to the other page and force link            
            AppSelector appSelector = (AppSelector)sender;
            AccessoriesMousePage.Current?.SetID(appSelector.SelectedID);
        }
        public void SetID(int ID)
        {
            // check if this.appselector isnull and do not set again if already matching
            if (this.AppSelectorKB != null && this.AppSelectorKB.SelectedID != ID)
            {
                this.AppSelectorKB.SelectedID = ID;
            }
        }
        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();

            rBtnBottom.StartEntranceAnimation();
            rBtnBottom.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnTop.StartEntranceAnimation();
            rBtnTop.StartRadiateAnimation();

            rBtnBottom.StartEntranceAnimation();
            rBtnBottom.StartRadiateAnimation();
        }

        #endregion
    }
}
