using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;
using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        private NavigationServiceEx NavigationService
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            ActivationHelper.LoadAppResourceDictionaries();

            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                NavigationService.Navigate(data.ViewModelName, data.Parameters);
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page
                // based on the URI, navigate to the page determined by config.json

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
                    //// is the choose path page enabled?
                    //else if (configurationService.Configuration.IsChoosePathPageEnabled)
                    //{
                    //    // yes, go to it
                    //    NavigationService.Navigate(typeof(ViewModels.ChoosePathViewModel).FullName);
                    //}
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
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // If your app has multiple handlers of ProtocolActivationEventArgs
            // use this method to determine which to use. (possibly checking args.Uri.Scheme)
            return true;
        }
    }
}


