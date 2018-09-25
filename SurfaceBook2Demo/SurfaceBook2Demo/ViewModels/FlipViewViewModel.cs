using System;
using System.Collections.Generic;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using MetroLog;

using SurfaceBook2Demo.Services;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


namespace SurfaceBook2Demo.ViewModels
{
    public class FlipViewViewModel : ViewModelBase
    {
        //private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<FlipViewViewModel>();

        #region Public Properties

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

            // ExperienceIntroPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceIntroPage",
                Order = 1
            }
            );

            // ExperienceDayPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "ExperienceDayPage",
                Order = 2
            }
            );

            //// ExperienceDayWorkPage
            //section.Pages.Add(new NavigationPage()
            //{
            //    Name = "ExperienceDayWorkPage",
            //    Order = 2
            //}
            //);

            //// ExperienceDayCreatePage
            //section.Pages.Add(new NavigationPage()
            //{
            //    Name = "ExperienceDayCreatePage",
            //    Order = 3
            //}
            //);

            //// ExperienceDayRelaxPage
            //section.Pages.Add(new NavigationPage()
            //{
            //    Name = "ExperienceDayRelaxPage",
            //    Order = 4
            //}
            //);

            //// ExperienceDayPlayPage
            //section.Pages.Add(new NavigationPage()
            //{
            //    Name = "ExperienceDayPlayPage",
            //    Order = 5
            //}
            //);

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

            // AccessoriesDialPage
            section.Pages.Add(new NavigationPage()
            {
                Name = "AccessoriesDialPage",
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
                Name = "Compare",
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

        #endregion
    }

}
