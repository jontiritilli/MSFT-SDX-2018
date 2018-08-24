using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;

using SDX.Toolkit.Models;


namespace SurfaceTestBed.ViewModels
{
    public class FlipViewViewModel : ViewModelBase
    {
        public List<NavigationSection> Sections = new List<NavigationSection>();

        public FlipViewViewModel()
        {
            // create Page One section
            NavigationSection section = new NavigationSection()
            {
                Name = "Page1",
                Text = "Page One",
                Order = 0
            };
            section.Pages.Add(new NavigationPage()
            {
                Name = "Page1",
                Text = "Page 1",
                Order = 0
            }
            );
            this.Sections.Add(section);

            // create Page Two section
            section = new NavigationSection()
            {
                Name = "Page2",
                Text = "Page Two",
                Order = 0
            };
            section.Pages.Add(new NavigationPage()
            {
                Name = "Page2",
                Text = "Page 2",
                Order = 0
            }
            );
            this.Sections.Add(section);

            // create Page Three section
            section = new NavigationSection()
            {
                Name = "Page3",
                Text = "Page Three",
                Order = 0
            };
            section.Pages.Add(new NavigationPage()
            {
                Name = "Page3",
                Text = "Page 3",
                Order = 0
            }
            );
            this.Sections.Add(section);

            // create Last 2 section
            section = new NavigationSection()
            {
                Name = "Last2",
                Text = "Last Two",
                Order = 0
            };
            section.Pages.Add(new NavigationPage()
            {
                Name = "Page4",
                Text = "Page 4",
                Order = 0
            });
            section.Pages.Add(new NavigationPage()
            {
                Name = "Page5",
                Text = "Page 5",
                Order = 0
            });
            this.Sections.Add(section);
        }
    }
}
