using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml;

using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Converters;
using YogaC930AudioDemo.Helpers;
using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.Activation
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

        #endregion
    }
}
