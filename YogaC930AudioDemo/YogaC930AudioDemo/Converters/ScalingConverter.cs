using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

using YogaC930AudioDemo.Helpers;


namespace YogaC930AudioDemo.Converters
{
    public class ScalingConverter : IValueConverter
    {
        private const double DEFAULT_RESOLUTION = 1920;
        private const double DEFAULT_SCALE = 1.0;

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

            if (null != value)
            {
                if (targetType.Equals(typeof(double)))
                {
                    if (value is double valueDouble)
                    {
                        return ConvertDouble(valueDouble);
                    }
                    else if (value is int valueDoubleInt)
                    {
                        return ConvertDouble(valueDoubleInt);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (targetType.Equals(typeof(int)))
                {
                    if (value is double valueDouble2)
                    {
                        return (int)ConvertDouble(valueDouble2);
                    }
                    else if (value is int valueInt32)
                    {
                        return (int)ConvertDouble(valueInt32);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (targetType.Equals(typeof(GridLength)))
                {
                    if (value is double valueGridLength)
                    {
                        return new GridLength(ConvertDouble(valueGridLength));
                    }
                    else if (value is int valueGridLengthInt)
                    {
                        return new GridLength(ConvertDouble(valueGridLengthInt));
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (targetType.Equals(typeof(Thickness)))
                {
                    return ParseThickness(value);
                }
                else if (targetType.Equals(typeof(CornerRadius)))
                {
                    return ParseCornerRadius(value);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, String language)
        {
            throw new NotSupportedException("Only one way bindings are supported with this converter");
        }

        private double ConvertDouble(double doubleValue)
        {
            // w2 = w1 * (S2 / S1) * (R1 / R2)
            return doubleValue * (_scale / 1.0) * (DEFAULT_RESOLUTION / _resolutionX);
        }

        private Thickness ParseThickness(object value)
        {
            Thickness thickness = new Thickness();

            if (value is string stringValue)
            {
                List<double> thicknesses = ParseParameters(stringValue);

                if (thicknesses.Count == 1)
                {
                    thickness = new Thickness(ConvertDouble(thicknesses[0]));
                }
                else if ((thicknesses.Count == 2) || (thicknesses.Count == 3))
                {
                    thickness = new Thickness(ConvertDouble(thicknesses[0]), ConvertDouble(thicknesses[1]),
                        ConvertDouble(thicknesses[0]), ConvertDouble(thicknesses[1]));
                }
                else if (thicknesses.Count == 4)
                {
                    thickness = new Thickness(ConvertDouble(thicknesses[0]), ConvertDouble(thicknesses[1]),
                        ConvertDouble(thicknesses[2]), ConvertDouble(thicknesses[3]));
                }
            }

            return thickness;
        }

        private CornerRadius ParseCornerRadius(object value)
        {
            CornerRadius cornerRadius = new CornerRadius();

            if (value is string stringValue)
            {
                List<double> radii = ParseParameters(stringValue);

                if (radii.Count == 1)
                {
                    cornerRadius = new CornerRadius(ConvertDouble(radii[0]));
                }
                else if ((radii.Count == 2) || (radii.Count == 3))
                {
                    cornerRadius = new CornerRadius(ConvertDouble(radii[0]), ConvertDouble(radii[1]),
                        ConvertDouble(radii[0]), ConvertDouble(radii[1]));
                }
                else if (radii.Count == 4)
                {
                    cornerRadius = new CornerRadius(ConvertDouble(radii[0]), ConvertDouble(radii[1]),
                        ConvertDouble(radii[2]), ConvertDouble(radii[3]));
                }
            }

            return cornerRadius;
        }

        private List<double> ParseParameters(string value)
        {
            List<string> parameters = value.Split(',').ToList<string>();
            List<double> values = new List<double>();

            foreach (string parameter in parameters)
            {
                double result;

                if (Double.TryParse(parameter, out result))
                {
                    values.Add(result);
                }
            }

            return values;
        }
    }
}
