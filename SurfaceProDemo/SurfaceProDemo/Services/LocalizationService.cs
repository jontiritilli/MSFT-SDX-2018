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
            viewModel.HeroText = GetStringValue(_languageCurrent.INTRO_TITLE, "INTRO_TITLE");
            viewModel.RowCount = GetIntValue(_languageCurrent.INTRO_ROWCOUNT, 2);
            viewModel.SwipeText = GetStringValue(_languageCurrent.INTRO_SWIPE, "INTRO_SWIPE");
        }

        public void LoadExperienceIntroViewModel(ExperienceIntroViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_HEADLINE, "EXPERIENCE_OVERVIEW_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_COPY, "EXPERIENCE_OVERVIEW_COPY");
            viewModel.SwipeText = GetStringValue(_languageCurrent.SWIPE_CONTINUE, "SWIPE_CONTINUE");

            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPLEFT_TITLE, "EXPERIENCE_OVERVIEW_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPLEFT_COPY, "EXPERIENCE_OVERVIEW_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPLEFT_LEGAL, "EXPERIENCE_OVERVIEW_POPLEFT_LEGAL");
            viewModel.PopTopHeadline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPTOP_TITLE, "EXPERIENCE_OVERVIEW_POPTOP_TITLE");
            viewModel.PopTopLede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPTOP_COPY, "EXPERIENCE_OVERVIEW_POPTOP_COPY");
            viewModel.PopTopLegal = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPTOP_LEGAL, "EXPERIENCE_OVERVIEW_POPTOP_LEGAL");
            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPRIGHT_TITLE, "EXPERIENCE_OVERVIEW_POPTOP_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPRIGHT_COPY, "EXPERIENCE_OVERVIEW_POPTOP_COPY");
        }

        public void LoadExperienceFlipViewViewModel(ExperienceFlipViewViewModel viewModel)
        {
            viewModel.StudioCaptionText = GetStringValue(_languageCurrent.STUDIO, "STUDIO");
            viewModel.LaptopCaptionText = GetStringValue(_languageCurrent.LAPTOP, "LAPTOP");
            viewModel.TabletCaptionText = GetStringValue(_languageCurrent.TABLET, "TABLET");
        }

        public void LoadExperienceTransformViewModel(ExperienceTransformViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_STUDIO_HEADLINE, "EXPERIENCE_STUDIO_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_STUDIO_COPY, "EXPERIENCE_STUDIO_COPY");
            viewModel.PopHeadline = GetStringValue(_languageCurrent.EXPERIENCE_STUDIO_TRYIT_POP_TITLE, "EXPERIENCE_STUDIO_TRYIT_POP_TITLE");
            viewModel.PopLede = GetStringValue(_languageCurrent.EXPERIENCE_STUDIO_TRYIT_POP_COPY, "EXPERIENCE_STUDIO_TRYIT_POP_COPY");
            viewModel.PopTryItCaption = GetStringValue(_languageCurrent.EXPERIENCE_STUDIO_TRYIT, "EXPERIENCE_STUDIO_TRYIT");
        }

        public void LoadExperiencePerformanceViewModel(ExperiencePerformanceViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_HEADLINE, "EXPERIENCE_STUDIO_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_COPY, "EXPERIENCE_STUDIO_COPY");
            viewModel.PopTryItHeadline = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_TRYIT_POP_TITLE, "EXPERIENCE_STUDIO_TRYIT_POP_TITLE");
            viewModel.PopTryItLede = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_TRYIT_POP_COPY, "EXPERIENCE_STUDIO_TRYIT_POP_COPY");

            viewModel.PopCenterHeadline = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_HEADLINE, "EXPERIENCE_LAPTOP_POPCENTER_HEADLINE");
            viewModel.PopCenterLede = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_COPY, "EXPERIENCE_LAPTOP_POPCENTER_COPY");
            viewModel.PopCenterBulletOne = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_ONE, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_ONE");
            viewModel.PopCenterBulletTwo = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_TWO, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_TWO");
            viewModel.PopCenterBulletThree = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_THREE, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_THREE");
            viewModel.PopCenterBulletFour = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_FOUR, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_FOUR");
            viewModel.PopCenterFive = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_FIVE, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_FIVE");
            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPRIGHT_TITLE, "EXPERIENCE_LAPTOP_POPRIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPRIGHT_COPY, "EXPERIENCE_LAPTOP_POPRIGHT_COPY");
        }

        public void LoadExperienceQuietViewModel(ExperienceQuietViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_HEADLINE, "EXPERIENCE_TABLET_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_COPY, "EXPERIENCE_TABLET_COPY");
            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPLEFT_TITLE, "EXPERIENCE_TABLET_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPLEFT_COPY, "EXPERIENCE_TABLET_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPLEFT_LEGAL, "EXPERIENCE_TABLET_POPLEFT_COPY");
            viewModel.PopTopHeadline = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPTOP_TITLE, "EXPERIENCE_TABLET_POPTOP_TITLE");
            viewModel.PopTopLede = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPTOP_COPY, "EXPERIENCE_TABLET_POPTOP_COPY");
            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPRIGHT_TITLE, "EXPERIENCE_TABLET_POPRIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.EXPERIENCE_TABLET_POPRIGHT_COPY, "EXPERIENCE_TABLET_POPRIGHT_COPY");
        }

        public void LoadExperiencePopupViewModel(ExperiencePopupViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_HEADLINE, "EXPERIENCE_LAPTOP_POPCENTER_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_COPY, "EXPERIENCE_LAPTOP_POPCENTER_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_LEGAL, "EXPERIENCE_LAPTOP_POPCENTER_LEGAL");

            viewModel.appSelectorData[0].Message = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_ONE, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_ONE");
            viewModel.appSelectorData[1].Message = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_TWO, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_TWO");
            viewModel.appSelectorData[2].Message = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_THREE, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_THREE");
            viewModel.appSelectorData[3].Message = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_FOUR, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_FOUR");
            viewModel.appSelectorData[4].Message = GetStringValue(_languageCurrent.EXPERIENCE_LAPTOP_POPCENTER_BULLET_FIVE, "EXPERIENCE_LAPTOP_POPCENTER_BULLET_FIVE");

        }

        public void LoadAccessoriesPenViewModel(AccessoriesPenViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_HEADLINE, "ACCESSORIES_INTERACTIVE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_COPY, "ACCESSORIES_INTERACTIVE_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_LEGAL, "ACCESSORIES_INTERACTIVE_LEGAL");
            viewModel.TryIt = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_TITLE, "ACCESSORIES_INTERACTIVE_TRYIT_TITLE");
            viewModel.TryItCaption = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_COPY, "ACCESSORIES_INTERACTIVE_TRYIT_COPY");            
            // TODO: add code to load list
            // bullet one

            viewModel.ListItems.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Jot,
                viewModel.ICON_WIDTH, // width
                "", // header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_ONE, "ACCESSORIES_INTERACTIVE_BULLET_ONE") // order
            ));

            // bullet one

            viewModel.ListItems.Add(ListItem.CreateListItem(
                1, // order
                ListItemIcon.Write, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_TWO, "ACCESSORIES_INTERACTIVE_BULLET_TWO") // order
            ));

            viewModel.ListItems.Add(ListItem.CreateListItem(
                2, // order
                ListItemIcon.Pressure, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_THREE, "ACCESSORIES_INTERACTIVE_BULLET_THREE") // order
            ));

            viewModel.ListItems.Add(ListItem.CreateListItem(
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

            viewModel.PopBottomHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");
            viewModel.PopBottomLede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_COPY, "ACCESSORIES_LEFT_POPLEFT_COPY");
            viewModel.PopBottomLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_LEGAL, "ACCESSORIES_LEFT_POPLEFT_LEGAL");

            viewModel.PopTopHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_TITLE, "ACCESSORIES_LEFT_POPTOP_TITLE");
            viewModel.PopTopLede= GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_COPY, "ACCESSORIES_LEFT_POPTOP_COPY");
            viewModel.PopTopLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_LEGAL, "ACCESSORIES_LEFT_POPTOP_LEGAL");


        }

        public void LoadAccessoriesMouseViewModel(AccessoriesMouseViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_HEADLINE, "ACCESSORIES_RIGHT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_COPY, "ACCESSORIES_RIGHT_COPY");

            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPLEFT_TITLE, "ACCESSORIES_RIGHT_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPLEFT_COPY, "ACCESSORIES_RIGHT_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPLEFT_LEGAL, "ACCESSORIES_RIGHT_POPLEFT_LEGAL");

            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPRIGHT_TITLE, "ACCESSORIES_RIGHT_POPRIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPRIGHT_COPY, "ACCESSORIES_RIGHT_POPRIGHT_COPY");
            viewModel.PopRightLegal = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPRIGHT_LEGAL, "ACCESSORIES_RIGHT_POPRIGHT_LEGAL");

        }

        public void LoadBestOfMicrosoftViewModel(BestOfMicrosoftViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.BEST_HEADLINE, "BEST_HEADLINE");
            viewModel.LegalBulletOne = GetStringValue(_languageCurrent.BEST_BULLET_ONE_LEGAL, "BEST_BULLET_ONE_LEGAL");
            viewModel.LegalBulletTwo = GetStringValue(_languageCurrent.BEST_BULLET_TWO_LEGAL, "BEST_BULLET_TWO_LEGAL");
            viewModel.LegalBulletThree = GetStringValue(_languageCurrent.BEST_BULLET_THREE_LEGAL, "BEST_BULLET_THREE_LEGAL");
            viewModel.LegalBulletFour = GetStringValue(_languageCurrent.BEST_BULLET_FOUR_LEGAL, "BEST_BULLET_FOUR_LEGAL");
            viewModel.LegalBulletFive = GetStringValue(_languageCurrent.BEST_BULLET_FIVE_LEGAL, "BEST_BULLET_FIVE_LEGAL");
            viewModel.BulletOneCTA = GetStringValue(_languageCurrent.BEST_BULLET_ONE_CTA, "BEST_BULLET_ONE_CTA");
            viewModel.BulletTwoCTA = GetStringValue(_languageCurrent.BEST_BULLET_TWO_CTA, "BEST_BULLET_TWO_CTA");
            viewModel.BulletThreeCTA = GetStringValue(_languageCurrent.BEST_BULLET_THREE_CTA, "BEST_BULLET_THREE_CTA");
            viewModel.BulletFourCTA = GetStringValue(_languageCurrent.BEST_BULLET_FOUR_CTA, "BEST_BULLET_FOUR_CTA");

            // bullet one
            viewModel.ItemList.Add( ListItem.CreateListItem(
                0, // order
                ListItemIcon.Start, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_CTA, "BEST_BULLET_ONE_CTA")  // order
                                                                                             // null // custom icon path
            ));

            // bullet two
            viewModel.ItemList.Add(ListItem.CreateListItem(
                1, // order
                ListItemIcon.Sync, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_CTA, "BEST_BULLET_TWO_CTA")  // order
                                                                                                 // null // custom icon path
            ));

            // bullet four
            viewModel.ItemList.Add(ListItem.CreateListItem(
                2, // order
                ListItemIcon.Hello, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_CTA, "BEST_BULLET_THREE_CTA") // order

            // null // custom icon path
            ));

            // bullet five
            viewModel.ItemList.Add(ListItem.CreateListItem(
                3, // order
                ListItemIcon.Office, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_CTA, "BEST_BULLET_FOUR_CTA")
            // null // custom icon path
            ));

            // bullet three OPTIONAL BULLET
            if (!string.IsNullOrWhiteSpace(_languageCurrent.BEST_BULLET_FIVE_TITLE)
                && !string.IsNullOrWhiteSpace(_languageCurrent.BEST_BULLET_FIVE_COPY))
            {
                viewModel.ItemList.Add(ListItem.CreateListItem(
                4, // order
                ListItemIcon.Custom, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_FIVE_CTA, "BEST_BULLET_FIVE_CTA")
            // null // custom icon path
            ));
            }
            
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

        // compare page one
        public void LoadComparePopupProViewModel(ComparePopupProViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_HEADLINE, "COMPARE_DEVICE_ONE_POP_HEADLINE");
            viewModel.SubHead = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_SUBHEAD, "COMPARE_DEVICE_ONE_POP_SUBHEAD");
            viewModel.Lede = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_COPY, "COMPARE_DEVICE_ONE_POP_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_COPY_LEGAL, "COMPARE_DEVICE_ONE_POP_COPY_LEGAL");
            viewModel.BulletOneLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_ONE_LEGAL, "COMPARE_DEVICE_ONE_POP_BULLET_ONE_LEGAL");
            viewModel.BulletTwoLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_TWO_LEGAL, "COMPARE_DEVICE_ONE_POP_BULLET_TWO_TITLE");
            viewModel.BulletThreeLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_THREE_LEGAL, "COMPARE_DEVICE_ONE_POP_BULLET_THREE_LEGAL");
            viewModel.BulletFourLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_FOUR_LEGAL, "COMPARE_DEVICE_ONE_POP_BULLET_FOUR_LEGAL");
            viewModel.BulletFiveLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_FIVE_LEGAL, "COMPARE_DEVICE_ONE_POP_BULLET_FIVE_LEGAL");

            // bullet one
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Performance, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_ONE_TITLE, "COMPARE_DEVICE_ONE_POP_BULLET_ONE_TITLE"), // headline
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_ONE_COPY, "COMPARE_DEVICE_ONE_POP_BULLET_ONE_COPY") // lede
            ));

            // bullet two
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Wifi,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_TWO_TITLE, "COMPARE_DEVICE_ONE_POP_BULLET_TWO_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_TWO_COPY, "COMPARE_DEVICE_ONE_POP_BULLET_TWO_COPY")
            ));

            // bullet three
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                2,
                ListItemIcon.Battery,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_THREE_TITLE, "COMPARE_DEVICE_ONE_POP_BULLET_THREE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_THREE_COPY, "COMPARE_DEVICE_ONE_POP_BULLET_THREE_COPY")
            ));

            // bullet four
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                3,
                ListItemIcon.Display,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_FOUR_TITLE, "COMPARE_DEVICE_ONE_POP_BULLET_FOUR_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_FOUR_COPY, "COMPARE_DEVICE_ONE_POP_BULLET_FOUR_COPY")
            ));

            // bullet five
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                4,
                ListItemIcon.Sound,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_FIVE_TITLE, "COMPARE_DEVICE_ONE_POP_BULLET_FIVE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_ONE_POP_BULLET_FIVE_COPY, "COMPARE_DEVICE_ONE_POP_BULLET_FIVE_COPY")
            ));
        }

        // compare page two
        public void LoadComparePopupBookViewModel(ComparePopupBookViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_HEADLINE, "COMPARE_DEVICE_TWO_POP_HEADLINE");
            viewModel.SubHead = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_SUBHEAD, "COMPARE_DEVICE_TWO_POP_SUBHEAD");
            viewModel.Lede = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_COPY, "COMPARE_DEVICE_TWO_POP_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_COPY_LEGAL, "COMPARE_DEVICE_TWO_POP_COPY_LEGAL");
            viewModel.BulletOneLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_ONE_LEGAL, "COMPARE_DEVICE_TWO_POP_BULLET_ONE_LEGAL");
            viewModel.BulletTwoLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_TWO_LEGAL, "COMPARE_DEVICE_TWO_POP_BULLET_TWO_TITLE");
            viewModel.BulletThreeLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_THREE_LEGAL, "COMPARE_DEVICE_TWO_POP_BULLET_THREE_LEGAL");
            viewModel.BulletFourLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_FOUR_LEGAL, "COMPARE_DEVICE_TWO_POP_BULLET_FOUR_LEGAL");
            viewModel.BulletFiveLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_FIVE_LEGAL, "COMPARE_DEVICE_TWO_POP_BULLET_FIVE_LEGAL");

            // bullet one
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Design, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_ONE_TITLE, "COMPARE_DEVICE_TWO_POP_BULLET_ONE_TITLE"), // headline
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_ONE_COPY, "COMPARE_DEVICE_TWO_POP_BULLET_ONE_COPY") // lede
            ));

            // bullet two
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Battery,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_TWO_TITLE, "COMPARE_DEVICE_TWO_POP_BULLET_TWO_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_TWO_COPY, "COMPARE_DEVICE_TWO_POP_BULLET_TWO_COPY")
            ));

            // bullet three
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                2,
                ListItemIcon.ScreenSize,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_THREE_TITLE, "COMPARE_DEVICE_TWO_POP_BULLET_THREE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_THREE_COPY, "COMPARE_DEVICE_TWO_POP_BULLET_THREE_COPY")
            ));

            // bullet four
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                3,
                ListItemIcon.Laptop,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_FOUR_TITLE, "COMPARE_DEVICE_TWO_POP_BULLET_FOUR_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_FOUR_COPY, "COMPARE_DEVICE_TWO_POP_BULLET_FOUR_COPY")
            ));

            // bullet five
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                4,
                ListItemIcon.Connection,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_FIVE_TITLE, "COMPARE_DEVICE_TWO_POP_BULLET_FIVE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_TWO_POP_BULLET_FIVE_COPY, "COMPARE_DEVICE_TWO_POP_BULLET_FIVE_COPY")
            ));
        }

        // compare page three
        public void LoadComparePopupStudioViewModel(ComparePopupStudioViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_HEADLINE, "COMPARE_DEVICE_THREE_POP_HEADLINE");
            viewModel.SubHead = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_SUBHEAD, "COMPARE_DEVICE_THREE_POP_SUBHEAD");
            viewModel.Lede = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_COPY, "COMPARE_DEVICE_THREE_POP_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_COPY_LEGAL, "COMPARE_DEVICE_THREE_POP_COPY_LEGAL");
            viewModel.BulletOneLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_ONE_LEGAL, "COMPARE_DEVICE_THREE_POP_BULLET_ONE_LEGAL");
            viewModel.BulletTwoLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_TWO_LEGAL, "COMPARE_DEVICE_THREE_POP_BULLET_TWO_TITLE");
            //viewModel.BulletThreeLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_THREE_LEGAL, "COMPARE_DEVICE_THREE_POP_BULLET_THREE_LEGAL");
            viewModel.BulletFourLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_FOUR_LEGAL, "COMPARE_DEVICE_THREE_POP_BULLET_FOUR_LEGAL");
            viewModel.BulletFiveLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_FIVE_LEGAL, "COMPARE_DEVICE_THREE_POP_BULLET_FIVE_LEGAL");

            // bullet one
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Creative, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_ONE_TITLE, "COMPARE_DEVICE_THREE_POP_BULLET_ONE_TITLE"), // headline
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_ONE_COPY, "COMPARE_DEVICE_THREE_POP_BULLET_ONE_COPY") // lede
            ));

            // bullet two
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Pen,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_TWO_TITLE, "COMPARE_DEVICE_THREE_POP_BULLET_TWO_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_TWO_COPY, "COMPARE_DEVICE_THREE_POP_BULLET_TWO_COPY")
            ));

            // bullet three
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                2,
                ListItemIcon.Performance,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_THREE_TITLE, "COMPARE_DEVICE_THREE_POP_BULLET_THREE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_THREE_COPY, "COMPARE_DEVICE_THREE_POP_BULLET_THREE_COPY")
            ));

            // bullet four
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                3,
                ListItemIcon.Display,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_FOUR_TITLE, "COMPARE_DEVICE_THREE_POP_BULLET_FOUR_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_FOUR_COPY, "COMPARE_DEVICE_THREE_POP_BULLET_FOUR_COPY")
            ));

            // bullet five
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                4,
                ListItemIcon.Connection,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_FIVE_TITLE, "COMPARE_DEVICE_THREE_POP_BULLET_FIVE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_THREE_POP_BULLET_FIVE_COPY, "COMPARE_DEVICE_THREE_POP_BULLET_FIVE_COPY")
            ));
        }

        // compare page four
        public void LoadComparePopupLaptopViewModel(ComparePopupLaptopViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_HEADLINE, "COMPARE_DEVICE_FOUR_POP_HEADLINE");
            viewModel.SubHead = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_SUBHEAD, "COMPARE_DEVICE_FOUR_POP_SUBHEAD");
            viewModel.Lede = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_COPY, "COMPARE_DEVICE_FOUR_POP_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_COPY_LEGAL, "COMPARE_DEVICE_FOUR_POP_COPY_LEGAL");
            viewModel.BulletOneLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_ONE_LEGAL, "COMPARE_DEVICE_FOUR_POP_BULLET_ONE_LEGAL");
            viewModel.BulletTwoLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_TWO_LEGAL, "COMPARE_DEVICE_FOUR_POP_BULLET_TWO_TITLE");
            viewModel.BulletThreeLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_THREE_LEGAL, "COMPARE_DEVICE_FOUR_POP_BULLET_THREE_LEGAL");
            viewModel.BulletFourLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_FOUR_LEGAL, "COMPARE_DEVICE_FOUR_POP_BULLET_FOUR_LEGAL");
            viewModel.BulletFiveLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_FIVE_LEGAL, "COMPARE_DEVICE_FOUR_POP_BULLET_FIVE_LEGAL");

            // bullet one
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Creative, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_ONE_TITLE, "COMPARE_DEVICE_FOUR_POP_BULLET_ONE_TITLE"), // headline
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_ONE_COPY, "COMPARE_DEVICE_FOUR_POP_BULLET_ONE_COPY") // lede
            ));

            // bullet two
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Performance,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_TWO_TITLE, "COMPARE_DEVICE_FOUR_POP_BULLET_TWO_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_TWO_COPY, "COMPARE_DEVICE_FOUR_POP_BULLET_TWO_COPY")
            ));

            // bullet three
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                2,
                ListItemIcon.Laptop,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_THREE_TITLE, "COMPARE_DEVICE_FOUR_POP_BULLET_THREE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_THREE_COPY, "COMPARE_DEVICE_FOUR_POP_BULLET_THREE_COPY")
            ));

            // bullet four
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                3,
                ListItemIcon.Display,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_FOUR_TITLE, "COMPARE_DEVICE_FOUR_POP_BULLET_FOUR_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_FOUR_COPY, "COMPARE_DEVICE_FOUR_POP_BULLET_FOUR_COPY")
            ));

            // bullet five
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                4,
                ListItemIcon.Sound,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_FIVE_TITLE, "COMPARE_DEVICE_FOUR_POP_BULLET_FIVE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FOUR_POP_BULLET_FIVE_COPY, "COMPARE_DEVICE_FOUR_POP_BULLET_FIVE_COPY")
            ));
        }

        // compare page five
        public void LoadComparePopupGoViewModel(ComparePopupGoViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_HEADLINE, "COMPARE_DEVICE_FIVE_POP_HEADLINE");
            viewModel.SubHead = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_SUBHEAD, "COMPARE_DEVICE_FIVE_POP_SUBHEAD");
            viewModel.Lede = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_COPY, "COMPARE_DEVICE_FIVE_POP_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_COPY_LEGAL, "COMPARE_DEVICE_FIVE_POP_COPY_LEGAL");
            viewModel.BulletOneLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_ONE_LEGAL, "COMPARE_DEVICE_FIVE_POP_BULLET_ONE_LEGAL");
            viewModel.BulletTwoLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_TWO_LEGAL, "COMPARE_DEVICE_FIVE_POP_BULLET_TWO_TITLE");
            viewModel.BulletThreeLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_THREE_LEGAL, "COMPARE_DEVICE_FIVE_POP_BULLET_THREE_LEGAL");
            viewModel.BulletFourLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_FOUR_LEGAL, "COMPARE_DEVICE_FIVE_POP_BULLET_FOUR_LEGAL");
            viewModel.BulletFiveLegal = GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_FIVE_LEGAL, "COMPARE_DEVICE_FIVE_POP_BULLET_FIVE_LEGAL");

            // bullet one
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Lightweight, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_ONE_TITLE, "COMPARE_DEVICE_FIVE_POP_BULLET_ONE_TITLE"), // headline
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_ONE_COPY, "COMPARE_DEVICE_FIVE_POP_BULLET_ONE_COPY") // lede
            ));

            // bullet two
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                1,
                ListItemIcon.Performance,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_TWO_TITLE, "COMPARE_DEVICE_FIVE_POP_BULLET_TWO_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_TWO_COPY, "COMPARE_DEVICE_FIVE_POP_BULLET_TWO_COPY")
            ));

            // bullet three
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                2,
                ListItemIcon.Battery,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_THREE_TITLE, "COMPARE_DEVICE_FIVE_POP_BULLET_THREE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_THREE_COPY, "COMPARE_DEVICE_FIVE_POP_BULLET_THREE_COPY")
            ));

            // bullet four
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                3,
                ListItemIcon.Display,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_FOUR_TITLE, "COMPARE_DEVICE_FIVE_POP_BULLET_FOUR_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_FOUR_COPY, "COMPARE_DEVICE_FIVE_POP_BULLET_FOUR_COPY")
            ));

            // bullet five
            viewModel.CompareListItems.Add(ListItem.CreateListItem(
                4,
                ListItemIcon.Versatile,
                viewModel.ICON_WIDTH,
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_FIVE_TITLE, "COMPARE_DEVICE_FIVE_POP_BULLET_FIVE_TITLE"),
                GetStringValue(_languageCurrent.COMPARE_DEVICE_FIVE_POP_BULLET_FIVE_COPY, "COMPARE_DEVICE_FIVE_POP_BULLET_FIVE_COPY")
            ));
        }

        private string GetStringValue(string value, string name)
        {
            //if (String.IsNullOrWhiteSpace(value))
            //{
            //    return String.Format("Missing translation for {0}", name);
            //}
            //else
            //{
                return value;
            //}
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
