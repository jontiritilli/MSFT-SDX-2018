using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

using SDX.Toolkit.Controls;


namespace SDX.Toolkit.Models
{
    public class NavigationSection : INavigationItem
    {
        #region Private Members

        private string _name;
        private string _text;
        private int _order;

        #endregion


        #region Public Properties

        public List<INavigationItem> Items = new List<INavigationItem>();
        public Button UIButton = null;
        public TextBlockEx UIText = null;

        #endregion


        #region INavigationItem Properties

        public string Name { get => _name; set => _name = value; }
        public string Text { get => _text; set => _text = value; }
        public int Order { get => _order; set => _order = value; }

        #endregion


        #region Public Methods

        public int GetItemIndex(INavigationItem item)
        {
            return this.Items.IndexOf(item);
        }

        public int GetPageIndex(NavigationPage page)
        {
            int pageIndex = -1;
            int flipViewIndex = 0;

            // let's try to get lucky by finding the page
            // as a direct member of Items
            pageIndex = this.Items.IndexOf((INavigationItem)page);

            // if we didn't find it
            if (-1 == pageIndex)
            {
                // loop through the Items collection
                foreach (INavigationItem item in this.Items)
                {
                    // if the item is a flipview
                    if (item is NavigationFlipView flipView)
                    {
                        // look for the page in the flipview
                        int z = flipView.Items.IndexOf((INavigationItem)page);

                        // if we found it
                        if (-1 != z)
                        {
                            // the page index is the flipview index
                            pageIndex = flipViewIndex;

                            // break out of the foreach
                            break;
                        }
                    }

                    // increment the index
                    flipViewIndex++;
                }
            }

            return pageIndex;
        }

        #endregion
    }
}
