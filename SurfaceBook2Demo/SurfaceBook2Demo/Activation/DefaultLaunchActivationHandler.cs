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

            // load the ResourceDictionaries
            ActivationHelper.LoadAppResourceDictionaries(ActivationHelper.GetDeviceSignature());

            ActivationHelper.HandleActivation();

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null;
        }
    }
}
