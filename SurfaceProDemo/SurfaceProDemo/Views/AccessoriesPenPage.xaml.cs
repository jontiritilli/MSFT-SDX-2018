using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Helpers;

namespace SurfaceProDemo.Views
{
    public sealed partial class AccessoriesPenPage : Page, INavigate
    {
        #region Private Members

        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members
        public static AccessoriesPenPage Current { get; private set; }
        #endregion


        #region Construction

        public AccessoriesPenPage()
        {
            InitializeComponent();
            AccessoriesPenPage.Current = this;
            rBtnCenter.Clicked += OnPenTryItClicked;
            this.ColoringBook.OnPenScreenContacted += OnPenScreenContacted;
            this.Loaded += AccessoriesPenPage_Loaded;
        }

        private void AccessoriesPenPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            AccessoriesPenPage.Current.HasLoaded = true;
            if (AccessoriesPenPage.Current.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            this.rBtnCenter.StartEntranceAnimation();
            this.rBtnCenter.StartRadiateAnimation();
        }
        #endregion

        #region Private Methods

        private void OnPenTryItClicked(object sender, EventArgs e)
        {
            this.ColoringBook.FadeInColoringImage();
        }

        private void OnPenScreenContacted(object sender, EventArgs e)
        {
            this.rBtnCenter.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (AccessoriesPenPage.Current.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                AccessoriesPenPage.Current.HasNavigatedTo = true;
            }

        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rBtnCenter.ResetEntranceAnimation();
            this.rBtnCenter.ResetRadiateAnimation();
        }

        #endregion
    }
}
