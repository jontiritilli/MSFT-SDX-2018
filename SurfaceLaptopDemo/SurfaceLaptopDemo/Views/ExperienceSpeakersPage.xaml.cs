using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SDX.Toolkit.Helpers;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSpeakersPage : Page, INavigate
    {
        #region Private Members

        private ExperienceSpeakersViewModel ViewModel
        {
            get { return DataContext as ExperienceSpeakersViewModel; }
        }
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;
        #endregion

        #region Public Members

        public static ExperienceSpeakersPage Current { get; private set; }

        #endregion

        #region Construction

        public ExperienceSpeakersPage()
        {
            InitializeComponent();
            ExperienceSpeakersPage.Current = this;
            this.AppSelectorImageSpeakers.AppSelector = this.AppSelectorSpeakers;
            this.AppSelectorImageMinorSpeakers.AppSelector = this.AppSelectorSpeakers;
            this.Loaded += ExperienceSpeakersPage_Loaded;
        }

        private void ExperienceSpeakersPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateFromPage();
            ExperienceSpeakersPage.Current.HasLoaded = true;
            if (ExperienceSpeakersPage.Current.HasNavigatedTo)
            {
                AnimationHelper.PerformPageEntranceAnimation(this);
            }
        }
        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            
            if (ExperienceSpeakersPage.Current.HasLoaded)
            {
                AnimationHelper.PerformPageEntranceAnimation(this);
            }
            else
            {
                ExperienceSpeakersPage.Current.HasNavigatedTo = true;
            }
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
