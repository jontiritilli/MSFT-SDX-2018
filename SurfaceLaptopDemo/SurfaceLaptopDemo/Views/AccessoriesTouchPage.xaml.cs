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

        #endregion

        #region Public Members

        public static AccessoriesTouchPage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesTouchPage()
        {
            InitializeComponent();
            PinchZoomElement.ZoomEvent += HandleZoomChanged;
        }

        #endregion

        #region private Methods

        private void HandleZoomChanged(object sender, RoutedEventArgs e)
        {
            rButtonOne.CloseTryIt();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            PinchZoomElement.ResetControl();
            rButtonOne.StartEntranceAnimation();
            rButtonOne.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            PinchZoomElement.ResetControl();
        }

        #endregion
    }
}
