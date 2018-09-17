using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceStudioDemo.ViewModels
{
    public class AccessoriesPenViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/caprock_background_light.jpg";

        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;

        public string Headline;
        public string Lede;

        public string DialTryItTitle;
        public string DialTryItLede;

        public string PenTryItTitle;
        public string PenTryItLede;

        public List<ListItem> ListItems = new List<ListItem>();
        public double ICON_WIDTH = StyleHelper.GetApplicationDouble(LayoutSizes.AccessoriesPenListIconWidth);

        #endregion

        #region Construction

        public AccessoriesPenViewModel()
        {
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
