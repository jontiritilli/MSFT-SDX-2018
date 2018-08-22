using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;


namespace SDX.Toolkit.Helpers
{
    public enum ControlStyles
    {
        Hero,               // Segoe UI Light, 480px, white
        Headline,           // Segoe UI Light, 200px, white
        Lede,               // Segoe UI Regular, 86px, white
        ListHeadline,       // Segoe UI Regular, 110px, White
        ListLede,           // Segoe UI Regular, 86px, White
        CTAText,            // Segoe UI Bold, 60px, #454545
        PopupText,          // Segoe UI Regular, 86px, Black
        NavBarActive,       // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
        NavBarInactive,     // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
        SwipeLeft,          // Segoe UI Bold, 60px, White
        SwipeToContinue,    // Segoe UI Semibold, 70px, White
        ButtonText,         // Segoe UI Bold, 60px, White  (Button.Height=204px, Color: Black on 20% opacity)
        SliderText,         // Segoe UI Bold, 60px, White
        TouchHere,          // Segoe UI Bold, 60px, White
        FastChargeHeader,   // Segoe UI Regular, 100px, Black
        FastChargeLede,     // Segoe UI Regular, 86px, Black
        FastChargePercent,  // Segoe UI Light, 300px, White
        FastChargeTimeline, // Segoe UI Bold, 60px, Black
        GaugePoint,         // Segoe UI Bold, 60px, White
        GaugePointBullet,   // Segoe UI Bold, 60px, White
        Footnote,           // Segoe UI Regular, 60px, #5C5C5C
        LearnMore,          // Segoe UI Bold, 60px, #454545
        CompareHeader,      // Segoe UI Light, 200px, Black
        CompareTitle,       // Segoe UI Bold, 80px, Black
        CompareText,        // Segoe UI Regular, 80px, Black
        CompareLegal        // Segoe UI Regular, 70px, #5C5C5C
    }

    public static class StyleHelper
    {
        // these are "pixel" to "point" conversions based on pixels/SCALE_FACTOR = point
        private static readonly double SCALE_FACTOR = 4.2;

        public static readonly double FONTSIZE_60px = Math.Truncate(60d / SCALE_FACTOR);
        public static readonly double FONTSIZE_70px = Math.Truncate(70d / SCALE_FACTOR);
        public static readonly double FONTSIZE_80px = Math.Truncate(80d / SCALE_FACTOR);
        public static readonly double FONTSIZE_86px = Math.Truncate(86d / SCALE_FACTOR);
        public static readonly double FONTSIZE_100px = Math.Truncate(100d / SCALE_FACTOR);
        public static readonly double FONTSIZE_110px = Math.Truncate(110d / SCALE_FACTOR);
        public static readonly double FONTSIZE_200px = Math.Truncate(200d / SCALE_FACTOR);
        public static readonly double FONTSIZE_300px = Math.Truncate(300d / SCALE_FACTOR);
        public static readonly double FONTSIZE_400px = Math.Truncate(400d / SCALE_FACTOR);
        public static readonly double FONTSIZE_480px = Math.Truncate(480d / SCALE_FACTOR);

        public static readonly double ELEMENT_HEIGHT_204PX = Math.Truncate(204d / SCALE_FACTOR);


        public static double PopupBackgroundOpacity { get => 0.95d; }

        public static Style GetApplicationStyle(string name)
        {
            Style style = null;

            try
            {
                style = (Style)Application.Current.Resources[name];
            }
            catch (Exception)
            {

            }

            return style;
        }

        public static string GetFontFamily()
        {
            // default font family
            string fontFamily = "SegoeUI";

            //if (null != LocalizationService.Current)
            //{
            //    if (LocalizationService.Current.IsLanguageJapanese())
            //    {
            //        fontFamily = "UE Gothic";
            //    }
            //}

            return fontFamily;
        }

        public static void SetFontCharacteristics(Control element, ControlStyles fontStyle)
        {
            // get font family
            string fontFamily = GetFontFamily();

            switch (fontStyle)
            {
                case ControlStyles.Hero:            // Segoe UI Light, 480px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_480px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.Headline:        // Segoe UI Light, 200px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_200px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.Lede:            // Segoe UI Regular, 86px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ListHeadline:    // Segoe UI Regular, 110px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_110px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ListLede:        // Segoe UI Regular, 86px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.CTAText:         // Segoe UI Bold, 60px, #454545
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    break;

                case ControlStyles.PopupText:           // Segoe UI Regular, 86px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.NavBarActive:          // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    element.Background = new SolidColorBrush(Colors.Black);
                    element.Foreground = new SolidColorBrush(Colors.White);
                    element.HorizontalAlignment = HorizontalAlignment.Left;
                    element.VerticalAlignment = VerticalAlignment.Center;
                    element.Margin = new Thickness(5d, 5d, 5d, 5d);
                    break;

                case ControlStyles.NavBarInactive:          // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    element.Background = new SolidColorBrush(Colors.Black);
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 112, 112, 112));
                    element.HorizontalAlignment = HorizontalAlignment.Left;
                    element.VerticalAlignment = VerticalAlignment.Center;
                    element.Margin = new Thickness(5d, 5d, 5d, 5d);
                    break;

                case ControlStyles.SwipeLeft:       // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.SwipeToContinue: // Segoe UI Semibold, 70px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ButtonText:      // Segoe UI Bold, 60px, White  (Button.Height=204px, Color: Black on 20% opacity)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    element.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
                    element.Height = ELEMENT_HEIGHT_204PX;
                    break;

                case ControlStyles.SliderText:    // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.TouchHere:       // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.FastChargeHeader:// Segoe UI Regular, 100px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_100px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.FastChargeLede:  // Segoe UI Regular, 86px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.FastChargePercent: // Segoe UI Light, 300px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_300px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.FastChargeTimeline:// Segoe UI Bold, 60px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.GaugePoint:      // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.GaugePointBullet:      // Segoe UI Bold, 60px, Black, 30% Opacity
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush() { Color = Colors.Black, Opacity = 0.3 };
                    break;

                case ControlStyles.CompareHeader:       // Segoe UI Light, 200px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_200px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareTitle:        // Segoe UI Bold, 80px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_80px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareText:         // Segoe UI Regular, 80px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_80px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareLegal:        // Segoe UI Regular, 70px, #5C5C5C
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_70px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
                    break;

                case ControlStyles.Footnote:      // Segoe UI Regular, 60px, #5C5C5C
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
                    break;

                case ControlStyles.LearnMore:      // Segoe UI Bold, 60px, #454545
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    break;

                default:
                    break;
            }
        }

        public static void SetFontCharacteristics(TextElement element, ControlStyles fontStyle)
        {
            // get font family
            string fontFamily = GetFontFamily();

            switch (fontStyle)
            {
                case ControlStyles.Hero:            // Segoe UI Light, 480px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_480px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.Headline:        // Segoe UI Light, 200px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_200px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.Lede:            // Segoe UI Regular, 86px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ListHeadline:    // Segoe UI Regular, 110px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_110px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ListLede:        // Segoe UI Regular, 86px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.CTAText:         // Segoe UI Bold, 60px, #454545
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    break;

                case ControlStyles.PopupText:           // Segoe UI Regular, 86px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.NavBarActive:          // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    //element.Background = new SolidColorBrush(Colors.Black);
                    element.Foreground = new SolidColorBrush(Colors.White);
                    //element.HorizontalAlignment = HorizontalAlignment.Left;
                    //element.VerticalAlignment = VerticalAlignment.Center;
                    //element.Margin = new Thickness(5d, 5d, 5d, 5d);
                    break;

                case ControlStyles.NavBarInactive:          // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    //element.Background = new SolidColorBrush(Colors.Black);
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 112, 112, 112));
                    //element.HorizontalAlignment = HorizontalAlignment.Left;
                    //element.VerticalAlignment = VerticalAlignment.Center;
                    //element.Margin = new Thickness(5d, 5d, 5d, 5d);
                    break;

                case ControlStyles.SwipeLeft:       // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.SwipeToContinue: // Segoe UI Semibold, 70px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ButtonText:      // Segoe UI Bold, 60px, White  (Button.Height=204px, Color: Black on 20% opacity)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    //element.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
                    //element.Height = ELEMENT_HEIGHT_204PX;
                    break;

                case ControlStyles.SliderText:    // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.TouchHere:       // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.FastChargeHeader:// Segoe UI Regular, 100px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_100px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.FastChargeLede:  // Segoe UI Regular, 86px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.FastChargePercent: // Segoe UI Light, 300px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_300px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.FastChargeTimeline:// Segoe UI Bold, 60px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.GaugePoint:      // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.GaugePointBullet:      // Segoe UI Bold, 60px, Black, 30% Opacity
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush() { Color = Colors.Black, Opacity = 0.3 };
                    break;

                case ControlStyles.CompareHeader:       // Segoe UI Light, 200px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_200px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareTitle:        // Segoe UI Bold, 80px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_80px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareText:         // Segoe UI Regular, 80px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_80px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareLegal:        // Segoe UI Regular, 70px, #5C5C5C
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_70px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
                    break;

                case ControlStyles.Footnote:      // Segoe UI Regular, 60px, #5C5C5C
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
                    break;

                case ControlStyles.LearnMore:      // Segoe UI Bold, 60px, #454545
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    break;

                default:
                    break;
            }
        }

        public static void SetFontCharacteristics(TextBlock element, ControlStyles fontStyle)
        {
            // get font family
            string fontFamily = GetFontFamily();

            switch (fontStyle)
            {
                case ControlStyles.Hero:            // Segoe UI Light, 480px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_480px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.Headline:        // Segoe UI Light, 200px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_200px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.Lede:            // Segoe UI Regular, 86px, white
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ListHeadline:    // Segoe UI Regular, 110px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_110px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ListLede:        // Segoe UI Regular, 86px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.CTAText:         // Segoe UI Bold, 60px, #454545
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    break;

                case ControlStyles.PopupText:           // Segoe UI Regular, 86px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.NavBarActive:          // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    //element.Background = new SolidColorBrush(Colors.Black);
                    element.Foreground = new SolidColorBrush(Colors.White);
                    element.HorizontalAlignment = HorizontalAlignment.Left;
                    element.VerticalAlignment = VerticalAlignment.Center;
                    element.Margin = new Thickness(5d, 5d, 5d, 5d);
                    break;

                case ControlStyles.NavBarInactive:          // Segoe UI Semibold, 70px, White (active) / #707070 (inactive)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    //element.Background = new SolidColorBrush(Colors.Black);
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 112, 112, 112));
                    element.HorizontalAlignment = HorizontalAlignment.Left;
                    element.VerticalAlignment = VerticalAlignment.Center;
                    element.Margin = new Thickness(5d, 5d, 5d, 5d);
                    break;

                case ControlStyles.SwipeLeft:       // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.SwipeToContinue: // Segoe UI Semibold, 70px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.SemiBold;
                    element.FontSize = FONTSIZE_70px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.ButtonText:      // Segoe UI Bold, 60px, White  (Button.Height=204px, Color: Black on 20% opacity)
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    //element.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
                    element.Height = ELEMENT_HEIGHT_204PX;
                    break;

                case ControlStyles.SliderText:    // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.TouchHere:       // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.FastChargeHeader:// Segoe UI Regular, 100px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_100px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.FastChargeLede:  // Segoe UI Regular, 86px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_86px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.FastChargePercent: // Segoe UI Light, 300px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_300px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.FastChargeTimeline:// Segoe UI Bold, 60px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    element.FontSize = FONTSIZE_60px;
                    break;

                case ControlStyles.GaugePoint:      // Segoe UI Bold, 60px, White
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case ControlStyles.GaugePointBullet:      // Segoe UI Bold, 60px, Black, 30% Opacity
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush() { Color = Colors.Black, Opacity = 0.3 };
                    break;

                case ControlStyles.CompareHeader:       // Segoe UI Light, 200px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Light;
                    element.FontSize = FONTSIZE_200px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareTitle:        // Segoe UI Bold, 80px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_80px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareText:         // Segoe UI Regular, 80px, Black
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_80px;
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case ControlStyles.CompareLegal:        // Segoe UI Regular, 70px, #5C5C5C
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_70px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
                    break;

                case ControlStyles.Footnote:      // Segoe UI Regular, 60px, #5C5C5C
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Normal;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
                    break;

                case ControlStyles.LearnMore:      // Segoe UI Bold, 60px, #454545
                    element.FontFamily = new FontFamily(fontFamily);
                    element.FontStyle = FontStyle.Normal;
                    element.FontWeight = FontWeights.Bold;
                    element.FontSize = FONTSIZE_60px;
                    element.Foreground = new SolidColorBrush(Color.FromArgb(255, 69, 69, 69));
                    break;

                default:
                    break;
            }
        }
    }
}
