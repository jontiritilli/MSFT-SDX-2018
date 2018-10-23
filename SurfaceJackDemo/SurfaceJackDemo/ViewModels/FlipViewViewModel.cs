using System;
using System.Collections.Generic;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceJackDemo.Services;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


namespace SurfaceJackDemo.ViewModels
{
    public class FlipViewViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND_CRUZ = "ms-appx:///Assets/Backgrounds/cruz_generic_bg.png";
        private const string URI_BACKGROUND_CAPROCK = "ms-appx:///Assets/Backgrounds/caprock_generic_bg.jpg";
        private const string URI_BACKGROUND_FOXBURG = "ms-appx:///Assets/Backgrounds/foxburg_generic_bg.jpg";
        private const string URI_BACKGROUND_SB213 = "ms-appx:///Assets/Backgrounds/sb2_generic_bg.jpg";
        private const string URI_BACKGROUND_SB215 = "ms-appx:///Assets/Backgrounds/sb2_15_generic_bg.jpg";

        private const string URI_CLOSE_APP_BUTTON_CRUZ = "ms-appx:///Assets/Universal/close-app-button.png";
        private const string URI_CLOSE_APP_BUTTON_CAPROCK = "ms-appx:///Assets/Universal/caprock_close_app_button.png";
        private const string URI_CLOSE_APP_BUTTON_FOXBURG = "ms-appx:///Assets/Universal/fox_close.png";
        private const string URI_CLOSE_APP_BUTTON_SB2_13 = "ms-appx:///Assets/Universal/sb2_15_close.png";
        private const string URI_CLOSE_APP_BUTTON_SB2_15 = "ms-appx:///Assets/Universal/sb2_13_close.png";

        #endregion

        #region Public Properties

        // background
        public string BackgroundUri;

        // close button
        public string CloseButtonUri;

        // root of the page tree
        public NavigationFlipView Root = null;

        // our navigation sections for the navigation bar
        public List<NavigationSection> Sections = new List<NavigationSection>();

        // navigation bar section names
        public string NavBarAudio;
        public string NavBarDesign;
        public string NavBarTech;
        public string NavBarProductivity;
        public string NavBarSpecs;
        public string NavBarPartner;
        public double Volume;

        // music bar playlist
        public Playlist Playlist = null;

        // This method is a hack because UWP does not support binding a StaticResource
        // with a Converter, so we must add these properties to the ViewModel.
        public GridLength MusicBarHeight
        {
            get
            {
                return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PlayerHeight));
            }
        }

        public GridLength NavigationBarHeight
        {
            get
            {
                return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarHeight));
            }
        }

        public GridLength AppCloseWidth
        {
            get
            {
                return new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.AppCloseWidth));
            }
        }

        #endregion

        #region Construction

        public FlipViewViewModel()
        {
            this.Volume = ConfigurationService.Current.GetVolume();
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadFlipViewViewModel(this);
            }

            // render the navigation sections for the nav bar
            // (this has to happen AFTER the localization is loaded)
            RenderNavigation();

            // load the playlist
            if ((null != PlaylistService.Current) && (PlaylistService.Current.IsLoaded))
            {
                this.Playlist = PlaylistService.Current.DefaultPlaylist;
            }

            // determine background
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Go:
                case DeviceType.Unknown:
                case DeviceType.Pro:
                default:
                    this.BackgroundUri = URI_BACKGROUND_CRUZ;
                    this.CloseButtonUri = URI_CLOSE_APP_BUTTON_CRUZ;
                    break;

                case DeviceType.Laptop:
                    this.BackgroundUri = URI_BACKGROUND_FOXBURG;
                    this.CloseButtonUri = URI_CLOSE_APP_BUTTON_FOXBURG;
                    break;

                case DeviceType.Studio:
                    this.BackgroundUri = URI_BACKGROUND_CAPROCK;
                    this.CloseButtonUri = URI_CLOSE_APP_BUTTON_CAPROCK;
                    break;

                case DeviceType.Book13:
                    this.BackgroundUri = URI_BACKGROUND_SB213;
                    this.CloseButtonUri = URI_CLOSE_APP_BUTTON_SB2_13;
                    break;

                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND_SB215;
                    this.CloseButtonUri = URI_CLOSE_APP_BUTTON_SB2_15;
                    break;
            }
        }

        #endregion

        public void RenderNavigation()
        {
            // This method creates the sections and the page tree

            // =================================================
            // Create the sections
            // =================================================
            NavigationSection sectionExperience = new NavigationSection()
            {
                Name = "Audio",
                Text = this.NavBarAudio,  // from language file
                Order = 0
            };
            this.Sections.Add(sectionExperience);

            NavigationSection sectionDesign = new NavigationSection()
            {
                Name = "Design",
                Text = this.NavBarDesign,  // from language file
                Order = 1
            };
            this.Sections.Add(sectionDesign);

            NavigationSection sectionTech = new NavigationSection()
            {
                Name = "Tech",
                Text = this.NavBarTech,  // from language file
                Order = 2
            };
            this.Sections.Add(sectionTech);

            NavigationSection sectionProductivity = new NavigationSection()
            {
                Name = "Productivity",
                Text = this.NavBarProductivity,  // from language file
                Order = 3
            };
            this.Sections.Add(sectionProductivity);

            NavigationSection sectionSpecs = new NavigationSection()
            {
                Name = "Specs",
                Text = this.NavBarSpecs,  // from language file
                Order = 4
            };
            this.Sections.Add(sectionSpecs);

            //NavigationSection sectionPartner = new NavigationSection()
            //{
            //    Name = "Partner",
            //    Text = this.NavBarPartner,  // from language file
            //    Order = 5
            //};
            //this.Sections.Add(sectionPartner);

            // =================================================
            // Create the page tree
            // =================================================

            // create the root
            this.Root = new NavigationFlipView()
            {
                Name = "RootFlipView",
                Order = 0,
                SelectedIndex = 0,
            };

            // =================================================
            // Create Experience pages
            // =================================================

            //// AudioTryItPage
            //this.Root.Items.Add(new NavigationPage()
            //{
            //    Name = "AudioTryItPage",
            //    Order = 0,
            //    Section = sectionExperience,
            //}
            //);

            // AudioListenPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AudioListenPage",
                Order = 0,
                Section = sectionExperience,
            }
            );


            // =================================================
            // Create Design pages
            // =================================================

            // DesignPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "DesignPage",
                Order = 1,
                Section = sectionDesign,
            }
            );

            // =================================================
            // Create Tech pages
            // =================================================

            // TechPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "TechPage",
                Order = 2,
                Section = sectionTech,
            }
            );

            // =================================================
            // Create Productivity pages
            // =================================================


            // ProductivityPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ProductivityPage",
                Order = 3,
                Section = sectionProductivity,
            }
            );

            // =================================================
            // Create Specs pages
            // =================================================

            // ComparePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "SpecsPage",
                Order = 4,
                Section = sectionSpecs,
            });

            // ComparePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "PartnerPage",
                Order = 5,
                Section = sectionSpecs,
            });
        }
    }

}
