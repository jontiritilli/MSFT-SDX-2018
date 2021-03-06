﻿using SDX.Toolkit.Helpers;
using SurfaceBook2Demo.ViewModels;
using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SurfaceBook2Demo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExperienceDayWorkPopupPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayWorkPopupViewModel ViewModel
        {
            get { return DataContext as ExperienceDayWorkPopupViewModel; }
        }

        public RoutedEventHandler CloseButton_Clicked;

        #region Public Static Properties

        public static ExperienceDayWorkPopupPage Current { get; private set; }

        #endregion
        #endregion
        public ExperienceDayWorkPopupPage()
        {
            this.InitializeComponent();
            ExperienceDayWorkPopupPage.Current = this;
            this.AppSelectorImageKB.AppSelector = this.AppSelectorKB;
            this.ContentArea.Background = StyleHelper.GetAcrylicBrush();
        }

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
    }
}
