using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Controls;

namespace SDX.Toolkit.Helpers
{
    public enum PopupTypes
    {
        Fullscreen,
        Text,
        Video,
        Image,
        Animation
    }

    public enum PopupTexts
    {
        None,
        Missing,
        Float_PixelSense,
        Float_Thin,
        Float_Light,
        Studio_LeftText,
        Gibson_Pen,
        Gibson_Keyboard,
        Gibson_Alcantara,
        Lexington_TouchPad,
        Lexington_Mouse
    }

    public static class PopupHelper
    {
        public static Popup CreatePopup(PopupTypes type, string text, double leftOffset, double topOffset, double width)
        {
            // create the popup
            Popup popup = new Popup()
            {
                IsOpen = false,
                IsLightDismissEnabled = true,
                HorizontalOffset = leftOffset,
                VerticalOffset = topOffset
            };

            switch (type)
            {
                case PopupTypes.Fullscreen:
                    break;

                case PopupTypes.Text:
                    PopupContentText popupText = new PopupContentText()
                    {
                        Text = text,
                        AutoStart = true
                    };
                    if (!Double.IsInfinity(width) && !double.IsNaN(width))
                    {
                        popupText.Width = width;
                    }
                    popup.Child = popupText;
                    break;

                case PopupTypes.Video:
                    // using ImageGallery and not creating it here
                    //popup.Child = new PopupContentImageGallery()
                    //{
                    //    Width = Window.Current.Bounds.Width,
                    //    Height = Window.Current.Bounds.Height
                    //};
                    break;

                case PopupTypes.Image:
                    break;

                case PopupTypes.Animation:
                    popup.Child = new PopupContentBatteryLife()
                    {
                        AutoStart = true
                    };
                    break;

                default:
                    break;
            }

            return popup;
        }
    }
}
