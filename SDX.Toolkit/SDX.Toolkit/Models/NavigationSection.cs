﻿using System;
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

        public int GetPageIndex(NavigationPage page)
        {
            return this.Items.IndexOf(page);
        }

        #endregion
    }
}
