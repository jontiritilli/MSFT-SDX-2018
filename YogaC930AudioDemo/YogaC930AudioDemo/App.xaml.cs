using System;
using GalaSoft.MvvmLight.Ioc;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Core;

using YogaC930AudioDemo.Converters;
using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.Services;
using Windows.UI.Xaml.Controls;
using YogaC930AudioDemo.Views;
using Windows.UI.Xaml.Controls.Primitives;

namespace YogaC930AudioDemo
{
    public sealed partial class App : Application
    {
        private DispatcherTimer InteractionTimer = new DispatcherTimer();

        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();

            EnteredBackground += App_EnteredBackground;

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
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }

            CoreWindow cw = CoreWindow.GetForCurrentThread();
            if (null != cw)
            {
                cw.KeyUp += OnKeyUp;
                cw.PointerReleased += OnPointerReleased;
            }

            SetupNoInteractionTimer();

            // add the font size styles
            LoadStyles();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);

            //CoreWindow cw = CoreWindow.GetForCurrentThread();
            //if (null != cw)
            //{
            //    cw.KeyUp += OnKeyUp;
            //    cw.PointerReleased += OnPointerReleased;
            //}
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.AttractorLoopViewModel));
        }

        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Helpers.Singleton<SuspendAndResumeService>.Instance.SaveStateAsync();
            deferral.Complete();
        }

        private void LoadStyles()
        {
            ScalingConverter converter = new ScalingConverter();

            if (null != Application.Current.Resources)
            {
                // load canvas sizes
                Size effectiveSize = WindowHelper.GetViewSizeInfo();

                Application.Current.Resources.Add("CanvasWidth", effectiveSize.Width);
                Application.Current.Resources.Add("CanvasHeight", effectiveSize.Height);

                // load font sizes

                // headline
                Application.Current.Resources.Add("HeadlineTextStyleFontSize", converter.Convert(31, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("HeadlineTextStyleLineHeight", converter.Convert(38, typeof(double), null, String.Empty));

                // body
                Application.Current.Resources.Add("BodyTextStyleFontSize", converter.Convert(22, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("BodyTextStyleLineHeight", converter.Convert(37.5, typeof(double), null, String.Empty));

                // legal
                Application.Current.Resources.Add("LegalTextStyleFontSize", converter.Convert(12.5, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("LegalTextStyleLineHeight", converter.Convert(15, typeof(double), null, String.Empty));

                // ListItemFirstLineTextStyle
                Application.Current.Resources.Add("ListItemFirstLineTextStyleFontSize", converter.Convert(17, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("ListItemFirstLineTextStyleLineHeight", converter.Convert(42.5, typeof(double), null, String.Empty));

                // ListItemHeroLineTextStyle
                Application.Current.Resources.Add("ListItemHeroLineTextStyleFontSize", converter.Convert(40, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("ListItemHeroLineTextStyleLineHeight", converter.Convert(40, typeof(double), null, String.Empty));

                // ListItemLastLineTextStyle
                Application.Current.Resources.Add("ListItemLastLineTextStyleFontSize", converter.Convert(22, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("ListItemLastLineTextStyleLineHeight", converter.Convert(37.5, typeof(double), null, String.Empty));

                // ColorSpecTextStyle
                Application.Current.Resources.Add("ColorSpecTextStyleFontSize", converter.Convert(12.5, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("ColorSpecTextStyleLineHeight", converter.Convert(42.5, typeof(double), null, String.Empty));

                // NavigationBarLightBackgroundNormalTextStyle
                Application.Current.Resources.Add("NavigationBarLightBackgroundNormalTextStyleFontSize", converter.Convert(19.5, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("NavigationBarLightBackgroundNormalTextStyleLineHeight", converter.Convert(19.5, typeof(double), null, String.Empty));

                // PlayAudioDemoLightBackgroundTextStyle
                Application.Current.Resources.Add("PlayAudioDemoLightBackgroundTextStyleFontSize", converter.Convert(22, typeof(double), null, String.Empty));
                Application.Current.Resources.Add("PlayAudioDemoLightBackgroundTextStyleLineHeight", converter.Convert(22, typeof(double), null, String.Empty));

                // load textblock.xaml
                ResourceDictionary resourceDictionary = new ResourceDictionary()
                {
                    Source = new Uri("ms-appx:///Styles/TextBlock.xaml", UriKind.Absolute),
                };
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }

        private void OnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = InterruptVideoTimer();
        }


        private void OnPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            args.Handled = InterruptVideoTimer();
        }

        private bool InterruptVideoTimer()
        {
            InteractionTimer.Start();

            return true;
        }

        private void SetupNoInteractionTimer()
        {
            InteractionTimer.Interval = TimeSpan.FromSeconds(35);
            InteractionTimer.Start();
            InteractionTimer.Tick += ResetVolume;
        }

        private void ResetVolume(object sender, object e)
        {
            Popup PlayerPopup = FlipViewPage.Current?.GetPlayerPopup();

            if (null != PlayerPopup && PlayerPopup.IsOpen)
            {
                PlayerPopupPage.Current?.GetVolumeControl().ResetVolume();
            }
        }

    }
}
