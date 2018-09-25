using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceLaptopDemo.Services;
using SDX.Toolkit.Models;


namespace SurfaceLaptopDemo.ViewModels
{
    public class FlipViewViewModel : ViewModelBase
    {
        #region Constants


        #endregion

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

            // ExperienceColorsPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceColorsPage",
                Order = 1,
                Section = sectionExperience,
            }
            );

            // ExperienceInnovationPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceInnovationPage",
                Order = 2,
                Section = sectionExperience,
            }
            );

            // ExperiencePerformancePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperiencePerformancePage",
                Order = 3,
                Section = sectionExperience,
            }
            );

            // ExperienceSpeakersPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceSpeakersPage",
                Order = 4,
                Section = sectionExperience,
            }
            );

            // ExperienceSleekPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ExperienceSleekPage",
                Order = 5,
                Section = sectionExperience,
            }
            );
            
            // =================================================
            // Create Accessories Pages
            // =================================================
            
            // AccessoriesTouchPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesTouchPage",
                Order = 6,
                Section = sectionAccessories,
            }
            );

            // AccessoriesMousePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesMousePage",
                Order = 7,
                Section = sectionAccessories,
            }
            );

            // Pen page removed as of version 6 of design
            // AccessoriesPenPage
            //this.Root.Items.Add(new NavigationPage()
            //{
            //    Name = "AccessoriesPenPage",
            //    Order = 8,
            //    Section = sectionAccessories,
            //}
            //);

            // =================================================
            // Create Best of Microsoft Pages
            // =================================================


            // BestOfMicrosoftPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "BestOfMicrosoftPage",
                Order = 8,
                Section = sectionBestOfMicrosoft,
            }
            );

            // =================================================
            // Create Compare Pages
            // =================================================

            // ComparePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ComparePage",
                Order = 9,
                Section = sectionCompare,
            });

            // =================================================

        }
    }

}
