using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Services.Store.Engagement;

using MetroLog;


namespace SDX.Telemetry.Services
{
    public enum TelemetryEvents
    {
        StartApplication,
        EndApplication,
        NavExperience,
        NavAccessories,
        NavBestOf,
        NavComparison,
        ViewExperience,
        ViewAccessories,
        ViewBestOf,
        ViewComparison,
        StartPen,
        EndPen,
        StartPinch,
        EndPinch,
        ComparisonHot,
    }

    public class TelemetryService
    {
        #region Public Static Properties

        public static TelemetryService Current { get; private set; }

        #endregion


        #region Private Members

        private ILogger TestLog = null;
        private StoreServicesCustomEventLogger ProdLog = null;

        #endregion


        #region Public Properties

        public TelemetryKeys ActiveKey { get; private set; }

        #endregion


        #region Construction

        public TelemetryService()
        {
            // save a reference to ourself
            TelemetryService.Current = this;

        }

        public async Task Initialize(TelemetryKeys telemetryKey)
        {
            // save the telemetry key
            this.ActiveKey = telemetryKey;

            // configure for logging/telemetry
            switch (telemetryKey)
            {
                // for Test, we'll just log to a file
                case TelemetryKeys.Test:
                default:
                    TestLog = LogManagerFactory.DefaultLogManager.GetLogger<TelemetryService>();
                    break;

                // for Prod, we'll log to the Store
                case TelemetryKeys.Prod:
                    ProdLog = StoreServicesCustomEventLogger.GetDefault();
                    break;
            }
        }

        #endregion


        #region Public Methods

        public void LogTelemetryEvent(TelemetryEvents telemetryEvent)
        {
            // convert the event to a string
            string eventName = telemetryEvent.ToString();

            // log it
            switch (this.ActiveKey)
            {
                case TelemetryKeys.Test:
                default:
                    if (null != TestLog)
                    {
                        TestLog.Log(LogLevel.Info, eventName);
                    }
                    break;

                case TelemetryKeys.Prod:
                    if (null != ProdLog)
                    {
                        ProdLog.Log(eventName);
                    }
                    break;
            }
        }

        #endregion


    }
}
