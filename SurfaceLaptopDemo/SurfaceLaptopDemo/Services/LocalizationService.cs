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
            viewModel.PopupHeadline = GetStringValue(_languageCurrent.EXPERIENCE_STYLE_POP_TITLE, "EXPERIENCE_STYLE_POP_TITLE");
            viewModel.PopupLede = GetStringValue(_languageCurrent.EXPERIENCE_STYLE_POP_COPY, "EXPERIENCE_STYLE_POP_COPY");
        }

        public void LoadExperienceInnovationViewModel(ExperienceInnovationViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_HEADLINE, "EXPERIENCE_DESIGN_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_COPY, "EXPERIENCE_DESIGN_COPY");

            viewModel.TryIt = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_TRYIT, "EXPERIENCE_DESIGN_TRYIT");
            viewModel.TryIt_Headline = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_TRYIT_POP_TITLE, "EXPERIENCE_DESIGN_TRYIT_POP_TITLE");
            viewModel.TryIt_Lede = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_TRYIT_POP_COPY, "EXPERIENCE_DESIGN_TRYIT_POP_COPY");

            viewModel.PopTop_Headline = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_POPTOP_TITLE, "EXPERIENCE_DESIGN_POPTOP_TITLE");
            viewModel.PopTop_Lede = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_POPTOP_COPY, "EXPERIENCE_DESIGN_POPTOP_COPY");

            viewModel.PopRight_Headline = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_POPRIGHT_TITLE, "EXPERIENCE_DESIGN_POPRIGHT_TITLE");
            viewModel.PopRight_Lede = GetStringValue(_languageCurrent.EXPERIENCE_DESIGN_POPRIGHT_COPY, "EXPERIENCE_DESIGN_POPRIGHT_COPY");
        }

        public void LoadExperiencePerformanceViewModel(ExperiencePerformanceViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_DOMORE_HEADLINE, "EXPERIENCE_DOMORE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_DOMORE_COPY, "EXPERIENCE_DOMORE_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.EXPERIENCE_DOMORE_LEGAL, "EXPERIENCE_DOMORE_LEGAL");
        }

        public void LoadExperienceSpeakersViewModel(ExperienceSpeakersViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_ENJOYMORE_HEADLINE, "EXPERIENCE_ENJOYMORE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_ENJOYMORE_COPY, "EXPERIENCE_ENJOYMORE_COPY");
        }

        public void LoadExperienceSleekViewModel(ExperienceSleekViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_POWERFUL_HEADLINE, "EXPERIENCE_POWERFUL_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_POWERFUL_COPY, "EXPERIENCE_POWERFUL_COPY");

            viewModel.PopRight_Headline = GetStringValue(_languageCurrent.EXPERIENCE_POPRIGHT_TITLE, "EXPERIENCE_POPRIGHT_TITLE");
            viewModel.PopRight_Lede = GetStringValue(_languageCurrent.EXPERIENCE_POPRIGHT_COPY, "EXPERIENCE_POPRIGHT_COPY");

            viewModel.PopLeft_Headline = GetStringValue(_languageCurrent.EXPERIENCE_POPLEFT_TITLE, "EXPERIENCE_POPLEFT_TITLE");
            viewModel.PopLeft_Lede = GetStringValue(_languageCurrent.EXPERIENCE_POPLEFT_COPY, "EXPERIENCE_POPLEFT_COPY");

            viewModel.PopBottom_Headline = GetStringValue(_languageCurrent.EXPERIENCE_POPBOTTOM_TITLE, "EXPERIENCE_POPBOTTOM_TITLE");
            viewModel.PopBottom_Lede = GetStringValue(_languageCurrent.EXPERIENCE_POPBOTTOM_COPY, "EXPERIENCE_POPBOTTOM_COPY");
            viewModel.PopBottom_Legal = GetStringValue(_languageCurrent.EXPERIENCE_POPBOTTOM_LEGAL, "EXPERIENCE_POPBOTTOM_LEGAL");
        }

        public void LoadAccessoriesTouchViewModel(AccessoriesTouchViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_HEADLINE, "ACCESSORIES_INTERACTIVE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_COPY, "ACCESSORIES_INTERACTIVE_COPY");

            viewModel.TryIt_Headline = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_TITLE, "EXPERIENCE_DESIGN_TRYIT_POP_TITLE");
            viewModel.TryIt_Lede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_COPY, "EXPERIENCE_DESIGN_TRYIT_POP_COPY");
        }

        public void LoadAccessoriesMouseViewModel(AccessoriesMouseViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_HEADLINE, "ACCESSORIES_LEFT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_COPY, "ACCESSORIES_LEFT_COPY");

            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_COPY, "ACCESSORIES_LEFT_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_LEGAL, "ACCESSORIES_LEFT_POPLEFT_LEGAL");

            viewModel.PopCenterHeadline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPCENTER_TITLE, "ACCESSORIES_RIGHT_POPCENTER_TITLE");
            viewModel.PopCenterLede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPCENTER_COPY, "ACCESSORIES_RIGHT_POPCENTER_COPY");

            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPRIGHT_TITLE, "ACCESSORIES_LEFT_POPRIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPRIGHT_COPY, "ACCESSORIES_LEFT_POPLEFT_COPY");

        }

        public void LoadAccessoriesPenViewModel(AccessoriesPenViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_HEADLINE, "ACCESSORIES_LEFT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_COPY, "ACCESSORIES_LEFT_COPY");
        }

        public void LoadBestOfMicrosoftViewModel(BestOfMicrosoftViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.BEST_HEADLINE, "BEST_HEADLINE");
            viewModel.Legal = GetStringValue(_languageCurrent.BEST_BULLET_THREE_LEGAL, "BEST_BULLET_THREE_LEGAL");

            // bullet one
            viewModel.LeftItemList.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Start, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_TITLE, "BEST_BULLET_ONE_TITLE"), // headline
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_COPY, "BEST_BULLET_ONE_COPY"), // lede
                null, //CTA URI
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_CTA, "BEST_BULLET_ONE_CTA") //CTA Text
            ));

            // bullet two
            viewModel.LeftItemList.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Hello,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_TITLE, "BEST_BULLET_TWO_TITLE"),
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_COPY, "BEST_BULLET_TWO_COPY"),
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_CTA, "BEST_BULLET_TWO_CTA")
            ));

            // bullet three OPTIONAL CHINA BULLET
            viewModel.LeftItemList.Add(ListItem.CreateListItem(
                2,
                ListItemIcon.Custom,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_TITLE, "BEST_BULLET_FIVE_TITLE"),
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_COPY, "BEST_BULLET_FIVE_COPY"),
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_CTA, "BEST_BULLET_FIVE_CTA")
            ));

            // bullet four
            viewModel.RightItemList.Add(ListItem.CreateListItem(
                0,
                ListItemIcon.Sync,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_TITLE, "BEST_BULLET_THREE_TITLE"),
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_COPY, "BEST_BULLET_THREE_COPY"),
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_CTA, "BEST_BULLET_THREE_CTA")
            ));

            // bullet five
            viewModel.RightItemList.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Office,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_TITLE, "BEST_BULLET_FOUR_TITLE"),
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_COPY, "BEST_BULLET_FOUR_COPY"),
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_CTA, "BEST_BULLET_FOUR_CTA")
            ));
        }

        public void LoadCompareViewModel(CompareViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.COMPARE_HEADLINE, "COMPARE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.COMPARE_COPY, "COMPARE_COPY");
            viewModel.ProTitle = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_TITLE, "COMPARE_DEVICE_ONE_TITLE");
            viewModel.BookTitle = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_TITLE, "COMPARE_DEVICE_TWO_TITLE");
            viewModel.StudioTitle = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_TITLE, "COMPARE_DEVICE_THREE_TITLE");
            viewModel.LaptopTitle = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_TITLE, "COMPARE_DEVICE_FOUR_TITLE");
            viewModel.GoTitle = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_TITLE, "COMPARE_DEVICE_FIVE_TITLE");

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
