﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceJackDemo.Models
{
    public sealed class LanguageFile
    {
        #region Public Properties

        public string INTRO_TITLE_DEVICE_ONE_TITLE = "INTRO_TITLE_DEVICE_ONE_TITLE";
        public string INTRO_TITLE_DEVICE_ONE_CTA = "INTRO_TITLE_DEVICE_ONE_CTA";
        public string INTRO_TITLE_DEVICE_TWO_TITLE = "INTRO_TITLE_DEVICE_TWO_TITLE";
        public string INTRO_TITLE_DEVICE_TWO_CTA = "INTRO_TITLE_DEVICE_TWO_CTA";
        public string AUDIO = "AUDIO";
        public string DESIGN = "DESIGN";
        public string TECH = "TECH";
        public string PRODUCTIVITY = "PRODUCTIVITY";
        public string SPECS = "SPECS";
        public string PROMO = "PROMO";
        public string TRACKNAV_TITLE_ONE = "TRACKNAV_TITLE_ONE";
        public string TRACKNAV_SUBTITLE_ONE = "TRACKNAV_SUBTITLE_ONE";
        public string TRACKNAV_TITLE_TWO = "TRACKNAV_TITLE_TWO";
        public string TRACKNAV_SUBTITLE_TWO = "TRACKNAV_SUBTITLE_TWO";
        public string TRACKNAV_TITLE_THREE = "TRACKNAV_TITLE_THREE";
        public string TRACKNAV_SUBTITLE_THREE = "TRACKNAV_SUBTITLE_THREE";
        public string TRACKNAV_TITLE_FOUR = "TRACKNAV_TITLE_FOUR";
        public string TRACKNAV_SUBTITLE_FOUR = "TRACKNAV_SUBTITLE_FOUR";
        public string AUDIO_GATEWAY_HEADLINE = "AUDIO_GATEWAY_HEADLINE";
        public string AUDIO_GATEWAY_COPY = "AUDIO_GATEWAY_COPY";
        public string AUDIO_GATEWAY_CTA = "AUDIO_GATEWAY_CTA";
        public string AUDIO_TRACK_HEADLINE = "AUDIO_TRACK_HEADLINE";
        public string AUDIO_TRACK_COPY = "AUDIO_TRACK_COPY";
        public string AUDIO_TRACK_LEGAL = "AUDIO_TRACK_LEGAL";
        public string AUDIO_TRACK_BULLET_LIST_TITLE = "AUDIO_TRACK_BULLET_LIST_TITLE";
        public string AUDIO_TRACK_BULLET_ONE_TITLE = "AUDIO_TRACK_BULLET_ONE_TITLE";
        public string ADDIO_TRACK_BULLET_ONE_SUBTITLE = "ADDIO_TRACK_BULLET_ONE_SUBTITLE";
        public string AUDIO_TRACK_BULLET_TWO_TITLE = "AUDIO_TRACK_BULLET_TWO_TITLE";
        public string AUDIO_TRACK_BULLET_TWO_SUBTITLE = "ADDIO_TRACK_BULLET_TWO_SUBTITLE";
        public string AUDIO_TRACK_BULLET_THREE_TITLE = "AUDIO_TRACK_BULLET_THREE_TITLE";
        public string ADDIO_TRACK_BULLET_THREE_SUBTITLE = "ADDIO_TRACK_BULLET_THREE_SUBTITLE";
        public string AUDIO_TRACK_BULLET_FOUR_TITLE = "AUDIO_TRACK_BULLET_FOUR_TITLE";
        public string AUDIO_TRACK_BULLET_FOUR_SUBTITLE = "AUDIO_TRACK_BULLET_FOUR_SUBTITLE";
        public string AUDIO_TRACK_TRYIT = "AUDIO_TRACK_TRYIT";
        public string AUDIO_TRACK_TRYIT_POPUP_HEADLINE = "AUDIO_TRACK_TRYIT_POPUP_HEADLINE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_ONE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_ONE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_TWO = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_TWO";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_THREE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_THREE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FOUR = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FOUR";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FIVE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FIVE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_SIX = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_SIX";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_TITLE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_COPY";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_LEGAL";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_TITLE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_COPY";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_LEGAL";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_TITLE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_COPY";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_LEGAL";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_TITLE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_COPY";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_LEGAL";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_TITLE";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_COPY";
        public string AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_LEGAL";
        public string DESIGN_DESIGN_HEADLINE = "DESIGN_DESIGN_HEADLINE";
        public string DESIGN_DESIGN_COPY = "DESIGN_DESIGN_COPY";
        public string DESIGN_DESIGN_POPLEFT_TITLE = "DESIGN_DESIGN_POPLEFT_TITLE";
        public string DESIGN_DESIGN_POPLEFT_COPY = "DESIGN_DESIGN_POPLEFT_COPY";
        public string DESIGN_DESIGN_POPTOP_TITLE = "DESIGN_DESIGN_POPTOP_TITLE";
        public string DESIGN_DESIGN_POPTOP_COPY = "DESIGN_DESIGN_POPTOP_COPY";
        public string DESIGN_DESIGN_POPRIGHT_TITLE = "DESIGN_DESIGN_POPRIGHT_TITLE";
        public string DESIGN_DESIGN_POPRIGHT_COPY = "DESIGN_DESIGN_POPRIGHT_COPY";
        public string TECH_TECH_HEADLINE = "TECH_TECH_HEADLINE";
        public string TECH_TECH_COPY = "TECH_TECH_COPY";
        public string TECH_TECH_POPLEFT_TITLE = "TECH_TECH_POPLEFT_TITLE";
        public string TECH_TECH_POPLEFT_COPY = "TECH_TECH_POPLEFT_COPY";
        public string TECH_TECH_POPLEFT_BATTERY_HR = "TECH_TECH_POPLEFT_BATTERY_HR";
        public string TECH_TECH_POPMID_TITLE = "TECH_TECH_POPMID_TITLE";
        public string TECH_TECH_POPMID_COPY = "TECH_TECH_POPMID_COPY";
        public string TECH_TECH_RIGHT_TITLE = "TECH_TECH_RIGHT_TITLE";
        public string TECH_TECH_RIGHT_COPY = "TECH_TECH_RIGHT_COPY";
        public string TECH_TECH_TOP_TITLE = "TECH_TECH_TOP_TITLE";
        public string TECH_TECH_TOP_COPY = "TECH_TECH_TOP_COPY";
        public string TECH_TECH_TOP_LEGAL = "TECH_TECH_TOP_LEGAL";
        public string PRODUCTIVITY_PRODUCTIVITY_HEADLINE = "PRODUCTIVITY_PRODUCTIVITY_HEADLINE";
        public string PRODUCTIVITY_PRODUCTIVITY_COPY = "PRODUCTIVITY_PRODUCTIVITY_COPY";
        public string PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE = "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE";
        public string PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY = "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY";
        public string PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL = "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL";
        public string PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE = "PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE";
        public string PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY = "PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY";
        public string PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE = "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE";
        public string PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY = "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY";
        public string PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL = "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL";
        public string SPECS_SPECS_HEADLINE = "SPECS_SPECS_HEADLINE";
        public string SPECS_SPECS_BULLETONE_TITLE = "SPECS_SPECS_BULLETONE_TITLE";
        public string SPECS_SPECS_BULLETONE_COPY = "SPECS_SPECS_BULLETONE_COPY";
        public string SPECS_SPECS_BULLETONE_LEGAL = "SPECS_SPECS_BULLETONE_LEGAL";
        public string SPECS_SPECS_BULLETTWO_TITLE = "SPECS_SPECS_BULLETTWO_TITLE";
        public string SPECS_SPECS_BULLETTWO_COPY = "SPECS_SPECS_BULLETTWO_COPY";
        public string SPECS_SPECS_BULLETTWO_LEGAL = "SPECS_SPECS_BULLETTWO_LEGAL";
        public string SPECS_SPECS_BULLETTHREE_TITLE = "SPECS_SPECS_BULLETTHREE_TITLE";
        public string SPECS_SPECS_BULLETTHREE_COPY = "SPECS_SPECS_BULLETTHREE_COPY";
        public string SPECS_SPECS_BULLETTHREE_LEGAL = "SPECS_SPECS_BULLETTHREE_LEGAL";
        public string SPECS_SPECS_BULLETFOUR_TITLE = "SPECS_SPECS_BULLETFOUR_TITLE";
        public string SPECS_SPECS_BULLETFOUR_COPY = "SPECS_SPECS_BULLETFOUR_COPY";
        public string SPECS_SPECS_BULLETFOUR_LEGAL = "SPECS_SPECS_BULLETFOUR_LEGAL";
        public string SPECS_SPECS_BULLETFIVE_TITLE = "SPECS_SPECS_BULLETFIVE_TITLE";
        public string SPECS_SPECS_BULLETFIVE_COPY = "SPECS_SPECS_BULLETFIVE_COPY";
        public string SPECS_SPECS_BULLETFIVE_LEGAL = "SPECS_SPECS_BULLETFIVE_LEGAL";
        public string SPECS_SPECS_BULLETSIX_TITLE = "SPECS_SPECS_BULLETSIX_TITLE";
        public string SPECS_SPECS_BULLETSIX_COPY = "SPECS_SPECS_BULLETSIX_COPY";
        public string SPECS_SPECS_BULLETSIX_LEGAL = "SPECS_SPECS_BULLETSIX_LEGAL";
        public string SPECS_SPECS_BULLETSEVEN_TITLE = "SPECS_SPECS_BULLETSEVEN_TITLE";
        public string SPECS_SPECS_BULLETSEVEN_COPY = "SPECS_SPECS_BULLETSEVEN_COPY";
        public string SPECS_SPECS_BULLETSEVEN_LEGAL = "SPECS_SPECS_BULLETSEVEN_LEGAL";
        public string SPECS_WITB_HEADLINE = "SPECS_WITB_HEADLINE";
        public string SPECS_WITB_BULLETONE_TITLE = "SPECS_WITB_BULLETONE_TITLE";
        public string SPECS_WITB_BULLETTWO_TITLE = "SPECS_WITB_BULLETTWO_TITLE";
        public string SPECS_WITB_BULLETTHREE_TITLE = "SPECS_WITB_BULLETTHREE_TITLE";
        public string SPECS_WITB_BULLETFOUR_TITLE = "SPECS_WITB_BULLETFOUR_TITLE";
        public string PROMO_PROMO_HEADLINE = "PROMO_PROMO_HEADLINE";
        public string PROMO_PROMO_SUBHEAD = "PROMO_PROMO_SUBHEAD";
        public string PROMO_PROMO_COPY = "PROMO_PROMO_COPY";
        public string PROMO_PROMO_LEGAL = "PROMO_PROMO_LEGAL";


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

            languageFile.INTRO_TITLE_DEVICE_ONE_TITLE = "INTRO_TITLE_DEVICE_ONE_TITLE";
            languageFile.INTRO_TITLE_DEVICE_ONE_CTA = "INTRO_TITLE_DEVICE_ONE_CTA";
            languageFile.INTRO_TITLE_DEVICE_TWO_TITLE = "INTRO_TITLE_DEVICE_TWO_TITLE";
            languageFile.INTRO_TITLE_DEVICE_TWO_CTA = "INTRO_TITLE_DEVICE_TWO_CTA";
            languageFile.AUDIO = "AUDIO";
            languageFile.DESIGN = "DESIGN";
            languageFile.TECH = "TECH";
            languageFile.PRODUCTIVITY = "PRODUCTIVITY";
            languageFile.SPECS = "SPECS";
            languageFile.PROMO = "PROMO";
            languageFile.TRACKNAV_TITLE_ONE = "TRACKNAV_TITLE_ONE";
            languageFile.TRACKNAV_SUBTITLE_ONE = "TRACKNAV_SUBTITLE_ONE";
            languageFile.TRACKNAV_TITLE_TWO = "TRACKNAV_TITLE_TWO";
            languageFile.TRACKNAV_SUBTITLE_TWO = "TRACKNAV_SUBTITLE_TWO";
            languageFile.TRACKNAV_TITLE_THREE = "TRACKNAV_TITLE_THREE";
            languageFile.TRACKNAV_SUBTITLE_THREE = "TRACKNAV_SUBTITLE_THREE";
            languageFile.TRACKNAV_TITLE_FOUR = "TRACKNAV_TITLE_FOUR";
            languageFile.TRACKNAV_SUBTITLE_FOUR = "TRACKNAV_SUBTITLE_FOUR";
            languageFile.AUDIO_GATEWAY_HEADLINE = "AUDIO_GATEWAY_HEADLINE";
            languageFile.AUDIO_GATEWAY_COPY = "AUDIO_GATEWAY_COPY";
            languageFile.AUDIO_GATEWAY_CTA = "AUDIO_GATEWAY_CTA";
            languageFile.AUDIO_TRACK_HEADLINE = "AUDIO_TRACK_HEADLINE";
            languageFile.AUDIO_TRACK_COPY = "AUDIO_TRACK_COPY";
            languageFile.AUDIO_TRACK_LEGAL = "AUDIO_TRACK_LEGAL";
            languageFile.AUDIO_TRACK_BULLET_LIST_TITLE = "AUDIO_TRACK_BULLET_LIST_TITLE";
            languageFile.AUDIO_TRACK_BULLET_ONE_TITLE = "AUDIO_TRACK_BULLET_ONE_TITLE";
            languageFile.ADDIO_TRACK_BULLET_ONE_SUBTITLE = "ADDIO_TRACK_BULLET_ONE_SUBTITLE";
            languageFile.AUDIO_TRACK_BULLET_TWO_TITLE = "AUDIO_TRACK_BULLET_TWO_TITLE";
            languageFile.AUDIO_TRACK_BULLET_TWO_SUBTITLE = "ADDIO_TRACK_BULLET_TWO_SUBTITLE";
            languageFile.AUDIO_TRACK_BULLET_THREE_TITLE = "AUDIO_TRACK_BULLET_THREE_TITLE";
            languageFile.ADDIO_TRACK_BULLET_THREE_SUBTITLE = "ADDIO_TRACK_BULLET_THREE_SUBTITLE";
            languageFile.AUDIO_TRACK_BULLET_FOUR_TITLE = "AUDIO_TRACK_BULLET_FOUR_TITLE";
            languageFile.AUDIO_TRACK_BULLET_FOUR_SUBTITLE = "AUDIO_TRACK_BULLET_FOUR_SUBTITLE";
            languageFile.AUDIO_TRACK_TRYIT = "AUDIO_TRACK_TRYIT";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_HEADLINE = "AUDIO_TRACK_TRYIT_POPUP_HEADLINE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_TITLE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_ONE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_ONE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_TWO = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_TWO";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_THREE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_THREE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FOUR = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FOUR";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FIVE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_FIVE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_SIX = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_SPEC_SIX";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_ONE_LEGAL";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_TITLE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_COPY";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_TWO_LEGAL";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_TITLE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_COPY";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_THREE_LEGAL";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_TITLE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_COPY";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FOUR_LEGAL";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_TITLE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_COPY";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_FIVE_LEGAL";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_TITLE = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_TITLE";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_COPY = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_COPY";
            languageFile.AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_LEGAL = "AUDIO_TRACK_TRYIT_POPUP_BUTTON_SIX_LEGAL";
            languageFile.DESIGN_DESIGN_HEADLINE = "DESIGN_DESIGN_HEADLINE";
            languageFile.DESIGN_DESIGN_COPY = "DESIGN_DESIGN_COPY";
            languageFile.DESIGN_DESIGN_POPLEFT_TITLE = "DESIGN_DESIGN_POPLEFT_TITLE";
            languageFile.DESIGN_DESIGN_POPLEFT_COPY = "DESIGN_DESIGN_POPLEFT_COPY";
            languageFile.DESIGN_DESIGN_POPTOP_TITLE = "DESIGN_DESIGN_POPTOP_TITLE";
            languageFile.DESIGN_DESIGN_POPTOP_COPY = "DESIGN_DESIGN_POPTOP_COPY";
            languageFile.DESIGN_DESIGN_POPRIGHT_TITLE = "DESIGN_DESIGN_POPRIGHT_TITLE";
            languageFile.DESIGN_DESIGN_POPRIGHT_COPY = "DESIGN_DESIGN_POPRIGHT_COPY";
            languageFile.TECH_TECH_HEADLINE = "TECH_TECH_HEADLINE";
            languageFile.TECH_TECH_COPY = "TECH_TECH_COPY";
            languageFile.TECH_TECH_POPLEFT_TITLE = "TECH_TECH_POPLEFT_TITLE";
            languageFile.TECH_TECH_POPLEFT_COPY = "TECH_TECH_POPLEFT_COPY";
            languageFile.TECH_TECH_POPLEFT_BATTERY_HR = "TECH_TECH_POPLEFT_BATTERY_HR";
            languageFile.TECH_TECH_POPMID_TITLE = "TECH_TECH_POPMID_TITLE";
            languageFile.TECH_TECH_POPMID_COPY = "TECH_TECH_POPMID_COPY";
            languageFile.TECH_TECH_RIGHT_TITLE = "TECH_TECH_RIGHT_TITLE";
            languageFile.TECH_TECH_RIGHT_COPY = "TECH_TECH_RIGHT_COPY";
            languageFile.TECH_TECH_TOP_TITLE = "TECH_TECH_TOP_TITLE";
            languageFile.TECH_TECH_TOP_COPY = "TECH_TECH_TOP_COPY";
            languageFile.TECH_TECH_TOP_LEGAL = "TECH_TECH_TOP_LEGAL";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_HEADLINE = "PRODUCTIVITY_PRODUCTIVITY_HEADLINE";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_COPY = "PRODUCTIVITY_PRODUCTIVITY_COPY";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE = "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_TITLE";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY = "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_COPY";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL = "PRODUCTIVITY_PRODUCTIVITY_POPLEFT_LEGAL";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE = "PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_TITLE";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY = "PRODUCTIVITY_PRODUCTIVITY_POPRIGHT_COPY";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE = "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_TITLE";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY = "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_COPY";
            languageFile.PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL = "PRODUCTIVITY_PRODUCTIVITY_POPBOTTOM_LEGAL";
            languageFile.SPECS_SPECS_HEADLINE = "SPECS_SPECS_HEADLINE";
            languageFile.SPECS_SPECS_BULLETONE_TITLE = "SPECS_SPECS_BULLETONE_TITLE";
            languageFile.SPECS_SPECS_BULLETONE_COPY = "SPECS_SPECS_BULLETONE_COPY";
            languageFile.SPECS_SPECS_BULLETONE_LEGAL = "SPECS_SPECS_BULLETONE_LEGAL";
            languageFile.SPECS_SPECS_BULLETTWO_TITLE = "SPECS_SPECS_BULLETTWO_TITLE";
            languageFile.SPECS_SPECS_BULLETTWO_COPY = "SPECS_SPECS_BULLETTWO_COPY";
            languageFile.SPECS_SPECS_BULLETTWO_LEGAL = "SPECS_SPECS_BULLETTWO_LEGAL";
            languageFile.SPECS_SPECS_BULLETTHREE_TITLE = "SPECS_SPECS_BULLETTHREE_TITLE";
            languageFile.SPECS_SPECS_BULLETTHREE_COPY = "SPECS_SPECS_BULLETTHREE_COPY";
            languageFile.SPECS_SPECS_BULLETTHREE_LEGAL = "SPECS_SPECS_BULLETTHREE_LEGAL";
            languageFile.SPECS_SPECS_BULLETFOUR_TITLE = "SPECS_SPECS_BULLETFOUR_TITLE";
            languageFile.SPECS_SPECS_BULLETFOUR_COPY = "SPECS_SPECS_BULLETFOUR_COPY";
            languageFile.SPECS_SPECS_BULLETFOUR_LEGAL = "SPECS_SPECS_BULLETFOUR_LEGAL";
            languageFile.SPECS_SPECS_BULLETFIVE_TITLE = "SPECS_SPECS_BULLETFIVE_TITLE";
            languageFile.SPECS_SPECS_BULLETFIVE_COPY = "SPECS_SPECS_BULLETFIVE_COPY";
            languageFile.SPECS_SPECS_BULLETFIVE_LEGAL = "SPECS_SPECS_BULLETFIVE_LEGAL";
            languageFile.SPECS_SPECS_BULLETSIX_TITLE = "SPECS_SPECS_BULLETSIX_TITLE";
            languageFile.SPECS_SPECS_BULLETSIX_COPY = "SPECS_SPECS_BULLETSIX_COPY";
            languageFile.SPECS_SPECS_BULLETSIX_LEGAL = "SPECS_SPECS_BULLETSIX_LEGAL";
            languageFile.SPECS_SPECS_BULLETSEVEN_TITLE = "SPECS_SPECS_BULLETSEVEN_TITLE";
            languageFile.SPECS_SPECS_BULLETSEVEN_COPY = "SPECS_SPECS_BULLETSEVEN_COPY";
            languageFile.SPECS_SPECS_BULLETSEVEN_LEGAL = "SPECS_SPECS_BULLETSEVEN_LEGAL";
            languageFile.SPECS_WITB_HEADLINE = "SPECS_WITB_HEADLINE";
            languageFile.SPECS_WITB_BULLETONE_TITLE = "SPECS_WITB_BULLETONE_TITLE";
            languageFile.SPECS_WITB_BULLETTWO_TITLE = "SPECS_WITB_BULLETTWO_TITLE";
            languageFile.SPECS_WITB_BULLETTHREE_TITLE = "SPECS_WITB_BULLETTHREE_TITLE";
            languageFile.SPECS_WITB_BULLETFOUR_TITLE = "SPECS_WITB_BULLETFOUR_TITLE";
            languageFile.PROMO_PROMO_HEADLINE = "PROMO_PROMO_HEADLINE";
            languageFile.PROMO_PROMO_SUBHEAD = "PROMO_PROMO_SUBHEAD";
            languageFile.PROMO_PROMO_COPY = "PROMO_PROMO_COPY";
            languageFile.PROMO_PROMO_LEGAL = "PROMO_PROMO_LEGAL";

            return languageFile;
        }

        #endregion

    }
}