using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace SDX.Toolkit.Converters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            GridLength gridLength;

            if (null != value)
            {
                gridLength = new GridLength((double)value);
            }

            return gridLength;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}

