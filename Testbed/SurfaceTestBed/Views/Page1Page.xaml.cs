using System;
using SurfaceTestBed.ViewModels;
using SDX.Toolkit.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using SDX.Toolkit.Controls;

namespace SurfaceTestBed.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1Page : Page
    {

        private RadialController _dialController;
        private SurfaceDial _dial;
        private RadialControllerMenuItem _screenColorMenuItem;

        private Page1ViewModel ViewModel
        {
            get { return DataContext as Page1ViewModel; }
        }

        public Page1Page()
        {
            InitializeComponent();
            //TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.AliceBlue);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a reference to the RadialController.
            _dialController = RadialController.CreateForCurrentView();

            // Remove standard menu items
            RadialControllerConfiguration _dialConfiguration = RadialControllerConfiguration.GetForCurrentView();
            _dialConfiguration.SetDefaultMenuItems(new RadialControllerSystemMenuItemKind[] { });

            // Create an icon for the custom tool.
            RandomAccessStreamReference icon =
                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/dial_icon_custom_visual.png"));

            // Create a menu item for the custom tool.
            _screenColorMenuItem =
                RadialControllerMenuItem.CreateFromIcon("Screen Color", icon);

            // Add the custom tool to the RadialController menu.
            _dialController.Menu.Items.Add(_screenColorMenuItem);

            //screenColorMenuItem.Invoked += ColorMenuItem_Invoked;

            _dialController.ScreenContactStarted += OnControllerScreenContactStarted;

            _dialController.ScreenContactContinued += OnControllerScreenContactContinued;

            _dialController.ScreenContactEnded += OnControllerScreenContactEnded;

            _dialController.RotationChanged += OnControllerRotationChanged;
        }

        private void OnControllerScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            if (null == _dial)
            {
                _dial = new SurfaceDial();
                DialCanvas.Children.Add(_dial);
            }

            _dial.Width = 500;
            _dial.Height = 500;

            Canvas.SetLeft(_dial, args.Contact.Position.X - 200);
            Canvas.SetTop(_dial, args.Contact.Position.Y - 200);

            _dial.Visibility = Visibility.Visible;
        }

        private void OnControllerScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (null != _dial)
            {
                Canvas.SetLeft(_dial, args.Contact.Position.X - 200);
                Canvas.SetTop(_dial, args.Contact.Position.Y - 200);
            }
        }

        private void OnControllerScreenContactEnded(RadialController sender, object args)
        {
            if (null != _dial)
            {
                _dial.Visibility = Visibility.Collapsed;
            }
        }

        private void OnControllerRotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (null != _dial)
            {
                _dial.Rotation += args.RotationDeltaInDegrees;
                if (_dial.Rotation > 0)
                {
                    _dial.Rotation = 0;
                }
                else if (_dial.Rotation < -315)
                {
                    _dial.Rotation = -315;
                }
                LayoutRoot.Background = _dial.ColorBrush;
            }
        }
    }
}
