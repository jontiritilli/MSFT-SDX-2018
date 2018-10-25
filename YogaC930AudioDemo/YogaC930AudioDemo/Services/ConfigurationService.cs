using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

using GalaSoft.MvvmLight;

using Newtonsoft.Json;

using YogaC930AudioDemo.Models;
using YogaC930AudioDemo.Helpers;
using Windows.System.Profile;
using Windows.System;

namespace YogaC930AudioDemo.Services
{
    public class ConfigurationService
    {
        #region Private Constants

        private const string LOCALSTATE_SUBFOLDER = "IdleContent";
        private const string URI_ATTRACTOR_DEFAULT = @"ms-appx:///Assets/AttractorLoop/Lenovo_Yoga_C930_Attract_Loop_240718.mp4";
        private const string IMAGE_EXTENSION_1 = ".png";
        private const string IMAGE_EXTENSION_2 = ".jpg";
        private const string VIDEO_EXTENSION_1 = ".mp4";
        private const string VIDEO_EXTENSION_2 = ".m4v";

        #endregion


        #region Public Properties

        public bool IsLoading { get; private set; }

        public bool IsLoaded { get; private set; }

        public ConfigurationFile Configuration { get; private set; }

        #endregion


        #region Public Static Properties

        public static ConfigurationService Current { get; private set; }

        #endregion


        #region Construction / Initialization

        public ConfigurationService()
        {
            // save this instance
            ConfigurationService.Current = this;
        }

        public async Task Initialize()
        {
            if (this.IsLoading || this.IsLoaded) { return; }

            this.IsLoading = true;

            if (null == this.Configuration)
            {
                // get the local folder
                StorageFolder localFolder = await GetLocalStorageFolder();

                if (null != localFolder)
                {
                    // look in the local storage folder for the file
                    this.Configuration = await GetConfigurationFile(localFolder);
                }

                if (null == this.Configuration)
                {
                    // look in the root local folder for the file
                    this.Configuration = await GetConfigurationFile(ApplicationData.Current.LocalFolder);
                }

                // if we didn't find it
                if (null == this.Configuration)
                {
                    // look in the install location
                    this.Configuration = await GetConfigurationFile(Package.Current.InstalledLocation);
                }

                // if we still didn't find it
                if (null == this.Configuration)
                {
                    // get the default configuration
                    this.Configuration = ConfigurationFile.CreateDefault();
                }
            }

            this.IsLoaded = true;
            this.IsLoading = false;
        }

        private async Task<ConfigurationFile> GetConfigurationFile(StorageFolder storageFolder)
        {
            ConfigurationFile config = null;

            // default config file path
            string defaultPath = Path.Combine(storageFolder.Path, "config.json");

            try
            {
                // try to open and read it
                StorageFile configFile = await StorageFile.GetFileFromPathAsync(defaultPath);
                string fileContents = await FileIO.ReadTextAsync(configFile);

                // if we got some data
                if (!String.IsNullOrWhiteSpace(fileContents))
                {
                    // deserialize it
                    config = JsonConvert.DeserializeObject<ConfigurationFile>(fileContents);
                }
            }
            catch (FileNotFoundException)
            {

            }

            return config;
        }

        #endregion


        #region Public Static Methods

        public static bool DoesOSBuildSupportSuspend()
        {
            ulong build = GetOSBuild();

            return (build >= 17134);    // 17134 is the build number for v1803
        }

        public static ulong GetOSBuild()
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong major = (version & 0xFFFF000000000000L) >> 48;
            ulong minor = (version & 0x0000FFFF00000000L) >> 32;
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            ulong revision = (version & 0x000000000000FFFFL);
            var osVersion = $"{major}.{minor}.{build}.{revision}";

            return build;
        }

        public static void SuspendApp()
        {
            AsyncHelper.RunSync(() => SuspendAppAsync());
        }

        public static async Task SuspendAppAsync()
        {
            AppResourceGroupInfo appResourceGroupInfo = null;

            // for Windows v1803 aka build 17134 and above, we can minimize by suspending

            // figure out if we can suspend and can get the references we need
            if (ConfigurationService.DoesOSBuildSupportSuspend())
            {
                // get app diagnostic information
                IList<AppDiagnosticInfo> diagnosticInformations = await AppDiagnosticInfo.RequestInfoForAppAsync();

                // if we got them
                if ((null != diagnosticInformations) && (diagnosticInformations.Count > 0))
                {
                    // loop through the informations
                    foreach (AppDiagnosticInfo diagnosticInformation in diagnosticInformations)
                    {
                        // if we find one with Id = "App"
                        if (0 == String.Compare(diagnosticInformation.AppInfo.Id, "App", true))
                        {
                            // get the resource groups from that diagnostic information
                            IList<AppResourceGroupInfo> resourceGroups = diagnosticInformation.GetResourceGroups();

                            // if we got them
                            if ((null != resourceGroups) && (resourceGroups.Count > 0))
                            {
                                // take the first resource group
                                appResourceGroupInfo = resourceGroups[0];
                            }
                        }
                    }
                }
            }

            // if we got a suspendable object, use it
            if (null != appResourceGroupInfo)
            {
                // we can suspend and got an resource info object to use
                AppExecutionStateChangeResult result = await appResourceGroupInfo.StartSuspendAsync();
            }
            else
            {
                // we can't suspend, so must exit
                App.Current.Exit();
            }
        }

        #endregion


        #region Configuration Methods

        public StorageFile GetAttractorLoopFile()
        {
            // set default
            StorageFile storageFile = null;

            if (this.IsLoaded)
            {
                // get the local folder
                StorageFolder localFolder = AsyncHelper.RunSync<StorageFolder>(() => GetLocalStorageFolder());

                if (null != localFolder)
                {
                    // package extensions
                    List<string> extensions = new List<string>() { VIDEO_EXTENSION_1, VIDEO_EXTENSION_2 };

                    // get the first file with one of our video file extensions
                    storageFile = AsyncHelper.RunSync<StorageFile>(() => GetFirstFileWithExtensions(localFolder, extensions));

                    // if we didn't get it
                    if (null == storageFile)
                    {
                        // try to get our embedded video
                        storageFile = AsyncHelper.RunSync<StorageFile>(() => GetFileFromAppxUri(URI_ATTRACTOR_DEFAULT));
                    }
                }
            }

            return storageFile;
        }

        #endregion


        #region File Helpers

        public StorageFile GetFileFromLocalStorage(string fileName)
        {
            StorageFile file = null;

            if (this.IsLoaded)
            {
                StorageFolder localStorage = AsyncHelper.RunSync<StorageFolder>(() => GetLocalStorageFolder());

                if (null != localStorage)
                {
                    // try to get our file
                    file = AsyncHelper.RunSync<StorageFile>(() => GetFileFromLocalStorageAsync(fileName));
                }
            }

            return file;
        }

        public async Task<StorageFolder> GetLocalStorageFolder()
        {
            // default to the LocalState folder
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            // if the subfolder constant is not null/empty
            if (!String.IsNullOrEmpty(LOCALSTATE_SUBFOLDER))
            {
                try
                {
                    // get a list of the subfolder in the localstate folder
                    IReadOnlyList<StorageFolder> folders = await storageFolder.GetFoldersAsync();

                    // loop through them
                    foreach (StorageFolder folder in folders)
                    {
                        // if we find one that matches our subfolder name
                        if (0 == String.Compare(folder.Name, LOCALSTATE_SUBFOLDER, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // save it
                            storageFolder = folder;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return storageFolder;
        }

        public async Task<StorageFolder> GetSubFolder(StorageFolder folder, string subFolderPath)
        {
            // default
            StorageFolder subFolder = null;

            // get the full path name
            string searchPath = Path.Combine(folder.Path, subFolderPath);

            // try to get that folder
            subFolder = await StorageFolder.GetFolderFromPathAsync(searchPath);

            return subFolder;
        }

        private async Task<StorageFile> GetFileFromLocalStorageAsync(string fileName)
        {
            StorageFile file = null;

            StorageFolder localFolder = await GetLocalStorageFolder();

            if (null != localFolder)
            {
                file = (StorageFile)(await localFolder.TryGetItemAsync(fileName));
            }

            return file;
        }

        private async Task<BitmapImage> GetBitmapFromFile(StorageFile file, int decodePixelWidth)
        {
            BitmapImage image = null;

            if (null != file)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    image = new BitmapImage() { DecodePixelWidth = decodePixelWidth };
                    await image.SetSourceAsync(stream);
                }
            }

            return image;
        }

        private async Task<StorageFile> GetFileFromAppxUri(string uriString)
        {
            StorageFile file = null;

            if (!String.IsNullOrEmpty(uriString))
            {
                Uri uri = new Uri(uriString);

                if (null != uri)
                {
                    file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                }
            }

            return file;
        }

        private async Task<StorageFile> FindOrDeployFileAsset(string extension, bool shouldCopy = true)
        {
            StorageFile file = null;

            // do we need to copy the file we found?
            bool needToCopy = false;

            try
            {
                // package extensions
                List<string> extensions = new List<string>() { extension };

                // get the local storage folder; we always deploy here
                StorageFolder destination = await GetLocalStorageFolder();

                // look for a onenote file
                file = await this.GetFirstFileWithExtensions(destination, extensions);

                // if there's not one in the source
                if (null == file)
                {
                    // get the default one from our installed location
                    file = await this.GetFirstFileWithExtensions(Package.Current.InstalledLocation, extensions);
                    needToCopy = true;
                }

                // we're out of options to find the file
                if (null != file)
                {
                    // do we need to copy?
                    if (shouldCopy && needToCopy)
                    {
                        // copy the file
                        await file.CopyAsync(destination);

                        // get the file in the new location
                        file = await destination.GetFileAsync(Path.Combine(destination.Path, file.Name));
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return file;
        }

        private async Task<StorageFile> GetFirstFileWithExtensions(StorageFolder folder, List<string> extensions)
        {
            StorageFile file = null;

            // execute the query
            IReadOnlyList<StorageFile> files = await GetAllFilesWithExtensions(folder, extensions);

            // if we got results
            if ((null != files) && (files.Count > 0))
            {
                // take the first one
                file = files.First<StorageFile>();
            }

            return file;
        }

        private async Task<IReadOnlyList<StorageFile>> GetAllFilesWithExtensions(StorageFolder folder, List<string> extensions)
        {
            IReadOnlyList<StorageFile> files = null;

            if (null != folder)
            {
                try
                {
                    // build query for matching files
                    QueryOptions options = new QueryOptions(CommonFileQuery.OrderByName, extensions);
                    StorageFileQueryResult query = folder.CreateFileQueryWithOptions(options);

                    // execute the query
                    files = await query.GetFilesAsync();
                }
                catch (Exception ex)
                {

                }
            }

            return files;
        }

        protected async Task<bool> VerifyFileExists(StorageFolder folder, string fileName)
        {
            bool foundIt = false;

            // if we got a folder
            if (null != folder)
            {
                try
                {
                    // get the extension
                    string extension = Path.GetExtension(fileName);

                    // build query for matching files
                    QueryOptions options = new QueryOptions(CommonFileQuery.OrderByName, new List<string>() { extension });
                    StorageFileQueryResult query = folder.CreateFileQueryWithOptions(options);

                    // execute the query
                    IReadOnlyList<StorageFile> files = await query.GetFilesAsync();

                    // if we got results
                    if ((null != files) && (files.Count > 0))
                    {
                        foreach (StorageFile file in files)
                        {
                            if (file.Name.ToUpperInvariant() == fileName.ToUpperInvariant())
                            {
                                // we found the file
                                foundIt = true;

                                break;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return foundIt;
        }

        #endregion
    }
}
