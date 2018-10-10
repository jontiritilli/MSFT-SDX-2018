using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SDX.Toolkit.Controls
{
    public class AltRowListView : ListView
    {
        private Brush BackGroundWhiteAcrylic = new AcrylicBrush()
        {
            BackgroundSource = AcrylicBackgroundSource.Backdrop,
            Opacity = 0.77,
            TintColor = Colors.White,
            TintOpacity = 0.75,
            FallbackColor = Colors.White,
        };
        private Brush BackGroundGrayAcrylic = new AcrylicBrush()
        {
            BackgroundSource = AcrylicBackgroundSource.Backdrop,
            Opacity = 0.47,
            TintColor = Colors.White,
            TintOpacity = 0.45,
            FallbackColor = Colors.White,
        };
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var listViewItem = element as ListViewItem;
            if (listViewItem != null)
            {
                var index = IndexFromContainer(element);

                if (index % 2 == 0)
                {
                    listViewItem.Background = BackGroundWhiteAcrylic;
                }
                else
                {
                    listViewItem.Background = BackGroundGrayAcrylic;
                }
            }

        }

    }
}
