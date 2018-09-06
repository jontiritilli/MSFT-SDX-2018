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
        public string Name;
        public string Text;
        public int Order;
        public List<NavigationPage> Pages = new List<NavigationPage>();
        public Button UIButton = null;
        public TextBlockEx UIText = null;

        public int GetPageIndex(NavigationPage page)
        {
            return this.Pages.IndexOf(page);
        }
    }
}
