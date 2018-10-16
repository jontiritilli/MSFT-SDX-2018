using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YogaC930AudioDemo.Models
{
    public class ConfigurationFile
    {

        public bool IsAttractorLoopEnabled = true;
        public string AttractorLoopFileName = "ms-appx:///Assets/Attractor/Lenovo_Yoga_C930_Attract_Loop_240718.mp4";
        public string Language = "en-US";

        public static ConfigurationFile CreateDefault()
        {
            ConfigurationFile config = new ConfigurationFile()
            {
                IsAttractorLoopEnabled = true,
                Language = String.Empty,
            };

            return config;
        }
    }
}
