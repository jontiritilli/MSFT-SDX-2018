using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;


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

        #region Construction

        public AudioTryItPage()
        {
            InitializeComponent();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        #endregion
    }
}
