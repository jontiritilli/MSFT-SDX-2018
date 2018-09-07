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
        NavigationSectionActive
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
        NavigationBarWidthHome
    }

    public enum LayoutThicknesses
    {
        HeroMargin,
        PopupBorder,
        PopupMargin
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
