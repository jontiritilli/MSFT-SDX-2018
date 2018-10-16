using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;

using GalaSoft.MvvmLight.Ioc;

using Newtonsoft.Json;

using YogaC930AudioDemo.Models;
using YogaC930AudioDemo.ViewModels;


namespace YogaC930AudioDemo.Services
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
                    // file spec pattern; language files all start with LADA-
                    string startsWith = "LADA-";

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
                                    // filename format spec is "LADA-<ISO LANGUAGE (2)>-<ISO COUNTRY (2)>.json"
                                    string name = file.Name;
                                    if (name.Length > 4)
                                    {
                                        // remove "LADA-"
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


        #region Load View Model Methods

        public void LoadAttractorLopViewModel(AttractorLoopViewModel viewModel)
        {
            viewModel.Title = GetStringValue(_languageCurrent.ATTRACTOR_TITLE, "ATTRACTOR_TITLE");
            viewModel.CTA = GetStringValue(_languageCurrent.ATTRACTOR_CTA, "ATTRACTOR_CTA");
        }

        public void LoadFlipViewViewModel(FlipViewViewModel viewModel)
        {
            viewModel.NavExplore = GetStringValue(_languageCurrent.NAV_EXPLORE, "NAV_EXPLORE");
            viewModel.NavDesktop = GetStringValue(_languageCurrent.NAV_GOTODESKTOP, "NAV_GOTODESKTOP");
            viewModel.NavAudioDemo = GetStringValue(_languageCurrent.AUDIO_PLAY_CTA, "AUDIO_PLAY_CTA");
        }

        public void LoadAudioViewModel(AudioViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_HEADLINE, "AUDIO_HEADLINE");
            viewModel.BodyFirst = GetStringValue(_languageCurrent.AUDIO_BODY_FIRST, "AUDIO_BODY_FIRST");
            viewModel.BodyBold = GetStringValue(_languageCurrent.AUDIO_BODY_BOLD, "AUDIO_BODY_BOLD");
            viewModel.BodyLast = GetStringValue(_languageCurrent.AUDIO_BODY_LAST, "AUDIO_BODY_LAST");
        }

        public void LoadFeaturesViewModel(FeaturesViewModel viewModel)
        {
            viewModel.PenHeadline = GetStringValue(_languageCurrent.FEATURES_PEN_HEADLINE, "FEATURES_PEN_HEADLINE");
            viewModel.PenBodyFirst = GetStringValue(_languageCurrent.FEATURES_PEN_BODY_FIRST, "FEATURES_PEN_BODY_FIRST");
            viewModel.PenBodyBold = GetStringValue(_languageCurrent.FEATURES_PEN_BODY_BOLD, "FEATURES_PEN_BODY_BOLD");
            viewModel.PenBodyLast = GetStringValue(_languageCurrent.FEATURES_PEN_BODY_LAST, "FEATURES_PEN_BODY_LAST");

            viewModel.InkHeadline = GetStringValue(_languageCurrent.FEATURES_INK_HEADLINE, "FEATURES_INK_HEADLINE");
            viewModel.InkBodyFirst = GetStringValue(_languageCurrent.FEATURES_INK_BODY_FIRST, "FEATURES_INK_BODY_FIRST");
            viewModel.InkBodyBold = GetStringValue(_languageCurrent.FEATURES_INK_BODY_BOLD, "FEATURES_INK_BODY_BOLD");
            viewModel.InkBodyLast = GetStringValue(_languageCurrent.FEATURES_INK_BODY_LAST, "FEATURES_INK_BODY_LAST");

            viewModel.WebCamHeadline = GetStringValue(_languageCurrent.FEATURES_WEBCAM_HEADLINE, "FEATURES_WEBCAM_HEADLINE");
            viewModel.WebCamBodyFirst = GetStringValue(_languageCurrent.FEATURES_WEBCAM_BODY_FIRST, "FEATURES_WEBCAM_BODY_FIRST");
            viewModel.WebCamBodyBold = GetStringValue(_languageCurrent.FEATURES_WEBCAM_BODY_BOLD, "FEATURES_WEBCAM_BODY_BOLD");
            viewModel.WebCamBodyLast = GetStringValue(_languageCurrent.FEATURES_WEBCAM_BODY_LAST, "FEATURES_WEBCAM_BODY_LAST");
        }

        public void LoadHingeDesignPopupViewModel(HingeDesignPopupViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_POP_RIGHT_HEADLINE, "AUDIO_POP_RIGHT_HEADLINE");
            viewModel.BodyFirst = GetStringValue(_languageCurrent.AUDIO_POP_RIGHT_BODY_FIRST, "AUDIO_POP_RIGHT_BODY_FIRST");
            viewModel.BodyBold = GetStringValue(_languageCurrent.AUDIO_POP_RIGHT_BODY_BOLD, "AUDIO_POP_RIGHT_BODY_BOLD");
            viewModel.BodyLast = GetStringValue(_languageCurrent.AUDIO_POP_RIGHT_BODY_LAST, "AUDIO_POP_RIGHT_BODY_LAST");
        }

        public void LoadSpeakerDesignPopupViewModel(SpeakerDesignPopupViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_POP_LEFT_HEADLINE, "AUDIO_POP_LEFT_HEADLINE");
            viewModel.Body = GetStringValue(_languageCurrent.AUDIO_POP_LEFT_BODY, "AUDIO_POP_LEFT_BODY");
        }

        public void LoadPlayerPopupViewModel(PlayerPopupViewModel viewModel)
        {
            viewModel.Body = GetStringValue(_languageCurrent.AUDIO_POP_DEMO_COPY, "AUDIO_POP_DEMO_COPY");
        }

        public void LoadSpeedsAndFeedsViewModel(SpeedsAndFeedsViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.PERFORMANCE_HEADLINE, "PERFORMANCE_HEADLINE");
            viewModel.Body = GetStringValue(_languageCurrent.PERFORMANCE_BODY, "PERFORMANCE_BODY");
            viewModel.Footnote = GetStringValue(_languageCurrent.PERFORMANCE_FOOTNOTE, "PERFORMANCE_FOOTNOTE");

            viewModel.TopColor = GetStringValue(_languageCurrent.PERFORMANCE_TOPDEVICE_COLOR, "PERFORMANCE_TOPDEVICE_COLOR");
            viewModel.BottomColor = GetStringValue(_languageCurrent.PERFORMANCE_BOTTOMDEVICE_COLOR, "PERFORMANCE_BOTTOMDEVICE_COLOR");

            viewModel.WindowsBodyFirst = GetStringValue(_languageCurrent.PERFORMANCE_WINDOWS_FIRST, "PERFORMANCE_WINDOWS_FIRST");
            viewModel.WindowsBodyBold = GetStringValue(_languageCurrent.PERFORMANCE_WINDOWS_BOLD, "PERFORMANCE_WINDOWS_BOLD");
            viewModel.WindowsBodyLast = GetStringValue(_languageCurrent.PERFORMANCE_WINDOWS_LAST, "PERFORMANCE_WINDOWS_LAST");

            viewModel.IntelBodyFirst = GetStringValue(_languageCurrent.PERFORMANCE_INTEL_FIRST, "PERFORMANCE_INTEL_FIRST");
            viewModel.IntelBodyBold = GetStringValue(_languageCurrent.PERFORMANCE_INTEL_BOLD, "PERFORMANCE_INTEL_BOLD");
            viewModel.IntelBodyLast = GetStringValue(_languageCurrent.PERFORMANCE_INTEL_LAST, "PERFORMANCE_INTEL_LAST");

            viewModel.DolbyBodyFirst = GetStringValue(_languageCurrent.PERFORMANCE_DOLBY_FIRST, "PERFORMANCE_DOLBY_FIRST");
            viewModel.DolbyBodyBold = GetStringValue(_languageCurrent.PERFORMANCE_DOLBY_BOLD, "PERFORMANCE_DOLBY_BOLD");
            viewModel.DolbyBodyLast = GetStringValue(_languageCurrent.PERFORMANCE_DOLBY_LAST, "PERFORMANCE_DOLBY_LAST");

            viewModel.FourKBodyFirst = GetStringValue(_languageCurrent.PERFORMANCE_4K_FIRST, "PERFORMANCE_4K_FIRST");
            viewModel.FourKBodyBold = GetStringValue(_languageCurrent.PERFORMANCE_4K_BOLD, "PERFORMANCE_4K_BOLD");
            viewModel.FourKBodyLast = GetStringValue(_languageCurrent.PERFORMANCE_4K_LAST, "PERFORMANCE_4K_LAST");

            viewModel.HoursBodyFirst = GetStringValue(_languageCurrent.PERFORMANCE_HOURS_FIRST, "PERFORMANCE_HOURS_FIRST");
            viewModel.HoursBodyBold = GetStringValue(_languageCurrent.PERFORMANCE_HOURS_BOLD, "PERFORMANCE_HOURS_BOLD");
            viewModel.HoursBodyLast = GetStringValue(_languageCurrent.PERFORMANCE_HOURS_LAST, "PERFORMANCE_HOURS_LAST");

            viewModel.FiftySevenBodyFirst = GetStringValue(_languageCurrent.PERFORMANCE_57_FIRST, "PERFORMANCE_57_FIRST");
            viewModel.FiftySevenBodyBold = GetStringValue(_languageCurrent.PERFORMANCE_57_BOLD, "PERFORMANCE_57_BOLD");
            viewModel.FiftySevenBodyLast = GetStringValue(_languageCurrent.PERFORMANCE_57_LAST, "PERFORMANCE_57_LAST");
        }

        #endregion


        #region Helper Methods

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

#endregion
    }
}
