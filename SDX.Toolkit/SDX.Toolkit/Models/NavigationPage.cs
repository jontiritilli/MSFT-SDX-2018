﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace SDX.Toolkit.Models
{
    public class NavigationPage : INavigationItem
    {
        #region Private Members

        private string _name;
        private string _text;
        private int _order;
        private NavigationSection _section;
        #endregion


        #region Public Properties


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
            return this;
        }

        public INavigationItem GetLastPageChild()
        {
            return this;
        }

        #endregion
    }
}
