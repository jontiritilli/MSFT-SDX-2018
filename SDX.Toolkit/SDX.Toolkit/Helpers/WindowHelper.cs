using System;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;


namespace SDX.Toolkit.Helpers
{
    public enum DeviceType
    {
        Unknown,
        Go,
        Laptop,
        Pro,
        Studio,
        Book13,
        Book15
    }

    public static class WindowHelper
    {
        #region Public Static Constants

        public static readonly Rect WINDOW_BOUNDS = ApplicationView.GetForCurrentView().VisibleBounds;

        public static readonly double CANVAS_X = WINDOW_BOUNDS.Width;
        public static readonly double CANVAS_Y = WINDOW_BOUNDS.Height - 80d;   // navbar takes up 80 effective pixels height

        public static readonly double LEFT_MARGIN = 80d;
        public static readonly double TOP_MARGIN = 80d;
        public static readonly double TOP_MARGIN_NARROW = 40d;
        public static readonly double RIGHT_MARGIN = 80d;
        public static readonly double BOTTOM_MARGIN = 50d;
        public static readonly double LEFT_MARGIN_CANVAS = 20d;
        public static readonly double TOP_MARGIN_CANVAS = 20d;

        public static readonly double SPACE_BETWEEN_ELEMENTS = 30d;

        public static readonly double WIDTH_HEADER = 400d;
        public static readonly double WIDTH_TEXT_POPUP = 360d;

        public static readonly int Z_ORDER_SHADOW = 5;
        public static readonly int Z_ORDER_IMAGE = 90;
        public static readonly int Z_ORDER_CONTROLS = 100;
        public static readonly int Z_ORDER_OVERLAY = 1000;

        #endregion


        #region Public Static Methods

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

        public static DeviceType GetDeviceTypeFromResolution()
        {
            DeviceType deviceType = DeviceType.Unknown;

            // get the physical resolution
            Size size = WindowHelper.GetScreenResolutionInfo();

            switch(size.Width)
            {
                case 4500:
                    deviceType = DeviceType.Studio;
                    break;

                case 3240:
                    deviceType = DeviceType.Book15;
                    break;

                case 3000:
                    deviceType = DeviceType.Book13;
                    break;

                case 2736:
                    deviceType = DeviceType.Pro;
                    break;

                case 2256:
                    deviceType = DeviceType.Laptop;
                    break;

                case 1800:
                    deviceType = DeviceType.Go;
                    break;
            }

            return deviceType;
        }

        #endregion
    }
}