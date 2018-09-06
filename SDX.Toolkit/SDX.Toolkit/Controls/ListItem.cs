using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
// UNCOMMENT once telemtry is added
//using SDX.Telemtry.Services;

namespace SDX.Toolkit.Controls
{
    public enum ListItemIcon
    {
        // for interactive pen
        Jot,
        Write,
        Pressure,
        Palm,
        // for interactive touch
        Touch,
        Rotate,
        Drag,
        // for product highlight modals
        Design,
        Power,
        Laptop,
        Display,
        Sound,
        Mobility,
        Battery,
        Detail,
        Size,
        Connection,
        Pen,
        Custom,
        // for best of page
        Start,
        Sync,
        Hello,
        Office,
    }
    
    public class ListItem
    {
        #region Constants

        // pen icons
        private const string ICON_JOT_URI = @"ms-appx:///Assets/List/inkingDraw.svg";
        private const string ICON_WRITE_URI = @"ms-appx:///Assets/List/inkingWrite.svg";
        private const string ICON_PRESSURE_URI = @"ms-appx:///Assets/List/inkingPressure.svg";
        private const string ICON_PALM_URI = @"ms-appx:///Assets/List/inkingPalm.svg";
        // touch icons
        private const string ICON_TOUCH_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_ROTATE_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_DRAG_URI = @"ms-appx:///Assets/List/??.png";
        // product highlights modal icons
        private const string ICON_DESIGN_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_POWER_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_LAPTOP_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_DISPLAY_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_SOUND_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_MOBILITY_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_BATTERY_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_DETAIL_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_SIZE_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_CONNECTION_URI = @"ms-appx:///Assets/List/??.png";
        private const string ICON_PEN_URI = @"ms-appx:///Assets/List/??.png";
        // BOM icons
        private const string ICON_START_URI = @"ms-appx:///Assets/List/bomWindows.png";
        private const string ICON_HELLO_URI = @"ms-appx:///Assets/List/bomSmile.png";
        private const string ICON_SYNC_URI = @"ms-appx:///Assets/List/bomSync.png";
        private const string ICON_OFFICE_URI = @"ms-appx:///Assets/List/bomOffice.png";

        #endregion

        #region Private Members

        private int _order;
        private ListItemIcon _icon;
        private double _width = 60d;
        private string _headline;
        private string _lede;
        private string _iconPath;
        private string _ctaURI;
        private string _ctaText;
        private string _ctaTelemetryId;

        #endregion

        #region Construction

        #endregion

        #region Static Methods

        public static ListItem CreateListItem(int order, ListItemIcon icon, double width, string headline, string lede, string _ctaURI = "", string _ctaText = "", string _ctaTelemetryId = "", string _iconPath = "")
        {
            // create the item
            ListItem item = new ListItem();

            // set basic properties
            item.Order = order;
            item.Icon = icon;
            item.IconWidth = width;
            item.Headline = headline;
            item.Lede = lede;
            item.CTAUri = _ctaURI;
            item.CTAText = _ctaText;
            // set calculated properties
            item.IconPath = GetIconPath(icon, _iconPath);
            // commented out until Tel.Svc is added
            //item.TelemetryId = GetTelemetryId(icon, _ctaTelemetryId); 

            return item;
        }

        public static string GetIconPath(ListItemIcon icon, string iconPath)
        {
            if (String.IsNullOrWhiteSpace(iconPath))
            {
                switch (icon)
                {
                    case ListItemIcon.Jot:
                        iconPath = ICON_JOT_URI;
                        break;

                    case ListItemIcon.Write:
                        iconPath = ICON_WRITE_URI;
                        break;

                    case ListItemIcon.Pressure:
                        iconPath = ICON_PRESSURE_URI;
                        break;

                    case ListItemIcon.Palm:
                        iconPath = ICON_PALM_URI;
                        break;

                    case ListItemIcon.Touch:
                        iconPath = ICON_TOUCH_URI;
                        break;

                    case ListItemIcon.Rotate:
                        iconPath = ICON_ROTATE_URI;
                        break;

                    case ListItemIcon.Drag:
                        iconPath = ICON_DRAG_URI;
                        break;

                    case ListItemIcon.Design:
                        iconPath = ICON_DESIGN_URI;
                        break;

                    case ListItemIcon.Power:
                        iconPath = ICON_POWER_URI;
                        break;

                    case ListItemIcon.Laptop:
                        iconPath = ICON_LAPTOP_URI;
                        break;

                    case ListItemIcon.Display:
                        iconPath = ICON_DISPLAY_URI;
                        break;

                    case ListItemIcon.Sound:
                        iconPath = ICON_SOUND_URI;
                        break;

                    case ListItemIcon.Mobility:
                        iconPath = ICON_MOBILITY_URI;
                        break;

                    case ListItemIcon.Battery:
                        iconPath = ICON_BATTERY_URI;
                        break;

                    case ListItemIcon.Detail:
                        iconPath = ICON_DETAIL_URI;
                        break;

                    case ListItemIcon.Size:
                        iconPath = ICON_SIZE_URI;
                        break;

                    case ListItemIcon.Connection:
                        iconPath = ICON_CONNECTION_URI;
                        break;

                    case ListItemIcon.Pen:
                        iconPath = ICON_DRAG_URI;
                        break;

                    case ListItemIcon.Start:
                        iconPath = ICON_START_URI;
                        break;

                    case ListItemIcon.Sync:
                        iconPath = ICON_SYNC_URI;
                        break;

                    case ListItemIcon.Hello:
                        iconPath = ICON_HELLO_URI;
                        break;

                    case ListItemIcon.Office:
                        iconPath = ICON_OFFICE_URI;
                        break;

                    default:
                        break;
                }
            }

            return iconPath;
        }

        // UNCOMMENT once telemetry is added
        //public static string GetTelemetryId(ListItemIcon icon, string telemetryId)
        //{
        //    if (String.IsNullOrWhiteSpace(telemetryId))
        //    {
        //        switch (icon)
        //        {
        //            case ListItemIcon.Hello:
        //                telemetryId = TelemetryService.TELEMETRY_BESTOFCTAHELLO;
        //                break;

        //            case ListItemIcon.Start:
        //                telemetryId = TelemetryService.TELEMETRY_BESTOFCTAWINDOWS;
        //                break;

        //            case ListItemIcon.Sync:
        //                break;

        //            case ListItemIcon.Office:
        //                telemetryId = TelemetryService.TELEMETRY_BESTOFCTAOFFICE;
        //                break;

        //            case ListItemIcon.Best5:
        //                telemetryId = TelemetryService.TELEMETRY_BESTOFCTABULLET5;
        //                break;

        //            case ListItemIcon.Jot:
        //            case ListItemIcon.Write:
        //            case ListItemIcon.Pressure:
        //            case ListItemIcon.Palm:
        //            default:
        //                break;
        //        }
        //    }
        //    return telemetryId;
        //}

        #endregion
            
        #region Public Properties

        public int Order
        {
            get => _order;
            set => _order = value;
        }

        public ListItemIcon Icon
        {
            get => _icon;
            set => _icon = value;
        }

        public double IconWidth
        {
            get => _width;
            set => _width = value;
        }

        public string Headline
        {
            get => _headline;
            set => _headline = value;
        }

        public string Lede
        {
            get => _lede;
            set => _lede = value;
        }

        public string IconPath
        {
            get => _iconPath;
            set => _iconPath = value;
        }
        public string CTAUri
        { 
            get => _ctaURI;
            set => _ctaURI = value;
        }

        public string CTAText
        {
            get => _ctaText;
            set => _ctaText = value;
        }

        public string TelemtryID
        {
            get => _ctaTelemetryId;
            set => _ctaTelemetryId = value;
        }

        #endregion
    }
}
