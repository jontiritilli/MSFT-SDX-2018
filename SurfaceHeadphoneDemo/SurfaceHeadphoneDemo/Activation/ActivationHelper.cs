using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml;

using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Converters;
using SDX.Toolkit.Helpers;

using SurfaceHeadphoneDemo.Services;


namespace SurfaceHeadphoneDemo.Activation
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
                    NavigationService.Navigate(typeof(ViewModels.FirmwareUpdateViewModel).FullName);
                }
            }
            else
            {
                //Log.Trace("Configuration Service is INVALID, so we are navigating to the FlipView.");
                // go to the flipview by default
                NavigationService.Navigate(typeof(ViewModels.FirmwareUpdateViewModel).FullName);
            }
        }

        public static void LoadStyles()
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

        public static string GetDeviceSignature()
        {
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
                    path = @"foxburg";
                    break;

                default:
                    path = @"cruz";    // for testing, run the cruz version
                    break;
            }

            return path;
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

        #endregion
    }
}
