using System;

using SurfaceLaptopDemo.ViewModels;

using SDX.Toolkit.Helpers;
using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class BestOfMicrosoftPage : Page, INavigate
    {
        #region Private Members

        private BestOfMicrosoftViewModel ViewModel
        {
            get { return DataContext as BestOfMicrosoftViewModel; }
        }

        #endregion

        #region Construction

        public BestOfMicrosoftPage()
        {
            InitializeComponent();
            this.Loaded += BestOfMicrosoftPage_Loaded;
        }

        private void BestOfMicrosoftPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigateFromPage();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
