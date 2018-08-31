using System;
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
        private NavigationPage _navPage;

        #endregion

        #region Constructors

        public NavigateEventArgs()
        {
        }

        public NavigateEventArgs(NavigationActions navAction, NavigationSection navSection, NavigationPage navPage)
        {
            NavAction = navAction;
            NavSection = navSection;
            NavPage = navPage;
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

        public NavigationPage NavPage
        {
            get { return _navPage; }
            set { _navPage = value; }
        }

        #endregion
    }

    public sealed class NavigationBar : Control
    {
        #region Private Constants

        private const string URI_HOME = "ms-appx:///Assets/NavigationBar/navHome.svg";
        private const string URI_CHEVRON_BACK = "ms-appx:///Assets/NavigationBar/navChevronLeft.svg";
        private const string URI_CHEVRON_FORWARD = "ms-appx:///Assets/NavigationBar/navChevronRight.svg";

        //private const string URI_GOBACK_ACTIVE = "ms-appx:///Assets/NavigationBar/back-nav-arrow-hover.png";
        //private const string URI_GOBACK_INACTIVE = "ms-appx:///Assets/NavigationBar/back-nav-arrow.png";
        //private const string URI_GOFORWARD_ACTIVE = "ms-appx:///Assets/NavigationBar/forward-nav-arrow-hover.png";
        //private const string URI_GOFORWARD_INACTIVE = "ms-appx:///Assets/NavigationBar/forward-nav-arrow.png";

        //private const string URI_HOME = "ms-appx:///Assets/NavigationBar/home.png";

        private const double BUTTON_SPACING = 30d;
        private const double WIDTH_ARROW = 15d;
        private const double WIDTH_HOME = 30d;

        #endregion

        #region Private Members

        // UI members we need to keep track of
        Grid _layoutRoot;
        Canvas _lineCanvas;
        Line _navLine;
        Button _navGoBack;
        Button _navGoForward;
        Button _navHome = null;
        Image _imgGoBack;
        Image _imgGoForward;
        Image _imgHome;

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
            this.SelectedPage = null;
            this.CanGoBack = false;
            this.CanGoForward = true;

            if (null == this.NavigationSections)
            {
                this.NavigationSections = new List<NavigationSection>();
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

        public NavigationPage SelectedPage { get; private set; }

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

        #endregion

        #region Custom Events

        public delegate void NavigateEvent(object sender, NavigateEventArgs e);

        public event NavigateEvent Navigate;

        private void RaiseNavigateEvent(NavigationBar navigationBar, NavigateEventArgs e)
        {
            Navigate?.Invoke(navigationBar, e);
        }

        private void RaiseNavigateEvent(NavigationBar navigationBar, NavigationActions navAction, NavigationSection navSection, NavigationPage navPage)
        {
            NavigateEventArgs args = new NavigateEventArgs(navAction, navSection, navPage);

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
            // get the nav grid
            if (null == _layoutRoot) { _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot"); }

            // if we couldn't get the grid we can't do anything, so exit
            if (null == _layoutRoot) { return; }

            // set up the grid

            // rows - this is static
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(6) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.5, GridUnitType.Star) });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.5, GridUnitType.Star) });

            // columns - this is dynamic based on sections
            // go back and spacer
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });

            // column for each section
            for (int i = 0; i < this.NavigationSections.Count; i++)
            {
                _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            // spacer and go forward
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
            _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });

            // create the progress line and the canvas that contains it

            // canvas
            _lineCanvas = new Canvas()
            {
                Height = 6,
                Margin = new Thickness(0)
            };
            _layoutRoot.Children.Add(_lineCanvas);
            Grid.SetRow(_lineCanvas, 0);
            Grid.SetColumn(_lineCanvas, 0);
            Grid.SetColumnSpan(_lineCanvas, 2 + this.NavigationSections.Count);

            // line
            _navLine = new Line()
            {
                Name = "ProgressLine",
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 6d,
                X1 = 0,
                Y1 = 3,
                X2 = 0,
                Y2 = 3,
                Margin = new Thickness(0)
            };
            _lineCanvas.Children.Add(_navLine);

            // test
            //TestHelper.AddGridCellBorders(_layoutRoot, 6, 8, Colors.DarkRed);

            // create the button style
            Style buttonStyle = StyleHelper.GetApplicationStyle("NoInteractionButton");

            // set column spacing for the grid
            _layoutRoot.ColumnSpacing = BUTTON_SPACING;

            // grid row for menu items
            int MenuItemRow = 2;

            // create the Go Back button
            if (null == _navGoBack)
            {
                // if the goback image is null, create it
                if (null == _imgGoBack)
                {
                    _imgGoBack = new Image()
                    {
                        Source = new SvgImageSource() { UriSource = new Uri(URI_CHEVRON_BACK), RasterizePixelWidth = (int)WIDTH_ARROW },
                        Width = WIDTH_ARROW
                    };
                }

                // create the button and add the image
                _navGoBack = new Button()
                {
                    Name = "GoBack",
                    Content = _imgGoBack,
                    Visibility = (this.CanGoBack ? Visibility.Visible : Visibility.Collapsed)
                };
                if (null != buttonStyle) { _navGoBack.Style = buttonStyle; }

                // set inherited Grid properties
                Grid.SetRow(_navGoBack, MenuItemRow);
                Grid.SetColumn(_navGoBack, 0);

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
                        Source = new SvgImageSource() { UriSource = new Uri(URI_CHEVRON_FORWARD), RasterizePixelWidth = (int)WIDTH_ARROW },
                        Width = WIDTH_ARROW
                    };
                }

                // create the button and add the image
                _navGoForward = new Button()
                {
                    Name = "GoForward",
                    Content = _imgGoForward,
                    Visibility = (this.CanGoForward ? Visibility.Visible : Visibility.Collapsed)
                };
                if (null != buttonStyle) { _navGoForward.Style = buttonStyle; }

                // set inherited Grid properties
                Grid.SetRow(_navGoForward, MenuItemRow);
                Grid.SetColumn(_navGoForward, this.NavigationSections.Count + 3);

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
                            Source = new SvgImageSource() { UriSource = new Uri(URI_HOME), RasterizePixelWidth = (int)WIDTH_ARROW },
                            Width = WIDTH_ARROW,
                            Visibility = (this.CanGoBack ? Visibility.Visible : Visibility.Collapsed)
                        };
                    }

                    // create the button and add the image
                    _navHome = new Button()
                    {
                        Name = "Home",
                        Content = _imgHome,
                        Visibility = Visibility.Collapsed
                    };
                    if (null != buttonStyle) { _navHome.Style = buttonStyle; }

                    // set inherited Grid properties
                    Grid.SetRow(_navHome, MenuItemRow);
                    Grid.SetColumn(_navHome, this.NavigationSections.Count + 3);

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

                Button sectionButton = new Button()
                {
                    Name = section.Name,
                    Content = section.Text
                };
                StyleHelper.SetFontCharacteristics(sectionButton, ControlStyles.NavBarActive);
                if (null != buttonStyle) { sectionButton.Style = buttonStyle; }
                sectionButton.Click += this.NavItem_Click;

                // add to the grid
                Grid.SetRow(sectionButton, MenuItemRow);
                Grid.SetColumn(sectionButton, 2 + j);
                _layoutRoot.Children.Add(sectionButton);

                // save the button with the section
                section.UIButton = sectionButton;

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
                // get a reference to the button for this section
                Button button = section.UIButton;

                if (null != button)
                {
                    // if this section is the new selected section
                    if (section == this.SelectedSection)
                    {
                        // highlight it
                        StyleHelper.SetFontCharacteristics(button, ControlStyles.NavBarActive);

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
                        StyleHelper.SetFontCharacteristics(button, ControlStyles.NavBarInactive);
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

        public int GetPageIndexFromPage(NavigationSection section, NavigationPage page)
        {
            int index = -1;     // -1 is an error

            // if the section and page are valid
            if ((null != this.NavigationSections) && (this.NavigationSections.Count > 0)
                && (null != section) && (null != section.Pages) && (section.Pages.Count > 0)
                && (null != page))
            {
                // get the index of the section in the list of sections
                int sectionIndex = this.NavigationSections.IndexOf(section);

                // get the index of the page in the current section
                int pageIndex = section.Pages.IndexOf(page);

                // if both are valid
                if ((0 <= sectionIndex) && (0 <= pageIndex))
                {
                    // if this is the first section
                    if (0 == sectionIndex)
                    {
                        // then the page index is our value
                        index = pageIndex;
                    }
                    else
                    {
                        // if the section isn't the first, we have to add the pages from the previous sections
                        int previousPageCount = 0;

                        // loop through the previous sections and add their page counts
                        for (int i = 0; i < sectionIndex; i++)
                        {
                            // make sure there's a section here and it has pages
                            if ((null != this.NavigationSections[i]) && (null != this.NavigationSections[i].Pages))
                            {
                                // it does, so add those pages to our count
                                previousPageCount += this.NavigationSections[i].Pages.Count;
                            }
                        }

                        // take the count of previous pages and add the page index to it 
                        index = previousPageCount + pageIndex;
                    }
                }
            }

            return index;
        }

        public void GetPageFromPageIndex(int pageIndex, out NavigationSection section, out NavigationPage page)
        {
            // default values
            section = null;
            page = null;

            // if there are sections
            if ((null != this.NavigationSections) && (this.NavigationSections.Count > 0))
            {
                // loop through the sections
                foreach (NavigationSection navigationSection in this.NavigationSections)
                {
                    // does the section have pages?
                    if ((null != navigationSection) && (null != navigationSection.Pages))
                    {
                        // how many pages are in this section?
                        int pageCount = navigationSection.Pages.Count;

                        // if there are more pages in this section than remain in the index
                        if (pageCount > pageIndex)
                        {
                            // this is the section where our page will be
                            section = navigationSection;
                            page = section.Pages[pageIndex];

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
            NavigationPage page = null;

            // convert the index to section and page
            this.GetPageFromPageIndex(pageIndex, out section, out page);

            // if we got them
            if ((null != section) && (null != page))
            {
                // what action?
                if (0 == pageIndex)
                {
                    // go home
                    navigationAction = NavigationActions.Home;
                }

                // move to it
                MoveToPage(section, page, navigationAction);
            }
        }

        public void MoveToPreviousPage()
        {
            // get the selected section and page
            NavigationSection section = this.SelectedSection;
            NavigationPage page = this.SelectedPage;

            // if we can go back and we have a selected section and page
            if ((this.CanGoBack) && (null != this.SelectedSection) && (null != this.SelectedPage))
            {
                // get the indices of the selected section and page
                int sectionIndex = this.NavigationSections.IndexOf(section);
                int pageIndex = this.SelectedSection.Pages.IndexOf(page);

                // is the current page the first in the current section?
                if (0 == pageIndex)
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
                        if ((null != section) && (null != section.Pages) && (section.Pages.Count > 0))
                        {
                            // get its last page
                            page = section.Pages[section.Pages.Count - 1];
                        }
                    }
                }
                else
                {
                    // get the previous page
                    page = section.Pages[pageIndex - 1];
                }

                // if we made it through the gauntlet with non-null section and page
                if ((null != section) && (null != page))
                {
                    MoveToPage(section, page, NavigationActions.GoBack);
                }
            }
        }

        public void MoveToNextPage()
        {
            // get the selected section and page
            NavigationSection section = this.SelectedSection;
            NavigationPage page = this.SelectedPage;

            // if we can go forward and we have a selected section and page
            if ((this.CanGoForward) && (null != this.SelectedSection) && (null != this.SelectedPage))
            {
                // get the indices of the selected section and page
                int sectionIndex = this.NavigationSections.IndexOf(section);
                int pageIndex = this.SelectedSection.Pages.IndexOf(page);

                // is the current page the last in the current section?
                if ((section.Pages.Count - 1) == pageIndex)
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
                        if ((null != section) && (null != section.Pages) && (section.Pages.Count > 0))
                        {
                            // get its first page
                            page = section.Pages[0];
                        }
                    }
                }
                else
                {
                    // get the next page
                    page = section.Pages[pageIndex + 1];
                }

                // if we made it through the gauntlet with non-null section and page
                if ((null != section) && (null != page))
                {
                    MoveToPage(section, page, NavigationActions.GoForward);
                }
            }
        }

        public void MoveToSection(NavigationSection section)
        {
            // if the section is valid and it has pages
            if ((null != section) && (null != section.Pages) && (section.Pages.Count > 0))
            {
                // get the first page of the section
                NavigationPage page = section.Pages.First<NavigationPage>();

                // if we got it
                if (null != page)
                {
                    // move to the section and page; assume we're triggered by a button click
                    MoveToPage(section, page, NavigationActions.Section);
                }
            }
        }

        public void MoveToPage(NavigationSection section, NavigationPage page, NavigationActions navigationAction = NavigationActions.Unknown)
        {
            // if the section is valid and it has pages
            if ((null != section) && (null != section.Pages) && (section.Pages.Count > 0))
            {
                // does the section contain the page we've been passed?
                if (section.Pages.Contains<NavigationPage>(page))
                {
                    // it does, so save this section and page as selected
                    this.SelectedSection = section;
                    this.SelectedPage = page;

                    // set our go back/forward flags

                    // what's the index of this section and page
                    int sectionIndex = this.NavigationSections.IndexOf(section);
                    int pageIndex = section.Pages.IndexOf(page);

                    // if this is the first section
                    if (0 == sectionIndex)
                    {
                        if (0 == pageIndex)
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
                        if ((section.Pages.Count - 1) == pageIndex)
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
                    RaiseNavigateEvent(this, navigationAction, this.SelectedSection, this.SelectedPage);

                }
            }
        }

        #endregion

    }
}
