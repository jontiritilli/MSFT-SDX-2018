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
using Windows.UI.Xaml.Controls.Primitives;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using MetroLog;

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

        //private ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<FlipViewPage>();

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
            //Log.Trace("Entering constructor for FlipViewPage");

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

            //Log.Trace("Initializing Navigation Bar from viewModel.");

            // initialize the navigation bar sections
            foreach (NavigationSection section in ViewModel.Sections)
            {
                this.BottomNavBar.NavigationSections.Add(section);
            }

            // initialize the navigation bar root
            this.BottomNavBar.Root = ViewModel.Root;

            //Log.Trace("Exiting constructor.");
        }

        #endregion


        #region Event Handlers

        private void FlipViewEx_Loaded(object sender, RoutedEventArgs e)
        {
            //Log.Trace("Entering FlipViewEx.Loaded.");

            // set the current page
            //this.ContentFlipView.SelectedIndex = 0;   // not necessary and will interfere with page timer

            // save the current page so we can navigate from it
            _previousPage = (INavigate)((FlipViewItemEx)this.ContentFlipView.SelectedItem).GetChildViewAsObject();

            // navigate to it
            _previousPage.NavigateToPage(INavigateMoveDirection.Forward);

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

                    // navigate to the new page
                    if (null != flipView.SelectedItem)
                    {
                        INavigateMoveDirection moveDirection = INavigateMoveDirection.Unknown;

                        // get the pageIndex of the new page
                        int nextPageIndex = flipView.SelectedIndex;

                        // find the index of the previous page
                        int previousPageIndex = flipView.GetIndexOfChildView(_previousPage);

                        // if we got it
                        if (-1 != previousPageIndex)
                        {
                            // are we moving forward or backward to get to the new page?
                            if (previousPageIndex < nextPageIndex)
                            {
                                moveDirection = INavigateMoveDirection.Forward;
                            }
                            else if (nextPageIndex < previousPageIndex)
                            {
                                moveDirection = INavigateMoveDirection.Backward;
                            }
                        }

                        // save the current page so we can navigate away from it later
                        _previousPage = (INavigate)((FlipViewItemEx)flipView.SelectedItem).GetChildViewAsObject();

                        // navigate to it
                        _previousPage.NavigateToPage(moveDirection);

                        // tell the navbar to move to it
                        this.BottomNavBar.MoveToPageIndex(nextPageIndex, (INavigateMoveDirection.Forward == moveDirection));
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
                    // get the page index
                    int pageIndex = navBar.Root.SelectedIndex;

                    // move the flipview to that index
                    if (pageIndex != this.ContentFlipView.SelectedIndex)
                    {
                        this.ContentFlipView.SelectedIndex = pageIndex;
                    }

                    // get the slider flipview page index
                    INavigationItem sliderItem = navBar.Root.Items.Find(item => item.Name == "ExperienceDayPage");
                    if ((null != sliderItem) && (sliderItem is NavigationFlipView sliderNavigationFlipView))
                    {
                        FlipViewEx dayFlipView = ExperienceDayPage.GetDayFlipView();

                        if ((null != ExperienceDayPage.Current) && (null != dayFlipView))
                        {
                            if (dayFlipView.SelectedIndex != sliderNavigationFlipView.SelectedIndex)
                            {
                                dayFlipView.SelectedIndex = sliderNavigationFlipView.SelectedIndex;
                            }
                        }
                    }
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

        public Popup GetExperienceDayWorkPagePopup()
        {
            return this.ExperienceDayWorkPopup;
        }

        #endregion
    }
}
