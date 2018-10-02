using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;
using SDX.Toolkit.Models;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;

namespace SurfaceProDemo.ViewModels
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


        #region Navigation Bar Setup

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

            // -------------------------------------------------
            // ExperienceFlipViewPage
            // -------------------------------------------------
            NavigationFlipView experienceFlipViewPage = new NavigationFlipView()
            {
                Name = "ExperienceFlipViewPage",
                Order = 2,
                SelectedIndex = 0,
                Section = sectionExperience,
            };
            this.Root.Items.Add(experienceFlipViewPage);

            // =================================================
            // Children of ExperienceDayPage
            // =================================================

            // ExperienceTransformPage
            experienceFlipViewPage.Items.Add(new NavigationPage()
            {
                Name = "ExperienceTransformPage",
                Order = 0,
                Section = sectionExperience,
            }
            );

            // ExperiencePerformancePage
            experienceFlipViewPage.Items.Add(new NavigationPage()
            {
                Name = "ExperiencePerformancePage",
                Order = 1,
                Section = sectionExperience,
            }
            );

            // ExperienceQuietPage
            experienceFlipViewPage.Items.Add(new NavigationPage()
            {
                Name = "ExperienceQuietPage",
                Order = 2,
                Section = sectionExperience,
            }
            );

            // =================================================
            // Create Accessories Pages
            // =================================================


            // AccessoriesPenPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesPenPage",
                Order = 3,
                Section = sectionExperience,
            }
            );

            // AccessoriesKeyboardPage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "AccessoriesKeyboardPage",
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
            // Create Best of Microsoft Pages
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
            // Create Compare Pages
            // =================================================

            // ComparePage
            this.Root.Items.Add(new NavigationPage()
            {
                Name = "ComparePage",
                Order = 7,
                Section = sectionCompare,
            });


        }

        #endregion
    }
}
