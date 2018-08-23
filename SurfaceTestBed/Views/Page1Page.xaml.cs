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
using SurfaceTestBed.Controls;

namespace SurfaceTestBed.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1Page : Page
    {
        private RadialController myController;
        private RadialControllerMenuItem screenColorMenuItem;
        private DialColorControl dialControl;

        private Page1ViewModel ViewModel
        {
            get { return DataContext as Page1ViewModel; }
        }

        public Page1Page()
        {
            this.InitializeComponent();

            TestHelper.AddGridCellBorders(this.LayoutRoot, 7, 3, Colors.AliceBlue);

            RadialControllerConfiguration myConfiguration = RadialControllerConfiguration.GetForCurrentView();
            myConfiguration.SetDefaultMenuItems(new RadialControllerSystemMenuItemKind[] { });

            // Create a reference to the RadialController.
            myController = RadialController.CreateForCurrentView();

            // Disable standard menu??
            myController.Menu.IsEnabled = false;

            // Create an icon for the custom tool.
            RandomAccessStreamReference icon =
              RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/dial_icon_custom_visual.png"));

            // Create a menu item for the custom tool.
            screenColorMenuItem =
              RadialControllerMenuItem.CreateFromIcon("Screen Color", icon);

            // Add the custom tool to the RadialController menu.
            myController.Menu.Items.Add(screenColorMenuItem);

            //screenColorMenuItem.Invoked += ColorMenuItem_Invoked;

            myController.ScreenContactStarted += MyController_ScreenContactStarted;
            myController.ScreenContactContinued += MyController_ScreenContactContinued;
            myController.ScreenContactEnded += MyController_ScreenContactEnded;

            myController.RotationChanged += MyController_RotationChanged;
        }

        private async void MyController_ScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (null == dialControl)
                {
                    dialControl = new Controls.DialColorControl();
                    DialCanvas.Children.Add(dialControl);
                }

                dialControl.Width = 500;
                dialControl.Height = 500;

                Canvas.SetLeft(dialControl, args.Contact.Position.X - 250);
                Canvas.SetTop(dialControl, args.Contact.Position.Y - 250);

                dialControl.Visibility = Visibility.Visible;
            });
        }

        private async void MyController_ScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (null != dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Canvas.SetLeft(dialControl, args.Contact.Position.X - 250);
                    Canvas.SetTop(dialControl, args.Contact.Position.Y - 250);
                });
            }
        }

        private async void MyController_ScreenContactEnded(RadialController sender, object args)
        {
            if (null != dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    dialControl.Visibility = Visibility.Collapsed;
                });
            }
        }

        private async void MyController_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (null != dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    dialControl.Rotation += args.RotationDeltaInDegrees;
                    if (dialControl.Rotation > 0)
                    {
                        dialControl.Rotation = 0;
                    }
                    else if (dialControl.Rotation < -315)
                    {
                        dialControl.Rotation = -315;
                    }
                    LayoutRoot.Background = dialControl.ColorBrush;
                });
            }
        }

        private void ColorMenuItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            Debug.WriteLine("Item invoked");
            myController.Menu.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            myController.Menu.Items.Remove(screenColorMenuItem);
            Frame.GoBack();
        }
    }
}
