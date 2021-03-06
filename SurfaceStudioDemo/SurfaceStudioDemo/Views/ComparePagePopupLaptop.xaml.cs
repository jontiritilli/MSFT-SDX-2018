﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using SDX.Toolkit.Helpers;
using SurfaceStudioDemo.ViewModels;

namespace SurfaceStudioDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComparePagePopupLaptop : Page, INavigate
    {
        public RoutedEventHandler CloseButton_Clicked;

        #region Private Members

        private ComparePopupLaptopViewModel ViewModel
        {
            get { return DataContext as ComparePopupLaptopViewModel; }
        }

        #endregion

        #region Construction

        public ComparePagePopupLaptop()
        {
            InitializeComponent();
            ComparePagePopupLaptop.Current = this;
            this.ContentArea.Background = StyleHelper.GetAcrylicBrush();
        }

        #endregion

        #region Public Static Properties

        public static ComparePagePopupLaptop Current { get; private set; }

        #endregion

        #region Private Methods

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
        }

        public void NavigateFromPage()
        {
        }

        #endregion
    }
}
