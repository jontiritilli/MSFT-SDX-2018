using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using SurfaceTestBed.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Models;
using Windows.UI.Xaml;

namespace SurfaceTestBed.Views
{
    public sealed partial class FlipViewPage : Page
    {
        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        public FlipViewPage()
        {
            // We use NavigationCacheMode.Required to keep track the selected item on navigation. For further information see the following links.
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.controls.page.navigationcachemode.aspx
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh771188.aspx
            NavigationCacheMode = NavigationCacheMode.Required;
            InitializeComponent();

            foreach (NavigationSection section in ViewModel.Sections)
            {
                this.BottomNavBar.NavigationSections.Add(section);
            }
        }

        private void FlipViewEx_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void FlipViewEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // stop the page timer
            //if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // if we have a bottom nav bar
            if (null != this.BottomNavBar)
            {
                // get the sender
                if (sender is FlipViewEx flipView)
                {
                    // get the pageIndex of the new page
                    int pageIndex = flipView.SelectedIndex;

                    // tell the navbar to move to it
                    this.BottomNavBar.MoveToPageIndex(pageIndex);
                }
            }
        }

        private void BottomNavBar_OnNavigation(object sender, NavigateEventArgs e)
        {
            // stop the page timer
            //if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // if we have a flipview
            if (null != this.ContentFlipView)
            {
                // get the sender
                if (sender is NavigationBar navBar)
                {
                    // convert the section/page from the event args to a page index
                    int pageIndex = navBar.GetPageIndexFromPage(e.NavSection, e.NavPage);

                    // move the flipview to that index
                    this.ContentFlipView.SelectedIndex = pageIndex;
                }
            }
        }
    }
}
