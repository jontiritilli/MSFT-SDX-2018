using System;

using Windows.UI.Xaml.Controls;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class BestOfMicrosoftPage : Page, INavigate
    {
        #region Private Members

        private BestOfMicrosoftViewModel ViewModel
        {
            get { return DataContext as BestOfMicrosoftViewModel; }
        }

        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members
        public static BestOfMicrosoftPage Current { get; private set; }
        #endregion


        #region Construction

        public BestOfMicrosoftPage()
        {
            InitializeComponent();
            BestOfMicrosoftPage.Current = this;
            this.Loaded += BestOfMicrosoftPage_Loaded;
        }

        private void BestOfMicrosoftPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
            BestOfMicrosoftPage.Current.HasLoaded = true;
            if (BestOfMicrosoftPage.Current.HasNavigatedTo)
            {
                SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            }
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            if (BestOfMicrosoftPage.Current.HasLoaded)
            {
                SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            }
            else
            {
                BestOfMicrosoftPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
