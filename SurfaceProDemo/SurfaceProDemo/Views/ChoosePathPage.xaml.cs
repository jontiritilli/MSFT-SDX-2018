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

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Helpers;
using SurfaceProDemo.Services;
using SDX.Toolkit.Controls;

namespace SurfaceProDemo.Views
{
    public sealed partial class ChoosePathPage : Page
    {
        #region Private Constants

        private const string URI_BACKGROUND = @"ms-appx:///Assets/Backgrounds/gradient-bg.jpg";

        #endregion


        #region Private Members for UI

        private Grid _page;
        private Button _buttonSurfacePro;
        private Button _buttonSurfaceJack;


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

            this.Content = RenderUI(ViewModel);
        }

        #endregion


        #region Event Handlers

        private void _buttonSurfacePro_Click(object sender, RoutedEventArgs e)
        {
            // navigate to the next page (flipview)

            // get the locator from app.xaml
            ViewModels.ViewModelLocator Locator = Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

            // use it to get the navigation service
            NavigationServiceEx NavigationService = Locator?.NavigationService;

            // navigate to the flipview page
            NavigationService?.Navigate(typeof(FlipViewViewModel).FullName);
        }

        private void _buttonSurfaceJack_Click(object sender, RoutedEventArgs e)
        {
            // launch deep-link uri to launch SurfaceJackDemo
        }

        #endregion


        #region UI

        // Intro Page
        private Grid RenderUI(ChoosePathViewModel viewModel)
        {
            // create the grid
            _page = new Grid()
            {
                Name = "ChoosePathPage",
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            _page.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(WindowHelper.TOP_MARGIN) });
            _page.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // header
            _page.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(WindowHelper.SPACE_BETWEEN_ELEMENTS) });
            _page.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) }); // content
            _page.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(WindowHelper.BOTTOM_MARGIN) });

            _page.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(WindowHelper.LEFT_MARGIN) });
            _page.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
            _page.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
            _page.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(WindowHelper.RIGHT_MARGIN) });

            // test only
            //TestHelper.AddGridCellBorders(_page, 5, 4, Colors.Purple);

            // get the button style
            Style buttonStyle = StyleHelper.GetApplicationStyle("CallToActionButton");

            // create Surface Pro button
            _buttonSurfacePro = new Button()
            {
                Name = "SurfaceProButton",
                Content = new TextBlockEx() { Text = viewModel.ChooseSurfacePro, TextAlignment = TextAlignment.Center, TextStyle = TextStyles.PageLedeDark },
            };
            if (null != buttonStyle) { _buttonSurfacePro.Style = buttonStyle; }
            Grid.SetRow(_buttonSurfacePro, 3);
            Grid.SetColumn(_buttonSurfacePro, 1);
            _page.Children.Add(_buttonSurfacePro);

            // button event handler
            _buttonSurfacePro.Click += this._buttonSurfacePro_Click;

            // create Surface Pro button
            _buttonSurfaceJack = new Button()
            {
                Name = "SurfaceJackButton",
                Content = new TextBlockEx() { Text = viewModel.ChooseSurfaceJack, TextAlignment = TextAlignment.Center, TextStyle = TextStyles.PageLedeDark },
            };
            if (null != buttonStyle) { _buttonSurfaceJack.Style = buttonStyle; }
            
            //StyleHelper.SetFontCharacteristics(_buttonSurfaceJack, ControlStyles.ButtonText);
            Grid.SetRow(_buttonSurfaceJack, 3);
            Grid.SetColumn(_buttonSurfaceJack, 2);
            _page.Children.Add(_buttonSurfaceJack);

            // button event handler
            _buttonSurfaceJack.Click += this._buttonSurfaceJack_Click;

            return _page;
        }

        private static void OnNavigateToPage()
        {

        }

        private static void OnNavigateFromPage()
        {

            //// telemetry - FirstSwipe
            //TelemetryService.Current?.SendTelemetry("FirstSwipe", System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        #endregion
    }
}
