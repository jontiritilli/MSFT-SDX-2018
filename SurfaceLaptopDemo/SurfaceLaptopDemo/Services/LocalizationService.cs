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

using SurfaceLaptopDemo.Models;
using SurfaceLaptopDemo.ViewModels;
using SDX.Toolkit.Controls;


namespace SurfaceLaptopDemo.Services
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

        public void LoadFlipViewViewModel(FlipViewViewModel viewModel)
        {
            viewModel.NavBarExperience = GetStringValueToUpper(_languageCurrent.EXPERIENCE, "EXPERIENCE");
            viewModel.NavBarAccessories = GetStringValueToUpper(_languageCurrent.ACCESSORIES, "ACCESSORIES");
            viewModel.NavBarBestOfMicrosoft = GetStringValueToUpper(_languageCurrent.BEST, "BEST");
            viewModel.NavBarCompare = GetStringValueToUpper(_languageCurrent.COMPARISON, "COMPARISON");
        }

        public void LoadExperienceHeroViewModel(ExperienceHeroViewModel viewModel)
        {
            viewModel.HeroText = GetStringValue(_languageCurrent.INTRO_TITLE, "INTRO_TITLE");
            viewModel.RowCount = GetIntValue(_languageCurrent.INTRO_ROWCOUNT, 1);
            viewModel.SwipeText = GetStringValue(_languageCurrent.INTRO_SWIPE, "INTRO_SWIPE");
        }

        public void LoadExperienceColorsViewModel(ExperienceColorsViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_STYLE_HEADLINE, "EXPERIENCE_STYLE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_STYLE_COPY, "EXPERIENCE_STYLE_COPY");
        }

        public void LoadExperienceInnovationViewModel(ExperienceInnovationViewModel viewModel)
        {

        }

        public void LoadExperiencePerformanceViewModel(ExperiencePerformanceViewModel viewModel)
        {

        }

        public void LoadExperienceSpeakersViewModel(ExperienceSpeakersViewModel viewModel)
        {

        }

        public void LoadExperienceSleekViewModel(ExperienceSleekViewModel viewModel)
        {

        }

        public void LoadAccessoriesTouchViewModel(AccessoriesTouchViewModel viewModel)
        {

        }

        public void LoadAccessoriesMouseViewModel(AccessoriesMouseViewModel viewModel)
        {

        }

        public void LoadAccessoriesPenViewModel(AccessoriesPenViewModel viewModel)
        {

        }

        public void LoadBestOfMicrosoftViewModel(BestOfMicrosoftViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.BEST_HEADLINE, "BEST_HEADLINE");
            viewModel.Legal = GetStringValue(_languageCurrent.BEST_BULLET_THREE_LEGAL, "BEST_BULLET_THREE_LEGAL");
            viewModel.BulletOneCTA = GetStringValue(_languageCurrent.BEST_BULLET_ONE_CTA, "BEST_BULLET_ONE_CTA");
            viewModel.BulletTwoCTA = GetStringValue(_languageCurrent.BEST_BULLET_TWO_CTA, "BEST_BULLET_TWO_CTA");
            viewModel.BulletThreeCTA = GetStringValue(_languageCurrent.BEST_BULLET_THREE_CTA, "BEST_BULLET_THREE_CTA");
            viewModel.BulletFourCTA = GetStringValue(_languageCurrent.BEST_BULLET_FOUR_CTA, "BEST_BULLET_FOUR_CTA");

            // bullet one

            viewModel.LeftItemList[0] = ListItem.CreateListItem(
                0, // order
                ListItemIcon.Start, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_COPY, "") // order
                                                                          // null // custom icon path
            );

            // bullet two

            viewModel.LeftItemList[1] = ListItem.CreateListItem(
                1, // order
                ListItemIcon.Hello, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_COPY, "") // order
                                                                          // null // custom icon path
            );

            // bullet three OPTIONAL BULLET

            viewModel.LeftItemList[2] = ListItem.CreateListItem(
                2, // order
                ListItemIcon.Custom, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_COPY, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_COPY, "") // order
                                                                           // null // custom icon path
            );

            // bullet four

            viewModel.RightItemList[0] = ListItem.CreateListItem(
                0, // order
                ListItemIcon.Sync, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_COPY, "") // order
                                                                           // null // custom icon path
            );

            // bullet five

            viewModel.RightItemList[1] = ListItem.CreateListItem(
                1, // order
                ListItemIcon.Office, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_COPY, "") // order
                                                                            // null // custom icon path
            );
        }

        public void LoadCompareViewModel(CompareViewModel viewModel)
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
