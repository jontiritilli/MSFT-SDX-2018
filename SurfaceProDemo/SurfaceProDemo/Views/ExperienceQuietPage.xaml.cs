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

            rBtnLeft.PopupChild = PopLeft;
            rBtnTop.PopupChild = PopTop;
            rBtnRight.PopupChild = PopRight;
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Tablet, this.img_Tablet.TranslateDirection, 100, 500, 0);
            this.rBtnLeft.StartEntranceAnimation();
            this.rBtnLeft.StartRadiateAnimation();

            this.rBtnTop.StartEntranceAnimation();
            this.rBtnTop.StartRadiateAnimation();

            this.rBtnRight.StartEntranceAnimation();
            this.rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            this.rBtnTop.ResetEntranceAnimation();
            this.rBtnTop.ResetRadiateAnimation();

            this.rBtnLeft.ResetEntranceAnimation();
            this.rBtnLeft.ResetRadiateAnimation();

            this.rBtnRight.ResetEntranceAnimation();
            this.rBtnRight.ResetRadiateAnimation();
        }

        #endregion
    }
}
