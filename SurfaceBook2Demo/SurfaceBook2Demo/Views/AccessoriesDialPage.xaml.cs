using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Controls;


namespace SurfaceBook2Demo.Views
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
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            this.RadiatingButtonPen.StartEntranceAnimation();
            this.RadiatingButtonPen.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        #endregion
    }
}
