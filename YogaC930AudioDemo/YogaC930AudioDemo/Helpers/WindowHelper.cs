using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace YogaC930AudioDemo.Helpers
{

    public static class WindowHelper
    {
        #region Public Static Methods

        public static Size GetScreenResolutionInfo()
        {
            // these calculations are all in PHSYICAL pixels

            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
            Rect bounds = applicationView.VisibleBounds;
            double scale = displayInformation.RawPixelsPerViewPixel;
            Size size = new Size(bounds.Width * scale, bounds.Height * scale);

            return size;
        }

        public static Size GetViewSizeInfo()
        {
            // these calculations are all in EFFECTIVE pixels

            // call api to get size
            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            Rect bounds = applicationView.VisibleBounds;
            Size size = new Size(bounds.Width, bounds.Height);

            return size;
        }

        public static double GetRawPixelsPerViewPixel()
        {
            double scale = 0;

            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
            scale = displayInformation.RawPixelsPerViewPixel;

            return scale;
        }

        #endregion
    }
}
