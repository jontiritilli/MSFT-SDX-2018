using System;
using Windows.UI;

namespace SDX.Toolkit.Helpers
{
    public static class Utilities
    {
        public static double ClampAngle(double currentAngle)
        {
            if (currentAngle > 360)
            {
                return (currentAngle % 360);
            }
            else if (currentAngle < 0)
            {
                return 360 - (Math.Abs(currentAngle) % 360);
            }
            else
            {
                return currentAngle;
            }

        }

        public static Color ConvertHSV2RGB(float sliceNum)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            int slice = (int)Math.Ceiling(sliceNum / 72);
            switch (slice)
            {
                case 1:
                    r = 12;
                    g = 98;
                    b = 145;
                    break;
                case 2:
                    r = 152;
                    g = 216;
                    b = 216;
                    break;
                case 3:
                    r = 255;
                    g = 179;
                    b = 103;
                    break;
                case 4:
                    r = 150;
                    g = 101;
                    b = 245;
                    break;
                default:
                    r = 230;
                    g = 48;
                    b = 97;
                    break;
            }
            return Color.FromArgb(255, r, g, b);
        }
    }
}
