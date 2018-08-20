using System;
using System.Collections.Generic;
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
using SDX.Toolkit.Services;
using System.Globalization;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum NavigationActions
    {
        Unknown = 0,
        GoBack = 1,
        GoForward = 2,
        Experience = 3,
        Accessories = 4,
        BestOfWindows = 5,
        Comparison = 6
    }

    public enum NavigationSections
    {
        Experience = 0,
        Accessories = 1,
        BestOf = 2,
        Comparison = 3,
        Unknown = 99
    }

    public enum NavigationPages
    {
        IntroPage,
        ProductRotationStaticPage,
        ProductRotationTabletPage,
        ProductRotationStudioPage,
        ProductRotationLaptopPage,
        AccessoriesPenPage,
        AccessoriesKeyboardPage,
        AccessoriesMousePage,
        BestOfPage,
        ComparePage,
        Unknown
    }

    public class NavigateEventArgs : EventArgs
    {
        #region Private Members

        private NavigationActions _navAction;
        private NavigationSections _navSection;
        private NavigationPages _navPage;

        #endregion

        #region Constructors

        public NavigateEventArgs()
        {
        }

        public NavigateEventArgs(NavigationActions navAction, NavigationSections navSection, NavigationPages navPage)
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

        public NavigationSections NavSection
        {
            get { return _navSection; }
            set { _navSection = value; }
        }

        public NavigationPages NavPage
        {
            get { return _navPage; }
            set { _navPage = value; }
        }

        #endregion
    }

    public sealed class BottomNavBar : Control
    {
        #region Private Constants

        private const string URI_GOBACK_ACTIVE = "ms-appx:///Assets/BottomNavBar/back-nav-arrow-hover.png";
        private const string URI_GOBACK_INACTIVE = "ms-appx:///Assets/BottomNavBar/back-nav-arrow.png";
        private const string URI_GOFORWARD_ACTIVE = "ms-appx:///Assets/BottomNavBar/forward-nav-arrow-hover.png";
        private const string URI_GOFORWARD_INACTIVE = "ms-appx:///Assets/BottomNavBar/forward-nav-arrow.png";

        private const double BUTTON_SPACING = 30d;
        private const double WIDTH_ARROW = 15d;

        #endregion

        #region Private Members

        // UI members we need to keep track of
        Grid _layoutRoot;
        Line _navLine;
        List<Button> _navButtons = new List<Button>();
        Button _navExperience = null;
        Button _navAccessories = null;
        Button _navBestOf = null;
        Button _navCompare = null;
        Button _navGoBack;
        Button _navGoForward;
        Image _imgGoBack;
        Image _imgGoForward;

        #endregion

        #region Construction/Destruction

        public BottomNavBar()
        {
            this.DefaultStyleKey = typeof(BottomNavBar);

            // no focus visual indications
            this.UseSystemFocusVisuals = false;

            // event handlers
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            this.KeyUp += BottomNavBar_OnKeyUp;

            // set default properties
            this.SelectedSection = NavigationSections.Experience;
            this.SelectedPage = NavigationPages.IntroPage;
            this.CanGoBack = false;
            this.CanGoForward = true;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();

            this.UpdateUI();
        }

        #endregion

        #region Public Properties

        public NavigationSections SelectedSection { get; private set; }

        public NavigationPages SelectedPage { get; private set; }

        public bool CanGoBack { get; private set; }

        public bool CanGoForward { get; private set; }

        #endregion

        #region Dependency Properties

        // ExperienceButtonText
        public static readonly DependencyProperty ExperienceButtonTextProperty =
            DependencyProperty.Register("ExperienceButtonText", typeof(string), typeof(BottomNavBar), new PropertyMetadata(null, OnExperienceButtonTextChanged));

        public string ExperienceButtonText
        {
            get { return (string)GetValue(ExperienceButtonTextProperty); }
            set { SetValue(ExperienceButtonTextProperty, value); }
        }

        // AccessoriesButtonText
        public static readonly DependencyProperty AccessoriesButtonTextProperty =
            DependencyProperty.Register("AccessoriesButtonText", typeof(string), typeof(BottomNavBar), new PropertyMetadata(null, OnAccessoriesButtonTextChanged));

        public string AccessoriesButtonText
        {
            get { return (string)GetValue(AccessoriesButtonTextProperty); }
            set { SetValue(AccessoriesButtonTextProperty, value); }
        }

        // BestOfButtonText
        public static readonly DependencyProperty BestOfButtonTextProperty =
            DependencyProperty.Register("BestOfButtonText", typeof(string), typeof(BottomNavBar), new PropertyMetadata(null, OnBestOfButtonTextChanged));

        public string BestOfButtonText
        {
            get { return (string)GetValue(BestOfButtonTextProperty); }
            set { SetValue(BestOfButtonTextProperty, value); }
        }

        // CompareButtonText
        public static readonly DependencyProperty CompareButtonTextProperty =
            DependencyProperty.Register("CompareButtonText", typeof(string), typeof(BottomNavBar), new PropertyMetadata(null, OnCompareButtonTextChanged));

        public string CompareButtonText
        {
            get { return (string)GetValue(CompareButtonTextProperty); }
            set { SetValue(CompareButtonTextProperty, value); }
        }

        #endregion

        #region Custom Events

        public delegate void NavigateEvent(object sender, NavigateEventArgs e);

        public event NavigateEvent Navigate;

        private void RaiseNavigateEvent(BottomNavBar bottomNavBar, NavigateEventArgs e)
        {
            Navigate?.Invoke(bottomNavBar, e);
        }

        private void RaiseNavigateEvent(BottomNavBar bottomNavBar, NavigationActions navAction, NavigationSections navSection, NavigationPages navPage)
        {
            NavigateEventArgs args = new NavigateEventArgs(navAction, navSection, navPage);

            RaiseNavigateEvent(bottomNavBar, args);
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            ((BottomNavBar)sender).UpdateUI();
        }

        private static void OnExperienceButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAccessoriesButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnBestOfButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnCompareButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

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
                        RaiseNavigateEvent(this, NavigationActions.GoBack, this.SelectedSection, this.SelectedPage);
                    }
                    break;

                case "GoForward":
                    if (this.CanGoForward)
                    {
                        MoveToNextPage();
                        RaiseNavigateEvent(this, NavigationActions.GoForward, this.SelectedSection, this.SelectedPage);
                    }
                    break;

                case "Experience":
                    if (CanMoveToSection(NavigationSections.Experience))
                    {
                        MoveToSection(NavigationSections.Experience);
                        RaiseNavigateEvent(this, NavigationActions.Experience, this.SelectedSection, this.SelectedPage);
                    }
                    // telemetry - NavExperience
                    TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_NAVEXPERIENCE, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                    break;

                case "Accessories":
                    if (CanMoveToSection(NavigationSections.Accessories))
                    {
                        MoveToSection(NavigationSections.Accessories);
                        RaiseNavigateEvent(this, NavigationActions.Accessories, this.SelectedSection, this.SelectedPage);
                    }
                    // telemetry - NavAccessories
                    TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_NAVACCESSORIES, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                    break;

                case "BestOf":
                    if (CanMoveToSection(NavigationSections.BestOf))
                    {
                        MoveToSection(NavigationSections.BestOf);
                        RaiseNavigateEvent(this, NavigationActions.BestOfWindows, this.SelectedSection, this.SelectedPage);
                    }
                    // telemetry - NavBestOf
                    TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_NAVBESTOF, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                    break;

                case "Compare":
                    if (CanMoveToSection(NavigationSections.Comparison))
                    {
                        MoveToSection(NavigationSections.Comparison);
                        RaiseNavigateEvent(this, NavigationActions.Comparison, this.SelectedSection, this.SelectedPage);
                    }
                    // telemetry - NavCompare
                    TelemetryService.Current?.SendTelemetry(TelemetryService.TELEMETRY_NAVCOMPARISON, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
                    break;

                default:
                    break;
            }

            // update the UI to reflect the change
            this.UpdateUI();
        }

        //protected override void OnPreviewKeyUp(KeyRoutedEventArgs e)
        //{
        //    e.Handled = HandleKey(e.Key);

        //    base.OnPreviewKeyUp(e);
        //}

        private void BottomNavBar_OnKeyUp(object sender, KeyRoutedEventArgs e)
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
                        RaiseNavigateEvent(this, NavigationActions.GoForward, this.SelectedSection, this.SelectedPage);
                        handled = true;
                    }
                    break;

                case VirtualKey.Left:
                    if (this.CanGoBack)
                    {
                        MoveToPreviousPage();
                        RaiseNavigateEvent(this, NavigationActions.GoBack, this.SelectedSection, this.SelectedPage);
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

            // get the progress line
            if (null == _navLine) { _navLine = (Line)this.GetTemplateChild("ProgressLine"); }

            // test
            //TestHelper.AddGridCellBorders(_navGrid, 6, 8, Colors.DarkRed);

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
                        Source = new BitmapImage() { UriSource = new Uri((this.CanGoBack) ? URI_GOBACK_ACTIVE : URI_GOBACK_INACTIVE), DecodePixelWidth = (int)WIDTH_ARROW },
                        Width = WIDTH_ARROW
                    };
                }

                // create the button and add the image
                _navGoBack = new Button()
                {
                    Name = "GoBack",
                    Content = _imgGoBack
                };
                if (null != buttonStyle) { _navGoBack.Style = buttonStyle; }

                // set Grid properties
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
                        Source = new BitmapImage() { UriSource = new Uri((this.CanGoForward) ? URI_GOFORWARD_ACTIVE : URI_GOFORWARD_INACTIVE), DecodePixelWidth = (int)WIDTH_ARROW },
                        Width = WIDTH_ARROW
                    };
                }

                // create the button and add the image
                _navGoForward = new Button()
                {
                    Name = "GoForward",
                    Content = _imgGoForward
                };
                if (null != buttonStyle) { _navGoForward.Style = buttonStyle; }

                // add font properties
                //StyleHelper.SetFontCharacteristics(_navGoForward, ControlStyles.NavBarActive);

                // set inherited Grid properties
                Grid.SetRow(_navGoForward, MenuItemRow);
                Grid.SetColumn(_navGoForward, 7);

                // add the Click event handler
                _navGoForward.Click += this.NavItem_Click;

                // add it to the grid
                _layoutRoot.Children.Add(_navGoForward);
            }

            // experience button
            _navExperience = new Button()
            {
                Name = "Experience",
                Content = this.ExperienceButtonText
            };
            StyleHelper.SetFontCharacteristics(_navExperience, ControlStyles.NavBarActive);
            if (null != buttonStyle) { _navExperience.Style = buttonStyle; }
            _navExperience.Click += this.NavItem_Click;

            // add to the grid
            Grid.SetRow(_navExperience, MenuItemRow);
            Grid.SetColumn(_navExperience, 2);
            _layoutRoot.Children.Add(_navExperience);

            // add to the list
            _navButtons.Add(_navExperience);

            // accessories button
            _navAccessories = new Button()
            {
                Name = "Accessories",
                Content = this.AccessoriesButtonText
            };
            StyleHelper.SetFontCharacteristics(_navAccessories, ControlStyles.NavBarInactive);
            if (null != buttonStyle) { _navAccessories.Style = buttonStyle; }
            _navAccessories.Click += this.NavItem_Click;

            // add to the grid
            Grid.SetRow(_navAccessories, MenuItemRow);
            Grid.SetColumn(_navAccessories, 3);
            _layoutRoot.Children.Add(_navAccessories);

            // add to the list
            _navButtons.Add(_navAccessories);

            // best of button
            _navBestOf = new Button()
            {
                Name = "BestOf",
                Content = this.BestOfButtonText
            };
            StyleHelper.SetFontCharacteristics(_navBestOf, ControlStyles.NavBarInactive);
            if (null != buttonStyle) { _navBestOf.Style = buttonStyle; }
            _navBestOf.Click += this.NavItem_Click;

            // add to the grid
            Grid.SetRow(_navBestOf, MenuItemRow);
            Grid.SetColumn(_navBestOf, 4);
            _layoutRoot.Children.Add(_navBestOf);

            // add to the list
            _navButtons.Add(_navBestOf);

            // compare button
            _navCompare = new Button()
            {
                Name = "Compare",
                Content = this.CompareButtonText
            };
            StyleHelper.SetFontCharacteristics(_navCompare, ControlStyles.NavBarInactive);
            if (null != buttonStyle) { _navCompare.Style = buttonStyle; }
            _navCompare.Click += this.NavItem_Click;

            // add to the grid
            Grid.SetRow(_navCompare, MenuItemRow);
            Grid.SetColumn(_navCompare, 5);
            _layoutRoot.Children.Add(_navCompare);

            // add to the list
            _navButtons.Add(_navCompare);
        }

        private void UpdateUI()
        {
            // temp
            double X = this.ActualHeight;
            double XX = this.Height;

            NavigationSections _selected = this.SelectedSection;

            if (null == _navButtons)
            {
                return;
            }

            // can go back
            if (null != _navGoBack)
            {
                _imgGoBack.Source = new BitmapImage() { UriSource = new Uri(this.CanGoBack ? URI_GOBACK_ACTIVE : URI_GOBACK_INACTIVE), DecodePixelWidth = (int)WIDTH_ARROW };
            }

            // can go forward
            if (null != _navGoForward)
            {
                _imgGoForward.Source = new BitmapImage() { UriSource = new Uri(this.CanGoForward ? URI_GOFORWARD_ACTIVE : URI_GOFORWARD_INACTIVE), DecodePixelWidth = (int)WIDTH_ARROW };
            }

            // update nav text blocks
            if (null != _navButtons)
            {
                // loop through the nav items
                for (int i = 0; i < _navButtons.Count; i++)
                {
                    // construct the identifier string
                    // get a reference to the menu item
                    Button button = _navButtons[i];

                    if (null != button)
                    {
                        // if it's the new selected item
                        if (i == (int)_selected)
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

                // if we have a navline and are in "Intro" mode
                if ((null != _navLine) && (NavigationSections.Unknown == this.SelectedSection))
                {
                    Button button = _navButtons[0];

                    if (null != button)
                    {
                        GeneralTransform ttv = button.TransformToVisual(Window.Current.Content);
                        Point p = ttv.TransformPoint(new Point(0, 0));
                        _navLine.X2 = p.X;
                    }
                }
            }
        }

        #endregion

        #region Code Helpers

        #endregion

        #region UI Helpers

        private bool CanMoveToSection(NavigationSections section)
        {
            bool retVal = false;

            switch (section)
            {
                case NavigationSections.Experience:
                    retVal = true;
                    break;

                case NavigationSections.Accessories:
                    retVal = true;
                    break;

                case NavigationSections.BestOf:
                    retVal = true;
                    break;

                case NavigationSections.Comparison:
                    retVal = true;
                    break;

                case NavigationSections.Unknown:
                    retVal = false;
                    break;
            }

            return retVal;

        }

        public void MoveToSection(NavigationSections section)
        {
            bool updateUI = false;

            if (this.CanMoveToSection(section))
            {
                switch (section)
                {
                    case NavigationSections.Experience:
                        this.SelectedSection = section;
                        this.SelectedPage = NavigationPages.IntroPage;
                        this.CanGoBack = false;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationSections.Accessories:
                        this.SelectedSection = section;
                        this.SelectedPage = NavigationPages.AccessoriesPenPage;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationSections.BestOf:
                        this.SelectedSection = section;
                        this.SelectedPage = NavigationPages.BestOfPage;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationSections.Comparison:
                        this.SelectedSection = section;
                        this.SelectedPage = NavigationPages.ComparePage;
                        this.CanGoBack = true;
                        this.CanGoForward = false;
                        updateUI = true;
                        break;

                    case NavigationSections.Unknown:
                        break;
                }
            }

            if (updateUI) { this.UpdateUI(); }
        }

        public bool CanMoveToPage(NavigationPages page)
        {
            bool retVal = false;

            switch (page)
            {
                case NavigationPages.IntroPage:
                    retVal = true;
                    break;

                case NavigationPages.ProductRotationStaticPage:
                    retVal = true;
                    break;

                case NavigationPages.ProductRotationTabletPage:
                    retVal = true;
                    break;

                case NavigationPages.ProductRotationStudioPage:
                    retVal = true;
                    break;

                case NavigationPages.ProductRotationLaptopPage:
                    retVal = true;
                    break;

                case NavigationPages.AccessoriesPenPage:
                    retVal = true;
                    break;

                case NavigationPages.AccessoriesKeyboardPage:
                    retVal = true;
                    break;

                case NavigationPages.AccessoriesMousePage:
                    retVal = true;
                    break;

                case NavigationPages.BestOfPage:
                    retVal = true;
                    break;

                case NavigationPages.ComparePage:
                    retVal = true;
                    break;

                case NavigationPages.Unknown:
                    retVal = false;
                    break;

                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }

        //public void GoForward()
        //{
        //    if (this.CanGoForward)
        //    {
        //        NavigationPages newPage = this.SelectedPage + 1;

        //        MoveToPage(newPage);
        //    }
        //}

        //public void GoBack()
        //{
        //    if (this.CanGoBack)
        //    {
        //        NavigationPages newPage = this.SelectedPage - 1;

        //        MoveToPage(newPage);
        //    }
        //}

        public void MoveToPage(NavigationPages page)
        {
            bool updateUI = false;

            if (CanMoveToPage(page))
            {

                switch (page)
                {
                    case NavigationPages.IntroPage:
                        this.SelectedSection = NavigationSections.Experience;
                        this.SelectedPage = page;
                        this.CanGoBack = false;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.ProductRotationStaticPage:
                        this.SelectedSection = NavigationSections.Experience;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.ProductRotationTabletPage:
                        this.SelectedSection = NavigationSections.Experience;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.ProductRotationStudioPage:
                        this.SelectedSection = NavigationSections.Experience;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.ProductRotationLaptopPage:
                        this.SelectedSection = NavigationSections.Experience;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.AccessoriesPenPage:
                        this.SelectedSection = NavigationSections.Accessories;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.AccessoriesKeyboardPage:
                        this.SelectedSection = NavigationSections.Accessories;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.AccessoriesMousePage:
                        this.SelectedSection = NavigationSections.Accessories;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.BestOfPage:
                        this.SelectedSection = NavigationSections.BestOf;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = true;
                        updateUI = true;
                        break;

                    case NavigationPages.ComparePage:
                        this.SelectedSection = NavigationSections.Comparison;
                        this.SelectedPage = page;
                        this.CanGoBack = true;
                        this.CanGoForward = false;
                        updateUI = true;
                        break;

                    case NavigationPages.Unknown:
                        break;
                }
            }

            if (updateUI) { this.UpdateUI(); }
        }

        public void MoveToPreviousPage()
        {
            if (this.CanGoBack)
            {
                int current = (int)this.SelectedPage;

                NavigationPages back = (NavigationPages)(current - 1);

                if (CanMoveToPage(back))
                {
                    MoveToPage(back);
                }
            }
        }

        public void MoveToNextPage()
        {
            if (this.CanGoForward)
            {
                int current = (int)this.SelectedPage;

                NavigationPages forward = (NavigationPages)(current + 1);

                if (CanMoveToPage(forward))
                {
                    MoveToPage(forward);
                }
            }
        }

        #endregion

    }
}
