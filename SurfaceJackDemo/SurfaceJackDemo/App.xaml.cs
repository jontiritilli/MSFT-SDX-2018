﻿using System;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

using GalaSoft.MvvmLight.Ioc;

using SurfaceJackDemo.Services;

using SDX.Toolkit.Helpers;
using SDX.Telemetry.Models;
using SDX.Telemetry.Services;


namespace SurfaceJackDemo
{
    public sealed partial class App : Application
    {
        #region Private Members

        private int _keyUpCount = 0;
        private int _mouseUpCount = 0;
        private int _penUpCount = 0;
        private int _touchUpCount = 0;

        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        #endregion


        #region Public Static Properties

        public static App Current { get; private set; }

        #endregion


        #region Construction

        public App()
        {
            // save a pointer to ourself
            App.Current = this;

            InitializeComponent();

            // app lifecycle event handlers
            EnteredBackground += OnEnteredBackground;
            Suspending += OnSuspending;
            Resuming += OnResuming;
            UnhandledException += OnUnhandledException;

            // add our handler for keys pressed/released
            //CoreWindow thisWindow = CoreWindow.GetForCurrentThread();
            //if (null != thisWindow)
            if (null != Window.Current.CoreWindow)
            {
                Window.Current.CoreWindow.KeyUp += OnKeyUp;
                Window.Current.CoreWindow.PointerReleased += OnPointerReleased;
            }

            // we want full screen, but leave this off during dev 
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;

            // we want landscape only
            DisplayOrientations orientations = DisplayOrientations.Landscape;
            DisplayInformation.AutoRotationPreferences = orientations;

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);

            // register our configuration service and initialize it
            SimpleIoc.Default.Register<ConfigurationService>();
            ConfigurationService configurationService = (ConfigurationService)SimpleIoc.Default.GetInstance<ConfigurationService>();
            if (null != configurationService)
            {
                // need this to start, so run this synchronously 
                AsyncHelper.RunSync(() => configurationService.Initialize());
            }

            // register our localization service and initialize it
            SimpleIoc.Default.Register<LocalizationService>();
            LocalizationService localizationService = (LocalizationService)SimpleIoc.Default.GetInstance<LocalizationService>();
            if (null != localizationService)
            {
                // need this to start, so run sync
                AsyncHelper.RunSync(() => localizationService.Initialize());
            }

            // register the telemetry service and initialize it
            SimpleIoc.Default.Register<TelemetryService>();
            TelemetryService telemetryService = (TelemetryService)SimpleIoc.Default.GetInstance<TelemetryService>();
            if (null != telemetryService)
            {
                if (null != configurationService)
                {
                    // get telemetry config values
                    string telemetryBaseUrl = configurationService.Configuration?.TelemetryBaseUrl;
                    string telemetryId = ConfigurationService.Current.GetTelemetryId();

                    // need this to start, so run this synchronously 
                    AsyncHelper.RunSync(() => telemetryService.Initialize(telemetryBaseUrl, telemetryId, new UWPTelemetryDependent()));
                }
            }

            // register the playlist service and initialize it
            SimpleIoc.Default.Register<PlaylistService>();
            PlaylistService playlistService = (PlaylistService)SimpleIoc.Default.GetInstance<PlaylistService>();
            if (null != playlistService)
            {
                // run this async because we don't need it to start the app
#pragma warning disable CS4014
                playlistService.Initialize();
#pragma warning restore CS4014
            }
        }

        #endregion


        #region Base Overrides

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
            // IMPORTANT: THIS CODE IS UNIQUE TO THE JACK APP BECAUSE THIS APP MUST 
            // RUN ON ALL DEVICES, SO IT USES STYLE XAML FROM ALL DEVICE APP PROJECTS.
            // THIS CODE SUPPLEMENTS THE xaml CODE IN App.xaml BY ADDING THE CORRECT
            // VERSION OF THE TextBlock.xaml, Sizes.xaml, AND _Thickness.xaml FILES
            // FROM EITHER THE 13 OR 15 SUBFOLDER OF Styles.

            // Subfolder in Styles directory
            string path = null;

            // which device are we running on?
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();

            switch (deviceType)
            {
                case DeviceType.Studio:
                    path = @"caprock";
                    break;

                case DeviceType.Book15:
                    path = @"sb2/15";
                    break;

                case DeviceType.Book13:
                    path = @"sb2/13";
                    break;

                case DeviceType.Pro:
                    path = @"cruz";
                    break;

                case DeviceType.Laptop:
                    path = @"cruz";
                    break;

                default:
                    path = @"cruz";    // for testing, run the cruz version
                    break;
            }

            // load the ResourceDictionaries
            LoadAppResourceDictionaries(path);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        #endregion


        #region Private Methods

        private void LoadAppResourceDictionaries(string path)
        {
            // calculate uri's for styles 
            string URI_TEXTBLOCK = String.Format("ms-appx:///Styles/{0}/TextBlock.xaml", path);
            string URI_SIZES = String.Format("ms-appx:///Styles/{0}/Sizes.xaml", path);
            string URI_THICKNESS = String.Format("ms-appx:///Styles/{0}/_Thickness.xaml", path);
            string URI_IMAGES = String.Format("ms-appx:///Styles/{0}/_Images.xaml", path);

            // load textblock styles
            ResourceDictionary resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_TEXTBLOCK, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            // load sizes
            resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_SIZES, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            // load thickness
            resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_THICKNESS, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            // load _images
            resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_IMAGES, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private ActivationService CreateActivationService()
        {
            // return the flipview by default
            return new ActivationService(this);
        }

        #endregion


        #region Event Handlers

        private async void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Helpers.Singleton<SuspendAndResumeService>.Instance.SaveStateAsync();
            deferral.Complete();
        }

        private void OnResuming(object sender, object e)
        {

        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            // get a deferral (not guaranteed)
            var deferral = e.SuspendingOperation.GetDeferral();

            // save/send the ImplementDown telemetry data
            TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_IMPLEMENTDOWN, (_penUpCount + _mouseUpCount + _touchUpCount + _keyUpCount).ToString(), false);

            // reset ImplementDown counters
            _penUpCount = _mouseUpCount = _keyUpCount = _touchUpCount = 0;

            // close telemetry
            if (null != TelemetryService.Current)
            {
                await TelemetryService.Current.CloseTelemetry();
            }

            // complete deferral
            deferral.Complete();
        }

        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // should log here, but we're not logging in this app
        }

        private void OnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = HandleKeyUp(args.VirtualKey);
        }

        public bool HandleKeyUp(VirtualKey key)
        {
            bool handled = false;

            switch (key)
            {
                case VirtualKey.Right:
                    _keyUpCount++;
                    break;

                case VirtualKey.Left:
                    _keyUpCount++;
                    break;

                default:
                    break;
            }

            return handled;
        }

        private void OnPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            args.Handled = HandlePointerReleased(args.CurrentPoint.PointerDevice.PointerDeviceType);
        }

        public bool HandlePointerReleased(PointerDeviceType pointerDeviceType)
        {
            bool handled = false;

            switch (pointerDeviceType)
            {
                case PointerDeviceType.Touch:
                    _touchUpCount++;
                    break;
                case PointerDeviceType.Pen:
                    _penUpCount++;
                    break;
                case PointerDeviceType.Mouse:
                    _mouseUpCount++;
                    break;
                default:
                    break;
            }

            handled = true;

            return handled;
        }

        #endregion
    }
}