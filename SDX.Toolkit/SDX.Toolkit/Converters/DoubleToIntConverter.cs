using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SDX.Toolkit.Converters
{
    public class DoubleToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            int iValue = -1;

            if (null != value)
            {
                iValue = System.Convert.ToInt32(Math.Round((double)value, 0));
            }

            return iValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
