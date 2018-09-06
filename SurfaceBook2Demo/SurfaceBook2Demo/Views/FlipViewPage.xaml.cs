using System;

using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceBook2Demo.Services;
using SurfaceBook2Demo.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


namespace SurfaceBook2Demo.Views
{
    public sealed partial class FlipViewPage : Page
    {
        #region Private Constants

        private const double PAGE_TIMER_DURATION = 5000d;

        #endregion


        #region Private Members

        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        private DispatcherTimer _pageMoveTimer = null;

        private INavigate _previousPage = null;

        #endregion


        #region Public Static Properties

        public static FlipViewPage Current { get; private set; }

        #endregion


        #region Construction

        public FlipViewPage()
        {
            // save a pointer to ourself
            FlipViewPage.Current = this;

            // We use NavigationCacheMode.Required to keep track the selected item on navigation. For further information see the following links.
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.controls.page.navigationcachemode.aspx
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh771188.aspx
            NavigationCacheMode = NavigationCacheMode.Required;
            InitializeComponent();

            // disappear the title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //this.GettingFocus += FlipView_GettingFocus;
            this.KeyUp += FlipView_KeyUp;
            this.PointerReleased += FlipView_PointerReleased;

            // configure focus
            this.FocusVisualMargin = new Thickness(0);
            this.FocusVisualPrimaryBrush = new SolidColorBrush(Colors.Transparent);
            this.FocusVisualPrimaryThickness = new Thickness(0);
            this.FocusVisualSecondaryBrush = new SolidColorBrush(Colors.Transparent);
            this.FocusVisualSecondaryThickness = new Thickness(0);

            // initialize the navigation bar
            foreach (NavigationSection section in ViewModel.Sections)
            {
                this.BottomNavBar.NavigationSections.Add(section);
            }
        }

        #endregion


        #region Event Handlers

        private void FlipViewEx_Loaded(object sender, RoutedEventArgs e)
        {
            // set the current page
            this.ContentFlipView.SelectedIndex = 0;

            // save the current page so we can navigate from it
            _previousPage = (INavigate)((FlipViewItemEx)this.ContentFlipView.SelectedItem).GetChildViewAsObject();

            // navigate to it
            _previousPage.NavigateToPage();

            // configure our page move timer
            _pageMoveTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(PAGE_TIMER_DURATION)
            };
            _pageMoveTimer.Tick += PageMoveTimer_Tick;
            _pageMoveTimer.Start();
        }

        private void FlipViewEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // stop the page timer
            if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // if we have a bottom nav bar
            if (null != this.BottomNavBar)
            {
                // get the sender
                if (sender is FlipViewEx flipView)
                {
                    // navigate from the previous page
                    if (null != _previousPage)
                    {
                        _previousPage.NavigateFromPage();
                    }

                    // get the pageIndex of the new page
                    int pageIndex = flipView.SelectedIndex;

                    // tell the navbar to move to it
                    this.BottomNavBar.MoveToPageIndex(pageIndex);

                    // navigate to the current page
                    if (null != flipView.SelectedItem)
                    {
                        // save the current page so we can navigate from it
                        _previousPage = (INavigate)((FlipViewItemEx)flipView.SelectedItem).GetChildViewAsObject();

                        // navigate to it
                        _previousPage.NavigateToPage();
                    }
                }
            }
        }

        private void BottomNavBar_OnNavigation(object sender, NavigateEventArgs e)
        {
            // stop the page timer
            if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

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

        private void PageMoveTimer_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // move to the next page
            if (null != this.BottomNavBar)
            {
                this.BottomNavBar.MoveToNextPage();
            }
        }

        private void FlipView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (null != this.BottomNavBar)
            {
                e.Handled = this.BottomNavBar.HandleKey(e.Key);
            }
        }

        private void FlipView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // i hate this, but App is not getting pointer hits
            App.Current?.HandlePointerReleased(e.Pointer.PointerDeviceType);
        }

        private void AppClose_Click(object sender, RoutedEventArgs e)
        {
            // this is not kosher by guidelines, but no other way to do this
            Application.Current.Exit();
        }

        #endregion


        #region Public Methods

        public void ShowAppClose()
        {
            if (null != this.AppClose)
            {
                this.AppClose.Visibility = Visibility.Visible;
            }
        }

        public void HideAppClose()
        {
            if (null != this.AppClose)
            {
                this.AppClose.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
