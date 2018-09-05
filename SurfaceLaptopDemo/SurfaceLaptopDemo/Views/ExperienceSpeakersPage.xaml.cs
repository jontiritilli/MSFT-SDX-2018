using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceSpeakersPage : Page, INavigate
    {
        #region Private Members

        private ExperienceSpeakersViewModel ViewModel
        {
            get { return DataContext as ExperienceSpeakersViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceSpeakersPage()
        {
            InitializeComponent();
            this.AppSelectorImageSpeakers.AppSelector = this.AppSelectorSpeakers;
        }

        #endregion


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
    }
}
