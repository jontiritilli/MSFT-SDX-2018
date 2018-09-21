﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Models;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum NavigationActions
    {
        Unknown = 0,
        GoBack = 1,
        GoForward = 2,
        Section = 3,
        Home = 4
    }

    public class NavigateEventArgs : EventArgs
    {
        #region Private Members

        private NavigationActions _navAction;
        private NavigationSection _navSection;
        private INavigationItem _navItem;

        #endregion

        #region Constructors

        public NavigateEventArgs()
        {
        }

        public NavigateEventArgs(NavigationActions navAction, NavigationSection navSection, INavigationItem navItem)
        {
            NavAction = navAction;
            NavSection = navSection;
            NavItem = navItem;
        }

        #endregion

        #region Public Properties

        public NavigationActions NavAction
        {
            get { return _navAction; }
            set { _navAction = value; }
        }

        public NavigationSection NavSection
        {
            get { return _navSection; }
            set { _navSection = value; }
        }

        public INavigationItem NavItem
        {
            get { return _navItem; }
            set { _navItem = value; }
        }

        #endregion
    }

    public sealed class NavigationBar : Control
    {
        #region Private Constants

        private const string URI_HOME_CRUZ = "ms-appx:///Assets/NavigationBar/cruz_home.png";
        private const string URI_HOME_JACK = "ms-appx:///Assets/NavigationBar/joplin_home.png";

        private const string URI_CHEVRON_BACK_STUDIO = "ms-appx:///Assets/NavigationBar/cap_nav_chevronLeft.png";
        private const string URI_CHEVRON_FORWARD_STUDIO = "ms-appx:///Assets/NavigationBar/cap_nav_chevronRight.png";

        private const string URI_CHEVRON_BACK_PRO = "ms-appx:///Assets/NavigationBar/cruz_chevron_left.png";
        private const string URI_CHEVRON_FORWARD_PRO = "ms-appx:///Assets/NavigationBar/cruz_chevron_right.png";

        private const string URI_CHEVRON_BACK_LAPTOP = "ms-appx:///Assets/NavigationBar/fox_chevron_left.png";
        private const string URI_CHEVRON_FORWARD_LAPTOP = "ms-appx:///Assets/NavigationBar/fox_chevron_right.png";

        private const string URI_CHEVRON_BACK_SB2_13 = "ms-appx:///Assets/NavigationBar/sb2_13_chevron_left.png";
        private const string URI_CHEVRON_FORWARD_SB2_13 = "ms-appx:///Assets/NavigationBar/sb2_13_chevron_right.png";

        private const string URI_CHEVRON_BACK_SB2_15 = "ms-appx:///Assets/NavigationBar/sb2_15_chevron_left.png";
        private const string URI_CHEVRON_FORWARD_SB2_15 = "ms-appx:///Assets/NavigationBar/sb2_15_chevron_right.png";


        #endregion

        #region Private Members

        // device-dependent images
        private string ChevronLeftUri;
        private string ChevronRightUri;
        private string HomeUri;

        // UI members we need to keep track of
        private Grid _layoutRoot;
        private Canvas _lineCanvas;
        private Line _navLine;
        private Button _navGoBack;
        private Button _navGoForward;
        private Button _navHome = null;
        private Image _imgGoBack;
        private Image _imgGoForward;
        private Image _imgHome;

        #endregion

        #region Construction/Destruction

        public NavigationBar()
        {
            this.DefaultStyleKey = typeof(NavigationBar);

            // no focus visual indications
            this.UseSystemFocusVisuals = false;

            // event handlers
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            this.KeyUp += NavigationBar_OnKeyUp;

            // set default properties
            this.SelectedSection = null;
            this.SelectedItem = null;
            this.CanGoBack = false;
            this.CanGoForward = true;

            if (null == this.NavigationSections)
            {
                this.NavigationSections = new List<NavigationSection>();
            }

            // populate our images based on size of device
            switch (WindowHelper.GetDeviceTypeFromResolution())
            {
                case DeviceType.Studio:
                    ChevronLeftUri = URI_CHEVRON_BACK_STUDIO;
                    ChevronRightUri = URI_CHEVRON_FORWARD_STUDIO;
                    HomeUri = URI_HOME_JACK;
                    break;

                case DeviceType.Pro:
                default:
                    ChevronLeftUri = URI_CHEVRON_BACK_PRO;
                    ChevronRightUri = URI_CHEVRON_FORWARD_PRO;
                    HomeUri = URI_HOME_CRUZ;
                    break;

                case DeviceType.Laptop:
                    ChevronLeftUri = URI_CHEVRON_BACK_LAPTOP;
                    ChevronRightUri = URI_CHEVRON_FORWARD_LAPTOP;
                    HomeUri = URI_HOME_CRUZ;
                    break;

                case DeviceType.Book15:
                    ChevronLeftUri = URI_CHEVRON_BACK_SB2_15;
                    ChevronRightUri = URI_CHEVRON_FORWARD_SB2_15;
                    HomeUri = URI_HOME_CRUZ;
                    break;

                case DeviceType.Book13:
                    ChevronLeftUri = URI_CHEVRON_BACK_SB2_13;
                    ChevronRightUri = URI_CHEVRON_FORWARD_SB2_13;
                    HomeUri = URI_HOME_CRUZ;
                    break;

            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();

            this.UpdateUI();
        }

        #endregion

        #region Public Properties

        public NavigationSection SelectedSection { get; private set; }

        public INavigationItem SelectedItem { get; private set; }

        public bool CanGoBack { get; private set; }

        public bool CanGoForward { get; private set; }

        #endregion

        #region Dependency Properties

        // NavigationSections
        public static readonly DependencyProperty NavigationSectionsProperty =
            DependencyProperty.Register("NavigationSections", typeof(List<NavigationSection>), typeof(NavigationBar), new PropertyMetadata(new List<NavigationSection>(), OnNavigationSectionsChanged));

        public List<NavigationSection> NavigationSections
        {
            get { return (List<NavigationSection>)GetValue(NavigationSectionsProperty); }
            set { SetValue(NavigationSectionsProperty, value); }
        }

        // IsHomeEnabled
        public static readonly DependencyProperty IsHomeEnabledProperty =
            DependencyProperty.Register("IsHomeEnabled", typeof(bool), typeof(NavigationBar), new PropertyMetadata(false, OnIsHomeEnabledChanged));

        public bool IsHomeEnabled
        {
            get => (bool)GetValue(IsHomeEnabledProperty);
            set => SetValue(IsHomeEnabledProperty, value);
        }

        // AreGridLinesEnabled
        public static readonly DependencyProperty AreGridLinesEnabledProperty =
            DependencyProperty.Register("AreGridLinesEnabled", typeof(bool), typeof(NavigationBar), new PropertyMetadata(false, AreGridLinesEnabledChanged));

        public bool AreGridLinesEnabled
        {
            get => (bool)GetValue(AreGridLinesEnabledProperty);
            set => SetValue(AreGridLinesEnabledProperty, value);
        }

        #endregion

        #region Custom Events

        public delegate void NavigateEvent(object sender, NavigateEventArgs e);

        public event NavigateEvent Navigate;

        private void RaiseNavigateEvent(NavigationBar navigationBar, NavigateEventArgs e)
        {
            Navigate?.Invoke(navigationBar, e);
        }

        private void RaiseNavigateEvent(NavigationBar navigationBar, NavigationActions navAction, NavigationSection navSection, INavigationItem navItem)
        {
            NavigateEventArgs args = new NavigateEventArgs(navAction, navSection, navItem);

            RaiseNavigateEvent(navigationBar, args);
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            ((NavigationBar)sender).UpdateUI();
        }

        private static void OnNavigationSectionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationBar navbar)
            {
                navbar.UpdateUI();
            }
        }

        private static void OnIsHomeEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationBar navbar)
            {
                navbar.UpdateUI();
            }
        }

        private static void AreGridLinesEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationBar navbar)
            {
                navbar.RenderUI();
                navbar.UpdateUI();
            }
        }

        private void NavItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = (Button)sender;

            switch (button.Name)
            {
                case "GoBack":
                    if (this.CanGoBack)
                    {
                        MoveToPreviousPage();
                    }
                    break;

                case "GoForward":
                    if (this.CanGoForward)
                    {
                        MoveToNextPage();
                    }
                    break;

                case "GoHome":
                    // move to the first page
                    MoveToPageIndex(0);
                    break;

                default:
                    // loop through the sections
                    foreach (NavigationSection section in this.NavigationSections)
                    {
                        // if the section name matches the button name
                        if (0 == String.Compare(section.Name, button.Name, true))
                        {
                            // move to this section
                            MoveToSection(section);

                            // telemetry
                            //TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_NAVEXPERIENCE, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                        }
                    }
                    break;
            }

            // update the UI to reflect the change
            //this.UpdateUI();  // this is done in MoveToPage() which is called by all MoveXXX() functions.
        }

        //protected override void OnPreviewKeyUp(KeyRoutedEventArgs e)
        //{
        //    e.Handled = HandleKey(e.Key);

        //    base.OnPreviewKeyUp(e);
        //}

        private void NavigationBar_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = HandleKey(e.Key);
        }

        public bool HandleKey(VirtualKey key)
        {
            bool handled = false;

            switch (key)
            {
                case VirtualKey.Right:
                    if (this.CanGoForward)
                    {
                        MoveToNextPage();
                        handled = true;
                    }
                    break;

                case VirtualKey.Left:
                    if (this.CanGoBack)
                    {
                        MoveToPreviousPage();
                        handled = true;
                    }
                    break;


                case VirtualKey.Escape:

                    ApplicationView view = ApplicationView.GetForCurrentView();

                    if (null != view)
                    {
                        if (view.IsFullScreenMode)
                        {
                            view.ExitFullScreenMode();
                        }
                    }
                    handled = true;
                    break;
            }

            //// i hate this, but App is not getting these keys
            //App.Current.HandleKeyUp(key);

            //return handled;

            return false;
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get our sizing values
            double _height = StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarHeight);
            double _lineHeight = StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarLineHeight);
            double _margin = StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarMargin);
            double _spacer = StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarSpacer);
            double _arrowWidth = StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarWidthArrow);
            double _homeWidth = StyleHelper.GetApplicationDouble(LayoutSizes.NavigationBarWidthHome);

            // get the nav grid
            if (null == _layoutRoot) { _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot"); }

            // if we couldn't get the grid we can't do anything, so exit
            if (null == _layoutRoot) { return; }

            // clear the grid
            _layoutRoot.Children.Clear();

            // set up the grid

            // set the grid height
            _layoutRoot.Height = _height;
            _layoutRoot.MaxHeight = _height;

            // rows - this is static
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_lineHeight) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_height - _lineHeight) });

            // columns - this is dynamic based on sections
            // margin, go back, and greedy-eater
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_margin) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_arrowWidth + 16) }); // includes style margins
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });

            // column for each section
            for (int i = 0; i < this.NavigationSections.Count; i++)
            {
                _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                // if this isn't the last column, add the between spacer
                if (i < (this.NavigationSections.Count - 1))
                {
                    _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_spacer) });
                }
            }

            // greedy-eater, go forward, and margin
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_arrowWidth + 16) }); // includes style margins
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_margin) });

            // AreGridLinesEnabled
            if (this.AreGridLinesEnabled)
            {
                TestHelper.AddGridCellBorders(_layoutRoot, 4, 13, Colors.DarkRed);
            }

            // create the progress line and the canvas that contains it
            // ===========================================================
            // canvas
            _lineCanvas = new Canvas()
            {
                Height = _lineHeight,
                Margin = new Thickness(0)
            };
            _layoutRoot.Children.Add(_lineCanvas);
            Grid.SetRow(_lineCanvas, 0);
            Grid.SetColumn(_lineCanvas, 0);
            Grid.SetColumnSpan(_lineCanvas, 3 + this.NavigationSections.Count);

            // line
            _navLine = new Line()
            {
                Name = "ProgressLine",
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = _lineHeight,
                X1 = 0,
                Y1 = (_lineHeight / 2),
                X2 = 0,
                Y2 = (_lineHeight / 2),
                Margin = new Thickness(0)
            };
            _lineCanvas.Children.Add(_navLine);
            // ===========================================================

            // create the button style
            Style buttonStyle = StyleHelper.GetApplicationStyle("NoInteractionButton");

            // grid row for menu items
            int MenuItemRow = 1;

            // create the Go Back button
            if (null == _navGoBack)
            {
                // if the goback image is null, create it
                if (null == _imgGoBack)
                {
                    _imgGoBack = new Image()
                    {
                        Source = new BitmapImage() { UriSource = new Uri(ChevronLeftUri), DecodePixelWidth = (int)_arrowWidth, DecodePixelType = DecodePixelType.Logical },
                        Width = _arrowWidth
                    };
                }

                // create the button and add the image
                _navGoBack = new Button()
                {
                    Name = "GoBack",
                    Content = _imgGoBack,
                    Visibility = (this.CanGoBack ? Visibility.Visible : Visibility.Collapsed),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                if (null != buttonStyle) { _navGoBack.Style = buttonStyle; }

                // set inherited Grid properties
                Grid.SetRow(_navGoBack, MenuItemRow);
                Grid.SetColumn(_navGoBack, 1);

                // add the Click event handler
                _navGoBack.Click += this.NavItem_Click;

                // add it to the grid
                _layoutRoot.Children.Add(_navGoBack);
            }

            // create the Go Forward button
            if (null == _navGoForward)
            {
                // if the goback image is null, create it
                if (null == _imgGoForward)
                {
                    _imgGoForward = new Image()
                    {
                        Source = new BitmapImage() { UriSource = new Uri(ChevronRightUri), DecodePixelWidth = (int)_arrowWidth, DecodePixelType = DecodePixelType.Logical },
                        Width = _arrowWidth
                    };
                }

                // create the button and add the image
                _navGoForward = new Button()
                {
                    Name = "GoForward",
                    Content = _imgGoForward,
                    Visibility = (this.CanGoForward ? Visibility.Visible : Visibility.Collapsed),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                if (null != buttonStyle) { _navGoForward.Style = buttonStyle; }

                // set inherited Grid properties
                Grid.SetRow(_navGoForward, MenuItemRow);
                Grid.SetColumn(_navGoForward, 3 + ((this.NavigationSections.Count * 2) - 1) + 1);

                // add the Click event handler
                _navGoForward.Click += this.NavItem_Click;

                // add it to the grid
                _layoutRoot.Children.Add(_navGoForward);
            }

            // if Home is enabled, then create the Home button
            if (this.IsHomeEnabled)
            {
                // create the Home button
                if (null == _navHome)
                {
                    // if the goback image is null, create it
                    if (null == _imgHome)
                    {
                        _imgHome = new Image()
                        {
                            Source = new BitmapImage() { UriSource = new Uri(HomeUri), DecodePixelWidth = (int)_homeWidth, DecodePixelType = DecodePixelType.Logical },
                            Width = _homeWidth,
                            Visibility = (this.CanGoBack ? Visibility.Visible : Visibility.Collapsed),
                        };
                    }

                    // create the button and add the image
                    _navHome = new Button()
                    {
                        Name = "Home",
                        Content = _imgHome,
                        Visibility = Visibility.Collapsed,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    if (null != buttonStyle) { _navHome.Style = buttonStyle; }

                    // set inherited Grid properties
                    Grid.SetRow(_navHome, MenuItemRow);
                    Grid.SetColumn(_navHome, 3 + ((this.NavigationSections.Count * 2) - 1) + 1);

                    // add the Click event handler
                    _navHome.Click += this.NavItem_Click;

                    // add it to the grid
                    _layoutRoot.Children.Add(_navHome);
                }
            }

            // counter for sections
            int j = 0;

            // loop through and create section buttons
            foreach (NavigationSection section in this.NavigationSections)
            {

                TextBlockEx sectionText = new TextBlockEx()
                {
                    Text = section.Text,
                    TextAlignment = TextAlignment.Center,
                    TextStyle = TextStyles.NavigationSection,
                };

                Button sectionButton = new Button()
                {
                    Name = section.Name,
                    Content = sectionText,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                if (null != buttonStyle) { sectionButton.Style = buttonStyle; }
                sectionButton.Click += this.NavItem_Click;

                // add to the grid
                Grid.SetRow(sectionButton, MenuItemRow);
                Grid.SetColumn(sectionButton, 3 + (j * 2));
                _layoutRoot.Children.Add(sectionButton);

                // save the button and text with the section
                section.UIButton = sectionButton;
                section.UIText = sectionText;

                // bump our counter
                j++;
            }

            // move to the first page
            MoveToPageIndex(0);
        }

        private void UpdateUI()
        {
            // temp
            double X = this.ActualHeight;
            double XX = this.Height;

            NavigationSection _selected = this.SelectedSection;

            // can go back
            if (null != _navGoBack)
            {
                _navGoBack.Visibility = (this.CanGoBack ? Visibility.Visible : Visibility.Collapsed);
            }

            // can go forward
            if (null != _navGoForward)
            {
                _navGoForward.Visibility = (this.CanGoForward ? Visibility.Visible : Visibility.Collapsed);
            }

            // if Home is enabled
            if ((this.IsHomeEnabled) && (null != _navHome) && (null != _navGoForward))
            {
                // if we can go forward
                if (this.CanGoForward)
                {
                    // we can go forward, so show go forward and hide home
                    _navGoForward.Visibility = Visibility.Visible;
                    _navHome.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // can't go forward, so hide go forward and show home
                    _navGoForward.Visibility = Visibility.Collapsed;
                    _navHome.Visibility = Visibility.Visible;
                }
            }

            // update nav section buttons

            // loop through the sections
            foreach (NavigationSection section in this.NavigationSections)
            {
                // get a reference to the button and text for this section
                Button button = section.UIButton;
                TextBlockEx text = section.UIText;

                if ((null != button) && (null != text))
                {
                    // if this section is the new selected section
                    if (section == this.SelectedSection)
                    {
                        // highlight it
                        text.TextStyle = TextStyles.NavigationSectionActive;

                        // set progress line length (if we have the line)
                        if (null != _navLine)
                        {
                            // get the position of the selected textblock
                            GeneralTransform ttv = button.TransformToVisual(Window.Current.Content);
                            Point p = ttv.TransformPoint(new Point(0, 0));
                            double x2 = p.X + button.ActualWidth;

                            // set the line length
                            _navLine.X2 = x2;

                            // set the color
                            _navLine.Stroke = new SolidColorBrush(Colors.White);
                        }
                    }
                    else
                    {
                        // lowlight it
                        text.TextStyle = TextStyles.NavigationSection;
                    }
                }
            }

            //// if we have a navline and are in "Intro" mode
            //if ((null != _navLine) && (NavigationSections.Unknown == this.SelectedSection))
            //{
            //    Button button = _navSectionButtons[0];

            //    if (null != button)
            //    {
            //        GeneralTransform ttv = button.TransformToVisual(Window.Current.Content);
            //        Point p = ttv.TransformPoint(new Point(0, 0));
            //        _navLine.X2 = p.X;
            //    }
            //}
        }

        #endregion

        #region Code Helpers

        #endregion

        #region UI Helpers

        public int GetPageIndexFromPage(NavigationSection section, INavigationItem item)
        {
            int index = -1;     // -1 is an error

            // if the section and page are valid
            if ((null != this.NavigationSections) && (this.NavigationSections.Count > 0)
                && (null != section) && (null != section.Items) && (section.Items.Count > 0)
                && (null != item))
            {
                // get the index of the section in the list of sections
                int sectionIndex = this.NavigationSections.IndexOf(section);

                // get the index of the item in the current section
                int itemIndex = section.GetItemIndex(item);

                // if both are valid
                if ((0 <= sectionIndex) && (0 <= itemIndex))
                {
                    // if this is the first section
                    if (0 == sectionIndex)
                    {
                        // then the page index is our value
                        index = itemIndex;
                    }
                    else
                    {
                        // if the section isn't the first, we have to add the items from the previous sections
                        int previousItemCount = 0;

                        // loop through the previous sections and add their page counts
                        for (int i = 0; i < sectionIndex; i++)
                        {
                            // make sure there's a section here and it has pages
                            if ((null != this.NavigationSections[i]) && (null != this.NavigationSections[i].Items))
                            {
                                // it does, so add those pages to our count
                                previousItemCount += this.NavigationSections[i].Items.Count;
                            }
                        }

                        // take the count of previous pages and add the page index to it 
                        index = previousItemCount + itemIndex;
                    }
                }
            }

            return index;
        }

        public void GetPageFromPageIndex(int pageIndex, out NavigationSection section, out INavigationItem item)
        {
            // default values
            section = null;
            item = null;

            // if there are sections
            if ((null != this.NavigationSections) && (this.NavigationSections.Count > 0))
            {
                // loop through the sections
                foreach (NavigationSection navigationSection in this.NavigationSections)
                {
                    // does the section have pages?
                    if ((null != navigationSection) && (null != navigationSection.Items))
                    {
                        // how many pages are in this section?
                        int pageCount = navigationSection.Items.Count;

                        // if there are more pages in this section than remain in the index
                        if (pageCount > pageIndex)
                        {
                            // this is the section where our page will be
                            section = navigationSection;
                            item = section.Items[pageIndex];

                            return;
                        }
                        else
                        {
                            // if we're here, this section can't exhaust pageIndex, 
                            // so subtract the section's page count from the index
                            pageIndex -= pageCount;
                        }
                    }
                }
            }
        }

        public void MoveToPageIndex(int pageIndex)
        {
            NavigationActions navigationAction = NavigationActions.Unknown;

            // we need a section and page
            NavigationSection section = null;
            INavigationItem item = null;

            // convert the index to section and page
            this.GetPageFromPageIndex(pageIndex, out section, out item);

            // if we got them
            if ((null != section) && (null != item))
            {
                // what action?
                if (0 == pageIndex)
                {
                    // go home
                    navigationAction = NavigationActions.Home;
                }

                // move to it
                MoveToPage(section, item, navigationAction);
            }
        }

        public void MoveToPreviousPage()
        {
            // get the selected section and page
            NavigationSection section = this.SelectedSection;
            INavigationItem item = this.SelectedItem;

            // if we can go back and we have a selected section and page
            if ((this.CanGoBack) && (null != this.SelectedSection) && (null != this.SelectedItem))
            {
                // get the indices of the selected section and page
                int sectionIndex = this.NavigationSections.IndexOf(section);
                int itemIndex = this.SelectedSection.Items.IndexOf(item);

                // is the current page the first in the current section?
                if (0 == itemIndex)
                {
                    // is the current section the first?
                    if (0 == sectionIndex)
                    {
                        // we can't go back from the first page of the first section
                        return;
                    }
                    else
                    {
                        // get the previous section
                        section = this.NavigationSections[sectionIndex - 1];

                        // if it's valid and has pages
                        if ((null != section) && (null != section.Items) && (section.Items.Count > 0))
                        {
                            // get its last page
                            item = section.Items[section.Items.Count - 1];
                        }
                    }
                }
                else
                {
                    // get the previous page
                    item = section.Items[itemIndex - 1];
                }

                // if we made it through the gauntlet with non-null section and page
                if ((null != section) && (null != item))
                {
                    MoveToPage(section, item, NavigationActions.GoBack);
                }
            }
        }

        public void MoveToNextPage()
        {
            // get the selected section and page
            NavigationSection section = this.SelectedSection;
            INavigationItem item = this.SelectedItem;

            // if we can go forward and we have a selected section and page
            if ((this.CanGoForward) && (null != this.SelectedSection) && (null != this.SelectedItem))
            {
                // get the indices of the selected section and page
                int sectionIndex = this.NavigationSections.IndexOf(section);
                int itemIndex = this.SelectedSection.Items.IndexOf(item);

                // is the current page the last in the current section?
                if ((section.Items.Count - 1) == itemIndex)
                {
                    // is the current section the last?
                    if ((this.NavigationSections.Count - 1) == sectionIndex)
                    {
                        // we can't go forward from the last page of the last section
                        return;
                    }
                    else
                    {
                        // get the next section
                        section = this.NavigationSections[sectionIndex + 1];

                        // if it's valid and has pages
                        if ((null != section) && (null != section.Items) && (section.Items.Count > 0))
                        {
                            // get its first page
                            item = section.Items[0];
                        }
                    }
                }
                else
                {
                    // get the next page
                    item = section.Items[itemIndex + 1];
                }

                // if we made it through the gauntlet with non-null section and page
                if ((null != section) && (null != item))
                {
                    MoveToPage(section, item, NavigationActions.GoForward);
                }
            }
        }

        public void MoveToSection(NavigationSection section)
        {
            // if the section is valid and it has pages
            if ((null != section) && (null != section.Items) && (section.Items.Count > 0))
            {
                // get the first page of the section
                INavigationItem item = section.Items.First<INavigationItem>();

                // if we got it
                if (null != item)
                {
                    // move to the section and page; assume we're triggered by a button click
                    MoveToPage(section, item, NavigationActions.Section);
                }
            }
        }

        public void MoveToPage(NavigationSection section, INavigationItem item, NavigationActions navigationAction = NavigationActions.Unknown)
        {
            // if the section is valid and it has pages
            if ((null != section) && (null != section.Items) && (section.Items.Count > 0))
            {
                // does the section contain the page we've been passed?
                if (section.Items.Contains<INavigationItem>(item))
                {
                    // it does, so save this section and page as selected
                    this.SelectedSection = section;
                    this.SelectedItem = item;

                    // set our go back/forward flags

                    // what's the index of this section and page
                    int sectionIndex = this.NavigationSections.IndexOf(section);
                    int itemIndex = section.Items.IndexOf(item);

                    // if this is the first section
                    if (0 == sectionIndex)
                    {
                        if (0 == itemIndex)
                        {
                            // we can't go back from the first page of the first section
                            this.CanGoBack = false;
                        }
                        else
                        {
                            // this is not the first page, so we can go back
                            this.CanGoBack = true;
                        }
                    }
                    else
                    {
                        // we aren't the first section, so can go back
                        this.CanGoBack = true;
                    }

                    // if this is the last section
                    if ((this.NavigationSections.Count - 1) == sectionIndex)
                    {
                        // is this the last page?
                        if ((section.Items.Count - 1) == itemIndex)
                        {
                            // this is the last page, so can't go forward
                            this.CanGoForward = false;
                        }
                        else
                        {
                            // not the last page, so can go forward
                            this.CanGoForward = true;
                        }
                    }
                    else
                    {
                        // this isn't the last section, so we can go forward
                        this.CanGoForward = true;
                    }

                    // we've made changes, so need to update the UI
                    this.UpdateUI();

                    // raise our navigate event
                    RaiseNavigateEvent(this, navigationAction, this.SelectedSection, this.SelectedItem);

                }
            }
        }

        #endregion

    }
}
