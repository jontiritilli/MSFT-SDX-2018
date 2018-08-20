using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDX.Toolkit.Services;
using Windows.Storage;

namespace SDX.Toolkit.Controls
{
    public enum ListItemIcon
    {
        Start,
        Sync,
        Hello,
        Office,
        Jot,
        Write,
        Pressure,
        Palm,
        Best5,
        Custom
    }

    public class FadeInListListItem
    {
        #region Constants

        private const string ICON_START_URI = @"ms-appx:///Assets/FadeInList/windows-icon.png";
        private const string ICON_SYNC_URI = @"ms-appx:///Assets/FadeInList/sync-icon.png";
        private const string ICON_HELLO_URI = @"ms-appx:///Assets/FadeInList/hello-icon.png";
        private const string ICON_OFFICE_URI = @"ms-appx:///Assets/FadeInList/office-icon.png";

        private const string ICON_JOT_URI = @"ms-appx:///Assets/FadeInList/icon-1.png";
        private const string ICON_WRITE_URI = @"ms-appx:///Assets/FadeInList/icon-2.png";
        private const string ICON_PRESSURE_URI = @"ms-appx:///Assets/FadeInList/icon-3.png";
        private const string ICON_PALM_URI = @"ms-appx:///Assets/FadeInList/icon-4.png";

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
        private string _ctaText;
        private string _ctaUri;
        private string _ctaTelemetryId;
        private string _iconPath;

        #endregion

        #region Construction

        public FadeInListListItem()
        {
        }

        #endregion


        #region Static Methods

        public static FadeInListListItem CreateListItem(int order, ListItemIcon icon, double width, string headline, string lede, string ctaText, string ctaUri, string ctaTelemetryId, string iconPath)
        {
            // create the item
            FadeInListListItem item = new FadeInListListItem();

            // set basic properties
            item.Order = order;
            item.Icon = icon;
            item.IconWidth = width;
            item.Headline = headline;
            item.Lede = lede;
            item.CTAText = ctaText;

            // set calculated properties
            item.TelemetryId = GetTelemetryId(icon, ctaTelemetryId);
            item.IconPath = GetIconPath(icon, iconPath);
            item.CTAUri = GetCTAUri(icon, ctaUri);

            return item;
        }

        public static string GetCTAUri(ListItemIcon icon, string ctaUri)
        {
            if (String.IsNullOrWhiteSpace(ctaUri))
            {
                switch (icon)
                {
                    case ListItemIcon.Start:
                        ctaUri = URI_CTA_START;
                        break;

                    case ListItemIcon.Sync:
                        ctaUri = URI_CTA_SYNC;
                        break;

                    case ListItemIcon.Hello:
                        ctaUri = URI_CTA_HELLO;
                        break;

                    case ListItemIcon.Office:
                        ctaUri = URI_CTA_OFFICE;
                        break;
                }
            }

            return ctaUri;
        }

        public static string GetIconPath(ListItemIcon icon, string iconPath)
        {
            if (String.IsNullOrWhiteSpace(iconPath))
            {
                switch (icon)
                {
                    case ListItemIcon.Hello:
                        iconPath = ICON_HELLO_URI;
                        break;

                    case ListItemIcon.Start:
                        iconPath = ICON_START_URI;
                        break;

                    case ListItemIcon.Sync:
                        iconPath = ICON_SYNC_URI;
                        break;

                    case ListItemIcon.Office:
                        iconPath = ICON_OFFICE_URI;
                        break;

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

                    default:
                        break;
                }
            }

            return iconPath;
        }

        public static string GetTelemetryId(ListItemIcon icon, string telemetryId)
        {
            if (String.IsNullOrWhiteSpace(telemetryId))
            {
                switch (icon)
                {
                    case ListItemIcon.Hello:
                        telemetryId = TelemetryService.TELEMETRY_BESTOFCTAHELLO;
                        break;

                    case ListItemIcon.Start:
                        telemetryId = TelemetryService.TELEMETRY_BESTOFCTAWINDOWS;
                        break;

                    case ListItemIcon.Sync:
                        break;

                    case ListItemIcon.Office:
                        telemetryId = TelemetryService.TELEMETRY_BESTOFCTAOFFICE;
                        break;

                    case ListItemIcon.Best5:
                        telemetryId = TelemetryService.TELEMETRY_BESTOFCTABULLET5;
                        break;

                    case ListItemIcon.Jot:
                    case ListItemIcon.Write:
                    case ListItemIcon.Pressure:
                    case ListItemIcon.Palm:
                    default:
                        break;
                }
            }

            return telemetryId;
        }

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

        public string CTAText
        {
            get => _ctaText;
            set => _ctaText = value;
        }

        public string CTAUri
        {
            get => _ctaUri;
            set => _ctaUri = value;
        }

        public string TelemetryId
        {
            get => _ctaTelemetryId;
            set => _ctaTelemetryId = value;
        }

        public string IconPath
        {
            get => _iconPath;
            set => _iconPath = value;
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
