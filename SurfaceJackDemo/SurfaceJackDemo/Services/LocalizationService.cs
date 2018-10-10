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

using SurfaceJackDemo.Models;
using SurfaceJackDemo.ViewModels;


namespace SurfaceJackDemo.Services
{
    public class LocalizationService
    {
        public static LocalizationService Current { get; private set; }

        private LanguageFile _languageDefault = null;
        private LanguageFile _languageCurrent = null;


        public LocalizationService()
        {
            // save this instance as current
            LocalizationService.Current = this;
        }

        public bool IsLoading { get; private set; }

        public bool IsLoaded { get; private set; }

        public string LanguageCode { get; private set; }

        public async Task Initialize()
        {
            if (this.IsLoading) { return; }

            this.IsLoading = true;

            if ((null == _languageDefault) || (null == _languageCurrent))
            {
                // default language file path
                string defaultPath = Path.Combine(Package.Current.InstalledLocation.Path, "default-en-US.json");

                try
                {
                    // try to open and read it
                    StorageFile defaultFile = await StorageFile.GetFileFromPathAsync(defaultPath);
                    string fileContents = await FileIO.ReadTextAsync(defaultFile);

                    // if we got some data
                    if (!String.IsNullOrWhiteSpace(fileContents))
                    {
                        // deserialize it
                        _languageDefault = JsonConvert.DeserializeObject<LanguageFile>(fileContents);
                    }
                }
                catch (FileNotFoundException)
                {

                }

                // if we failed to load the defaults
                if (null == _languageDefault)
                {
                    // create them
                    _languageDefault = LanguageFile.CreateDefault();
                }

                // find the first language file we can and load that as the current language
                IReadOnlyList<StorageFile> files = null;
                try
                {
                    // file spec pattern; language files all start with SDA-
                    string startsWith = "SDA-";

                    // get the config object
                    ConfigurationService configurationService = (ConfigurationService)SimpleIoc.Default.GetInstance<ConfigurationService>();
                    if (null != configurationService)
                    {
                        // is a language specified?
                        if (!String.IsNullOrWhiteSpace(configurationService.Configuration.Language))
                        {
                            // add the language to the file spec (e.g. en-US, en-CA, fr-CA)
                            startsWith += configurationService.Configuration.Language;
                        }

                        // get the folder where our language files are stored
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
                                    _languageCurrent = JsonConvert.DeserializeObject<LanguageFile>(fileContents);

                                    // set the language code of this file
                                    // filename format spec is "SDA-<ISO LANGUAGE (2)>-<ISO COUNTRY (2)>.json"
                                    string name = file.Name;
                                    if (name.Length > 4)
                                    {
                                        // remove "SDA-"
                                        name = name.Substring(4);
                                    }
                                    // find the .json extension
                                    int crop = name.IndexOf(".json");
                                    if (-1 != crop)
                                    {
                                        // crop it off
                                        name = name.Substring(0, crop);
                                    }
                                    // save our name and break out of the foreach
                                    this.LanguageCode = name;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                // if we failed to load the current language
                if (null == _languageCurrent)
                {
                    // set it to the default
                    _languageCurrent = _languageDefault;
                }
            }

            this.IsLoaded = true;
            this.IsLoading = false;
        }

        public bool IsLanguageJapanese()
        {
            bool retVal = false;

            if (0 == String.Compare(this.LanguageCode, "ja-JP", StringComparison.InvariantCultureIgnoreCase))
            {
                retVal = true;
            }

            return retVal;
        }

        public void LoadChoosePathViewModel(ChoosePathViewModel viewModel)
        {
            viewModel.DeviceOneTitle = GetStringValue(_languageCurrent.INTRO_TITLE_DEVICE_ONE_TITLE, "INTRO_TITLE_DEVICE_ONE_TITLE");
            viewModel.DeviceOneCTA = GetStringValueToUpper(_languageCurrent.INTRO_TITLE_DEVICE_ONE_CTA, "INTRO_TITLE_DEVICE_ONE_CTA");
            viewModel.DeviceTwoTitle = GetStringValue(_languageCurrent.INTRO_TITLE_DEVICE_TWO_TITLE, "INTRO_TITLE_DEVICE_TWO_TITLE");
            viewModel.DeviceTwoCTA = GetStringValueToUpper(_languageCurrent.INTRO_TITLE_DEVICE_TWO_CTA, "INTRO_TITLE_DEVICE_TWO_CTA");
        }

        public void LoadFlipViewViewModel(FlipViewViewModel viewModel)
        {
            viewModel.NavBarAudio = GetStringValueToUpper(_languageCurrent.AUDIO, "AUDIO");
            viewModel.NavBarDesign = GetStringValueToUpper(_languageCurrent.DESIGN, "DESIGN");
            viewModel.NavBarTech = GetStringValueToUpper(_languageCurrent.TECH, "TECH");
            viewModel.NavBarProductivity = GetStringValueToUpper(_languageCurrent.PRODUCTIVITY, "PRODUCTIVITY");
            viewModel.NavBarSpecs = GetStringValueToUpper(_languageCurrent.SPECS, "SPECS");
        }

        public void LoadAudioTryItViewModel(AudioTryItViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_GATEWAY_HEADLINE, "AUDIO_GATEWAY_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.AUDIO_GATEWAY_COPY, "AUDIO_GATEWAY_COPY");
            viewModel.CTA = GetStringValue(_languageCurrent.AUDIO_GATEWAY_CTA, "AUDIO_GATEWAY_CTA");
        }

        public void LoadAudioListenViewModel(AudioListenViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_TRACK_HEADLINE, "AUDIO_TRACK_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.AUDIO_TRACK_COPY, "AUDIO_TRACK_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.AUDIO_TRACK_LEGAL, "AUDIO_TRACK_LEGAL");
        }

        public void LoadDesignViewModel(DesignViewModel viewModel)
        {

        }

        public void LoadTechViewModel(TechViewModel viewModel)
        {

        }

        public void LoadProductivityViewModel(ProductivityViewModel viewModel)
        {

        }

        public void LoadSpecsViewModel(SpecsViewModel viewModel)
        {

        }

        public void LoadPartnerViewModel(PartnerViewModel viewModel)
        {

        }

        private string GetStringValue(string value, string name)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Format("Missing translation for {0}", name);
            }
            else
            {
                return value;
            }
        }

        private string GetStringValueToUpper(string value, string name)
        {
            if (null != value)
            {
                value = value.ToUpper();
            }

            return value;
        }

        private int GetIntValue(string value, int defaultValue)
        {
            int retVal = 0;

            if (!Int32.TryParse(value, out retVal))
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        private double GetDoubleValue(string value, double defaultValue)
        {
            double retVal;

            if (!Double.TryParse(value, out retVal))
            {
                retVal = defaultValue;
            }

            return retVal;
        }
    }
}
