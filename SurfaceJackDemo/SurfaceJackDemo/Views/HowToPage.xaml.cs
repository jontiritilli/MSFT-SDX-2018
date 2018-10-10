using SDX.Toolkit.Helpers;
using SurfaceJackDemo.ViewModels;
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

namespace SurfaceJackDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HowToPage : Page, INavigate
    {
        #region Private Members

        private HowToViewModel ViewModel
        {
            get { return DataContext as HowToViewModel; }
        }

        #endregion

        #region Public Static Properties

        public RoutedEventHandler CloseButton_Clicked;

        public static HowToPage Current { get; private set; }

        #endregion

        #region Construction

        public HowToPage()
        {
            this.InitializeComponent();
            HowToPage.Current = this;
            this.ContentArea.Background = new AcrylicBrush()
            {
                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                Opacity = 0.995,
                TintColor = Windows.UI.Colors.White,
                TintOpacity = 0.4,
                FallbackColor = Windows.UI.Colors.White,
            };
            this.Loaded += HowToPage_Loaded;
        }

        #endregion

        #region Private Methods

        private void HowToPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (null != itemListView)
            {
                itemListView.SelectedIndex = 0;
            }
        }

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                this.AppSelectorImageKB.SetSelectedID(listView.SelectedIndex);
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }
        #endregion

    }
}
