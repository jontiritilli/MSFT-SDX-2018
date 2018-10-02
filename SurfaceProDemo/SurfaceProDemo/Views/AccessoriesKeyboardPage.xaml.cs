using System;
using SDX.Toolkit.Controls;
using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class AccessoriesKeyboardPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesKeyboardViewModel ViewModel
        {
            get { return DataContext as AccessoriesKeyboardViewModel; }
        }

        #endregion

        #region Public Members
        public static AccessoriesKeyboardPage Current { get; private set; }
        #endregion

        #region Construction

        public AccessoriesKeyboardPage()
        {
            InitializeComponent();
            this.PopBottomLegal.SetOpacity(0.0d);
            AccessoriesKeyboardPage.Current = this;
            this.AppSelectorImageKB.AppSelector = this.AppSelectorKB;
            this.AppSelectorKB.SelectedIDChanged += SelectedIDChanged;

            rBtnTop.PopupChild = PopTop;
            rBtnBottom.PopupChild = PopBottom;
            this.Loaded += AccessoriesKeyboardPage_Loaded;


        }

        private void AccessoriesKeyboardPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
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

        public void NavigateToPage(INavigateMoveDirection moveDirection)
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
