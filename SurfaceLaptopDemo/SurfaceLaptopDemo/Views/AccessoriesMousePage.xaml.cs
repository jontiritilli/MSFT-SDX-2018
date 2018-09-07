using System;

using SurfaceLaptopDemo.ViewModels;

using SDX.Toolkit.Controls;
using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class AccessoriesMousePage : Page, INavigate
    {
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
            AccessoriesMousePage.Current = this;
            this.AppSelectorImageAccLeft.AppSelector = this.AppSelectorAccLeft;
            this.AppSelectorAccLeft.SelectedIDChanged += SelectedIDChanged;
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
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
