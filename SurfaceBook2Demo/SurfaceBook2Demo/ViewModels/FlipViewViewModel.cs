using System;
using System.Collections.Generic;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


namespace SurfaceBook2Demo.ViewModels
{
    public class FlipViewViewModel : ViewModelBase
    {
        //private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<FlipViewViewModel>();

        #region Public Properties

        // root of the page tree
        public NavigationFlipView Root = null;

        // our navigation sections for the navigation bar
        public List<NavigationSection> Sections = new List<NavigationSection>();

        // navigation bar section names
        public string NavBarExperience;
        public string NavBarAccessories;
        public string NavBarBestOfMicrosoft;
        public string NavBarCompare;

        // This method is a hack because UWP does not support binding a StaticResource
        // with a Converter, so we must add these properties to the ViewModel.
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
            //Log.Trace("Entering the constructor for FlipViewViewModel");

            //Log.Trace("Attempting to locate the Localization Service");
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();

            // if we got it
            if (null != localizationService)
            {
                //Log.Trace("We retrieved the Localization Service; attempting to load ourself.");
                // load ourself with values from the language file
                localizationService.LoadFlipViewViewModel(this);
            }

            //Log.Trace("We are about to call RenderNavigation");
            // render the navigation sections for the nav bar
            // (this has to happen AFTER the localization is loaded)
            RenderNavigation();

            //Log.Trace("RenderNavigation was called.");
        }

        #endregion


        #region UI Methods

        public void RenderNavigation()
        {
            // This method creates the sections and the page tree

            // =================================================
            // Create the sections
            // =================================================
            NavigationSection sectionExperience = new NavigationSection()
            {
                Name = "Experience",
                Text = this.NavBarExperience,  // from language file
                Order = 0
            };
            this.Sections.Add(sectionExperience);

            NavigationSection sectionAccessories = new NavigationSection()
            {
                Name = "Accessories",
                Text = this.NavBarAccessories,  // from language file
                Order = 1
            };
            this.Sections.Add(sectionAccessories);

            NavigationSection sectionBestOfMicrosoft = new NavigationSection()
            {
                Name = "BestOfMicrosoft",
                Text = this.NavBarBestOfMicrosoft,  // from language file
                Order = 2
            };
            this.Sections.Add(sectionBestOfMicrosoft);

            NavigationSection sectionCompare = new NavigationSection()
            {
                Name = "Compare",
                Text = this.NavBarCompare,  // from language file
                Order = 3
            };
            this.Sections.Add(sectionCompare);

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

            // ExperienceHeroPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceHeroPage",
                Order = 0,
                Section = sectionExperience,
            }
            );

            // ExperienceIntroPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceIntroPage",
                Order = 1,
                Section = sectionExperience,
            }
            );

            // ExperienceDayPage
            NavigationFlipView experienceDayFlipView = new NavigationFlipView()
            {
                Name = "ExperienceDayPage",
                Order = 2,
                SelectedIndex = 0,
                Section = sectionExperience,
            };
            this.Root.Items.Add(experienceDayFlipView);

            // =================================================
            // Children of ExperienceDayPage
            // =================================================

            // ExperienceDayWorkPage
            experienceDayFlipView.Items.Add(new NavigationPage()
            {
                Name = "ExperienceDayWorkPage",
                Order = 0,
                Section = sectionExperience,
            }
            );

            // ExperienceDayCreatePage
            experienceDayFlipView.Items.Add(new NavigationPage()
            {
                Name = "ExperienceDayCreatePage",
                Order = 1,
                Section = sectionExperience,
            }
            );

            // ExperienceDayRelaxPage
            experienceDayFlipView.Items.Add(new NavigationPage()
            {
                Name = "ExperienceDayRelaxPage",
                Order = 2,
                Section = sectionExperience,
            }
            );

            // ExperienceDayPlayPage
            experienceDayFlipView.Items.Add(new NavigationPage()
            {
                Name = "ExperienceDayPlayPage",
                Order = 4,
                Section = sectionExperience,
            }
            );

            // AccessoriesPenPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesPenPage",
                Order = 3,
                Section = sectionExperience,
            }
            );

            // =================================================
            // Create Accessories pages
            // =================================================


            // AccessoriesDialPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesDialPage",
                Order = 4,
                Section = sectionAccessories,
            }
            );

            // AccessoriesMousePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesMousePage",
                Order = 5,
                Section = sectionAccessories,
            }
            );

            // =================================================
            // Create Best of Microsoft pages
            // =================================================

            // BestOfMicrosoftPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "BestOfMicrosoftPage",
                Order = 6,
                Section = sectionBestOfMicrosoft,
            }
            );


            // =================================================
            // Compare Section
            // =================================================

            // ComparePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ComparePage",
                Order = 7,
                Section = sectionCompare,
            });

            // =================================================

        }

        #endregion
    }

}
