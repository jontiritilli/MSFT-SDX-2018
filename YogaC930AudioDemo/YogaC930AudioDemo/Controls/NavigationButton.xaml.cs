using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using YogaC930AudioDemo.Helpers;


namespace YogaC930AudioDemo.Controls
{
    public enum ColorSchemes
    {
        Light,
        Dark
    }

    public sealed partial class NavigationButton : UserControl
    {
        #region Private Properties


        #endregion


        #region Public Properties

        public Style NormalLightStyle;
        public Style PointerEnteredLightStyle;
        public Style PointerPressedLightStyle;
        public Style PointerReleasedLightStyle;

        public Style NormalDarkStyle;
        public Style PointerEnteredDarkStyle;
        public Style PointerPressedDarkStyle;
        public Style PointerReleasedDarkStyle;

        #endregion


        #region Construction

        public NavigationButton()
        {
            this.InitializeComponent();

            this.Loaded += this.NavigationButton_Loaded;
        }

        private void NavigationButton_Loaded(object sender, RoutedEventArgs e)
        {
            this.NormalLightStyle = StyleHelper.GetApplicationStyle("NavigationBarLightBackgroundNormalTextStyle");
            this.PointerEnteredLightStyle = StyleHelper.GetApplicationStyle("NavigationBarLightBackgroundPointerEnteredTextStyle");
            this.PointerPressedLightStyle = StyleHelper.GetApplicationStyle("NavigationBarLightBackgroundPointerPressedTextStyle");
            this.PointerReleasedLightStyle = StyleHelper.GetApplicationStyle("NavigationBarLightBackgroundPointerReleasedTextStyle");

            this.NormalDarkStyle = StyleHelper.GetApplicationStyle("NavigationBarDarkBackgroundNormalTextStyle");
            this.PointerEnteredDarkStyle = StyleHelper.GetApplicationStyle("NavigationBarDarkBackgroundPointerEnteredTextStyle");
            this.PointerPressedDarkStyle = StyleHelper.GetApplicationStyle("NavigationBarDarkBackgroundPointerPressedTextStyle");
            this.PointerReleasedDarkStyle = StyleHelper.GetApplicationStyle("NavigationBarDarkBackgroundPointerReleasedTextStyle");

            SetStyle(this.NormalLightStyle, this.NormalDarkStyle);
        }

        #endregion


        #region Dependency Properties

        // ColorScheme
        public static readonly DependencyProperty ColorSchemeProperty =
            DependencyProperty.Register("ColorScheme", typeof(ColorSchemes), typeof(NavigationButton),
                                        new PropertyMetadata(ColorSchemes.Light, OnColorSchemePropertyChanged));

        public ColorSchemes ColorScheme
        {
            get { return (ColorSchemes)GetValue(ColorSchemeProperty); }
            set { SetValue(ColorSchemeProperty, value); }
        }

        // Caption
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(NavigationButton), new PropertyMetadata(String.Empty));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        #endregion


        #region Custom Events

        public delegate void ClickEvent(object sender, PointerRoutedEventArgs e);

        public event ClickEvent Click;

        private void RaiseClickEvent(NavigationButton navigationBar, PointerRoutedEventArgs e)
        {
            Click?.Invoke(navigationBar, e);
        }

        #endregion


        #region Event Handlers

        private static void OnColorSchemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationButton navigationButton)
            {
                navigationButton.SetStyle(navigationButton.NormalLightStyle, navigationButton.NormalDarkStyle);
            }
        }

        private void TheTextBlock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetStyle(this.PointerEnteredLightStyle, this.PointerEnteredDarkStyle);
        }

        private void TheTextBlock_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            SetStyle(this.PointerEnteredLightStyle, this.PointerEnteredDarkStyle);
        }

        private void TheTextBlock_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SetStyle(this.NormalLightStyle, this.NormalDarkStyle);
        }

        private void TheTextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetStyle(this.PointerPressedLightStyle, this.PointerPressedDarkStyle);
        }

        private void TheTextBlock_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SetStyle(this.PointerReleasedLightStyle, this.PointerReleasedDarkStyle);

            RaiseClickEvent(this, e);
        }

        #endregion


        #region Helpers

        private void SetStyle(Style lightStyle, Style darkStyle)
        {
            if (null != this.TheTextBlock)
            {
                this.TheTextBlock.Style = (ColorSchemes.Light == this.ColorScheme) ? lightStyle : darkStyle;
            }
        }

        #endregion
    }
}
