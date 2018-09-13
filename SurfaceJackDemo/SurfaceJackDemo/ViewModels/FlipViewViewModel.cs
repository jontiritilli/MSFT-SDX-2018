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

        #endregion

        #region Public Properties

        // background
        public string BackgroundUri;

        // our navigation sections for the navigation bar
        public List<NavigationSection> Sections = new List<NavigationSection>();

        // navigation bar section names
        public string NavBarAudio;
        public string NavBarDesign;
        public string NavBarTech;
        public string NavBarProductivity;
        public string NavBarSpecs;
        public string NavBarPartner;

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
                case DeviceType.Pro:
                case DeviceType.Unknown:
                case DeviceType.Go:
                default:
                    this.BackgroundUri = URI_BACKGROUND_CRUZ;
                    break;

                case DeviceType.Laptop:
                    this.BackgroundUri = URI_BACKGROUND_FOXBURG;
                    break;

                case DeviceType.Studio:
                    this.BackgroundUri = URI_BACKGROUND_CAPROCK;
                    break;

                case DeviceType.Book13:
                    this.BackgroundUri = URI_BACKGROUND_SB213;
                    break;

                case DeviceType.Book15:
                    this.BackgroundUri = URI_BACKGROUND_SB215;
                    break;
            }
        }

        #endregion


        public void RenderNavigation()
        {
            // This method creates the sections and the pages in each section.
            // This object structure is used by the NavigationBar control to
            // render its section headers. This structure represents a hierachy
            // that maps to the linear page structure of the flipview.

            // =================================================
            // Audio Section
            // =================================================
            NavigationSection section = new NavigationSection()
            {
                Name = "Audio",
                Text = this.NavBarAudio,  // from language file
                Order = 0
            };

            // AudioTryItPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "AudioTryItPage",
                Order = 0
            }
            );

            // AudioListenPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "AudioListenPage",
                Order = 1
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================


            // =================================================
            // Design Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Design",
                Text = this.NavBarDesign,  // from language file
                Order = 1
            };

            // DesignPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "DesignPage",
                Order = 0
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================


            // =================================================
            // Tech Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Tech",
                Text = this.NavBarTech,  // from language file
                Order = 2
            };

            // TechPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "TechPage",
                Order = 0
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================

            // =================================================
            // Productivity Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Productivity",
                Text = this.NavBarProductivity,  // from language file
                Order = 3
            };

            // ProductivityPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ProductivityPage",
                Order = 0
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================

            // =================================================
            // Specs Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Specs",
                Text = this.NavBarSpecs,  // from language file
                Order = 4
            };

            // ComparePage
            section.Pages.Add(new NavigationPage()
            {
                Name = "SpecsPage",
                Order = 0
            });


            // add the section to the list
            this.Sections.Add(section);
            // =================================================

            // =================================================
            // Partner Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Partner",
                Text = this.NavBarPartner,  // from language file
                Order = 5
            };

            // ComparePage
            section.Pages.Add(new NavigationPage()
            {
                Name = "PartnerPage",
                Order = 0
            });


            // add the section to the list
            this.Sections.Add(section);
            // =================================================

        }
    }

}
