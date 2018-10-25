using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;

namespace SurfaceLaptopDemo.Activation
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

        public static void LoadAppResourceDictionaries()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            string file = localizationService.IsLanguageJapanese() ? "TextBlock-ja-JP.xaml" : "TextBlock.xaml";

            // calculate uri's for styles 
            string URI_TEXTBLOCK = String.Format("ms-appx:///Styles/{0}", file);

            // load textblock styles
            ResourceDictionary resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(URI_TEXTBLOCK, UriKind.Absolute),
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        #endregion
    }
}
