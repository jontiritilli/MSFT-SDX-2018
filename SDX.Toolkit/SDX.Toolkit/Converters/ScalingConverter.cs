using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

using SDX.Toolkit.Helpers;


namespace SDX.Toolkit.Converters
{
    public class ScalingConverter : IValueConverter
    {
        private const double DEFAULT_RESOLUTION = 2736; // surface pro
        private const double DEFAULT_SCALE = 2.0;       // surface pro

        private double _resolutionX;
        private double _scale;

        public static double DefaultResolution = DEFAULT_RESOLUTION;
        public static double DefaultScale = DEFAULT_SCALE;

        public ScalingConverter()
        {
            // get pixel width of physical screen
            Size resolution = WindowHelper.GetScreenResolutionInfo();
            _resolutionX = resolution.Width;
            if (resolution.Height > _resolutionX) { _resolutionX = resolution.Height; }

            // get scaling factor
            _scale = WindowHelper.GetRawPixelsPerViewPixel();
        }

        public object Convert(object value, Type targetType, object parameter, String language)
        {
            double result = 0;

            if (null != value)
            {
                if (targetType.Equals(typeof(double)))
                {
                    return ConvertDouble(ParseDouble(value));
                }
                else if (targetType.Equals(typeof(int)))
                {
                    return (int)ConvertDouble(ParseDouble(value));
                }
                else if (targetType.Equals(typeof(GridLength)))
                {
                    return new GridLength(ConvertDouble(ParseDouble(value)));
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
            // w2 = w1 * (R1 / S1) * (S2 / R2)
            //double result = doubleValue * (DEFAULT_SCALE / DEFAULT_RESOLUTION) * (_resolutionX / _scale);
            double result = doubleValue * (ScalingConverter.DefaultScale / ScalingConverter.DefaultResolution) * (_resolutionX / _scale);

            //Debug.WriteLine($"Input {doubleValue} resulted in Output {result}.");

            return result;
        }

        private double ParseDouble(object value)
        {
            double result = 0;

            if (value is double valueDouble)
            {
                result = valueDouble;
            }
            else if (value is float valueFloat)
            {
                result = (double)valueFloat;
            }
            else if (value is decimal valueDecimal)
            {
                result = System.Convert.ToDouble(valueDecimal);
            }
            else if (value is int valueInt)
            {
                result = (double)valueInt;
            }
            else if (value is string valueString)
            {
                if (!Double.TryParse(valueString, out result))
                {
                    result = 0;
                }
            }
            else
            {
                // none of our types
            }

            return result;
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
                else if (thicknesses.Count >= 4)
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
                else if (radii.Count >= 4)
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
