using System;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;


namespace SDX.Toolkit.Helpers
{
    public static class WindowHelper
    {
        public static Size GetScreenResolutionInfo()
        {
            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
            Rect bounds = applicationView.VisibleBounds;
            double scale = displayInformation.RawPixelsPerViewPixel;
            Size size = new Size(bounds.Width * scale, bounds.Height * scale);

            return size;
        }

        public static Size GetViewSizeInfo()
        {
            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            Rect bounds = applicationView.VisibleBounds;
            Size size = new Size(bounds.Width, bounds.Height);

            return size;
        }
    }
}