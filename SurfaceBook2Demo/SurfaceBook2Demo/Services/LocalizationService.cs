﻿using System;
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

using SDX.Toolkit.Controls;

using SurfaceBook2Demo.Models;
using SurfaceBook2Demo.ViewModels;


namespace SurfaceBook2Demo.Services
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

        public void LoadExperienceIntroViewModel(ExperienceIntroViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_HEADLINE, "EXPERIENCE_OVERVIEW_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_COPY, "EXPERIENCE_OVERVIEW_COPY");

            viewModel.PopupPixelSenseHeadline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPLEFT_TITLE, "EXPERIENCE_OVERVIEW_POPLEFT_TITLE");
            viewModel.PopupPixelSenseLede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPLEFT_COPY, "EXPERIENCE_OVERVIEW_POPLEFT_COPY");
            viewModel.PopupPixelSenseLegal = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPLEFT_LEGAL, "EXPERIENCE_OVERVIEW_POPLEFT_LEGAL");

            viewModel.PopupCompareHeadline = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPRIGHT_TITLE, "EXPERIENCE_OVERVIEW_POPRIGHT_TITLE");
            viewModel.PopupCompareLede = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPRIGHT_COPY, "EXPERIENCE_OVERVIEW_POPRIGHT_COPY");
            viewModel.PopupCompareLegal = GetStringValue(_languageCurrent.EXPERIENCE_OVERVIEW_POPRIGHT_LEGAL, "EXPERIENCE_OVERVIEW_POPRIGHT_LEGAL");

        }

        public void LoadExperienceDayViewModel(ExperienceDayViewModel viewModel)
        {
            viewModel.SliderBatteryCopy = GetStringValue(_languageCurrent.BATTERY_COPY, "BATTERY_COPY");
        }

        public void LoadExperienceDayWorkViewModel(ExperienceDayWorkViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_WORK_HEADLINE, "EXPERIENCE_WORK_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_WORK_COPY, "EXPERIENCE_WORK_COPY");

            viewModel.PopupBatteryLifeHeadline = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPLEFT_TITLE, "EXPERIENCE_WORK_POPLEFT_TITLE");
            viewModel.PopupBatteryLifeLede = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPLEFT_COPY, "EXPERIENCE_WORK_POPLEFT_COPY");
            viewModel.PopupBatteryLifeHours = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPLEFT_BATTERYXX, "EXPERIENCE_WORK_POPLEFT_BATTERYXX");
            viewModel.PopupBatteryLifeLegal = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPLEFT_LEGAL, "EXPERIENCE_WORK_POPLEFT_LEGAL");

            viewModel.PopupConnectionsHeadline = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPRIGHT_TITLE, "EXPERIENCE_WORK_POPRIGHT_TITLE");
            viewModel.PopupConnectionsLede = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPRIGHT_COPY, "EXPERIENCE_WORK_POPRIGHT_COPY");
            viewModel.PopupConnectionsLegal = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPRIGHT_LEGAL, "EXPERIENCE_WORK_POPRIGHT_LEGAL");

        }

        public void LoadExperienceDayWorkPopupViewModel(ExperienceDayWorkPopupViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_HEADLINE, "EXPERIENCE_WORK_POPTOP_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_COPY, "EXPERIENCE_WORK_POPTOP_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_LEGAL, "EXPERIENCE_WORK_POPTOP_LEGAL");
    
            viewModel.appSelectorData[0].Message = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_BULLET_ONE, "EXPERIENCE_WORK_POPTOP_BULLET_ONE");
            viewModel.appSelectorData[1].Message = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_BULLET_TWO, "EXPERIENCE_WORK_POPTOP_BULLET_TWO");
            viewModel.appSelectorData[2].Message = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_BULLET_THREE, "EXPERIENCE_WORK_POPTOP_BULLET_THREE");
            viewModel.appSelectorData[3].Message = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_BULLET_FOUR, "EXPERIENCE_WORK_POPTOP_BULLET_FOUR");
            viewModel.appSelectorData[4].Message = GetStringValue(_languageCurrent.EXPERIENCE_WORK_POPTOP_BULLET_FIVE, "EXPERIENCE_WORK_POPTOP_BULLET_FIVE");
        }

        public void LoadExperienceDayCreateViewModel(ExperienceDayCreateViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_HEADLINE, "EXPERIENCE_CREATE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_COPY, "EXPERIENCE_CREATE_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_COPY_LEGAL, "EXPERIENCE_CREATE_COPY_LEGAL");

            viewModel.PopupDialHeadline = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_POPLEFT_TITLE, "EXPERIENCE_CREATE_POPLEFT_TITLE");
            viewModel.PopupDialLede = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_POPLEFT_COPY, "EXPERIENCE_CREATE_POPLEFT_COPY");

            viewModel.PopupPenHeadline = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_POPRIGHT_TITLE, "EXPERIENCE_CREATE_POPRIGHT_TITLE");
            viewModel.PopupPenLede = GetStringValue(_languageCurrent.EXPERIENCE_CREATE_POPRIGHT_COPY, "EXPERIENCE_CREATE_POPRIGHT_COPY");

            viewModel.PopupTransformHeadline= GetStringValue(_languageCurrent.EXPERIENCE_CREATE_POPTOP_TITLE, "EXPERIENCE_CREATE_POPTOP_TITLE");
            viewModel.PopupTransformLede= GetStringValue(_languageCurrent.EXPERIENCE_CREATE_POPTOP_COPY, "EXPERIENCE_CREATE_POPTOP_COPY");
        }

        public void LoadExperienceDayRelaxViewModel(ExperienceDayRelaxViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_RELAX_HEADLINE, "EXPERIENCE_RELAX_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_RELAX_COPY, "EXPERIENCE_RELAX_COPY");

            viewModel.PopupHingeHeadline = GetStringValue(_languageCurrent.EXPERIENCE_RELAX_POPLEFT_TITLE, "EXPERIENCE_RELAX_POPLEFT_TITLE");
            viewModel.PopupHingeLede = GetStringValue(_languageCurrent.EXPERIENCE_RELAX_POPLEFT_COPY, "EXPERIENCE_RELAX_POPLEFT_COPY");

            viewModel.PopupDisplayHeadline = GetStringValue(_languageCurrent.EXPERIENCE_RELAX_POPTOP_TITLE, "EXPERIENCE_RELAX_POPTOP_TITLE");
            viewModel.PopupDisplayLede = GetStringValue(_languageCurrent.EXPERIENCE_RELAX_POPTOP_COPY, "EXPERIENCE_RELAX_POPTOP_COPY");
        }

        public void LoadExperienceDayPlayViewModel(ExperienceDayPlayViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.EXPERIENCE_PLAY_HEADLINE, "EXPERIENCE_PLAY_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.EXPERIENCE_PLAY_COPY, "EXPERIENCE_PLAY_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.EXPERIENCE_PLAY_COPY_LEGAL, "EXPERIENCE_PLAY_COPY_LEGAL");
        }

        public void LoadAccessoriesPenViewModel(AccessoriesPenViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_HEADLINE, "ACCESSORIES_INTERACTIVE_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_COPY, "ACCESSORIES_INTERACTIVE_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_TWO_LEGAL, "ACCESSORIES_INTERACTIVE_BULLET_TWO_LEGAL");

            viewModel.TryItTitle = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_TITLE, "ACCESSORIES_INTERACTIVE_TRYIT_TITLE");
            viewModel.TryItLede = GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_TRYIT_COPY, "ACCESSORIES_INTERACTIVE_TRYIT_COPY");

            // TODO: add code to load list
            // bullet one
            viewModel.ListItems.Add((ListItem.CreateListItem(
                0, // order
                ListItemIcon.Jot, 
                viewModel.ICON_WIDTH, // width
                "", // header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_ONE, "ACCESSORIES_INTERACTIVE_BULLET_ONE") // order
            )));

            // bullet two
            viewModel.ListItems.Add((ListItem.CreateListItem(
                1, // order
                ListItemIcon.Write, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_TWO, "ACCESSORIES_INTERACTIVE_BULLET_TWO") // order
            )));

            // bullet three
            viewModel.ListItems.Add((ListItem.CreateListItem(
                2, // order
                ListItemIcon.Pressure, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_THREE, "ACCESSORIES_INTERACTIVE_BULLET_THREE") // order
            )));

            // bullet 4
            viewModel.ListItems.Add((ListItem.CreateListItem(
                3, // order
                ListItemIcon.Palm, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",// header
                GetStringValue(_languageCurrent.ACCESSORIES_INTERACTIVE_BULLET_FOUR, "ACCESSORIES_INTERACTIVE_BULLET_FOUR") // order
            )));

        }

        public void LoadAccessoriesDialViewModel(AccessoriesDialViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_HEADLINE, "ACCESSORIES_LEFT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_COPY, "ACCESSORIES_LEFT_COPY");

            viewModel.PopupDialHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_TITLE, "ACCESSORIES_LEFT_POPLEFT_TITLE");
            viewModel.PopupDialLede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_COPY, "ACCESSORIES_LEFT_POPLEFT_COPY");
            viewModel.PopupDialLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPLEFT_LEGAL, "ACCESSORIES_LEFT_POPLEFT_LEGAL");

            viewModel.PopupPenHeadline = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_TITLE, "ACCESSORIES_LEFT_POPTOP_TITLE");
            viewModel.PopupPenLede = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_COPY, "ACCESSORIES_LEFT_POPTOP_COPY");
            viewModel.PopupPenLegal = GetStringValue(_languageCurrent.ACCESSORIES_LEFT_POPTOP_LEGAL, "ACCESSORIES_LEFT_POPTOP_LEGAL");
        }

        public void LoadAccessoriesMouseViewModel(AccessoriesMouseViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_HEADLINE, "ACCESSORIES_RIGHT_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_COPY, "ACCESSORIES_RIGHT_COPY");

            viewModel.PopupMouseHeadline = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPCENTER_TITLE, "ACCESSORIES_RIGHT_POPCENTER_TITLE");
            viewModel.PopupMouseLede = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_POPCENTER_COPY, "ACCESSORIES_RIGHT_POPCENTER_COPY");
            viewModel.PopupMouseLegal = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_COPY_LEGAL, "ACCESSORIES_RIGHT_COPY_LEGAL");
            viewModel.PopupMouseTryItTitle = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_INTERACTIVE_TRYIT_TITLE, "ACCESSORIES_RIGHT_INTERACTIVE_TRYIT_TITLE");
            viewModel.PopupMouseTryItCaption = GetStringValue(_languageCurrent.ACCESSORIES_RIGHT_INTERACTIVE_TRYIT_COPY, "ACCESSORIES_RIGHT_INTERACTIVE_TRYIT_COPY");
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
            viewModel.ItemList.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Start, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_ONE_CTA, "BEST_BULLET_ONE_CTA") // order
            ));
            // bullet two
            viewModel.ItemList.Add(ListItem.CreateListItem(
                1, // order
                ListItemIcon.Sync, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_TWO_CTA, "BEST_BULLET_TWO_CTA") // order
            ));

            // bullet three
            viewModel.ItemList.Add(ListItem.CreateListItem(
                2, // order
                ListItemIcon.Hello, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_TITLE, ""),
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_THREE_CTA, "BEST_BULLET_THREE_CTA") // order
                                                                                              // null // custom icon path
            ));

            // bullet four
            viewModel.ItemList.Add(ListItem.CreateListItem(
                3, // order
                ListItemIcon.Office, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_TITLE, ""), // order
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_COPY, ""), // order
                null,
                GetStringValue(_languageCurrent.BEST_BULLET_FOUR_CTA, "BEST_BULLET_FOUR_CTA") // order

            // null // custom icon path
            ));

            // bullet three OPTIONAL BULLET
            if (!String.IsNullOrWhiteSpace(_languageCurrent.BEST_BULLET_FIVE_TITLE)
                && !String.IsNullOrWhiteSpace(_languageCurrent.BEST_BULLET_FIVE_COPY))   // use AND here because BOM has headline AND lede and either one missing means we don't show the bullet
            {
                viewModel.ItemList.Add(ListItem.CreateListItem(
                    4, // order
                    ListItemIcon.Custom, // icon enum name
                    viewModel.ICON_WIDTH, // width
                    GetStringValue(_languageCurrent.BEST_BULLET_FIVE_TITLE, ""),
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
            viewModel.Legal = GetStringValue(_languageCurrent.COMPARE_LEGAL, "COMPARE_LEGAL");
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
                ListItemIcon.Lightweight,
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
