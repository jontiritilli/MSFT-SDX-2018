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

using SurfaceStudioDemo.ViewModels;

using SDX.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SurfaceStudioDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccessoriesPenPopupPage : Page, INavigate
    {
        public RoutedEventHandler CloseButton_Clicked;

        #region Private Members

        private AccessoriesPenPopupViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenPopupViewModel; }
        }

        #endregion

        #region Public Static Properties

        public static AccessoriesPenPopupPage Current { get; private set; }

        #endregion

        #region Private Methods

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        #endregion

        #region Construction

        public AccessoriesPenPopupPage()
        {
            this.InitializeComponent();
            AccessoriesPenPopupPage.Current = this;
            this.PenPagePopup.Background = StyleHelper.GetAcrylicBrush();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
