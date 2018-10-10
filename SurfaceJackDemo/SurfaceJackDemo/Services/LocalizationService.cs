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
using SDX.Toolkit.Controls;

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

        }

        public void LoadAudioListenViewModel(AudioListenViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_TRACK_HEADLINE, "AUDIO_TRACK_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.AUDIO_TRACK_COPY, "AUDIO_TRACK_COPY");
            viewModel.Legal = GetStringValue(_languageCurrent.AUDIO_TRACK_LEGAL, "AUDIO_TRACK_LEGAL");
            viewModel.BulletListTitle = GetStringValue(_languageCurrent.AUDIO_TRACK_BULLET_LIST_TITLE, "AUDIO_TRACK_BULLET_LIST_TITLE"); 
            viewModel.OverlayHeadline = GetStringValue(_languageCurrent.AUDIO_GATEWAY_HEADLINE, "AUDIO_GATEWAY_HEADLINE");
            viewModel.OverlayLede = GetStringValue(_languageCurrent.AUDIO_GATEWAY_COPY, "AUDIO_GATEWAY_COPY");
            viewModel.OverlayCTA = GetStringValue(_languageCurrent.AUDIO_GATEWAY_CTA, "AUDIO_GATEWAY_CTA");
        }

        public void LoadDesignViewModel(DesignViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.DESIGN_DESIGN_HEADLINE, "DESIGN_DESIGN_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.DESIGN_DESIGN_COPY, "DESIGN_DESIGN_COPY");
            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.DESIGN_DESIGN_POPLEFT_TITLE, "DESIGN_DESIGN_POPLEFT_TITLE");            
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.DESIGN_DESIGN_POPLEFT_COPY, "DESIGN_DESIGN_POPLEFT_COPY");
            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.DESIGN_DESIGN_POPRIGHT_TITLE, "DESIGN_DESIGN_POPRIGHT_TITLE"); 
            viewModel.PopRightLede = GetStringValue(_languageCurrent.DESIGN_DESIGN_POPRIGHT_COPY, "DESIGN_DESIGN_POPRIGHT_COPY");
            viewModel.PopTopHeadline = GetStringValue(_languageCurrent.DESIGN_DESIGN_POPTOP_TITLE, "DESIGN_DESIGN_POPTOP_TITLE");
            viewModel.PopTopLede = GetStringValue(_languageCurrent.DESIGN_DESIGN_POPTOP_COPY, "DESIGN_DESIGN_POPTOP_COPY");
        }

        public void LoadHowToViewModel(HowToViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE");
            viewModel.ListItems[0].Message = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE");
            viewModel.ListItems[1].Message = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO");
            viewModel.ListItems[2].Message = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE");
            viewModel.ListItems[3].Message = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR");
            viewModel.ListItems[4].Message = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE");
            viewModel.ListItems[5].Message = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX");
            viewModel.Legal = GetStringValue(_languageCurrent.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL, "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL");

        }

        public void LoadTechViewModel(TechViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.TECH_TECH_HEADLINE, "TECH_TECH_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.TECH_TECH_COPY, "TECH_TECH_COPY");
            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.TECH_TECH_POPLEFT_TITLE, "TECH_TECH_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.TECH_TECH_POPLEFT_COPY, "TECH_TECH_POPLEFT_COPY");
            viewModel.PopLeftHR = GetStringValue(_languageCurrent.TECH_TECH_POPLEFT_BATTERY_HR, "TECH_TECH_POPLEFT_BATTERY_HR");
            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.TECH_TECH_RIGHT_TITLE, "TECH_TECH_RIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.TECH_TECH_RIGHT_COPY, "TECH_TECH_RIGHT_COPY");
            viewModel.PopTopHeadline = GetStringValue(_languageCurrent.TECH_TECH_POPMID_TITLE, "TECH_TECH_POPMID_TITLE");
            viewModel.PopTopLede = GetStringValue(_languageCurrent.TECH_TECH_POPMID_COPY, "TECH_TECH_POPMID_COPY");
        }

        public void LoadProductivityViewModel(ProductivityViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_HEADLINE, "PRODUCTIVITY_PRODUCTIVITY_HEADLINE");
            viewModel.Lede = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_COPY, "PRODUCTIVITY_PRODUCTIVITY_COPY");
            viewModel.PopLeftHeadline = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE, "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE");
            viewModel.PopLeftLede = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY, "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY");
            viewModel.PopLeftLegal = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL, "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL");
            viewModel.PopRightHeadline = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE, "TECH_TECH_RIGHT_TITLE");
            viewModel.PopRightLede = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY, "TECH_TECH_RIGHT_COPY");
            viewModel.PopBottomHeadline = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE, "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE");
            viewModel.PopBottomLede = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY, "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY");
            viewModel.PopBottomLegal = GetStringValue(_languageCurrent.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL, "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL");
        }

        public void LoadSpecsViewModel(SpecsViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.SPECS_SPECS_HEADLINE, "SPECS_SPECS_HEADLINE");
            viewModel.LegalBulletOne = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETONE_LEGAL, "SPECS_SPECS_BULLETONE_LEGAL");
            viewModel.LegalBulletTwo = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETTWO_LEGAL, "SPECS_SPECS_BULLETTWO_LEGAL");
            viewModel.LegalBulletThree = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETTHREE_LEGAL, "SPECS_SPECS_BULLETTHREE_LEGAL");
            viewModel.LegalBulletFour = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETFOUR_LEGAL, "SPECS_SPECS_BULLETFOUR_LEGAL");
            viewModel.LegalBulletFive = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETFIVE_LEGAL, "SPECS_SPECS_BULLETFIVE_LEGAL");
            viewModel.LegalBulletSix = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETSIX_LEGAL, "SPECS_SPECS_BULLETSIX_LEGAL");
            viewModel.LegalBulletSeven = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETSEVEN_LEGAL, "SPECS_SPECS_BULLETSEVEN_LEGAL");

            // ---------------------------------------------------------------------------------------------------------------
            // NOTE - PSS: The copy has changed here to include both Imperial and Metric measurements, so we must choose
            // which to use. For now, we're going to use Imperial (BRT).
            // ---------------------------------------------------------------------------------------------------------------
            string bulletOneCopyBritish = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETONE_COPY_BRT, "SPECS_SPECS_BULLETONE_COPY_BRT");
            string bulletOneCopyMetric = GetStringValue(_languageCurrent.SPECS_SPECS_BULLETONE_COPY_MET, "SPECS_SPECS_BULLETONE_COPY_MET");

            // bullet one
            viewModel.ItemList.Add(ListItem.CreateListItem(
                0, // order
                ListItemIcon.Dimensions, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETONE_TITLE, "SPECS_SPECS_BULLETONE_TITLE"), // order
                bulletOneCopyBritish, // order  // NOTE - PSS - USING BRITISH FOR NOW SINCE THIS IS US-ONLY
                null,
                ""
            ));
            // ---------------------------------------------------------------------------------------------------------------

            // bullet two
            viewModel.ItemList.Add(ListItem.CreateListItem(
                1, // order
                ListItemIcon.Weight, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETTWO_TITLE, "SPECS_SPECS_BULLETTWO_TITLE"), // order
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETTWO_COPY, "SPECS_SPECS_BULLETTWO_COPY"), // order
                null,
                ""
            ));

            // bullet three
            viewModel.ItemList.Add(ListItem.CreateListItem(
                2, // order
                ListItemIcon.Speaker, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETTHREE_TITLE, "SPECS_SPECS_BULLETTHREE_TITLE"),
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETTHREE_COPY, "SPECS_SPECS_BULLETTHREE_COPY"), // order
                null,
                ""
                                                                                                // null // custom icon path
            ));

            // bullet four
            viewModel.ItemList.Add(ListItem.CreateListItem(
                3, // order
                ListItemIcon.Frequency, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETFOUR_TITLE, "SPECS_SPECS_BULLETFOUR_TITLE"), // order
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETFOUR_COPY, "SPECS_SPECS_BULLETFOUR_COPY"), // order
                null,
                ""

            // null // custom icon path
            ));

            viewModel.ItemList.Add(ListItem.CreateListItem(
                4, // order
                ListItemIcon.BatteryLife, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETFIVE_TITLE, "SPECS_SPECS_BULLETFIVE_TITLE"),
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETFIVE_COPY, "SPECS_SPECS_BULLETFIVE_COPY"), // order
                null,
                ""
            ));

            viewModel.ItemList.Add(ListItem.CreateListItem(
                5, // order
                ListItemIcon.NoiseCancellation, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETSIX_TITLE, "SPECS_SPECS_BULLETSIX_TITLE"),
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETSIX_COPY, "SPECS_SPECS_BULLETSIX_COPY"), // order
                null,
                ""
            ));

            viewModel.ItemList.Add(ListItem.CreateListItem(
                6, // order
                ListItemIcon.Inputs, // icon enum name
                viewModel.ICON_WIDTH, // width
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETSEVEN_TITLE, "SPECS_SPECS_BULLETSEVEN_TITLE"),
                GetStringValue(_languageCurrent.SPECS_SPECS_BULLETSEVEN_COPY, "SPECS_SPECS_BULLETSEVEN_COPY"), // order
                null,
                ""
            ));

        }

        public void LoadPartnerViewModel(InTheBoxViewModel viewModel)
        {
            viewModel.Headline = GetStringValue(_languageCurrent.SPECS_WITB_HEADLINE, "SPECS_WITB_HEADLINE");


            // TODO: add code to load list
            // bullet one
            viewModel.ListItems.Add((ListItem.CreateListItem(
                0, // order
                ListItemIcon.Jot,
                viewModel.ICON_WIDTH, // width
                "", // header
                GetStringValue(_languageCurrent.SPECS_WITB_BULLETONE_TITLE, "SPECS_WITB_BULLETONE_TITLE") // order
            )));

            // bullet two
            viewModel.ListItems.Add((ListItem.CreateListItem(
                1, // order
                ListItemIcon.Write, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.SPECS_WITB_BULLETTWO_TITLE, "SPECS_WITB_BULLETTWO_TITLE") // order
            )));

            // bullet three
            viewModel.ListItems.Add((ListItem.CreateListItem(
                2, // order
                ListItemIcon.Pressure, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",//header
                GetStringValue(_languageCurrent.SPECS_WITB_BULLETTHREE_TITLE, "SPECS_WITB_BULLETTHREE_TITLE") // order
            )));

            // bullet 4
            viewModel.ListItems.Add((ListItem.CreateListItem(
                3, // order
                ListItemIcon.Palm, // icon enum name
                viewModel.ICON_WIDTH, // width
                "",// header
                GetStringValue(_languageCurrent.SPECS_WITB_BULLETFOUR_TITLE, "SPECS_WITB_BULLETFOUR_TITLE") // order
            )));

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
