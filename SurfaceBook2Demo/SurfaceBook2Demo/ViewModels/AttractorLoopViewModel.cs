using System;

using Windows.Storage;

using GalaSoft.MvvmLight;

using SurfaceBook2Demo.Services;


namespace SurfaceBook2Demo.ViewModels
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
