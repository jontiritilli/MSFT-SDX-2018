using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

using SurfaceBook2Demo.Services;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;

namespace SurfaceBook2Demo.Activation
{
    class ActivationHelper
    {
        #region Private Static Members

        private static NavigationServiceEx NavigationService
        {
            get
            {
                return CommonServiceLocator.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        #endregion

        #region Public Static Methods

        public static string GetDeviceSignature()
        {
            string path = null;

            // which device are we running on?
            DeviceType deviceType = WindowHelper.GetDeviceTypeFromResolution();

            switch (deviceType)
            {
                case DeviceType.Book15:
                    path = "15";
                    break;

                case DeviceType.Book13:
                    path = "13";
                    break;

                default:
                    path = "13";    // for testing, run the 13 version
                    break;
            }

            return path;
        }

        public static void HandleActivation()
        {
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
        }

        public static void LoadAppResourceDictionaries(string path)
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            string file = localizationService.IsLanguageJapanese() ? "TextBlock-ja-JP.xaml" : "TextBlock.xaml";

            // calculate uri's for styles 
            string URI_TEXTBLOCK = String.Format("ms-appx:///Styles/{0}/{1}", path, file);
            string URI_SIZES = String.Format("ms-appx:///Styles/{0}/Sizes.xaml", path);
            string URI_THICKNESS = String.Format("ms-appx:///Styles/{0}/_Thickness.xaml", path);
            string URI_IMAGE = String.Format("ms-appx:///Styles/{0}/_Images.xaml", path);

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

            // load thickness
            resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_IMAGE, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        #endregion
    }
}
