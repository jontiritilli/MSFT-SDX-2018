using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Media.Core;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Controls;


namespace SDX.Toolkit.Helpers
{
    public class EventHelper
    {
        public BottomNavBar.NavigateEvent BottomNavBar_Navigation { get; set; }
        public AttractorLoopPlayer.InteractedEvent AttractorLoopPlayer_Interaction { get; set; }
        public AttractorLoopPlayer.ReadyEvent AttractorLoopPlayer_Ready { get; set; }
        public RadiatingButton.ClickedEvent RadiatingButton_Clicked { get; set; }
        public EventHandler<object> FadeInCompletedHandler { get; set; }
        public EventHandler<object> FadeOutCompletedHandler { get; set; }
        public SurfaceCarousel.SelectionChangedEvent SurfaceCarousel_SelectionChanged { get; set; }

        public EventHelper()
        {

        }

        public EventHelper(BottomNavBar.NavigateEvent navigationEvent)
        {
            if (null != navigationEvent)
            {
                this.BottomNavBar_Navigation = navigationEvent;
            }
        }

        public EventHelper(AttractorLoopPlayer.InteractedEvent interactedEvent, AttractorLoopPlayer.ReadyEvent readyEvent)
        {
            if (null != interactedEvent)
            {
                this.AttractorLoopPlayer_Interaction = interactedEvent;
            }

            if (null != readyEvent)
            {
                this.AttractorLoopPlayer_Ready = readyEvent;
            }
        }

        public EventHelper(EventHandler<object> fadeInCompletedHandler, EventHandler<object> fadeOutCompletedHandler)
        {
            if (null != fadeInCompletedHandler)
            {
                this.FadeInCompletedHandler = fadeInCompletedHandler;
            }

            if (null != fadeOutCompletedHandler)
            {
                this.FadeOutCompletedHandler = fadeOutCompletedHandler;
            }
        }

        public EventHelper(SurfaceCarousel.SelectionChangedEvent selectionChangedEvent)
        {
            if (null != selectionChangedEvent)
            {
                this.SurfaceCarousel_SelectionChanged = selectionChangedEvent;
            }
        }
    }
}
