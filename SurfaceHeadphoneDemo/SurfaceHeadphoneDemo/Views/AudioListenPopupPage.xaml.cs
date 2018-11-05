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

using SurfaceHeadphoneDemo.ViewModels;

using SDX.Toolkit.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SurfaceHeadphoneDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AudioListenPopupPage : Page, INavigate
    {
        public RoutedEventHandler CloseButton_Clicked;

        #region Private Members

        private AudioListenPopupViewModel ViewModel
        {
            get { return DataContext as AudioListenPopupViewModel; }
        }

        #endregion

        #region Public Static Properties

        public static AudioListenPopupPage Current { get; private set; }

        #endregion

        #region Private Methods

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        #endregion

        #region Construction

        public AudioListenPopupPage()
        {
            this.InitializeComponent();
            AudioListenPopupPage.Current = this;
            this.AudioListenPagePopup.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Dark);
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
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
