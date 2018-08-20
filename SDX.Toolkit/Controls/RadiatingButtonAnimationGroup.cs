using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class RadiatingButtonAnimationGroup : Control
    {
        #region Private Members

        // dependency property is a string that we translate to this list
        //List<string> _navList = new List<string>();

        // UI members we need to keep track of
        //Grid _navGrid;
        //StackPanel _navStackPanel;
        //List<Button> _navButtons;
        //Line _navLine;
        //Button _navGoBack;
        //Button _navGoForward;

        //Brush _textHighlight = new SolidColorBrush(Windows.UI.Colors.White);
        //Brush _textLowlight = new SolidColorBrush(ColorHelper.FromArgb(255, 112, 112, 112));

        #endregion


        #region Construction/Destruction

        public RadiatingButtonAnimationGroup()
        {
            this.DefaultStyleKey = typeof(RadiatingButtonAnimationGroup);

            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();

            this.UpdateUI();
        }

        #endregion

        #region Dependency Properties

        //// SelectedIndex
        //public static readonly DependencyProperty SelectedIndexProperty =
        //    DependencyProperty.Register("SelectedIndex", typeof(int), typeof(BottomNavBar), new PropertyMetadata(-1, OnSelectedIndexChanged));

        //public int SelectedIndex
        //{
        //    get { return (int)GetValue(SelectedIndexProperty); }
        //    set
        //    {
        //        // correct out of range values
        //        //if (value < -1)
        //        //{
        //        //    value = -1;
        //        //}
        //        //if (value >= this._navList.Count)
        //        //{
        //        //    value = this._navList.Count - 1;
        //        //}

        //        // save it
        //        SetValue(SelectedIndexProperty, value);

        //    }
        //}

        //// NavItems
        //public static readonly DependencyProperty NavItemsProperty =
        //    DependencyProperty.Register("NavItems", typeof(string), typeof(BottomNavBar), new PropertyMetadata(null, OnNavItemsChanged));

        //public string NavItems
        //{
        //    get { return (string)GetValue(NavItemsProperty); }
        //    set
        //    {
        //        // if it's valid
        //        if (String.IsNullOrWhiteSpace(value))
        //        { return; }

        //        // save it
        //        SetValue(NavItemsProperty, value);
        //    }
        //}

        //// CanGoBack
        //public static readonly DependencyProperty CanGoBackProperty =
        //        DependencyProperty.Register("CanGoBack", typeof(bool), typeof(BottomNavBar), new PropertyMetadata(false, OnCanGoBackChanged));

        //public bool CanGoBack
        //{
        //    get { return (bool)GetValue(CanGoBackProperty); }
        //    set { SetValue(CanGoBackProperty, value); }
        //}

        //// CanGoForward
        //public static readonly DependencyProperty CanGoForwardProperty =
        //        DependencyProperty.Register("CanGoForward", typeof(bool), typeof(BottomNavBar), new PropertyMetadata(true, OnCanGoForwardChanged));

        //public bool CanGoForward
        //{
        //    get { return (bool)GetValue(CanGoForwardProperty); }
        //    set { SetValue(CanGoForwardProperty, value); }
        //}

        #endregion

        #region Custom Events

        //public delegate void OnNavigation(object sender, BottomNavBarEventArgs e);

        //public event OnNavigation NavBarNavEvent;

        //private void RaiseNavigationEvent(BottomNavBar bottomNavBar, BottomNavBarEventArgs e)
        //{
        //    NavBarNavEvent?.Invoke(bottomNavBar, e);
        //}

        //private void RaiseNavigationEvent(BottomNavBar bottomNavBar, BottomNavBarNavType navAction)
        //{
        //    BottomNavBarEventArgs args = new BottomNavBarEventArgs(navAction);

        //    RaiseNavigationEvent(bottomNavBar, args);
        //}

        //private void RaiseNavigationEvent(BottomNavBar bottomNavBar, BottomNavBarNavType navAction, int selectedIndex, string itemText)
        //{
        //    BottomNavBarEventArgs args = new BottomNavBarEventArgs(navAction, selectedIndex, itemText);

        //    RaiseNavigationEvent(bottomNavBar, args);
        //}

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            ((RadiatingButtonAnimationGroup)sender).UpdateUI();
        }

        //private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((RadiatingButtonAnimationGroup)d).UpdateUI();
        //}

        //private static void OnNavItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    // re-render the UI
        //    ((RadiatingButtonAnimationGroup)d).RenderUI();

        //    // update the UI to set state
        //    ((RadiatingButtonAnimationGroup)d).UpdateUI();
        //}

        //private static void OnCanGoBackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((BottomNavBar)d).UpdateUI();
        //}

        //private static void OnCanGoForwardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((BottomNavBar)d).UpdateUI();
        //}

        //private void NavItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        //{
        //    Button button = (Button)sender;

        //    switch (button.Name)
        //    {
        //        case "GoBack":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.GoBack);
        //            break;

        //        case "GoForward":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.GoForward);
        //            break;

        //        case "Item_0":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.NavBar, 0, (string)button.Content);
        //            break;

        //        case "Item_1":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.NavBar, 1, (string)button.Content);
        //            break;

        //        case "Item_2":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.NavBar, 2, (string)button.Content);
        //            break;

        //        case "Item_3":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.NavBar, 3, (string)button.Content);
        //            break;

        //        case "Item_4":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.NavBar, 4, (string)button.Content);
        //            break;

        //        case "Item_5":
        //            RaiseNavigationEvent(this, BottomNavBarNavType.NavBar, 5, (string)button.Content);
        //            break;


        //    }
        //}

        #endregion

        #region UI Methods

        private void RenderUI()
        {
           
        }

        private void UpdateUI()
        {

        }

        #endregion

        #region Code Helpers

        #endregion

        #region UI Helpers

        #endregion
    }
}
