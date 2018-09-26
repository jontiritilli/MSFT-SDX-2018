using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;


namespace SurfaceProDemo.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
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
            // get the configuration service
            ConfigurationService configurationService = (ConfigurationService)SimpleIoc.Default.GetInstance<ConfigurationService>();

            // if we got it
            if (null != configurationService)
            {
                // is the attractor loop enabled?
                if (configurationService.Configuration.IsAttractorLoopEnabled)
                {
                    // yes, go to it
                    NavigationService.Navigate(typeof(ViewModels.AttractorLoopViewModel).FullName);
                }
                // is the choose path page enabled?
                else if (configurationService.Configuration.IsChoosePathPageEnabled)
                {
                    // yes, go to it
                    NavigationService.Navigate(typeof(ViewModels.ChoosePathViewModel).FullName);
                }
                else
                {
                    // no, go to the root flipview
                    NavigationService.Navigate(typeof(ViewModels.FlipViewViewModel).FullName);
                }
            }
            else
            {
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
