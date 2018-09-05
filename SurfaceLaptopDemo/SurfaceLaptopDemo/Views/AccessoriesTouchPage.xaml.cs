using System;

using SurfaceLaptopDemo.ViewModels;

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

        #region Construction


        public AccessoriesTouchPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PinchToZoomElement.ZoomMode = ZoomMode.Enabled;
            PinchToZoomElement.ChangeView(0, 0, 3);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        #endregion
    }
}
