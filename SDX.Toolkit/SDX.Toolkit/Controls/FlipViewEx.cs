using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SDX.Toolkit.Controls
{
    public sealed class FlipViewEx : FlipView
    {
        public FlipViewEx()
        {
            this.DefaultStyleKey = typeof(FlipViewEx);

            // no focus visual indications
            this.UseSystemFocusVisuals = false;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new FlipViewItemEx();
        }
    }

    public sealed class FlipViewItemEx : FlipViewItem
    {
        public FlipViewItemEx()
        {
            this.DefaultStyleKey = typeof(FlipViewItemEx);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if ((e.Key == VirtualKey.Left) || (e.Key == VirtualKey.Right) || (e.Key == VirtualKey.Up) || (e.Key == VirtualKey.Down))
            {
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        public object GetChildViewAsObject()
        {
            object child = null;

            // get our content
            if (null != this.Content)
            {
                // our immediate content is a frame
                if (this.Content is Frame frame)
                {
                    child = frame.Content;
                }
            }

            return child;
        }
    }
}
