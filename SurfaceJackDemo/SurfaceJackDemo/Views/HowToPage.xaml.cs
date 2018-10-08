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

        public RoutedEventHandler CloseButton_Clicked;

        #region Public Static Properties

        public static HowToPage Current { get; private set; }

        #endregion
        #endregion
        public HowToPage()
        {
            this.InitializeComponent();
            HowToPage.Current = this;
            this.ContentArea.Background = StyleHelper.GetAcrylicBrush();
        }

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }
    }
}
