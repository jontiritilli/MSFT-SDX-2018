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
    public class TextBlockHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            if(null != value)
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}

