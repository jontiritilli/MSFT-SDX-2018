using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using SDX.Toolkit.Helpers;

namespace SurfaceJackDemo.Views
{
    public sealed partial class AudioTryItPage : Page, INavigate
    {
        #region Private Members

        private AudioTryItViewModel ViewModel
        {
            get { return DataContext as AudioTryItViewModel; }
        }

        #endregion
        #region Public Members
        public RoutedEventHandler CloseButton_Clicked;
        #endregion
        #region Public Static Properties

        public static AudioTryItPage Current { get; private set; }

        #endregion

        #region Construction

        public AudioTryItPage()
        {
            InitializeComponent();
            AudioTryItPage.Current = this;
            this.AudioTryItPagePopup.Background = StyleHelper.GetAcrylicBrush("Dark");
        }

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
            // animations in
            //AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            // animations out
            //AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
