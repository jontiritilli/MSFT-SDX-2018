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

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SurfaceProDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExperiencePopupPage : Page, INavigate
    {

        #region Private Members

        private ExperiencePopupViewModel ViewModel
        {
            get { return DataContext as ExperiencePopupViewModel; }
        }

        #endregion

        public RoutedEventHandler CloseButton_Clicked;

        #region Public Static Properties

        public static ExperiencePopupPage Current { get; private set; }

        #endregion
        public ExperiencePopupPage()
        {
            this.InitializeComponent();
            ExperiencePopupPage.Current = this;
            this.AppSelectorImageKB.AppSelector = this.AppSelectorKB;
            this.ContentArea.Background = StyleHelper.GetAcrylicBrush();

        }
        #region INavigate Interface

        public void NavigateToPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {            
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }
        #endregion

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }
    }
}
