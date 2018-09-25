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

using SDX.Toolkit.Helpers;
using SurfaceProDemo.ViewModels;

namespace SurfaceProDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComparePagePopupPro : Page, INavigate
    {
        public RoutedEventHandler CloseButton_Clicked;

        #region Private Members

        private ComparePopupProViewModel ViewModel
        {
            get { return DataContext as ComparePopupProViewModel; }
        }

        #endregion

        #region Construction

        public ComparePagePopupPro()
        {
            InitializeComponent();
            ComparePagePopupPro.Current = this;
            this.ContentArea.Background = StyleHelper.GetAcrylicBrush();
        }

        #endregion

        #region Public Static Properties

        public static ComparePagePopupPro Current { get; private set; }

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
