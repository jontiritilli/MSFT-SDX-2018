using System;

using SDX.Telemetry.Services;


namespace SurfaceLaptopDemo.Models
{
    public class ConfigurationFile
    {
        public bool IsAttractorLoopEnabled = true;
        public string AttractorLoopFileName = "ms-appx:///Assets/Attractor/attractor.mp4";
        public string Language = "en-US";
        public bool IsTelemetryEnabled = false;
        public bool IsBlackSchemeEnabled = false;
        public bool IsCoralSchemeEnabled = false;
        public bool IsStudioCompareLegalEnabled = false;
        public TelemetryKeys TelemetryKey = TelemetryKeys.Test;


        public static ConfigurationFile CreateDefault()
        {
            ConfigurationFile config = new ConfigurationFile()
            {
                IsAttractorLoopEnabled = true,
                Language = String.Empty,
                IsTelemetryEnabled = false,
                TelemetryKey = TelemetryKeys.Test,
                IsBlackSchemeEnabled = false,
                IsCoralSchemeEnabled = false,
                IsStudioCompareLegalEnabled = false
            };

            return config;
        }
    }
}
