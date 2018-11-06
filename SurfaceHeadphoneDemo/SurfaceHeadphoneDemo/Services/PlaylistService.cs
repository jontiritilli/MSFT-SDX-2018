using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Newtonsoft.Json;

using SurfaceHeadphoneDemo.Models;
using SurfaceHeadphoneDemo.ViewModels;

using SDX.Toolkit.Models;


namespace SurfaceHeadphoneDemo.Services
{
    public class PlaylistService
    {
        public static PlaylistService Current { get; private set; }

        public Playlist DefaultPlaylist = null;
        public Playlist CurrentPlaylist = null;

        public PlaylistService()
        {
            // save this instance as current
            PlaylistService.Current = this;
        }

        public bool IsLoading { get; private set; }

        public bool IsLoaded { get; private set; }

        public async Task Initialize()
        {
            if (this.IsLoading) { return; }

            this.IsLoading = true;

            if ((null == DefaultPlaylist) || (null == CurrentPlaylist))
            {
                // default playlist file path
                string defaultPath = Path.Combine(Package.Current.InstalledLocation.Path, "playlist.json");

                try
                {
                    // try to open and read it
                    StorageFile defaultFile = await StorageFile.GetFileFromPathAsync(defaultPath);
                    string fileContents = await FileIO.ReadTextAsync(defaultFile);

                    // if we got some data
                    if (!String.IsNullOrWhiteSpace(fileContents))
                    {
                        // deserialize it
                        DefaultPlaylist = JsonConvert.DeserializeObject<Playlist>(fileContents);
                    }
                }
                catch (FileNotFoundException)
                {

                }

                // if we failed to load the defaults
                if (null == DefaultPlaylist)
                {
                    // create them
                    DefaultPlaylist = Playlist.CreateDefault();
                }

                // find the first playlist file we can and load that as the current language
                IReadOnlyList<StorageFile> files = null;
                try
                {
                    // file spec pattern; playlist files are named "playlist.json"
                    string startsWith = "playlist";

                    // get the config object
                    ConfigurationService configurationService = (ConfigurationService)SimpleIoc.Default.GetInstance<ConfigurationService>();
                    if (null != configurationService)
                    {
                        // get the folder where our playlist files are stored
                        StorageFolder queryFolder = await configurationService.GetLocalStorageFolder();

                        // build query for json files
                        QueryOptions options = new QueryOptions(CommonFileQuery.OrderByName, new List<string>() { ".json" });
                        StorageFileQueryResult query = queryFolder.CreateFileQueryWithOptions(options);

                        // execute the query
                        files = await query.GetFilesAsync();

                        // if we got results
                        if ((null != files) && (files.Count > 0))
                        {
                            // loop through them
                            foreach (StorageFile file in files)
                            {
                                // find the first that matches our app prefix and optional language spec
                                if (file.Name.StartsWith(startsWith, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    // read its contents
                                    string fileContents = await FileIO.ReadTextAsync(file);

                                    // deserialize into our language object
                                    CurrentPlaylist = JsonConvert.DeserializeObject<Playlist>(fileContents);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                // if we failed to load the current language
                if (null == CurrentPlaylist)
                {
                    // set it to the default
                    CurrentPlaylist = DefaultPlaylist;
                }
            }

            this.IsLoaded = true;
            this.IsLoading = false;
        }
    }
}
