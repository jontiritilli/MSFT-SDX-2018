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

        private RadialController _myController;
        private SurfaceDial _dialControl;
        private RadialControllerMenuItem _screenColorMenuItem;

        private Page1ViewModel ViewModel
        {
            get { return DataContext as Page1ViewModel; }
        }

        public Page1Page()
        {
            this.InitializeComponent();

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a reference to the RadialController.
            _myController = RadialController.CreateForCurrentView();

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
            _myController.Menu.Items.Add(_screenColorMenuItem);

            //screenColorMenuItem.Invoked += ColorMenuItem_Invoked;

            _myController.ScreenContactStarted += OnControllerScreenContactStarted;

            _myController.ScreenContactContinued += OnControllerScreenContactContinued;

            _myController.ScreenContactEnded += OnControllerScreenContactEnded;

            _myController.RotationChanged += OnControllerRotationChanged;
        }
        private async void OnControllerScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (null == _dialControl)
                {
                    _dialControl = new SurfaceDial();
                    DialCanvas.Children.Add(_dialControl);
                }

                Canvas.SetLeft(_dialControl, args.Contact.Position.X - 200);
                Canvas.SetTop(_dialControl, args.Contact.Position.Y - 200);

                _dialControl.Visibility = Visibility.Visible;
            });
        }

        private async void OnControllerScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (null != _dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Canvas.SetLeft(_dialControl, args.Contact.Position.X - 200);
                    Canvas.SetTop(_dialControl, args.Contact.Position.Y - 200);
                });
            }
        }

        private async void OnControllerScreenContactEnded(RadialController sender, object args)
        {
            if (null != _dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _dialControl.Visibility = Visibility.Collapsed;
                });
            }
        }

        private async void OnControllerRotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (null != _dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _dialControl.Rotation += args.RotationDeltaInDegrees;
                    if (_dialControl.Rotation > 0)
                    {
                        _dialControl.Rotation = 0;
                    }
                    else if (_dialControl.Rotation < -315)
                    {
                        _dialControl.Rotation = -315;
                    }
                    LayoutRoot.Background = _dialControl.ColorBrush;
                });
            }
        }
    }
}
