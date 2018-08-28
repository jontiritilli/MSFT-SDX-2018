﻿using System;

using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceHeroPage : Page, INavigate
    {
        #region Private Members

        private ExperienceHeroViewModel ViewModel
        {
            get { return DataContext as ExperienceHeroViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceHeroPage()
        {
            InitializeComponent();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        #endregion
    }
}
