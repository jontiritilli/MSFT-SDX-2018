using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace SDX.Toolkit.Helpers
{
    public enum TextStyles
    {
        Hero,
        Swipe,
        PageHeadline,
        PageHeadlineDark,
        PageLede,
        PageLedeDark,
        ListHeadline,
        ListLede,
        ListItemHeadlinePenTouch,
        ListItemLedePenTouch,
        ListItemHeadlineBestOf,
        ListItemLedeBestOf,
        ListItemHeadlineCompare,
        ListItemLedeCompare,
        ListItemCTAText,
        PopupHeadline,
        PopupLede,
        PopupBatteryLife,
        SliderBatteryLife,
        Legal,
        TryIt,
        ButtonCaption,
        NavigationSection,
        NavigationSectionActive,
        PlayerArtistName,
        PlayerTrackName,
    }

    public enum LayoutSizes
    {
        LeftMargin,
        TopMargin,
        RightMargin,
        BottomMargin,
        SpaceBelowHeader,
        AppCloseWidth,
        AppCloseHeight,
        HeaderWidth,
        HeaderSpacerRowHeightPageHeader,
        HeaderSpacerRowHeightListHeader,
        HeaderSpacerRowHeightListItemHeaderPenTouch,
        HeaderSpacerRowHeightListItemHeaderBestOf,
        HeaderSpacerRowHeightListItemHeaderCompare,
        HeaderSpacerRowHeightPopupHeader,
        AccessoriesPenListIconWidth,
        BestOfMicrosoftListIconWidth,
        CompareListIconWidth,
        AccessoryColorSelectorIconWidth,
        AccessoryPrimaryImageWidth,
        AccessoryPrimaryImageHeight,
        SwipeLeftEllipseRadius,
        SwipeLeftSpacer,
        SwipeToContinueArrowWidth,
        SwipeToContinueSpacer,
        PopupDefaultWidth,
        PopupSpacer,
        CanvasWidth,
        CanvasHeight,
        NavigationBarHeight,
        NavigationBarLineHeight,
        NavigationBarMargin,
        NavigationBarSpacer,
        NavigationBarWidthArrow,
        NavigationBarWidthHome,
        TryItBoxHeight,
        TryItBoxWidth,
        TryItPathHeight,
        TryItPathWidth,
        TryItIconHeight,
        RadiatingButtonIconHeight,
        RadiatingButtonEllipseRadius,
        RadiatingButtonGridWidth,
        RadiatingButtonEllipseTopSpacer,
        RadiatingButtonEllipseBottomSpacer,
        RadiatingButtonCaptionHeight,
        ColoringBookButtonWidth,
        ColoringBookButtonHeight,
        AppSelectorButtonWidth,
        AppSelectorButtonHeight,
        PlayerHeight,
        PlayerLeftMargin,
        PlayerRightMargin,
        PlayerTrackSpacer,
        PlayerButtonWidth,
        PlayerButtonSpacer,
        PlayerScrubHeight,
        PlayerScrubWidth,
    }

    public enum LayoutThicknesses
    {
        HeroMargin,
        PopupBorder,
        PopupMargin
    }

    public enum ColoringBookColors
    {
        Red,
        Blue,
        Teal,
        Orange,
        Purple
    }

    public enum BitmapImages
    {
        ColoringBookImage,
        ColoringBookColorRed,
        ColoringBookColorRedActive,
        ColoringBookColorBlue,
        ColoringBookColorBlueActive,
        ColoringBookColorTeal,
        ColoringBookColorTealActive,
        ColoringBookColorOrange,
        ColoringBookColorOrangeActive,
        ColoringBookColorPurple,
        ColoringBookColorPurpleActive,
        ColoringBookReset,
        PagePopupImage_1,
        PagePopupImage_2,
        PagePopupImage_3,
        PagePopupImage_4,
        PagePopupImage_5,
        PagePopupAppIcon_1,
        PagePopupAppIcon_2,
        PagePopupAppIcon_3,
        PagePopupAppIcon_4,
        PagePopupAppIcon_5,
        PagePopupAppIcon_Selected_1,
        PagePopupAppIcon_Selected_2,
        PagePopupAppIcon_Selected_3,
        PagePopupAppIcon_Selected_4,
        PagePopupAppIcon_Selected_5
    }

    public static class StyleHelper
    {
        #region Style Constants

        public const string STYLE_HERO = "Hero";
        public const string STYLE_SWIPE = "Swipe";
        public const string STYLE_PAGE_HEADLINE = "PageHeadline";
        public const string STYLE_PAGE_HEADLINE_DARK = "PageHeadlineDark";
        public const string STYLE_PAGE_LEDE = "PageLede";
        public const string STYLE_PAGE_LEDE_DARK = "PageLedeDark";
        public const string STYLE_LIST_HEADLINE = "ListHeadline";
        public const string STYLE_LIST_LEDE = "ListLede";
        public const string STYLE_LISTITEM_HEADLINE_PENTOUCH = "ListItemHeadlinePenTouch";
        public const string STYLE_LISTITEM_LEDE_PENTOUCH = "ListItemLedePenTouch";
        public const string STYLE_LISTITEM_HEADLINE_BESTOF = "ListItemHeadlineBestOf";
        public const string STYLE_LISTITEM_LEDE_BESTOF = "ListItemLedeBestOf";
        public const string STYLE_LISTITEM_HEADLINE_COMPARE = "ListItemHeadlineCompare";
        public const string STYLE_LISTITEM_LEDE_COMPARE = "ListItemLedeCompare";
        public const string STYLE_LISTITEM_CTATEXT = "ListItemCTAText";
        public const string STYLE_POPUP_HEADLINE = "PopupHeadline";
        public const string STYLE_POPUP_LEDE = "PopupLede";
        public const string STYLE_POPUP_BATTERYLIFE = "PopupBatteryLife";
        public const string STYLE_SLIDER_BATTERYLIFE = "SliderBatteryLife";
        public const string STYLE_LEGAL = "Legal";
        public const string STYLE_TRYIT = "TryIt";
        public const string STYLE_BUTTONCAPTION = "ButtonCaption";
        public const string STYLE_NAVIGATION_SECTION = "NavigationSection";
        public const string STYLE_NAVIGATION_SECTION_ACTIVE = "NavigationSectionActive";
        public const string STYLE_PLAYER_ARTISTNAME = "PlayerArtistName";
        public const string STYLE_PLAYER_TRACKNAME = "PlayerTrackName";

        #endregion


        #region Size Constants

        public const string SIZE_LEFT_MARGIN = "LeftMargin";
        public const string SIZE_TOP_MARGIN = "TopMargin";
        public const string SIZE_RIGHT_MARGIN = "RightMargin";
        public const string SIZE_BOTTOM_MARGIN = "BottomMargin";

        public const string SIZE_SPACE_BELOW_HEADER = "SpaceBelowHeader";

        public const string SIZE_APPCLOSE_WIDTH = "AppCloseWidth";
        public const string SIZE_APPCLOSE_HEIGHT = "AppCloseHeight";

        public const string SIZE_HEADER_WIDTH = "HeaderWidth";
        public const string SIZE_HEADER_SPACER_ROW_HEIGHT_PAGEHEADER = "HeaderSpacerRowHeightPageHeader";
        public const string SIZE_HEADER_SPACER_ROW_HEIGHT_LISTHEADER = "HeaderSpacerRowHeightListHeader";
        public const string SIZE_HEADER_SPACER_ROW_HEIGHT_LISTITEMHEADER_PENTOUCH = "HeaderSpacerRowHeightListHeaderPenTouch";
        public const string SIZE_HEADER_SPACER_ROW_HEIGHT_LISTITEMHEADER_BESTOF = "HeaderSpacerRowHeightListHeaderBestOf";
        public const string SIZE_HEADER_SPACER_ROW_HEIGHT_LISTITEMHEADER_COMPARE = "HeaderSpacerRowHeightListHeaderCompare";
        public const string SIZE_HEADER_SPACER_ROW_HEIGHT_POPUPHEADER = "HeaderSpacerRowHeightPopupHeader";

        public const string SIZE_ACCESSORIESPEN_LISTICONWIDTH = "AccessoriesPenListIconWidth";
        public const string SIZE_BESTOFMICROSOFT_LISTICONWIDTH = "BestOfMicrosoftListIconWidth";
        public const string SIZE_COMPARE_LISTICONWIDTH = "CompareListIconWidth";

        public const string SIZE_ACCESSORIES_COLORICONWIDTH = "AccessoryColorSelectorIconWidth";

        public const string SIZE_ACCESSORIES_PRIMARYIMAGEWIDTH = "AccessoryPrimaryImageWidth";
        public const string SIZE_ACCESSORIES_PRIMARYIMAGEHEIGHT = "AccessoryPrimaryImageHeight";

        public const string SIZE_SWIPELEFT_ELLIPSE_RADIUS = "SwipeLeftEllipseRadius";
        public const string SIZE_SWIPELEFT_SPACER = "SwipeLeftSpacer";

        public const string SIZE_SWIPETOCONTINUE_ARROW_WIDTH = "SwipeToContinueArrowWidth";
        public const string SIZE_SWIPETOCONTINUE_SPACER = "SwipeToContinueSpacer";

        public const string SIZE_POPUP_DEFAULT_WIDTH = "PopupDefaultWidth";
        public const string SIZE_POPUP_SPACER = "PopupSpacer";

        public const string SIZE_CANVAS_WIDTH = "CanvasWidth";
        public const string SIZE_CANVAS_HEIGHT = "CanvasHeight";

        public const string SIZE_NAVIGATIONBAR_HEIGHT = "NavigationBarHeight";
        public const string SIZE_NAVIGATIONBAR_LINEHEIGHT = "NavigationBarLineHeight";
        public const string SIZE_NAVIGATIONBAR_MARGIN = "NavigationBarMargin";
        public const string SIZE_NAVIGATIONBAR_SPACER = "NavigationBarSpacer";
        public const string SIZE_NAVIGATIONBAR_WIDTH_ARROW = "NavigationBarArrowWidth";
        public const string SIZE_NAVIGATIONBAR_WIDTH_HOME = "NavigationBarHomeWidth";

        public const string SIZE_TRYIT_BOX_HEIGHT = "TryItBoxHeight"; 
        public const string SIZE_TRYIT_BOX_WIDTH = "TryItBoxWidth";
        public const string SIZE_TRYIT_PATH_HEIGHT = "TryItPathHeight";
        public const string SIZE_TRYIT_PATH_WIDTH = "TryItPathWidth";
        public const string SIZE_TRYIT_ICON_HEIGHT = "TryItIconHeight";

        public const string SIZE_RADIATING_BUTTON_ICON_HEIGHT = "RadiatingButtonIconHeight";
        public const string SIZE_RADIATING_BUTTON_ELLIPSE_RADIUS = "RadiatingButtonEllipseRadius";
        public const string SIZE_RADIATING_BUTTON_GRID_WIDTH = "RadiatingButtonGridWidth";
        public const string SIZE_RADIATING_BUTTON_TOP_SPACER_HEIGHT = "RadiatingButtonEllipseTopSpacer";
        public const string SIZE_RADIATING_BUTTON_BOTTOM_SPACER_HEIGHT = "RadiatingButtonEllipseBottomSpacer";
        public const string SIZE_RADIATING_BUTTON_CAPTION_HEIGHT = "RadiatingButtonCaptionHeight";

        public const string SIZE_COLORINGBOOKBUTTONWIDTH = "ColoringBookButtonWidth";
        public const string SIZE_COLORINGBOOKBUTTONHEIGHT= "ColoringBookButtonHeight";

        public const string SIZE_APPSELECTORBUTTONWIDTH = "AppSelectorButtonWidth";
        public const string SIZE_APPSELECTORBUTTONHEIGHT = "AppSelectorButtonHeight";
        public const string SIZE_PLAYER_HEIGHT = "PlayerHeight";
        public const string SIZE_PLAYER_LEFTMARGIN = "PlayerLeftMargin";
        public const string SIZE_PLAYER_RIGHTMARGIN = "PlayerRightMargin";
        public const string SIZE_PLAYER_TRACKSPACER = "PlayerTrackSpacer";
        public const string SIZE_PLAYER_BUTTONWIDTH = "PlayerButtonWidth";
        public const string SIZE_PLAYER_BUTTONSPACER = "PlayerButtonSpacer";
        public const string SIZE_PLAYER_SCRUBHEIGHT = "PlayerScrubHeight";
        public const string SIZE_PLAYER_SCRUBWIDTH = "PlayerScrubWidth";

        #endregion


        #region Color Constants
        public const string RED = "ColoringBookColorRed";
        public const string BLUE = "ColoringBookColorBlue";
        public const string TEAL = "ColoringBookColorTeal";
        public const string ORANGE = "ColoringBookColorOrange";
        public const string PURPLE = "ColoringBookColorPurple";
        #endregion


        #region Image Constants
        public const string BITMAPIMAGE_COLORINGBOOK = "ColoringBookBitmapImage";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_RESET = "BitmapImageColoringBookReset";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_RED = "BitmapImageColoringBookRed";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_BLUE = "BitmapImageColoringBookBlue";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_TEAL = "BitmapImageColoringBookTeal";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_ORANGE = "BitmapImageColoringBookOrange";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_PURPLE = "BitmapImageColoringBookPurple";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_RED_ACTIVE = "BitmapImageColoringBookRedActive";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_BLUE_ACTIVE = "BitmapImageColoringBookBlueActive";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_TEAL_ACTIVE = "BitmapImageColoringBookTealActive";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_ORANGE_ACTIVE = "BitmapImageColoringBookOrangeActive";
        public const string BITMAPIMAGE_COLORINGBOOK_COLOR_PURPLE_ACTIVE = "BitmapImageColoringBookPurpleActive";

        public const string BITMAPIMAGE_PAGEPOPUP_1 = "BitmapImagePagePopUp_1";
        public const string BITMAPIMAGE_PAGEPOPUP_2 = "BitmapImagePagePopUp_2";
        public const string BITMAPIMAGE_PAGEPOPUP_3 = "BitmapImagePagePopUp_3";
        public const string BITMAPIMAGE_PAGEPOPUP_4 = "BitmapImagePagePopUp_4";
        public const string BITMAPIMAGE_PAGEPOPUP_5 = "BitmapImagePagePopUp_5";

        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_1 = "BitmapImagePagePopup_AppIcon_1";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_2 = "BitmapImagePagePopup_AppIcon_2";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_3 = "BitmapImagePagePopup_AppIcon_3";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_4 = "BitmapImagePagePopup_AppIcon_4";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_5 = "BitmapImagePagePopup_AppIcon_5";

        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_1 = "BitmapImagePagePopup_AppIcon_Selected_1";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_2 = "BitmapImagePagePopup_AppIcon_Selected_2";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_3 = "BitmapImagePagePopup_AppIcon_Selected_3";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_4 = "BitmapImagePagePopup_AppIcon_Selected_4";
        public const string BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_5 = "BitmapImagePagePopup_AppIcon_Selected_5";


        #endregion


        #region Thickness Constants

        public const string THICKNESS_HERO_MARGIN = "HeroMargin";
        public const string THICKNESS_POPUP_BORDER = "PopupBorder";
        public const string THICKNESS_POPUP_MARGIN = "PopupMargin";

        #endregion


        #region Size Methods

        public static double GetApplicationDouble(LayoutSizes size)
        {
            double value = -1;

            switch (size)
            {
                case LayoutSizes.LeftMargin:
                    value = GetApplicationDouble(SIZE_LEFT_MARGIN);
                    break;

                case LayoutSizes.TopMargin:
                    value = GetApplicationDouble(SIZE_TOP_MARGIN);
                    break;

                case LayoutSizes.RightMargin:
                    value = GetApplicationDouble(SIZE_RIGHT_MARGIN);
                    break;

                case LayoutSizes.BottomMargin:
                    value = GetApplicationDouble(SIZE_BOTTOM_MARGIN);
                    break;

                case LayoutSizes.SpaceBelowHeader:
                    value = GetApplicationDouble(SIZE_SPACE_BELOW_HEADER);
                    break;

                case LayoutSizes.HeaderWidth:
                    value = GetApplicationDouble(SIZE_HEADER_WIDTH);
                    break;

                case LayoutSizes.AppCloseWidth:
                    value = GetApplicationDouble(SIZE_APPCLOSE_WIDTH);
                    break;

                case LayoutSizes.AppCloseHeight:
                    value = GetApplicationDouble(SIZE_APPCLOSE_HEIGHT);
                    break;

                case LayoutSizes.HeaderSpacerRowHeightPageHeader:
                    value = GetApplicationDouble(SIZE_HEADER_SPACER_ROW_HEIGHT_PAGEHEADER);
                    break;

                case LayoutSizes.HeaderSpacerRowHeightListHeader:
                    value = GetApplicationDouble(SIZE_HEADER_SPACER_ROW_HEIGHT_LISTHEADER);
                    break;

                case LayoutSizes.HeaderSpacerRowHeightListItemHeaderPenTouch:
                    value = GetApplicationDouble(SIZE_HEADER_SPACER_ROW_HEIGHT_LISTITEMHEADER_PENTOUCH);
                    break;

                case LayoutSizes.HeaderSpacerRowHeightListItemHeaderBestOf:
                    value = GetApplicationDouble(SIZE_HEADER_SPACER_ROW_HEIGHT_LISTITEMHEADER_BESTOF);
                    break;

                case LayoutSizes.HeaderSpacerRowHeightListItemHeaderCompare:
                    value = GetApplicationDouble(SIZE_HEADER_SPACER_ROW_HEIGHT_LISTITEMHEADER_COMPARE);
                    break;

                case LayoutSizes.HeaderSpacerRowHeightPopupHeader:
                    value = GetApplicationDouble(SIZE_HEADER_SPACER_ROW_HEIGHT_POPUPHEADER);
                    break;

                case LayoutSizes.AccessoriesPenListIconWidth:
                    value = GetApplicationDouble(SIZE_ACCESSORIESPEN_LISTICONWIDTH);
                    break;

                case LayoutSizes.BestOfMicrosoftListIconWidth:
                    value = GetApplicationDouble(SIZE_BESTOFMICROSOFT_LISTICONWIDTH);
                    break;

                case LayoutSizes.CompareListIconWidth:
                    value = GetApplicationDouble(SIZE_COMPARE_LISTICONWIDTH);
                    break;

                case LayoutSizes.AccessoryColorSelectorIconWidth:
                    value = GetApplicationDouble(SIZE_ACCESSORIES_COLORICONWIDTH);
                    break;

                case LayoutSizes.AccessoryPrimaryImageWidth:
                    value = GetApplicationDouble(SIZE_ACCESSORIES_PRIMARYIMAGEWIDTH);
                    break;

                case LayoutSizes.AccessoryPrimaryImageHeight:
                    value = GetApplicationDouble(SIZE_ACCESSORIES_PRIMARYIMAGEHEIGHT);
                    break;

                case LayoutSizes.SwipeLeftEllipseRadius:
                    value = GetApplicationDouble(SIZE_SWIPELEFT_ELLIPSE_RADIUS);
                    break;

                case LayoutSizes.SwipeLeftSpacer:
                    value = GetApplicationDouble(SIZE_SWIPELEFT_SPACER);
                    break;

                case LayoutSizes.SwipeToContinueArrowWidth:
                    value = GetApplicationDouble(SIZE_SWIPETOCONTINUE_ARROW_WIDTH);
                    break;

                case LayoutSizes.SwipeToContinueSpacer:
                    value = GetApplicationDouble(SIZE_SWIPETOCONTINUE_SPACER);
                    break;

                case LayoutSizes.PopupDefaultWidth:
                    value = GetApplicationDouble(SIZE_POPUP_DEFAULT_WIDTH);
                    break;

                case LayoutSizes.PopupSpacer:
                    value = GetApplicationDouble(SIZE_POPUP_SPACER);
                    break;

                case LayoutSizes.CanvasWidth:
                    value = GetApplicationDouble(SIZE_CANVAS_WIDTH);
                    break;

                case LayoutSizes.CanvasHeight:
                    value = GetApplicationDouble(SIZE_CANVAS_HEIGHT);
                    break;

                case LayoutSizes.NavigationBarHeight:
                    value = GetApplicationDouble(SIZE_NAVIGATIONBAR_HEIGHT);
                    break;

                case LayoutSizes.NavigationBarLineHeight:
                    value = GetApplicationDouble(SIZE_NAVIGATIONBAR_LINEHEIGHT);
                    break;

                case LayoutSizes.NavigationBarMargin:
                    value = GetApplicationDouble(SIZE_NAVIGATIONBAR_MARGIN);
                    break;

                case LayoutSizes.NavigationBarSpacer:
                    value = GetApplicationDouble(SIZE_NAVIGATIONBAR_SPACER);
                    break;

                case LayoutSizes.NavigationBarWidthArrow:
                    value = GetApplicationDouble(SIZE_NAVIGATIONBAR_WIDTH_ARROW);
                    break;

                case LayoutSizes.NavigationBarWidthHome:
                    value = GetApplicationDouble(SIZE_NAVIGATIONBAR_WIDTH_HOME);
                    break;

                case LayoutSizes.TryItBoxHeight:
                    value = GetApplicationDouble(SIZE_TRYIT_BOX_HEIGHT);
                    break;
                    
                case LayoutSizes.TryItBoxWidth:
                    value = GetApplicationDouble(SIZE_TRYIT_BOX_WIDTH);
                    break;

                case LayoutSizes.TryItPathHeight:
                    value = GetApplicationDouble(SIZE_TRYIT_PATH_HEIGHT);
                    break;
                case LayoutSizes.TryItPathWidth:
                    value = GetApplicationDouble(SIZE_TRYIT_PATH_WIDTH);
                    break;

                case LayoutSizes.TryItIconHeight:
                    value = GetApplicationDouble(SIZE_TRYIT_ICON_HEIGHT);
                    break;

                case LayoutSizes.RadiatingButtonIconHeight:
                    value = GetApplicationDouble(SIZE_RADIATING_BUTTON_ICON_HEIGHT);
                    break;

                case LayoutSizes.RadiatingButtonEllipseRadius:
                    value = GetApplicationDouble(SIZE_RADIATING_BUTTON_ELLIPSE_RADIUS);
                    break;

                case LayoutSizes.RadiatingButtonGridWidth:
                    value = GetApplicationDouble(SIZE_RADIATING_BUTTON_GRID_WIDTH);
                    break;

                case LayoutSizes.RadiatingButtonEllipseTopSpacer:
                    value = GetApplicationDouble(SIZE_RADIATING_BUTTON_TOP_SPACER_HEIGHT);
                    break;

                case LayoutSizes.RadiatingButtonEllipseBottomSpacer:
                    value = GetApplicationDouble(SIZE_RADIATING_BUTTON_BOTTOM_SPACER_HEIGHT);
                    break;

                case LayoutSizes.RadiatingButtonCaptionHeight:
                    value = GetApplicationDouble(SIZE_RADIATING_BUTTON_CAPTION_HEIGHT);
                    break;

                case LayoutSizes.ColoringBookButtonWidth:
                    value = GetApplicationDouble(SIZE_COLORINGBOOKBUTTONWIDTH);
                    break;

                case LayoutSizes.ColoringBookButtonHeight:
                    value = GetApplicationDouble(SIZE_COLORINGBOOKBUTTONHEIGHT);
                    break;

                case LayoutSizes.AppSelectorButtonWidth:
                    value = GetApplicationDouble(SIZE_APPSELECTORBUTTONWIDTH);
                    break;

                case LayoutSizes.AppSelectorButtonHeight:
                    value = GetApplicationDouble(SIZE_APPSELECTORBUTTONHEIGHT);
                    break;

                case LayoutSizes.PlayerHeight:
                    value = GetApplicationDouble(SIZE_PLAYER_HEIGHT);
                    break;

                case LayoutSizes.PlayerLeftMargin:
                    value = GetApplicationDouble(SIZE_PLAYER_LEFTMARGIN);
                    break;

                case LayoutSizes.PlayerRightMargin:
                    value = GetApplicationDouble(SIZE_PLAYER_RIGHTMARGIN);
                    break;

                case LayoutSizes.PlayerTrackSpacer:
                    value = GetApplicationDouble(SIZE_PLAYER_TRACKSPACER);
                    break;

                case LayoutSizes.PlayerButtonSpacer:
                    value = GetApplicationDouble(SIZE_PLAYER_BUTTONSPACER);
                    break;

                case LayoutSizes.PlayerButtonWidth:
                    value = GetApplicationDouble(SIZE_PLAYER_BUTTONWIDTH);
                    break;

                case LayoutSizes.PlayerScrubHeight:
                    value = GetApplicationDouble(SIZE_PLAYER_SCRUBHEIGHT);
                    break;

                case LayoutSizes.PlayerScrubWidth:
                    value = GetApplicationDouble(SIZE_PLAYER_SCRUBWIDTH);
                    break;

                default:
                    break;
            }

            return value;
        }

        public static double GetApplicationDouble(string name)
        {
            double value = -1;

            try
            {
                value = (double)Application.Current.Resources[name];
            }
            catch (Exception)
            {

            }

            return value;
        }

        #endregion


        #region Style Methods

        public static Style GetApplicationStyle(TextStyles textStyle)
        {
            Style style = null;

            switch (textStyle)
            {
                case TextStyles.Hero:
                    style = GetApplicationStyle(STYLE_HERO);
                    break;

                case TextStyles.Swipe:
                    style = GetApplicationStyle(STYLE_SWIPE);
                    break;

                case TextStyles.PageHeadline:
                    style = GetApplicationStyle(STYLE_PAGE_HEADLINE);
                    break;

                case TextStyles.PageHeadlineDark:
                    style = GetApplicationStyle(STYLE_PAGE_HEADLINE_DARK);
                    break;

                case TextStyles.PageLede:
                    style = GetApplicationStyle(STYLE_PAGE_LEDE);
                    break;

                case TextStyles.PageLedeDark:
                    style = GetApplicationStyle(STYLE_PAGE_LEDE_DARK);
                    break;

                case TextStyles.ListHeadline:
                    style = GetApplicationStyle(STYLE_LIST_HEADLINE);
                    break;

                case TextStyles.ListLede:
                    style = GetApplicationStyle(STYLE_LIST_LEDE);
                    break;

                case TextStyles.ListItemHeadlinePenTouch:
                    style = GetApplicationStyle(STYLE_LISTITEM_HEADLINE_PENTOUCH);
                    break;

                case TextStyles.ListItemLedePenTouch:
                    style = GetApplicationStyle(STYLE_LISTITEM_LEDE_PENTOUCH);
                    break;

                case TextStyles.ListItemHeadlineBestOf:
                    style = GetApplicationStyle(STYLE_LISTITEM_HEADLINE_BESTOF);
                    break;

                case TextStyles.ListItemLedeBestOf:
                    style = GetApplicationStyle(STYLE_LISTITEM_LEDE_BESTOF);
                    break;

                case TextStyles.ListItemHeadlineCompare:
                    style = GetApplicationStyle(STYLE_LISTITEM_HEADLINE_COMPARE);
                    break;

                case TextStyles.ListItemLedeCompare:
                    style = GetApplicationStyle(STYLE_LISTITEM_LEDE_COMPARE);
                    break;

                case TextStyles.ListItemCTAText:
                    style = GetApplicationStyle(STYLE_LISTITEM_CTATEXT);
                    break;

                case TextStyles.PopupHeadline:
                    style = GetApplicationStyle(STYLE_POPUP_HEADLINE);
                    break;

                case TextStyles.PopupLede:
                    style = GetApplicationStyle(STYLE_POPUP_LEDE);
                    break;

                case TextStyles.PopupBatteryLife:
                    style = GetApplicationStyle(STYLE_POPUP_BATTERYLIFE);
                    break;

                case TextStyles.SliderBatteryLife:
                    style = GetApplicationStyle(STYLE_SLIDER_BATTERYLIFE);
                    break;

                case TextStyles.Legal:
                    style = GetApplicationStyle(STYLE_LEGAL);
                    break;

                case TextStyles.TryIt:
                    style = GetApplicationStyle(STYLE_TRYIT);
                    break;

                case TextStyles.ButtonCaption:
                    style = GetApplicationStyle(STYLE_BUTTONCAPTION);
                    break;

                case TextStyles.NavigationSection:
                    style = GetApplicationStyle(STYLE_NAVIGATION_SECTION);
                    break;

                case TextStyles.NavigationSectionActive:
                    style = GetApplicationStyle(STYLE_NAVIGATION_SECTION_ACTIVE);
                    break;

                case TextStyles.PlayerArtistName:
                    style = GetApplicationStyle(STYLE_PLAYER_ARTISTNAME);
                    break;

                case TextStyles.PlayerTrackName:
                    style = GetApplicationStyle(STYLE_PLAYER_TRACKNAME);
                    break;

                default:
                    break;
            }

            return style;
        }

        public static Style GetApplicationStyle(string name)
        {
            Style style = null;

            try
            {
                style = (Style)Application.Current.Resources[name];
            }
            catch (Exception)
            {

            }

            return style;
        }

        #endregion


        #region Color Methods

        public static Color GetApplicationColor(ColoringBookColors Color)
        {
            Color color = new Windows.UI.Color();
            switch (Color)
            {
                case ColoringBookColors.Red:
                    color = GetApplicationColor(RED);
                    break;

                case ColoringBookColors.Blue:
                    color = GetApplicationColor(BLUE);
                    break;

                case ColoringBookColors.Teal:
                    color = GetApplicationColor(TEAL);
                    break;

                case ColoringBookColors.Orange:
                    color = GetApplicationColor(ORANGE);
                    break;

                case ColoringBookColors.Purple:
                    color = GetApplicationColor(PURPLE);
                    break;

                default:
                    break;
            }
            return color;
        }

        public static Color GetApplicationColor(String name)
        {
            Color color = new Color();
            try
            {
                color = (Color)Application.Current.Resources[name];
            }
            catch
            {
            }

            return color;
        }
        #endregion


        #region Bitmap Methods

        public static BitmapImage GetApplicationBitmapImage(BitmapImages image)
        {
            BitmapImage bmImage = new BitmapImage();
            switch (image)
            {
                case BitmapImages.ColoringBookImage:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK);
                    break;
                    
                case BitmapImages.ColoringBookColorRed:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_RED);
                    break;

                case BitmapImages.ColoringBookColorBlue:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_BLUE);
                    break;

                case BitmapImages.ColoringBookColorTeal:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_TEAL);
                    break;

                case BitmapImages.ColoringBookColorOrange:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_ORANGE);
                    break;

                case BitmapImages.ColoringBookColorPurple:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_PURPLE);
                    break;

                case BitmapImages.ColoringBookColorRedActive:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_RED_ACTIVE);
                    break;

                case BitmapImages.ColoringBookColorBlueActive:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_BLUE_ACTIVE);
                    break;

                case BitmapImages.ColoringBookColorTealActive:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_TEAL_ACTIVE);
                    break;

                case BitmapImages.ColoringBookColorOrangeActive:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_ORANGE_ACTIVE);
                    break;

                case BitmapImages.ColoringBookColorPurpleActive:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_PURPLE_ACTIVE);
                    break;

                case BitmapImages.ColoringBookReset:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_COLORINGBOOK_COLOR_RESET);
                    break;

                case BitmapImages.PagePopupImage_1:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_1);
                    break;

                case BitmapImages.PagePopupImage_2:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_2);
                    break;

                case BitmapImages.PagePopupImage_3:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_3);
                    break;

                case BitmapImages.PagePopupImage_4:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_4);
                    break;

                case BitmapImages.PagePopupImage_5:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_5);
                    break;

                case BitmapImages.PagePopupAppIcon_1:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_1);
                    break;

                case BitmapImages.PagePopupAppIcon_2:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_2);
                    break;

                case BitmapImages.PagePopupAppIcon_3:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_3);
                    break;

                case BitmapImages.PagePopupAppIcon_4:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_4);
                    break;

                case BitmapImages.PagePopupAppIcon_5:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_5);
                    break;

                case BitmapImages.PagePopupAppIcon_Selected_1:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_1);
                    break;

                case BitmapImages.PagePopupAppIcon_Selected_2:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_2);
                    break;

                case BitmapImages.PagePopupAppIcon_Selected_3:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_3);
                    break;

                case BitmapImages.PagePopupAppIcon_Selected_4:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_4);
                    break;

                case BitmapImages.PagePopupAppIcon_Selected_5:
                    bmImage = GetApplicationBitmapImage(BITMAPIMAGE_PAGEPOPUP_APPICON_SELECTED_5);
                    break;
                default:
                    break;
            }
            return bmImage;
        }

        public static BitmapImage GetApplicationBitmapImage(String name)
        {
            BitmapImage bmImage = new BitmapImage();
            try
            {
                bmImage = (BitmapImage)Application.Current.Resources[name];
            }
            catch
            {
            }

            return bmImage;
        }
        #endregion


        #region Thickness Methods

        public static Thickness GetApplicationThickness(LayoutThicknesses layoutThickness)
        {
            Thickness thickness;

            switch (layoutThickness)
            {
                case LayoutThicknesses.HeroMargin:
                    GetApplicationThickness(THICKNESS_HERO_MARGIN);
                    break;

                case LayoutThicknesses.PopupBorder:
                    GetApplicationThickness(THICKNESS_POPUP_BORDER);
                    break;

                case LayoutThicknesses.PopupMargin:
                    GetApplicationThickness(THICKNESS_POPUP_MARGIN);
                    break;

                default:
                    break;
            }

            return thickness;
        }

        public static Thickness GetApplicationThickness(string name)
        {
            Thickness thickness;

            try
            {
                thickness = (Thickness)Application.Current.Resources[name];
            }
            catch (Exception)
            {

            }

            return thickness;
        }

        #endregion


        #region Helpers

        public static void SetRunStyleFromStyle(Run run, Style style)
        {
            if ((null != run) && (null != style))
            {
                // to easily get style values, must apply it to something
                TextBlock textBlock = new TextBlock()
                {
                    Style = style
                };

                // now copy relevant properties to the run
                run.FontFamily = textBlock.FontFamily;
                run.FontStyle = textBlock.FontStyle;
                run.FontWeight = textBlock.FontWeight;
                run.FontSize = textBlock.FontSize;
                run.Foreground = textBlock.Foreground;
            }
        }

        public static AcrylicBrush GetAcrylicBrush()
        {
            AcrylicBrush brush = new AcrylicBrush()
            {
                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                Opacity = 0.9,
                TintColor = Colors.Gray,
                TintOpacity = 0.4,
                FallbackColor = Colors.LightGray,
            };

            return brush;
        }

        #endregion
    }
}
