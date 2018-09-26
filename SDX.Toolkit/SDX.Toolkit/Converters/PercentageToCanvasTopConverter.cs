using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

using SDX.Toolkit.Helpers;


namespace SDX.Toolkit.Converters
{
    public class PercentageToCanvasTopConverter : IValueConverter
    {
        private static double CanvasHeight = WindowHelper.CANVAS_Y;

        public object Convert(object value, Type targetType, object parameter, String language)
        {
            double canvasTop = 0;

            if (null != parameter)
            {
                if (parameter is string paramString)
                {
                    double paramDouble;

                    if (Double.TryParse(paramString, out paramDouble))
                    {
                        canvasTop = (double)paramDouble * PercentageToCanvasTopConverter.CanvasHeight;
                    }
                }

            }

            return canvasTop;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}

