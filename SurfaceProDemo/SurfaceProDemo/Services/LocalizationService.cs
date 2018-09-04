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

using SurfaceProDemo.Models;
using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Controls;

namespace SurfaceProDemo.Services
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

        public void LoadChoosePathViewModel(ChoosePathViewModel viewModel)
        {
            viewModel.ChooseSurfacePro = GetStringValue(_languageCurrent.INTRO_TITLE_DEVICE_ONE, "INTRO_TITLE_DEVICE_ONE");
            viewModel.ChooseSurfaceJack = GetStringValue(_languageCurrent.INTRO_TITLE_DEVICE_TWO, "INTRO_TITLE_DEVICE_TWO");
        }

        public void LoadExperienceHeroViewModel(ExperienceHeroViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_HEADLINE, "EXPERIENCE_TABLET_HEADLINE");
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_COPY, "EXPERIENCE_TABLET_COPY");
        }

        public void LoadExperienceIntroViewModel(ExperienceIntroViewModel viewModel)
        {

        }

        public void LoadExperienceTransformViewModel(ExperienceTransformViewModel viewModel)
        {

        }

        public void LoadExperiencePerformanceViewModel(ExperiencePerformanceViewModel viewModel)
        {

        }

        public void LoadExperienceQuietViewModel(ExperienceQuietViewModel viewModel)
        {

        }

        public void LoadAccessoriesPenViewModel(AccessoriesPenViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_HEADLINE, "ACCESSORIES_INTERACTIVE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_COPY, "ACCESSORIES_INTERACTIVE_COPY");

            viewModel.TryItTitle = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_TITLE, "ACCESSORIES_INTERACTIVE_TRYIT_TITLE");
            viewModel.TryItLede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_COPY, "ACCESSORIES_INTERACTIVE_TRYIT_COPY");

            // TODO: add code to load list
            // bullet one

            viewModel.ListItems[0] = (ListItem.CreateListItem(
                0, // order
                ListItemIcon.Jot,
                viewModel.ICON_WIDTH, // width
                "", // header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_ONE, "ACCESSORIES_INTERACTIVE_BULLET_ONE") // order
            ));

            // bullet one

            viewModel.ListItems[1] = (ListItem.CreateListItem(
                1, // order
                ListItemIcon.Write, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_TWO, "ACCESSORIES_INTERACTIVE_BULLET_TWO") // order
            ));

            viewModel.ListItems[2] = (ListItem.CreateListItem(
                2, // order
                ListItemIcon.Pressure, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_THREE, "ACCESSORIES_INTERACTIVE_BULLET_THREE") // order
            ));

            viewModel.ListItems[3] = (ListItem.CreateListItem(
                3, // order
                ListItemIcon.Palm, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",// header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_FOUR, "ACCESSORIES_INTERACTIVE_BULLET_FOUR") // order
            ));

        }

        public void LoadAccessoriesKeyboardViewModel(AccessoriesKeyboardViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_HEADLINE, "ACCESSORIES_LEFT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_COPY, "ACCESSORIES_LEFT_COPY");

            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_COPY, "ACCESSORIES_LEFT_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_LEGAL, "ACCESSORIES_LEFT_POPLEFT_LEGAL");

            viewModel.PopTopHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");
            viewModel.PopTopLede= GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");
            viewModel.PopTopLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");


        }

        public void LoadAccessoriesMouseViewModel(AccessoriesMouseViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_HEADLINE, "ACCESSORIES_RIGHT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_COPY, "ACCESSORIES_RIGHT_COPY");

            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPLEFT_TITLE, "ACCESSORIES_RIGHT_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPLEFT_COPY, "ACCESSORIES_RIGHT_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPLEFT_LEGAL, "ACCESSORIES_RIGHT_POPLEFT_LEGAL");

            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPRIGHT_TITLE, "ACCESSORIES_RIGHT_POPRIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPRIGHT_TITLE, "ACCESSORIES_RIGHT_POPRIGHT_TITLE");
            viewModel.PopRightLegal = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPRIGHT_TITLE, "ACCESSORIES_RIGHT_POPRIGHT_TITLE");

        }

        public void LoadBestOfMicrosoftViewModel(BestOfMicrosoftViewModel viewModel)
        {

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
