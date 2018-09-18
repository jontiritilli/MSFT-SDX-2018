using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.Services;
using Windows.UI;

namespace SurfaceBook2Demo.ViewModels
{
    public class AccessoriesPenViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND13 = "ms-appx:///Assets/Backgrounds/sb2_13_background_light.jpg";
        private const string URI_BACKGROUND15 = "ms-appx:///Assets/Backgrounds/sb2_15_background_light.jpg";
       
        #endregion


        #region Public Properties

        public string BackgroundUri;        

        public string Headline;
        public string Lede;

        public string TryItTitle;
        public string TryItLede;
        public List<ListItem> ListItems = new List<ListItem>();
        public List<ColoringBookColor> Colors = new List<ColoringBookColor>();

        public int ColoringBookButtonWidth;
        public int ColoringBookButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        // TODO: need a list of items here for the List control.        
        #endregion


        #region Construction

        public AccessoriesPenViewModel()
        {
            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND15;                  
                    break;

                case DeviceType.Book13:
                default:
                    this.BackgroundUri = URI_BACKGROUND13;

                    break;
            }

          

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesPenViewModel(this);
            }
        }

        #endregion
    }
}
