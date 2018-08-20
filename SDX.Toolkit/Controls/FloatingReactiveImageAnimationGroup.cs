using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class FloatingReactiveImageAnimationGroup : Control
    {

        #region Private Members

        // UI elements
        private Border _layoutRoot = null;
        private List<FloatingReactiveImage> _floaters = new List<FloatingReactiveImage>();

        #endregion


        #region Construction/Destruction

        public FloatingReactiveImageAnimationGroup()
        {
            this.DefaultStyleKey = typeof(FloatingReactiveImageAnimationGroup);

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
            ((FloatingReactiveImageAnimationGroup)sender).UpdateUI();
        }

        //private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
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

        #endregion


        #region Public Methods

        public FloatingReactiveImage AddFloatingReactiveImage(string name, FRImageTypes imageType, int X1, int Y1, bool autoStart)
        {
            // create the image
            FloatingReactiveImage image = new FloatingReactiveImage()
            {
                Name = name,
                FRImageType = imageType,
                AutoStart = autoStart
            };

            // add it to the list
            _floaters.Add(image);



            return image;
        }

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
