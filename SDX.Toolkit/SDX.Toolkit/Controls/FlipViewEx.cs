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
        #region Dependency Properties

        // IsSwipeToNavigateEnabled
        public static readonly DependencyProperty IsSwipeToNavigateEnabledProperty =
            DependencyProperty.Register("IsSwipeToNavigateEnabled", typeof(bool), typeof(FlipViewEx), new PropertyMetadata(true, OnSwipeNavigationPropertyChanged));

        public bool IsSwipeToNavigateEnabled
        {
            get => (bool)GetValue(IsSwipeToNavigateEnabledProperty);
            set => SetValue(IsSwipeToNavigateEnabledProperty, value);
        }

        #endregion

        #region Event Handlers

        private static void OnSwipeNavigationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is FlipViewEx flipview)
            {
                flipview.ToggleSwipeToNavigate();
            }
        }

        private void ToggleSwipeToNavigate()
        {
            if (this.IsSwipeToNavigateEnabled)
            {
                foreach (FlipViewItemEx item in ((ItemCollection)this.Items))
                {
                    if ((Panel)((UserControl)((ContentControl)item.Content).Content).Content is Panel view)
                    {
                        view.ManipulationMode = ManipulationModes.System;
                    }
                }
            }
            else
            {
                foreach (FlipViewItemEx item in ((ItemCollection)this.Items))
                {
                    if ((Panel)((UserControl)((ContentControl)item.Content).Content).Content is Panel view)
                    {
                        view.ManipulationMode = ManipulationModes.None;
                    }
                }
            }
        }

        #endregion

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

        public int GetIndexOfChildView(object page)
        {
            int index = -1;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (page == ((FlipViewItemEx)this.Items[i]).GetChildViewAsObject())
                {
                    index = i;
                    break;
                }
            }

            return index;
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
