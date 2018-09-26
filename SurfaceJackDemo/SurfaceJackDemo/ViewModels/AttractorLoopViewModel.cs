using System;

using Windows.Storage;

using GalaSoft.MvvmLight;

using SurfaceJackDemo.Services;


namespace SurfaceJackDemo.ViewModels
{
    public class AttractorLoopViewModel : ViewModelBase
    {
        #region Public Properties

        public StorageFile MediaSourceStorageFile { get; set; }

        #endregion


        #region Construction

        public AttractorLoopViewModel()
        {
            this.MediaSourceStorageFile = ConfigurationService.Current.GetAttractorLoopFile();
        }

        #endregion
    }
}
