using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceStudioDemo.Services;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;
using Windows.UI.Xaml;


namespace SurfaceStudioDemo.ViewModels
{
    public class FlipViewViewModel : ViewModelBase
    {
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

            // ExperienceCreativityPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceCreativityPage",
                Order = 1,
                Section = sectionExperience,
            }
            );

            // ExperienceCraftedPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceCraftedPage",
                Order = 2,
                Section = sectionExperience,
            }
            );

            // ExperiencePixelSensePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperiencePixelSensePage",
                Order = 3,
                Section = sectionExperience,
            }
            );


            // =================================================
            // Create Accessories pages
            // =================================================

            
            // AccessoriesPenPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesPenPage",
                Order = 4,
                Section = sectionExperience,
            }
            );

            // AccessoriesDialPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesDialPage",
                Order = 5,
                Section = sectionAccessories,
            }
            );

            // AccessoriesMousePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesMousePage",
                Order = 6,
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
                Order = 7,
                Section = sectionBestOfMicrosoft,
            }
            );


            // =================================================
            // Create Compare pages
            // =================================================

            // ComparePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ComparePage",
                Order = 8,
                Section = sectionCompare,
            });


        }
    }

}
