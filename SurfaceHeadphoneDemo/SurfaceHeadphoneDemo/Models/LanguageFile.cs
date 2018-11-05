using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceHeadphoneDemo.Models
{
    public sealed class LanguageFile
    {
        #region Public Properties

        public string INTRO_TITLE_DEVICE_ONE_TITLE = "Meet the new Surface Pro";
        public string INTRO_TITLE_DEVICE_ONE_CTA = "EXPLORE";
        public string INTRO_TITLE_DEVICE_TWO_TITLE = "Take a listen to Surface Headphone";
        public string INTRO_TITLE_DEVICE_TWO_CTA = "EXPLORE";
        public string AUDIO = "AUDIO";
        public string DESIGN = "DESIGN";
        public string TECH = "TECH";
        public string PRODUCTIVITY = "PRODUCTIVITY";
        public string SPECS = "SPECS";
        public string CONTENTS = "CONTENTS";
        public string TRACKNAV_TITLE_ONE = "[Artist Name]";
        public string TRACKNAV_SUBTITLE_ONE = "[Track title]";
        public string TRACKNAV_TITLE_TWO = "[Artist Name]";
        public string TRACKNAV_SUBTITLE_TWO = "[Track title]";
        public string TRACKNAV_TITLE_THREE = "[Artist Name]";
        public string TRACKNAV_SUBTITLE_THREE = "[Track title]";
        public string TRACKNAV_TITLE_FOUR = "[Artist Name]";
        public string TRACKNAV_SUBTITLE_FOUR = "[Track title]";
        public string AUDIO_GATEWAY_HEADLINE = "Put on headphones";
        public string AUDIO_GATEWAY_COPY = "Because hearing is believing.";
        public string AUDIO_GATEWAY_CTA = "I’m ready";
        public string AUDIO_TRACK_HEADLINE = "The smarter way to listen";
        public string AUDIO_TRACK_COPY = "Surround yourself with rich audio, intuitive controls, and advanced features like adjustable noise cancellation and built-in digital assistant.";
        public string AUDIO_TRACK_LEGAL = "";
        public string AUDIO_TRACK_BULLET_LIST_TITLE = "Select a track";
        public string AUDIO_TRACK_BULLET_ONE_TITLE = "[Artist Name]";
        public string ADDIO_TRACK_BULLET_ONE_SUBTITLE = "[Track title]";
        public string AUDIO_TRACK_BULLET_TWO_TITLE = "[Artist Name]";
        public string ADDIO_TRACK_BULLET_TWO_SUBTITLE = "[Track title]";
        public string AUDIO_TRACK_BULLET_THREE_TITLE = "[Artist Name]";
        public string ADDIO_TRACK_BULLET_THREE_SUBTITLE = "[Track title]";
        public string AUDIO_TRACK_BULLET_FOUR_TITLE = "[Artist Name]";
        public string AUDIO_TRACK_BULLET_FOUR_SUBTITLE = "[Track title]";
        public string AUDIO_TRACK_TRYIT = "TRY IT";
        public string AUDIO_TRACK_TRYIT_POPUP_HEADLINE = "Learn how to use touch controls";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE = "Overview";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_LEFT = "Noise cancellation/Phone call control";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_BOTTOM_MID = "Mic mute";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_BOTTOM_RIGHT = "USB charger port";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_LOWER_RIGHT = "3.5 mm connection";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_MID_RIGHT = "Volume dial/Playback control";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_TOP_RIGHT = "Power on/off";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO = "Play and control music";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_TITLE = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_COPY = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_LEGAL = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE = "Adjust volume";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_TITLE = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_COPY = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_LEGAL = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR = "Answer calls";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_TITLE = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_COPY = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_LEGAL = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE = "Customize noise cancellation";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_TITLE = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_COPY = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_LEGAL = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX = "Ask for help";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_TITLE = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_COPY = "";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_LEGAL = "";
        public string DESIGN_DESIGN_HEADLINE = "Get comfortable";
        public string DESIGN_DESIGN_COPY = "Surface Headphone balances innovative design with Omnisonic audio that wraps you in your favorite music, shows, and more.";
        public string DESIGN_DESIGN_POPLEFT_TITLE = "Simple, intuitive controls";
        public string DESIGN_DESIGN_POPLEFT_COPY = "Skip songs, answer calls, mute your mic, and more with a touch or twist. Audio pauses when you take off the headphones.";
        public string DESIGN_DESIGN_POPTOP_TITLE = "All-day style";
        public string DESIGN_DESIGN_POPTOP_COPY = "Lightweight build and balanced fit you can wear for a full day of travel or at the office.";
        public string DESIGN_DESIGN_POPRIGHT_TITLE = "Comfort";
        public string DESIGN_DESIGN_POPRIGHT_COPY = "Soft, breathable over-ear cups let you listen comfortably for hours.";
        public string TECH_TECH_HEADLINE = "Hear what’s next";
        public string TECH_TECH_COPY = "Your music and phone calls sound spectacular with audio performance and wireless connectivity made for all-day immersive listening.";
        public string TECH_TECH_POPLEFT_TITLE = "Battery life";
        public string TECH_TECH_POPLEFT_COPY = "Up to 15 hours on a full charge with Bluetooth connection*. Stream almost an hour of music with just a 5-minute charge.*";
        public string TECH_TECH_POPLEFT_BATTERY_HR = "15 hrs";
        public string TECH_TECH_POPLEFT_BATTERY_KEGAL = "* Up to 15 hours of battery life when connected via Bluetooth. Battery life varies significantly with usage and settings.";
        public string TECH_TECH_POPMID_TITLE = "Amazing audio";
        public string TECH_TECH_POPMID_COPY = "Free-edge speaker design and optimized sonic cavity engineered for richer, clearer acoustics.";
        public string TECH_TECH_RIGHT_TITLE = "Be heard";
        public string TECH_TECH_RIGHT_COPY = "Exceptional clarity and accurate speech recognition thanks to 4 built-in microphones and innovative microphone technology.";
        public string PRODUCTIVITY_PRODUCTIVITY_HEADLINE = "More intelligent";
        public string PRODUCTIVITY_PRODUCTIVITY_COPY = "Get more done with hands-free voice control and smarter experiences designed to seamlessly integrate with your phone or computer.";
        public string PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE = "Microsoft Cortana";
        public string PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY = "Ask your digital assistant to play your favorite artist, start calls, join meetings, answer questions, and more.*";
        public string PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL = "*Cortana available in select markets. Experience may vary by region and device.";
        public string PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE = "Adjustable Active Noise Cancellation";
        public string PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY = "Turn up the silence on planes, or whenever surrounding noise is loudest. Turn it down when you want to be more aware of your surroundings.";
        public string PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE = "Hassle-free pairing";
        public string PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY = "Connect Surface Headphone to your Windows 10 PC right out of the box when you enable Swift Pair.*";
        public string PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL = "*Pairing functionality requires Windows 10 Spring Creators Update.";
        public string SPECS_SPECS_HEADLINE = "Specifications";
        public string SPECS_SPECS_BULLETONE_TITLE = "Dimensions";
        public string SPECS_SPECS_BULLETONE_COPY_BRT = "8.03” x 7.68” x 1.89”";
        public string SPECS_SPECS_BULLETONE_COPY_MET = "204 mm x 195 mm x 48 mm";
        public string SPECS_SPECS_BULLETONE_LEGAL = "";
        public string SPECS_SPECS_BULLETTWO_TITLE = "Weight";
        public string SPECS_SPECS_BULLETTWO_COPY = "0.64 lbs. (XX g)";
        public string SPECS_SPECS_BULLETTWO_LEGAL = "";
        public string SPECS_SPECS_BULLETTHREE_TITLE = "Speaker";
        public string SPECS_SPECS_BULLETTHREE_COPY = "40 mm Free Edge driver";
        public string SPECS_SPECS_BULLETTHREE_LEGAL = "";
        public string SPECS_SPECS_BULLETFOUR_TITLE = "Frequency response";
        public string SPECS_SPECS_BULLETFOUR_COPY = "20 Hz – 20 kHz";
        public string SPECS_SPECS_BULLETFOUR_LEGAL = "";
        public string SPECS_SPECS_BULLETFIVE_TITLE = "Battery life";
        public string SPECS_SPECS_BULLETFIVE_COPY = "Up to 15 hours (with music playback over Bluetooth, ANC on, hands-free Cortana** enabled)*";
        public string SPECS_SPECS_BULLETFIVE_LEGAL = "* Up to 15 hours of battery life when connected via Bluetooth. Battery life varies significantly with usage and settings.\n**Cortana available in select markets. Experience may vary by region and device.";
        public string SPECS_SPECS_BULLETSIX_TITLE = "Noise cancellation";
        public string SPECS_SPECS_BULLETSIX_COPY = "≤ 30 dB for active noise cancellation\n≤ 40 dB for passive noise cancellation";
        public string SPECS_SPECS_BULLETSIX_LEGAL = "";
        public string SPECS_SPECS_BULLETSEVEN_TITLE = "Inputs";
        public string SPECS_SPECS_BULLETSEVEN_COPY = "USB-C connector\n3.5 mm audio connector";
        public string SPECS_SPECS_BULLETSEVEN_LEGAL = "";
        public string SPECS_SPECS_BULLETEIGHT_TITLE = "Compatibility";
        public string SPECS_SPECS_BULLETEIGHT_COPY = "Windows 10, iOS, Android, MacOS";
        public string SPECS_SPECS_BULLETEIGHT_LEGAL = "";
        public string SPECS_SPECS_BULLETNINE_TITLE = "On-device controls";
        public string SPECS_SPECS_BULLETNINE_COPY = "Power button, mute button, Volume dial (right ear) Noise Cancellation dial (left ear)";
        public string SPECS_SPECS_BULLETNINE_LEGAL = "";
        public string SPECS_WITB_HEADLINE = "What’s in the box";
        public string SPECS_WITB_BULLETONE_TITLE = "Surface Headphone headphones";
        public string SPECS_WITB_BULLETTWO_TITLE = "5’ USB cable";
        public string SPECS_WITB_BULLETTHREE_TITLE = "4.3’ 3.5 mm stereo headphone cord";
        public string SPECS_WITB_BULLETFOUR_TITLE = "Hard-shell carrying case";
        public string SPECS_WITB_BULLETFIVE_TITLE = "Instructions and documentation";

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

            languageFile.INTRO_TITLE_DEVICE_ONE_TITLE = "Meet the new Surface Pro";
            languageFile.INTRO_TITLE_DEVICE_ONE_CTA = "EXPLORE";
            languageFile.INTRO_TITLE_DEVICE_TWO_TITLE = "Take a listen to Surface Headphone";
            languageFile.INTRO_TITLE_DEVICE_TWO_CTA = "EXPLORE";
            languageFile.AUDIO = "AUDIO";
            languageFile.DESIGN = "DESIGN";
            languageFile.TECH = "TECH";
            languageFile.PRODUCTIVITY = "PRODUCTIVITY";
            languageFile.SPECS = "SPECS";
            languageFile.CONTENTS = "CONTENTS";
            languageFile.TRACKNAV_TITLE_ONE = "[Artist Name]";
            languageFile.TRACKNAV_SUBTITLE_ONE = "[Track title]";
            languageFile.TRACKNAV_TITLE_TWO = "[Artist Name]";
            languageFile.TRACKNAV_SUBTITLE_TWO = "[Track title]";
            languageFile.TRACKNAV_TITLE_THREE = "[Artist Name]";
            languageFile.TRACKNAV_SUBTITLE_THREE = "[Track title]";
            languageFile.TRACKNAV_TITLE_FOUR = "[Artist Name]";
            languageFile.TRACKNAV_SUBTITLE_FOUR = "[Track title]";
            languageFile.AUDIO_GATEWAY_HEADLINE = "Put on headphones";
            languageFile.AUDIO_GATEWAY_COPY = "Because hearing is believing.";
            languageFile.AUDIO_GATEWAY_CTA = "I’m ready";
            languageFile.AUDIO_TRACK_HEADLINE = "The smarter way to listen";
            languageFile.AUDIO_TRACK_COPY = "Surround yourself with rich audio, intuitive controls, and advanced features like adjustable noise cancellation and built-in digital assistant.";
            languageFile.AUDIO_TRACK_LEGAL = "";
            languageFile.AUDIO_TRACK_BULLET_LIST_TITLE = "Select a track";
            languageFile.AUDIO_TRACK_BULLET_ONE_TITLE = "[Artist Name]";
            languageFile.ADDIO_TRACK_BULLET_ONE_SUBTITLE = "[Track title]";
            languageFile.AUDIO_TRACK_BULLET_TWO_TITLE = "[Artist Name]";
            languageFile.ADDIO_TRACK_BULLET_TWO_SUBTITLE = "[Track title]";
            languageFile.AUDIO_TRACK_BULLET_THREE_TITLE = "[Artist Name]";
            languageFile.ADDIO_TRACK_BULLET_THREE_SUBTITLE = "[Track title]";
            languageFile.AUDIO_TRACK_BULLET_FOUR_TITLE = "[Artist Name]";
            languageFile.AUDIO_TRACK_BULLET_FOUR_SUBTITLE = "[Track title]";
            languageFile.AUDIO_TRACK_TRYIT = "TRY IT";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_HEADLINE = "Learn how to use touch controls";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE = "Overview";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_LEFT = "Noise cancellation/Phone call control";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_BOTTOM_MID = "Mic mute";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_BOTTOM_RIGHT = "USB charger port";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_LOWER_RIGHT = "3.5 mm connection";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_MID_RIGHT = "Volume dial/Playback control";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_TOP_RIGHT = "Power on/off";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO = "Play and control music";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_TITLE = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_COPY = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_LEGAL = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE = "Adjust volume";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_TITLE = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_COPY = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_LEGAL = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR = "Answer calls";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_TITLE = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_COPY = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_LEGAL = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE = "Customize noise cancellation";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_TITLE = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_COPY = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_LEGAL = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX = "Ask for help";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_TITLE = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_COPY = "";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_LEGAL = "";
            languageFile.DESIGN_DESIGN_HEADLINE = "Get comfortable";
            languageFile.DESIGN_DESIGN_COPY = "Surface Headphone balances innovative design with Omnisonic audio that wraps you in your favorite music, shows, and more.";
            languageFile.DESIGN_DESIGN_POPLEFT_TITLE = "Simple, intuitive controls";
            languageFile.DESIGN_DESIGN_POPLEFT_COPY = "Skip songs, answer calls, mute your mic, and more with a touch or twist. Audio pauses when you take off the headphones.";
            languageFile.DESIGN_DESIGN_POPTOP_TITLE = "All-day style";
            languageFile.DESIGN_DESIGN_POPTOP_COPY = "Lightweight build and balanced fit you can wear for a full day of travel or at the office.";
            languageFile.DESIGN_DESIGN_POPRIGHT_TITLE = "Comfort";
            languageFile.DESIGN_DESIGN_POPRIGHT_COPY = "Soft, breathable over-ear cups let you listen comfortably for hours.";
            languageFile.TECH_TECH_HEADLINE = "Hear what’s next";
            languageFile.TECH_TECH_COPY = "Your music and phone calls sound spectacular with audio performance and wireless connectivity made for all-day immersive listening.";
            languageFile.TECH_TECH_POPLEFT_TITLE = "Battery life";
            languageFile.TECH_TECH_POPLEFT_COPY = "Up to 15 hours on a full charge with Bluetooth connection*. Stream almost an hour of music with just a 5-minute charge.*";
            languageFile.TECH_TECH_POPLEFT_BATTERY_HR = "15 hrs";
            languageFile.TECH_TECH_POPLEFT_BATTERY_KEGAL = "* Up to 15 hours of battery life when connected via Bluetooth. Battery life varies significantly with usage and settings.";
            languageFile.TECH_TECH_POPMID_TITLE = "Amazing audio";
            languageFile.TECH_TECH_POPMID_COPY = "Free-edge speaker design and optimized sonic cavity engineered for richer, clearer acoustics.";
            languageFile.TECH_TECH_RIGHT_TITLE = "Be heard";
            languageFile.TECH_TECH_RIGHT_COPY = "Exceptional clarity and accurate speech recognition thanks to 4 built-in microphones and innovative microphone technology.";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_HEADLINE = "More intelligent";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_COPY = "Get more done with hands-free voice control and smarter experiences designed to seamlessly integrate with your phone or computer.";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE = "Microsoft Cortana";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY = "Ask your digital assistant to play your favorite artist, start calls, join meetings, answer questions, and more.*";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL = "*Cortana available in select markets. Experience may vary by region and device.";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE = "Adjustable Active Noise Cancellation";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY = "Turn up the silence on planes, or whenever surrounding noise is loudest. Turn it down when you want to be more aware of your surroundings.";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE = "Hassle-free pairing";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY = "Connect Surface Headphone to your Windows 10 PC right out of the box when you enable Swift Pair.*";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL = "*Pairing functionality requires Windows 10 Spring Creators Update.";
            languageFile.SPECS_SPECS_HEADLINE = "Specifications";
            languageFile.SPECS_SPECS_BULLETONE_TITLE = "Dimensions";
            languageFile.SPECS_SPECS_BULLETONE_COPY_BRT = "8.03” x 7.68” x 1.89”";
            languageFile.SPECS_SPECS_BULLETONE_COPY_MET = "204 mm x 195 mm x 48 mm";
            languageFile.SPECS_SPECS_BULLETONE_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETTWO_TITLE = "Weight";
            languageFile.SPECS_SPECS_BULLETTWO_COPY = "0.64 lbs. (XX g)";
            languageFile.SPECS_SPECS_BULLETTWO_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETTHREE_TITLE = "Speaker";
            languageFile.SPECS_SPECS_BULLETTHREE_COPY = "40 mm Free Edge driver";
            languageFile.SPECS_SPECS_BULLETTHREE_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETFOUR_TITLE = "Frequency response";
            languageFile.SPECS_SPECS_BULLETFOUR_COPY = "20 Hz – 20 kHz";
            languageFile.SPECS_SPECS_BULLETFOUR_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETFIVE_TITLE = "Battery life";
            languageFile.SPECS_SPECS_BULLETFIVE_COPY = "Up to 15 hours (with music playback over Bluetooth, ANC on, hands-free Cortana** enabled)*";
            languageFile.SPECS_SPECS_BULLETFIVE_LEGAL = "* Up to 15 hours of battery life when connected via Bluetooth. Battery life varies significantly with usage and settings.\n**Cortana available in select markets. Experience may vary by region and device.";
            languageFile.SPECS_SPECS_BULLETSIX_TITLE = "Noise cancellation";
            languageFile.SPECS_SPECS_BULLETSIX_COPY = "≤ 30 dB for active noise cancellation\n≤ 40 dB for passive noise cancellation";
            languageFile.SPECS_SPECS_BULLETSIX_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETSEVEN_TITLE = "Inputs";
            languageFile.SPECS_SPECS_BULLETSEVEN_COPY = "USB-C connector\n3.5 mm audio connector";
            languageFile.SPECS_SPECS_BULLETSEVEN_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETEIGHT_TITLE = "Compatibility";
            languageFile.SPECS_SPECS_BULLETEIGHT_COPY = "Windows 10, iOS, Android, MacOS";
            languageFile.SPECS_SPECS_BULLETEIGHT_LEGAL = "";
            languageFile.SPECS_SPECS_BULLETNINE_TITLE = "On-device controls";
            languageFile.SPECS_SPECS_BULLETNINE_COPY = "Power button, mute button, Volume dial (right ear) Noise Cancellation dial (left ear)";
            languageFile.SPECS_SPECS_BULLETNINE_LEGAL = "";
            languageFile.SPECS_WITB_HEADLINE = "What’s in the box";
            languageFile.SPECS_WITB_BULLETONE_TITLE = "Surface Headphone headphones";
            languageFile.SPECS_WITB_BULLETTWO_TITLE = "5’ USB cable";
            languageFile.SPECS_WITB_BULLETTHREE_TITLE = "4.3’ 3.5 mm stereo headphone cord";
            languageFile.SPECS_WITB_BULLETFOUR_TITLE = "Hard-shell carrying case";
            languageFile.SPECS_WITB_BULLETFIVE_TITLE = "Instructions and documentation";

            return languageFile;
        }

        #endregion

    }
}
