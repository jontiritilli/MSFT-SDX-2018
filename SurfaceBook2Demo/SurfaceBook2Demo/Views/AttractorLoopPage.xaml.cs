using System;
using System.Globalization;

using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SurfaceBook2Demo.Services;
using SurfaceBook2Demo.ViewModels;
using SDX.Toolkit.Controls;
using SDX.Telemetry.Services;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class AttractorLoopPage : Page
    {
        #region Private Members

        private AttractorLoopViewModel ViewModel
        {
            get { return DataContext as AttractorLoopViewModel; }
        }

        #endregion


        #region Construction

        public AttractorLoopPage()
        {
            InitializeComponent();

            // disappear the title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        #endregion


        #region Event Handlers

        private void AttractorLoopPlayer_Interacted(object sender, InteractedEventArgs args)
        {
            // get the locator from app.xaml
            ViewModels.ViewModelLocator Locator = Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

            // use it to get the navigation service
            NavigationServiceEx NavigationService = Locator?.NavigationService;

            // navigate
            NavigationService?.Navigate(typeof(FlipViewViewModel).FullName);

        }

        #endregion
    }
}
