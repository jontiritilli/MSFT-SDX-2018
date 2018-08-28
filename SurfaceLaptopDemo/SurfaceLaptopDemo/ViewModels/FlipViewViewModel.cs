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
        #region Public Properties

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
            // This method creates the sections and the pages in each section.
            // This object structure is used by the NavigationBar control to
            // render its section headers. This structure represents a hierachy
            // that maps to the linear page structure of the flipview.

            // =================================================
            // Experience Section
            // =================================================
            NavigationSection section = new NavigationSection()
            {
                Name = "Experience",
                Text = this.NavBarExperience,  // from language file
                Order = 0
            };

            // ExperienceHeroPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceHeroPage",
                Order = 0
            }
            );

            // ExperienceColorsPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceColorsPage",
                Order = 1
            }
            );

            // ExperienceInnovationPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceInnovationPage",
                Order = 2
            }
            );

            // ExperiencePerformancePage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperiencePerformancePage",
                Order = 3
            }
            );

            // ExperienceSpeakersPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceSpeakersPage",
                Order = 4
            }
            );

            // ExperienceSleekPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceSleekPage",
                Order = 5
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================


            // =================================================
            // Accessories Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Accessories",
                Text = this.NavBarAccessories,  // from language file
                Order = 1
            };

            // AccessoriesPenPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "AccessoriesPenPage",
                Order = 0
            }
            );

            // AccessoriesKeyboardPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "AccessoriesKeyboardPage",
                Order = 1
            }
            );

            // AccessoriesMousePage
            section.Pages.Add(new NavigationPage()
            {
                Name = "AccessoriesMousePage",
                Order = 2
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================


            // =================================================
            // Best of Microsoft Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "BestOfMicrosoft",
                Text = this.NavBarBestOfMicrosoft,  // from language file
                Order = 2
            };

            // BestOfMicrosoftPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "BestOfMicrosoftPage",
                Order = 0
            }
            );

            // add the section to the list
            this.Sections.Add(section);
            // =================================================


            // =================================================
            // Compare Section
            // =================================================
            section = new NavigationSection()
            {
                Name = "Last2",
                Text = this.NavBarCompare,  // from language file
                Order = 3
            };

            // ComparePage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ComparePage",
                Order = 0
            });


            // add the section to the list
            this.Sections.Add(section);
            // =================================================

        }
    }

}
