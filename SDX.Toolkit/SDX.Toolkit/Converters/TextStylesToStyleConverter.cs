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
    public class TextStylesToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String language)
        {
            Style style = null;

            if (null != value)
            {
                style = StyleHelper.GetApplicationStyle((TextStyles)value);
            }

            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}

