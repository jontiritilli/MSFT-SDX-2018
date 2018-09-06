using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceQuietPage : Page, INavigate
    {
        #region Private Members

        private ExperienceQuietViewModel ViewModel
        {
            get { return DataContext as ExperienceQuietViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceQuietPage()
        {
            InitializeComponent();
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Tablet, this.img_Tablet.TranslateDirection, 100, 500, 0);
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
