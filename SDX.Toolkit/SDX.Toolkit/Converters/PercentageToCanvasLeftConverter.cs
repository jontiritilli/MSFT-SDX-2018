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
    public class PercentageToCanvasLeftConverter : IValueConverter
    {
        private static double CanvasWidth = WindowHelper.CANVAS_X;

        public object Convert(object value, Type targetType, object parameter, String language)
        {
            double canvasLeft = 0;

            if (null != parameter)
            {
                if (parameter is string paramString)
                {
                    double paramDouble;

                    if (Double.TryParse(paramString, out paramDouble))
                    {
                        canvasLeft = (double)paramDouble * PercentageToCanvasLeftConverter.CanvasWidth;
                    }
                }
            }

            return canvasLeft;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}

