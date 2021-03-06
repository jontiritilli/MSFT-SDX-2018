﻿using System;
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
            Resuming += this.App_Resuming;

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

        private void App_Resuming(object sender, object e)
        {
            // when we resume, make sure we're in full screen mode
            ApplicationView view = ApplicationView.GetForCurrentView();

            if (null != view)
            {
                view.TryEnterFullScreenMode();
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
