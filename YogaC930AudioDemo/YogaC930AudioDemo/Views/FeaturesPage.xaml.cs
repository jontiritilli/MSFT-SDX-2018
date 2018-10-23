﻿using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using YogaC930AudioDemo.ViewModels;

namespace YogaC930AudioDemo.Views
{
    public sealed partial class FeaturesPage : Page
    {
        private FeaturesViewModel ViewModel
        {
            get { return DataContext as FeaturesViewModel; }
        }

        public FeaturesPage()
        {
            InitializeComponent();

            this.Loaded += this.FeaturesPage_Loaded;
        }

        private void FeaturesPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // disable the system back button
            SystemNavigationManager mgr = SystemNavigationManager.GetForCurrentView();
            if (null != mgr)
            {
                mgr.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
