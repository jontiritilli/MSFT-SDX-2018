using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Media.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceHeadphoneDemo.Services;
using SurfaceHeadphoneDemo.ViewModels;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Controls;


namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class ChoosePathPage : Page, INavigate
    {
        #region Private Constants

        private const string URI_BACKGROUND = @"ms-appx:///Assets/Backgrounds/gradient-bg.jpg";

        #endregion

        #region Public Properties

        private ChoosePathViewModel ViewModel
        {
            get { return DataContext as ChoosePathViewModel; }
        }

        #endregion

        #region Construction

        public ChoosePathPage()
        {
            InitializeComponent();

            this.Loaded += this.ChoosePathPage_Loaded;
        }

        private void ChoosePathPage_Loaded(object sender, RoutedEventArgs e)
        {
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 9, 5, Colors.CornflowerBlue);
        }

        #endregion

        #region Event Handlers

        private async void DeviceOneButton_Click(object sender, RoutedEventArgs e)
        {
            // launch deep-link uri to launch SurfaceProDemo
            Uri uriPro = new Uri(@"surfacedemo:");
            bool success = await Windows.System.Launcher.LaunchUriAsync(uriPro);

            if (success)
            {
                //telemetry event??
            }
            else
            {
                //throw exception??
            }
        }

        private void DeviceTwoButton_Click(object sender, RoutedEventArgs e)
        {
            // navigate to the next page (flipview)

            // get the locator from app.xaml
            ViewModels.ViewModelLocator Locator = Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

            // use it to get the navigation service
            NavigationServiceEx NavigationService = Locator?.NavigationService;

            // navigate to the flipview page
            NavigationService?.Navigate(typeof(FlipViewViewModel).FullName);
        }

        #endregion

        #region INavigate

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        void INavigate.NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);

            //// telemetry - FirstSwipe
            //TelemetryService.Current?.SendTelemetry("FirstSwipe", System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion
    }
}
