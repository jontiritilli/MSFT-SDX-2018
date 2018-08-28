using System;
using System.Globalization;

using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SurfaceLaptopDemo.Services;
using SurfaceLaptopDemo.ViewModels;
using SDX.Toolkit.Controls;
using SDX.Telemetry.Services;


namespace SurfaceLaptopDemo.Views
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

            // telemetry - EndApplication
            TelemetryService.Current?.SendTelemetry("EndApplication", System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);

        }

        #endregion


        #region Event Handlers

        private void AttractorLoopPlayer_Interacted(object sender, InteractedEventArgs args)
        {
            // send telemetry - StartApplication
            TelemetryService.Current?.SendTelemetry("StartApplication", System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);

            // send telemetry - StartsApplicationType
            TelemetryService.Current?.SendTelemetry("StartsApplicationType", args.InteractionType.ToString(), true, 0);

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
