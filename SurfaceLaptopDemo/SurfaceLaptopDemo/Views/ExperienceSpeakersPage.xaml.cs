using System;

using SurfaceLaptopDemo.ViewModels;

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

        #endregion

        #region Construction

        public ExperienceSpeakersPage()
        {
            InitializeComponent();
            this.AppSelectorImageSpeakers.AppSelector = this.AppSelectorSpeakers;
            this.AppSelectorImageMinorSpeakers.AppSelector = this.AppSelectorSpeakers;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
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
