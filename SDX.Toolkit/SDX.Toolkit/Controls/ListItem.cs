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
        // for product highlight modals
        Design,
        Performance,
        Laptop,
        ScreenSize,
        Display,
        Sound,
        Lightweight,
        Battery,
        Detail,
        Creative,
        Connection,
        Wifi,
        Versatile,
        Pen,
        // for best of page
        Start,
        Hello,
        Sync,
        Office,
        Custom,
        //Specs
        Dimensions, 
        Weight,
        Speaker,
        Frequency,
        TouchControls,
        Compatibility,
        BatteryLife,
        NoiseCancellation,
        Inputs,
        HeadPhones,
        USB,
        Stereo,
        Case,
        Docs

    }
    
    public class ListItem
    {
        #region Constants

        // pen icons
        private const string ICON_JOT_URI = @"ms-appx:///Assets/List/inkingDraw.png";
        private const string ICON_WRITE_URI = @"ms-appx:///Assets/List/inkingWrite.png";
        private const string ICON_PRESSURE_URI = @"ms-appx:///Assets/List/inkingPressure.png";
        private const string ICON_PALM_URI = @"ms-appx:///Assets/List/inkingPalm.png";
        // product highlights modal icons
        private const string ICON_DESIGN_URI = @"ms-appx:///Assets/List/specs_design.png";
        private const string ICON_PERFORMANCE_URI = @"ms-appx:///Assets/List/specs_performance.png";
        private const string ICON_LAPTOP_URI = @"ms-appx:///Assets/List/specs_laptop.png";
        private const string ICON_SCREENSIZE_URI = @"ms-appx:///Assets/List/specs_screenSize.png";
        private const string ICON_DISPLAY_URI = @"ms-appx:///Assets/List/specs_eye.png";
        private const string ICON_AUDIO_URI = @"ms-appx:///Assets/List/specs_audio.png";
        private const string ICON_LIGHTWEIGHT_URI = @"ms-appx:///Assets/List/specs_feather.png";
        private const string ICON_BATTERY_URI = @"ms-appx:///Assets/List/specs_battery.png";
        private const string ICON_CREATIVE_URI = @"ms-appx:///Assets/List/specs_creative.png";
        private const string ICON_SIZE_URI = @"ms-appx:///Assets/List/specs_size.png";
        private const string ICON_VERSATILE_URI = @"ms-appx:///Assets/List/specs_versatile.png";
        private const string ICON_CONNECTION_URI = @"ms-appx:///Assets/List/specs_connections.png";
        private const string ICON_WIFI_URI = @"ms-appx:///Assets/List/specs_wifi.png";
        private const string ICON_PEN_URI = @"ms-appx:///Assets/List/specs_pen.png";
        // BOM icons
        private const string ICON_START_URI = @"ms-appx:///Assets/List/bom_windows.png";
        private const string ICON_HELLO_URI = @"ms-appx:///Assets/List/bom_smile.png";
        private const string ICON_SYNC_URI = @"ms-appx:///Assets/List/bom_sync.png";
        private const string ICON_OFFICE_URI = @"ms-appx:///Assets/List/bom_office.png";
        //Specs Links
        private const string ICON_DIMENSIONS_URI = @"ms-appx:///Assets/List/specs_dimensions.png";
        private const string ICON_WEIGHT_URI = @"ms-appx:///Assets/List/specs_weight.png";
        private const string ICON_SPEAKER_URI = @"ms-appx:///Assets/List/specs_speaker.png";
        private const string ICON_FREQUENCY_URI = @"ms-appx:///Assets/List/specs_frequency.png";
        private const string ICON_BATTERYLIFE_URI = @"ms-appx:///Assets/List/specs_batteryLife.png";
        private const string ICON_COMPATIBILITY_URI = @"ms-appx:///Assets/List/specs_compatability.png";
        private const string ICON_TOUCHCONTROLS_URI = @"ms-appx:///Assets/List/specs_touchControls.png";
        private const string ICON_NOISECANCELLATION_URI = @"ms-appx:///Assets/List/specs_noiseCancellation.png";
        private const string ICON_INPUTS_URI = @"ms-appx:///Assets/List/specs_inputs.png";
        //WITB Links
        private const string ICON_HEADPHONES_URI = @"ms-appx:///Assets/InTheBox/headphones.png";
        private const string ICON_USB_URI = @"ms-appx:///Assets/InTheBox/usb.png";
        private const string ICON_STEREO_URI = @"ms-appx:///Assets/InTheBox/stereo-cord.png";
        private const string ICON_CASE_URI = @"ms-appx:///Assets/InTheBox/case.png";
        private const string ICON_DOCS_URI = @"ms-appx:///Assets/InTheBox/instructions.png";
        // URI links
        private const string URI_CTA_START = @"rdx-hub:hub\windows"; //"rdx-hub:";
        private const string URI_CTA_SYNC = "";
        private const string URI_CTA_HELLO = @"rdx-hub:hub\windows#Windows_Hello"; //"ms-retaildemo-launchbioenrollment:about";
        private const string URI_CTA_OFFICE = @"rdx-hub:hub\office"; //"rdx-hub:";

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
            item.CTAUri = GetCTA_URI(icon, _ctaURI);
            item.CTAText = _ctaText;
            // set calculated properties
            item.IconPath = GetIconPath(icon, _iconPath);
            // commented out until Tel.Svc is added
            //item.TelemetryId = GetTelemetryId(icon, _ctaTelemetryId); 

            return item;
        }

        private static string GetCTA_URI(ListItemIcon icon, string ctaURI)
        {
            if (String.IsNullOrWhiteSpace(ctaURI))
            {
                switch (icon)
                {
                    case ListItemIcon.Start:
                        ctaURI = URI_CTA_START;
                        break;

                    case ListItemIcon.Sync:
                        ctaURI = URI_CTA_SYNC;
                        break;

                    case ListItemIcon.Hello:
                        ctaURI = URI_CTA_HELLO;
                        break;

                    case ListItemIcon.Office:
                        ctaURI = URI_CTA_OFFICE;
                        break;

                    default:
                        break;
                }
            }

            return ctaURI;
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

                    case ListItemIcon.Design:
                        iconPath = ICON_DESIGN_URI;
                        break;

                    case ListItemIcon.Performance:
                        iconPath = ICON_PERFORMANCE_URI;
                        break;

                    case ListItemIcon.Laptop:
                        iconPath = ICON_LAPTOP_URI;
                        break;
                    case ListItemIcon.ScreenSize:
                        iconPath = ICON_SCREENSIZE_URI;
                        break;
                        
                    case ListItemIcon.Display:
                        iconPath = ICON_DISPLAY_URI;
                        break;

                    case ListItemIcon.Sound:
                        iconPath = ICON_AUDIO_URI;
                        break;

                    case ListItemIcon.Lightweight:
                        iconPath = ICON_LIGHTWEIGHT_URI;
                        break;

                    case ListItemIcon.Battery:
                        iconPath = ICON_BATTERY_URI;
                        break;

                    case ListItemIcon.Creative:
                        iconPath = ICON_CREATIVE_URI;
                        break;

                    case ListItemIcon.Connection:
                        iconPath = ICON_CONNECTION_URI;
                        break;

                    case ListItemIcon.Versatile:
                        iconPath = ICON_VERSATILE_URI;
                        break;

                    case ListItemIcon.Wifi:
                        iconPath = ICON_WIFI_URI
;
                        break;
                    case ListItemIcon.Pen:
                        iconPath = ICON_PEN_URI;
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

                    case ListItemIcon.Dimensions:
                        iconPath = ICON_DIMENSIONS_URI;
                        break;

                    case ListItemIcon.Weight:
                        iconPath = ICON_WEIGHT_URI;
                        break;

                    case ListItemIcon.Speaker:
                        iconPath = ICON_SPEAKER_URI;
                        break;

                    case ListItemIcon.Frequency:
                        iconPath = ICON_FREQUENCY_URI;
                        break;

                    case ListItemIcon.Compatibility:
                        iconPath = ICON_COMPATIBILITY_URI;
                        break;

                    case ListItemIcon.TouchControls:
                        iconPath = ICON_TOUCHCONTROLS_URI;
                        break;

                    case ListItemIcon.BatteryLife:
                        iconPath = ICON_BATTERYLIFE_URI;
                        break;

                    case ListItemIcon.NoiseCancellation:
                        iconPath = ICON_NOISECANCELLATION_URI;
                        break;

                    case ListItemIcon.Inputs:
                        iconPath = ICON_INPUTS_URI;
                        break;

                    case ListItemIcon.HeadPhones:
                        iconPath = ICON_HEADPHONES_URI;
                        break;

                    case ListItemIcon.USB:
                        iconPath = ICON_USB_URI;
                        break;
                    case ListItemIcon.Stereo:
                        iconPath = ICON_STEREO_URI;
                        break;
                    case ListItemIcon.Case:
                        iconPath = ICON_CASE_URI;
                        break;
                    case ListItemIcon.Docs:
                        iconPath = ICON_DOCS_URI;
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
