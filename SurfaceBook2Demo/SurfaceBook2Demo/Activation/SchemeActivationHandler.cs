using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

using GalaSoft.MvvmLight.Ioc;

using MetroLog;

using SurfaceBook2Demo.Services;
using SurfaceBook2Demo.ViewModels;


namespace SurfaceBook2Demo.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        //private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<SchemeActivationHandler>();

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
            //Log.Trace("Entered SchemeActivationHandler.");

            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                //Log.Trace($"We are routing based on the URI Scheme: {args.Uri}");
                //Log.Trace($"ViewModelName is {data.ViewModelName}.");

                NavigationService.Navigate(data.ViewModelName, data.Parameters);
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page
                // based on the URI, navigate to the page determined by config.json
                //Log.Trace($"Data is INVALID; we are NOT routing based on the URI Scheme: {args.Uri}");

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
                    //    Log.Trace("Choose Path Page is ENABLED, so navigating to it.");
                    //    // yes, go to it
                    //    NavigationService.Navigate(typeof(ViewModels.ChoosePathViewModel).FullName);
                    //}
                    else
                    {
                        //Log.Trace("Attractor Loop is DISABLED, so navigating to the FlipView.");
                        // no, go to the root flipview
                        NavigationService.Navigate(typeof(ViewModels.FlipViewViewModel).FullName);
                    }
                }
                else
                {
                    //Log.Trace("Configuration service is INVALID, so navigating to the FlipView.");
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
