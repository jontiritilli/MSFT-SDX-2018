using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Media.Core;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace SDX.Toolkit.Helpers
{
    public static class ColorHelper
    {
        // allowed formats in hex:  RRGGBB, #RRGGBB, AARRGGBB, #AARRGGBB
        public static Color ConvertHexToColor(string hexColor)
        {
            Color colorOut;

            byte alpha = 255;
            byte red = 0;
            byte green = 0;
            byte blue = 0;

            // remove # if present 
            if (-1 != hexColor.IndexOf('#'))
            {
                hexColor = hexColor.Replace("#", "");
            }

            if (8 == hexColor.Length)
            {
                alpha = ColorHelper.ConvertHexToByte(hexColor.Substring(0, 2), 255);

                hexColor = hexColor.Substring(2);
            }

            if (6 == hexColor.Length)
            {
                red = ColorHelper.ConvertHexToByte(hexColor.Substring(0, 2), 0);
                green = ColorHelper.ConvertHexToByte(hexColor.Substring(2, 2), 0);
                blue = ColorHelper.ConvertHexToByte(hexColor.Substring(4, 2), 0);
            }

            colorOut = Color.FromArgb(alpha, red, green, blue);
            
            return colorOut;
        }

        public static byte ConvertHexToByte(string hexValue, byte defaultValue)
        {
            byte result = 0;

            if (!byte.TryParse(hexValue, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
