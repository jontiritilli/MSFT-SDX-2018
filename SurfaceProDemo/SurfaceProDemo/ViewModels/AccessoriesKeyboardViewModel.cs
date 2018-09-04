﻿using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SDX.Toolkit.Controls;
using SurfaceProDemo.Services;
using Windows.UI;

namespace SurfaceProDemo.ViewModels
{
    public class AccessoriesKeyboardViewModel : ViewModelBase
    {
        #region Constants

        private const string URI_BACKGROUND = "ms:appx///Assets/Backgrounds/gradient-bg.jpg";

        private const string URI_IMAGESELECTOR_IMAGE_1 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_2 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_3 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";
        private const string URI_IMAGESELECTOR_IMAGE_4 = "ms-appx:///Assets/Accessories/Images/keyboard_left.png";

        private const string URI_APPSELECTOR_COLOR_1 = "ms-appx:///Assets/Icons/AccessoriesColors/cobalt_swatch.png";
        private const string URI_APPSELECTOR_COLOR_2 = "ms-appx:///Assets/Icons/AccessoriesColors/black_swatch.png";
        private const string URI_APPSELECTOR_COLOR_3 = "ms-appx:///Assets/Icons/AccessoriesColors/platinum_swatch.png";
        private const string URI_APPSELECTOR_COLOR_4 = "ms-appx:///Assets/Icons/AccessoriesColors/burgundy_swatch.png";        

        private const string URI_APPSELECTOR_COLOR_1_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/cobalt_swatch.png";
        private const string URI_APPSELECTOR_COLOR_2_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/black_swatch.png";
        private const string URI_APPSELECTOR_COLOR_3_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/platinum_swatch.png";
        private const string URI_APPSELECTOR_COLOR_4_SELECTED = "ms-appx:///Assets/Icons/AccessoriesColors/burgundy_swatch.png";

        private const int APPSELECTOR_BUTTON_WIDTH = 40;
        private const int APPSELECTOR_BUTTON_HEIGHT = 40;

        private const int SELECTORIMAGE_IMAGEHEIGHT = 648;
        private const int SELECTORIMAGE_IMAGEWIDTH = 764;
        #endregion


        #region Public Properties

        public string BackgroundUri = URI_BACKGROUND;        
        

        public int ImageSelectorImageWidth;
        public int ImageSelectorImageHeight;


        public string Headline;
        public string Lede;

        public string PopLeftHeadline;
        public string PopLeftLede;
        public string PopLeftLegal;
        public string PopTopHeadline;
        public string PopTopLede;
        public string PopTopLegal;
       
        public List<AppSelectorData> appSelectorData = new List<AppSelectorData>();
        public List<AppSelectorImageURI> appSelectorImageURIs = new List<AppSelectorImageURI>();

        public int ColoringBookButtonWidth;
        public int ColoringBookButtonHeight;

        public string ColoringBookClearButtonURI;
        public double ICON_WIDTH = 96d;

        #endregion


        #region Construction

        public AccessoriesKeyboardViewModel()
        {
            // get the localization service
            LocalizationService localizationService = SimpleIoc.Default.GetInstance<LocalizationService>();
            // set the header and lede and colors list for app selector
            // use the event to handle color changed
            this.ImageSelectorImageWidth = SELECTORIMAGE_IMAGEWIDTH;
            this.ImageSelectorImageHeight= SELECTORIMAGE_IMAGEHEIGHT;

            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_1,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_1_SELECTED
            });
            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_2,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_2_SELECTED
            });
            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_3,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_3_SELECTED
            });
            this.appSelectorData.Add(new AppSelectorData()
            {
                Source_NotSelectedImage = URI_APPSELECTOR_COLOR_4,
                Source_SelectedImage = URI_APPSELECTOR_COLOR_4_SELECTED                
            });

            this.appSelectorImageURIs.Add(new AppSelectorImageURI() {
                URI= URI_IMAGESELECTOR_IMAGE_1,
                Width= SELECTORIMAGE_IMAGEWIDTH
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_2,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_3,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            this.appSelectorImageURIs.Add(new AppSelectorImageURI()
            {
                URI = URI_IMAGESELECTOR_IMAGE_4,
                Width = SELECTORIMAGE_IMAGEWIDTH
            });
            // if we got it
            if (null != localizationService)
            {
                // load ourself with values from the language file
                localizationService.LoadAccessoriesKeyboardViewModel(this);
            }
        }
        private void _AppSelector_SelectedIDChanged(object sender, EventArgs e)
        {
            // need to change the mouse page too
            //if ((null != _AppSelector) && (null != _AppSelector) && (null != _AppSelector))
            //{
            //    AppSelector appSelector = (AppSelector)sender;
            //    if (appSelector.SelectedID > 0)
            //    {// this is the only case there this needs to manage since there is a clear button so account for it
            //        this._SelectedColor = this.Colors[appSelector.SelectedID - 1].Color;
            //    }
            //    //else
            //    //{// should it disable the color? or leave the selection on the previous one?
            //    //    _inkCanvas.InkPresenter.StrokeContainer.Clear();
            //    //}


            //    SetupBrush();
            //}
        }

        #endregion
    }
}
