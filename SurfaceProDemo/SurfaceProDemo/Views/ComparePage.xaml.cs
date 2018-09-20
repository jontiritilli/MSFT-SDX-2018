using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class ComparePage : Page, INavigate
    {
        #region Private Members

        private CompareViewModel ViewModel
        {
            get { return DataContext as CompareViewModel; }
        }

        #endregion


        #region Construction

        public ComparePage()
        {
            InitializeComponent();
            this.rBtnPro.PopupChild = this.PopPro;
            this.rBtnBook.PopupChild = this.PopBook;
            this.rBtnStudio.PopupChild = this.PopStudio;
            this.rBtnLaptop.PopupChild = this.PopLaptop;
            this.rBtnGo.PopupChild = this.PopGo;

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            SDX.Toolkit.Helpers.AnimationHelper.PerformTranslateIn(this.img_Family, this.img_Family.TranslateDirection, 100, 500, 0);

            rBtnPro.StartEntranceAnimation();
            rBtnPro.StartRadiateAnimation();

            rBtnBook.StartEntranceAnimation();
            rBtnBook.StartRadiateAnimation();

            rBtnStudio.StartEntranceAnimation();
            rBtnStudio.StartRadiateAnimation();

            rBtnLaptop.StartEntranceAnimation();
            rBtnLaptop.StartRadiateAnimation();

            rBtnGo.StartEntranceAnimation();
            rBtnGo.StartRadiateAnimation();


        }

        public void NavigateFromPage()
        {
            // animations out
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
            rBtnPro.ResetEntranceAnimation();
            rBtnPro.ResetRadiateAnimation();

            rBtnBook.ResetEntranceAnimation();
            rBtnBook.ResetRadiateAnimation();

            rBtnStudio.ResetEntranceAnimation();
            rBtnStudio.ResetRadiateAnimation();

            rBtnLaptop.ResetEntranceAnimation();
            rBtnLaptop.ResetRadiateAnimation();

            rBtnGo.ResetEntranceAnimation();
            rBtnGo.ResetRadiateAnimation();
        }

        #endregion
    }
}
