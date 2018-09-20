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
    public sealed partial class ExperiencePixelSensePopupPage : Page, INavigate
    {
        public RoutedEventHandler CloseButton_Clicked;

        #region Private Members

        private ExperiencePixelSensePopupViewModel ViewModel
        {
            get { return DataContext as ExperiencePixelSensePopupViewModel; }
        }

        #endregion

        #region Construction

        public ExperiencePixelSensePopupPage()
        {
            InitializeComponent();
            ExperiencePixelSensePopupPage.Current = this;
            this.AppSelectorImagePixel.AppSelector = this.AppSelectorPixel;
            this.ContentArea.Background = StyleHelper.GetAcrylicBrush();
        }

        #endregion

        #region Public Static Properties

        public static ExperiencePixelSensePopupPage Current { get; private set; }

        #endregion

        #region Private Methods

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage()
        {
        }

        public void NavigateFromPage()
        {
        }

        #endregion
    }
}
