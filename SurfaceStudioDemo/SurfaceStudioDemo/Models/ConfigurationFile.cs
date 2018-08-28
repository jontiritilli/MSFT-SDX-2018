using System;


namespace SurfaceStudioDemo.Models
{
    public enum TelemetryKeys
    {
        Test,
        Prod
    }

    public class ConfigurationFile
    {
        public bool IsAttractorLoopEnabled = true;
        public string AttractorLoopFileName = "ms-appx:///Assets/Attractor/attractor.mp4";
        public string Language = "en-US";
        public bool IsTelemetryEnabled = false;
        public TelemetryKeys TelemetryKey = TelemetryKeys.Test;
        public string TelemetryBaseUrl = String.Empty;
        public string TelemetryProdId = String.Empty;
        public string TelemetryTestId = String.Empty;


        public static ConfigurationFile CreateDefault()
        {
            ConfigurationFile config = new ConfigurationFile()
            {
                IsAttractorLoopEnabled = true,
                Language = String.Empty,
                IsTelemetryEnabled = false,
                TelemetryKey = TelemetryKeys.Test,
                TelemetryBaseUrl = String.Empty,
                TelemetryProdId = String.Empty,
                TelemetryTestId = String.Empty
            };

            return config;
        }
    }
}
