using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        //private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<DefaultLaunchActivationHandler>();

        private NavigationServiceEx NavigationService
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        public DefaultLaunchActivationHandler()
        {
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            //Log.Trace("We are handling this activation with the DefaultLaunchActivationHandler");

            // get the configuration service
            ConfigurationService configurationService = (ConfigurationService)SimpleIoc.Default.GetInstance<ConfigurationService>();

            // if we got it
            if (null != configurationService)
            {
                //Log.Trace("Configuration Service is valid.");
                // is the attractor loop enabled?
                if (configurationService.Configuration.IsAttractorLoopEnabled)
                {
                    //Log.Trace("The Attractor Loop is ENABLED, so navigating to it.");
                    // yes, go to it
                    NavigationService.Navigate(typeof(ViewModels.AttractorLoopViewModel).FullName);
                }
                //// is the choose path page enabled?
                //else if (configurationService.Configuration.IsChoosePathPageEnabled)
                //{
                //    // yes, go to it
                //    NavigationService.Navigate(typeof(ViewModels.ChoosePathViewModel).FullName);
                //}
                else
                {
                    //Log.Trace("We are navigating to the FlipView.");
                    // no, go to the root flipview
                    NavigationService.Navigate(typeof(ViewModels.FlipViewViewModel).FullName);
                }
            }
            else
            {
                //Log.Trace("Configuration Service is INVALID, so we are navigating to the FlipView.");
                // go to the flipview by default
                NavigationService.Navigate(typeof(ViewModels.FlipViewViewModel).FullName);
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null;
        }
    }
}
