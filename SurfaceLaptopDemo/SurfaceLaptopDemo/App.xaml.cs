using System;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;

using SDX.Toolkit.Helpers;
using SDX.Telemetry.Services;


namespace SurfaceLaptopDemo
{
    public sealed partial class App : Application
    {
        #region Private Members

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
                // run this synchronously 
                AsyncHelper.RunSync(() => configurationService.Initialize());
            }

            // register our localization service and initialize it
            SimpleIoc.Default.Register<LocalizationService>();
            LocalizationService localizationService = (LocalizationService)SimpleIoc.Default.GetInstance<LocalizationService>();
            if (null != localizationService)
            {
                // async here might lead to a race condition, but no signs so far
                //localizationService.Initialize();
                AsyncHelper.RunSync(() => localizationService.Initialize());
            }

            // initialize the telemetry service
            SimpleIoc.Default.Register<TelemetryService>();
            TelemetryService telemetryService = (TelemetryService)SimpleIoc.Default.GetInstance<TelemetryService>();
            if (null != telemetryService)
            {
                if (null != configurationService)
                {
                    // DO NOT try to run this asynchronously; MetroLog hangs when invoked async
                    //AsyncHelper.RunSync(() => telemetryService.Initialize(configurationService.Configuration.TelemetryKey));
                    telemetryService.Initialize(configurationService.Configuration.IsTelemetryEnabled,
                                                configurationService.Configuration.TelemetryKey);

                    // log app start
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.StartApplication);
                }
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

            LoadAppResourceDictionaries();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        #endregion


        #region Private Methods

        private ActivationService CreateActivationService()
        {
            // return the flipview by default
            return new ActivationService(this);
        }

        private void LoadAppResourceDictionaries()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            string file = localizationService.IsLanguageJapanese() ? "TextBlock-ja-JP.xaml" : "TextBlock.xaml";

            // calculate uri's for styles 
            string URI_TEXTBLOCK = String.Format("ms-appx:///Styles/{0}", file);

            // load textblock styles
            ResourceDictionary resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_TEXTBLOCK, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
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

            // telemetry - app close event
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.EndApplication);

            // complete deferral
            deferral.Complete();
        }

        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // should log here, but we're not logging in this app
        }

        #endregion
    }
}
