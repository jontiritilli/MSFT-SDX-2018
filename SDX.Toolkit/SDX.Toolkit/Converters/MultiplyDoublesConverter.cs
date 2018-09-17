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
    public class MultiplyDoublesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            double operand1 = 0;
            double resultDouble = 0;

            if (null != value)
            {
                if (value is Double valueDouble)
                {
                    operand1 = valueDouble;
                }
            }
            if (null != parameter)
            {
                if (parameter is string paramString)
                {
                    double operand2;

                    if (Double.TryParse(paramString, out operand2))
                    {
                        resultDouble = operand1 * operand2;
                    }
                }
            }

            return resultDouble;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}

