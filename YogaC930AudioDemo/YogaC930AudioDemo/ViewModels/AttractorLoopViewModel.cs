using System;

using Windows.Storage;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using YogaC930AudioDemo.Services;


namespace YogaC930AudioDemo.ViewModels
{
    public class AttractorLoopViewModel : ViewModelBase
    {
        #region Public Properties

        public string Title;
        public string CTA;

        public StorageFile MediaSourceStorageFile { get; set; }

        #endregion


        #region Construction

        public AttractorLoopViewModel()
        {
            this.MediaSourceStorageFile = ConfigurationService.Current.GetAttractorLoopFile();

            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAttractorLopViewModel(this);
            }
        }

        #endregion
    }
}
