using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

using YogaC930AudioDemo.Helpers;


namespace YogaC930AudioDemo.Converters
{

    public class ScalingConverter : IValueConverter
    {
        private const double DEFAULT_RESOLUTION = 1920;

        private double _resolutionX;
        private double _scale;

        public ScalingConverter()
        {
            Size resolution = WindowHelper.GetScreenResolutionInfo();
            _resolutionX = resolution.Width;
            if (resolution.Height > _resolutionX) { _resolutionX = resolution.Height; }

            _scale = WindowHelper.GetRawPixelsPerViewPixel();
        }

        public object Convert(object value, Type targetType, object parameter, String language)
        {
            double result = 0;

            if ((null != value) && (value is double doubleValue))
            {
                result = doubleValue * (_resolutionX / (DEFAULT_RESOLUTION * _scale));
            }

            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }
    }
}
