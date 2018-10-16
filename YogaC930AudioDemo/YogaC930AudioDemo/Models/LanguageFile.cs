using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaC930AudioDemo.Models
{
    class LanguageFile
    {
        #region Public Properties

        public string ATTRACTOR_CTA = "Touch Here for Sound Demo";
        public string ATTRACTOR_TITLE = "YOGA C930 Innovation meets design";
        public string NAV_EXPLORE = "Explore Windows";
        public string NAV_GOTODESKTOP = "Go To Desktop";
        public string AUDIO_PLAY_CTA = "PLAY AUDIO DEMO";
        public string AUDIO_HEADLINE = "Surround sound that really surrounds";
        public string AUDIO_BODY_FIRST = "A marvel of portable audio engineering, the all-new";
        public string AUDIO_BODY_BOLD = "Rotating Soundbar with Dolby Atmos® Speaker System";
        public string AUDIO_BODY_LAST = "delivers breathtaking, three-dimensional sound that flows above and around you with depth and precision.";
        public string AUDIO_POP_LEFT_HEADLINE = "An audio system like no other";
        public string AUDIO_POP_LEFT_BODY = "Dual speaker sets feature powerful down-firing woofers, custom tweeters, and noise-reducing vibration buffers for audio that's bigger, deeper, and more accurate than ever.";
        public string AUDIO_POP_RIGHT_HEADLINE = "Immersive sound in every mode";
        public string AUDIO_POP_RIGHT_BODY_FIRST = "From laptop to tablet and back, the unique";
        public string AUDIO_POP_RIGHT_BODY_BOLD = "Rotating Soundbar with Dolby Atmos Speaker System";
        public string AUDIO_POP_RIGHT_BODY_LAST = "offers the best performance at any angle.";
        public string AUDIO_POP_DEMO_COPY = "Get ready for truly immersive audio.";
        public string FEATURES_PLAY_CTA = "PLAY AUDIO DEMO";
        public string FEATURES_PEN_HEADLINE = "Never lose a thought—or your digital pen";
        public string FEATURES_PEN_BODY_FIRST = "With a";
        public string FEATURES_PEN_BODY_BOLD = "Garaged Pen";
        public string FEATURES_PEN_BODY_LAST = "that fits right into and charges from the chassis, you'll always be ready to capture your thoughts.";
        public string FEATURES_INK_HEADLINE = "Do more with Windows Ink";
        public string FEATURES_INK_BODY_FIRST = "Quickly jot down notes. Draw on a map to get turn-by-turn instructions. Post an annotated video to social media, and more.";
        public string FEATURES_INK_BODY_BOLD = "Windows Ink";
        public string FEATURES_INK_BODY_LAST = "is a whole new way to do a whole lot more.";
        public string FEATURES_WEBCAM_HEADLINE = "Rest easy—own your webcam";
        public string FEATURES_WEBCAM_BODY_FIRST = "No more worrying about being seen in your pajamas during calls. Simply slide the";
        public string FEATURES_WEBCAM_BODY_BOLD = "TrueBlock Privacy Shutter";
        public string FEATURES_WEBCAM_BODY_LAST = "open when ready to stream and back when you're done.";
        public string PERFORMANCE_PLAY_CTA = "PLAY AUDIO DEMO";
        public string PERFORMANCE_HEADLINE = "Designed for powerful performance";
        public string PERFORMANCE_BODY = "Innovative features. Sleek, stylish design. Every element engineered for the best user experience.";
        public string PERFORMANCE_WINDOWS_FIRST = "Featuring";
        public string PERFORMANCE_WINDOWS_BOLD = "Windows 10";
        public string PERFORMANCE_WINDOWS_LAST = "Home Edition";
        public string PERFORMANCE_INTEL_FIRST = "8th Gen";
        public string PERFORMANCE_INTEL_BOLD = "Intel® Core™ i7";
        public string PERFORMANCE_INTEL_LAST = "processor";
        public string PERFORMANCE_DOLBY_FIRST = "Featuring";
        public string PERFORMANCE_DOLBY_BOLD = "Dolby Vision™";
        public string PERFORMANCE_DOLBY_LAST = "ultravivid imaging";
        public string PERFORMANCE_4K_FIRST = "Up to";
        public string PERFORMANCE_4K_BOLD = "4K UHD";
        public string PERFORMANCE_4K_LAST = "Up to";
        public string PERFORMANCE_HOURS_FIRST = "Up to";
        public string PERFORMANCE_HOURS_BOLD = "14 hours*";
        public string PERFORMANCE_HOURS_LAST = "of battery life";
        public string PERFORMANCE_57_FIRST = "Just";
        public string PERFORMANCE_57_BOLD = "0.57\"";
        public string PERFORMANCE_57_LAST = "at its thinnest";
        public string PERFORMANCE_TOPDEVICE_COLOR = "Mica";
        public string PERFORMANCE_BOTTOMDEVICE_COLOR = "Iron Gray";
        public string PERFORMANCE_FOOTNOTE = "*14 hours (FHD). 9 hours (UHD). Based on testing with MobileMark 2014.";

        #endregion


        #region Construction

        public LanguageFile()
        {

        }

        #endregion


        #region Public Static Methods

        public static LanguageFile CreateDefault()
        {
            LanguageFile languageFile = new LanguageFile();

            languageFile.ATTRACTOR_CTA = "Touch Here for Sound Demo";
            languageFile.ATTRACTOR_TITLE = "YOGA C930 Innovation meets design";
            languageFile.NAV_EXPLORE = "Explore Windows";
            languageFile.NAV_GOTODESKTOP = "Go To Desktop";
            languageFile.AUDIO_PLAY_CTA = "PLAY AUDIO DEMO";
            languageFile.AUDIO_HEADLINE = "Surround sound that really surrounds";
            languageFile.AUDIO_BODY_FIRST = "A marvel of portable audio engineering, the all-new";
            languageFile.AUDIO_BODY_BOLD = "Rotating Soundbar with Dolby Atmos® Speaker System";
            languageFile.AUDIO_BODY_LAST = "delivers breathtaking, three-dimensional sound that flows above and around you with depth and precision.";
            languageFile.AUDIO_POP_LEFT_HEADLINE = "An audio system like no other";
            languageFile.AUDIO_POP_LEFT_BODY = "Dual speaker sets feature powerful down-firing woofers, custom tweeters, and noise-reducing vibration buffers for audio that's bigger, deeper, and more accurate than ever.";
            languageFile.AUDIO_POP_RIGHT_HEADLINE = "Immersive sound in every mode";
            languageFile.AUDIO_POP_RIGHT_BODY_FIRST = "From laptop to tablet and back, the unique";
            languageFile.AUDIO_POP_RIGHT_BODY_BOLD = "Rotating Soundbar with Dolby Atmos Speaker System";
            languageFile.AUDIO_POP_RIGHT_BODY_LAST = "offers the best performance at any angle.";
            languageFile.AUDIO_POP_DEMO_COPY = "Get ready for truly immersive audio.";
            languageFile.FEATURES_PLAY_CTA = "PLAY AUDIO DEMO";
            languageFile.FEATURES_PEN_HEADLINE = "Never lose a thought—or your digital pen";
            languageFile.FEATURES_PEN_BODY_FIRST = "With a";
            languageFile.FEATURES_PEN_BODY_BOLD = "Garaged Pen";
            languageFile.FEATURES_PEN_BODY_LAST = "that fits right into and charges from the chassis, you'll always be ready to capture your thoughts.";
            languageFile.FEATURES_INK_HEADLINE = "Do more with Windows Ink";
            languageFile.FEATURES_INK_BODY_FIRST = "Quickly jot down notes. Draw on a map to get turn-by-turn instructions. Post an annotated video to social media, and more.";
            languageFile.FEATURES_INK_BODY_BOLD = "Windows Ink";
            languageFile.FEATURES_INK_BODY_LAST = "is a whole new way to do a whole lot more.";
            languageFile.FEATURES_WEBCAM_HEADLINE = "Rest easy—own your webcam";
            languageFile.FEATURES_WEBCAM_BODY_FIRST = "No more worrying about being seen in your pajamas during calls. Simply slide the";
            languageFile.FEATURES_WEBCAM_BODY_BOLD = "TrueBlock Privacy Shutter";
            languageFile.FEATURES_WEBCAM_BODY_LAST = "open when ready to stream and back when you're done.";
            languageFile.PERFORMANCE_PLAY_CTA = "PLAY AUDIO DEMO";
            languageFile.PERFORMANCE_HEADLINE = "Designed for powerful performance";
            languageFile.PERFORMANCE_BODY = "Innovative features. Sleek, stylish design. Every element engineered for the best user experience.";
            languageFile.PERFORMANCE_WINDOWS_FIRST = "Featuring";
            languageFile.PERFORMANCE_WINDOWS_BOLD = "Windows 10";
            languageFile.PERFORMANCE_WINDOWS_LAST = "Home Edition";
            languageFile.PERFORMANCE_INTEL_FIRST = "8th Gen";
            languageFile.PERFORMANCE_INTEL_BOLD = "Intel® Core™ i7";
            languageFile.PERFORMANCE_INTEL_LAST = "processor";
            languageFile.PERFORMANCE_DOLBY_FIRST = "Featuring";
            languageFile.PERFORMANCE_DOLBY_BOLD = "Dolby Vision™";
            languageFile.PERFORMANCE_DOLBY_LAST = "ultravivid imaging";
            languageFile.PERFORMANCE_4K_FIRST = "Up to";
            languageFile.PERFORMANCE_4K_BOLD = "4K UHD";
            languageFile.PERFORMANCE_4K_LAST = "Up to";
            languageFile.PERFORMANCE_HOURS_FIRST = "Up to";
            languageFile.PERFORMANCE_HOURS_BOLD = "14 hours*";
            languageFile.PERFORMANCE_HOURS_LAST = "of battery life";
            languageFile.PERFORMANCE_57_FIRST = "Just";
            languageFile.PERFORMANCE_57_BOLD = "0.57\"";
            languageFile.PERFORMANCE_57_LAST = "at its thinnest";
            languageFile.PERFORMANCE_TOPDEVICE_COLOR = "Mica";
            languageFile.PERFORMANCE_BOTTOMDEVICE_COLOR = "Iron Gray";
            languageFile.PERFORMANCE_FOOTNOTE = "*14 hours (FHD). 9 hours (UHD). Based on testing with MobileMark 2014.";

            return languageFile;
        }

        #endregion
    }
}
