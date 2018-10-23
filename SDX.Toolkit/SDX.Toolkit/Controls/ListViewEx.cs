using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SDX.Toolkit.Helpers;

namespace SDX.Toolkit.Controls
{
    public class ListViewEx : ListView
    {
        public static readonly DependencyProperty HasAltRowsProperty = 
            DependencyProperty.Register("HasAltRows", typeof(Boolean), typeof(ListViewEx), new PropertyMetadata(false));

        public Boolean HasAltRows
        {
            get { return (Boolean)GetValue(HasAltRowsProperty); }
            set { SetValue(HasAltRowsProperty, value); }
        }
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var listViewItem = element as ListViewItem;
            if (listViewItem != null && this.HasAltRows)
            {
                var index = IndexFromContainer(element);

                if (index % 2 == 0)
                {
                    listViewItem.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Light);
                }
                else
                {
                    listViewItem.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Gray);
                }
            }

        }

    }
}
