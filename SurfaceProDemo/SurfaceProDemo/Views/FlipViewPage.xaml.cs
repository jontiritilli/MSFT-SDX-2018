﻿using System;

using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using SurfaceProDemo.Services;
using SurfaceProDemo.ViewModels;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Models;

using SDX.Telemetry.Services;


namespace SurfaceProDemo.Views
{
    public sealed partial class FlipViewPage : Page
    {
        #region Private Constants

        private const double PAGE_TIMER_DURATION = 8000d;

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

        #region Public Static Methods

        public static NavigationBar GetNavigationBar()
        {
            NavigationBar bar = null;

            if (null != FlipViewPage.Current)
            {
                bar = FlipViewPage.Current.BottomNavBar;
            }

            return bar;
        }

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

            // initialize the root
            this.BottomNavBar.Root = ViewModel.Root;


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

        #endregion

        #region Event Handlers

        private void FlipViewEx_Loaded(object sender, RoutedEventArgs e)
        {
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
            // if the count of e.AddedItems == 1, then the SelectedIndex has actually
            // changed. if it's equal to 3 (the number of panels loaded in a VirtualizingStackPanel),
            // then this isn't a real change in the SelectedIndex, and we'll ignore it.
            if (3 != e.AddedItems.Count)
            {
                // stop the page timer
                if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }
            }

            // if we have a bottom nav bar
            if (null != this.BottomNavBar)
            {
                // get the sender
                if (sender is FlipViewEx flipView)
                {
                    // get the pageIndex of the new page
                    int nextPageIndex = flipView.SelectedIndex;

                    // find the index of the previous page
                    int previousPageIndex = flipView.GetIndexOfChildView(_previousPage);

                    // if the previous and next page are the same, then return
                    if (nextPageIndex == previousPageIndex)
                    {
                        return;
                    }

                    // navigate from the previous page
                    if (null != _previousPage)
                    {
                        _previousPage.NavigateFromPage();
                    }

                    // navigate to it
                    if (null != flipView.SelectedItem)
                    {
                        INavigateMoveDirection moveDirection = INavigateMoveDirection.Unknown;

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
                    INavigationItem sliderItem = navBar.Root.Items.Find(item => item.Name == "ExperienceFlipViewPage");
                    if ((null != sliderItem) && (sliderItem is NavigationFlipView sliderNavigationFlipView))
                    {
                        FlipViewEx deviceModeFlipView = ExperienceFlipViewPage.GetDeviceModeFlipView();

                        if ((null != ExperienceFlipViewPage.Current) && (null != deviceModeFlipView))
                        {
                            if (deviceModeFlipView.SelectedIndex != sliderNavigationFlipView.SelectedIndex)
                            {
                                deviceModeFlipView.SelectedIndex = sliderNavigationFlipView.SelectedIndex;
                            }
                        }
                    }
                }
            }

            // telemetry - log nav sections
            if (NavigationActions.Section == e.NavAction)
            {
                // we've gone to a section, so log it
                switch (e.NavSection.Name)
                {
                    case "Experience":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavExperience);
                        break;

                    case "Accessories":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavAccessories);
                        break;

                    case "BestOfMicrosoft":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavBestOf);
                        break;

                    case "Compare":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavComparison);
                        break;
                }
            }

            // telemetry - log page view
            switch (e.NavItem.Section.Name)
            {
                case "Experience":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewExperience);
                    break;

                case "Accessories":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewAccessories);
                    break;

                case "BestOfMicrosoft":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewBestOf);
                    break;

                case "Compare":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewComparison);
                    break;
            }
        }

        private void AppClose_Click(object sender, RoutedEventArgs e)
        {
            // log application exit
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.EndApplication);

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

        public void EnablePageNavigation(object sender, object e)
        {
            this.BottomNavBar.IsNavigationEnabled = true;
        }

        public void DisablePageNavigation(object sender, object e)
        {
            this.BottomNavBar.IsNavigationEnabled = false;
        }

        public Popup GetExperiencePagePopup()
        {
            return this.ExperiencePopup;
        }

        // load COMPARE PAGE full page popups
        public Popup GetComparePagePopupPro()
        {
            return this.CompareProPopup;
        }

        public Popup GetComparePagePopupBook()
        {
            return this.CompareBookPopup;
        }

        public Popup GetComparePagePopupStudio()
        {
            return this.CompareStudioPopup;
        }

        public Popup GetComparePagePopupLaptop()
        {
            return this.CompareLaptopPopup;
        }

        public Popup GetComparePagePopupGo()
        {
            return this.CompareGoPopup;
        }

        #endregion
    }
}
