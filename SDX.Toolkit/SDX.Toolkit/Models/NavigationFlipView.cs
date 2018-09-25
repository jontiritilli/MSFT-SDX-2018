using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace SDX.Toolkit.Models
{
    public class NavigationFlipView : INavigationItem
    {
        #region Private Members

        private string _name;
        private string _text;
        private int _order;
        private NavigationSection _section;

        #endregion


        #region Public Properties

        public List<INavigationItem> Items = new List<INavigationItem>();
        public int SelectedIndex = 0;

        #endregion


        #region INavigationItem Properties

        public string Name { get => _name; set => _name = value; }
        public string Text { get => _text; set => _text = value; }
        public int Order { get => _order; set => _order = value; }
        public NavigationSection Section { get => _section; set => _section = value; }

        #endregion


        #region INavigationItem Methods

        public INavigationItem GetFirstPageChild()
        {
            INavigationItem firstChild = null;

            if ((null != this.Items) && (this.Items.Count > 0))
            {
                firstChild = this.Items.ElementAt(0)?.GetFirstPageChild();
            }

            return firstChild;
        }

        public INavigationItem GetLastPageChild()
        {
            INavigationItem lastChild = null;

            if ((null != this.Items) && (this.Items.Count > 0))
            {
                lastChild = this.Items.ElementAt(this.Items.Count() - 1)?.GetLastPageChild();
            }

            return lastChild;
        }

        #endregion


        #region Public Methods

        public int GetItemIndex(INavigationItem item)
        {
            return this.Items.IndexOf(item);
        }

        public bool HasChild(INavigationItem child)
        {
            bool found = false;

            if (null != child)
            {
                // try the simple approach first
                found = this.Items.Contains(child);

                // if we didn't find it
                if (!found)
                {
                    // loop through our items
                    foreach (INavigationItem childItem in this.Items)
                    {
                        // is the child a NavigationFlipView?
                        if (childItem is NavigationFlipView flipView)
                        {
                            // see if it has the child
                            found = flipView.HasChild(child);

                            // if we found it, break out
                            if (found) { break; }
                        }
                    }
                }
            }

            return found;
        }

        public bool SelectPage(NavigationPage page)
        {
            bool selectedPage = false;

            // if the page is not null and the page is our child
            if ((null != page) && (this.HasChild(page)))
            {
                // simple case is that this page is a direct child
                if (this.Items.Contains(page))
                {
                    this.SelectedIndex = this.Items.IndexOf(page);

                    selectedPage = true;
                }

                if (!selectedPage)
                {
                    // if we didn't find it
                    foreach (INavigationItem childItem in this.Items)
                    {
                        // is the child a NavigationFlipView?
                        if (childItem is NavigationFlipView flipView)
                        {
                            // try to set the page on this child (knowing the page might not be its child
                            selectedPage = flipView.SelectPage(page);

                            // if the set succeeded
                            if (selectedPage)
                            {
                                // set our selected index to the index of our child that contains the page
                                this.SelectedIndex = this.Items.IndexOf(childItem);

                                // leave the foreach loop
                                break;
                            }
                        }
                    }
                }
            }

            return selectedPage;
        }

        public NavigationPage FindPreviousPage()
        {
            NavigationPage previousPage = null;

            // if we can move back
            if (this.CanGoBack())
            {
                // get the currently selected item
                INavigationItem item = this.Items.ElementAt(this.SelectedIndex);

                // if that item is a flipview
                if (item is NavigationFlipView flipView)
                {
                    // can it go back?
                    if (flipView.CanGoBack())
                    {
                        // yes, it can, so go back, so let it
                        previousPage = flipView.FindPreviousPage();
                    }
                }

                // if previousPage is null, we know the item isn't 
                // a flipview that can go back. therefore, it is
                // either a flipview on its first child, or a page
                if (null == previousPage)
                {
                    // get the previous item
                    INavigationItem previousItem = this.Items.ElementAt(this.SelectedIndex - 1);

                    // find the last child of this previous item
                    previousPage = (NavigationPage)previousItem.GetLastPageChild();
                }
            }

            return previousPage;
        }

        public NavigationPage FindNextPage()
        {
            NavigationPage nextPage = null;

            // if we can move forward
            if (this.CanGoForward())
            {
                // get the currently selected item
                INavigationItem item = this.Items.ElementAt(this.SelectedIndex);

                // if that item is a flipview
                if (item is NavigationFlipView flipView)
                {
                    // can it go forward?
                    if (flipView.CanGoForward())
                    {
                        // yes, it can, so go forward, so let it
                        nextPage = flipView.FindNextPage();
                    }
                }

                // if nextPage is null, we know the item isn't 
                // a flipview that can go forward. therefore, it is
                // either a flipview on its last child, or a page
                if (null == nextPage)
                {
                    // get the next item
                    INavigationItem nextItem = this.Items.ElementAt(this.SelectedIndex + 1);

                    // find the first child of this next item
                    nextPage = (NavigationPage)nextItem.GetFirstPageChild();
                }
            }

            return nextPage;
        }

        public bool CanGoBack()
        {
            bool canGoBack = false;

            // if we have items
            if ((null != this.Items) && (this.Items.Count() > 0))
            {
                // if our current selected index is > 0, then we can go back
                if (this.SelectedIndex > 0)
                {
                    canGoBack = true;
                }
                else
                {
                    // we might still be able to go back if the 
                    // zero'th item is a flipview and it can go back
                    if (this.Items.ElementAt(0) is NavigationFlipView flipView)
                    {
                        canGoBack = flipView.CanGoBack();
                    }
                }
            }

            return canGoBack;
        }

        public bool CanGoForward()
        {
            bool canGoForward = false;

            // if we have items
            if ((null != this.Items) && (this.Items.Count() > 0))
            {
                // if our current selected index is < Count - 1, then we can go forward
                if (this.SelectedIndex < (this.Items.Count - 1))
                {
                    canGoForward = true;
                }
                else
                {
                    // ok, we're on the last item, but we might still 
                    // be able to go forward if the last item is a 
                    // flipview and it can go forward
                    if (this.Items.ElementAt(this.Items.Count - 1) is NavigationFlipView flipView)
                    {
                        canGoForward = flipView.CanGoForward();
                    }
                }
            }

            return canGoForward;
        }

        public bool HasChildrenWithSection(NavigationSection section)
        {
            // because INavigationItem objects contain a Section, and because NavigationFlipView 
            // children must be in the same section (UI design constraint), we can just search
            // our immediate children to find the given section
            return (bool)(this.Items.Where(item => item.Section.Equals(section)).Count() > 0);
        }

        public INavigationItem FindFirstChildWithSection(NavigationSection section)
        {
            // because INavigationItem objects contain a Section, and because NavigationFlipView 
            // children must be in the same section (UI design constraint), we can just search
            // our immediate children to find the given section. Note that we order-by 
            // INavigationItem.Order and take the first.
            return this.Items.Where(item => item.Section.Equals(section)).OrderBy(item => item.Order).First();
        }

        #endregion
    }
}
