using System;

using SDX.Telemetry.Services;


namespace SurfaceJackDemo.Models
{
    public class ConfigurationFile
    {
        public bool IsAttractorLoopEnabled = true;
        public string AttractorLoopFileName = "ms-appx:///Assets/Attractor/attractor.mp4";
        public bool IsChoosePathPageEnabled = true;
        public string Language = "en-US";
        public bool IsTelemetryEnabled = false;
        public double Volume = 50;
        public TelemetryKeys TelemetryKey = TelemetryKeys.Test;

        public static ConfigurationFile CreateDefault()
        {
            ConfigurationFile config = new ConfigurationFile()
            {
                IsAttractorLoopEnabled = true,
                IsChoosePathPageEnabled = true,
                Language = String.Empty,
                IsTelemetryEnabled = false,
                TelemetryKey = TelemetryKeys.Test,
            };

            return config;
        }
    }
}
