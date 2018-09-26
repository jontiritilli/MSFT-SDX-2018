using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Controls;


namespace SDX.Toolkit.Models
{
    public class NavigationSection
    {
        #region Private Members

        #endregion


        #region Public Properties

        public string Name;
        public string Text;
        public int Order;
        public Button UIButton = null;
        public TextBlockEx UIText = null;

        #endregion


        #region Public Methods

        //public int GetItemIndex(INavigationItem item)
        //{
        //    return this.Items.IndexOf(item);
        //}

        //public int GetPageIndex(NavigationPage page)
        //{
        //    int pageIndex = -1;
        //    int flipViewIndex = 0;

        //    // let's try to get lucky by finding the page
        //    // as a direct member of Items
        //    pageIndex = this.Items.IndexOf((INavigationItem)page);

        //    // if we didn't find it
        //    if (-1 == pageIndex)
        //    {
        //        // loop through the Items collection
        //        foreach (INavigationItem item in this.Items)
        //        {
        //            // if the item is a flipview
        //            if (item is NavigationFlipView flipView)
        //            {
        //                // look for the page in the flipview
        //                int z = flipView.Items.IndexOf((INavigationItem)page);

        //                // if we found it
        //                if (-1 != z)
        //                {
        //                    // the page index is the flipview index
        //                    pageIndex = flipViewIndex;

        //                    // break out of the foreach
        //                    break;
        //                }
        //            }

        //            // increment the index
        //            flipViewIndex++;
        //        }
        //    }

        //    return pageIndex;
        //}

        #endregion
    }
}
