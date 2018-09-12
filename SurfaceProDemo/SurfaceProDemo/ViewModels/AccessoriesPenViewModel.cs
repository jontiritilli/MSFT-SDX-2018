using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SurfaceProDemo.Services;
using System.Collections.Generic;
using Windows.UI;

namespace SurfaceProDemo.ViewModels
{
    public class AccessoriesPenViewModel : ViewModelBase
    {

        #region Constants

        private const string URI_BACKGROUND = "ms-appx:///Assets/Backgrounds/generic-bg.png";           
       
        #endregion

        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;        

        public string Headline;
        public string Lede;

        public string TryItCaption;
        public string TryIt;
        public string TryItLede;
        public List<ListItem> ListItems = new List<ListItem>();

        public double ICON_WIDTH = 96d;

        #endregion

        #region Construction

        public AccessoriesPenViewModel()
        {

            this.BackgroundUri = URI_BACKGROUND;           

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
