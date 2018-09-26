using System;
using SurfaceBook2Demo.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceIntroPage : Page, INavigate
    {
        #region Private Members

        private ExperienceIntroViewModel ViewModel
        {
            get { return DataContext as ExperienceIntroViewModel; }
        }
        #endregion


        #region Construction

        public ExperienceIntroPage()
        {
            InitializeComponent();

            rBtnLeft.PopupChild = PopLeft;
            rBtnRight.PopupChild = PopRight;

        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            rBtnLeft.StartEntranceAnimation();
            rBtnLeft.StartRadiateAnimation();

            rBtnRight.StartEntranceAnimation();
            rBtnRight.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            // animations out
            rBtnLeft.ResetEntranceAnimation();
            rBtnLeft.ResetRadiateAnimation();

            rBtnRight.ResetEntranceAnimation();
            rBtnRight.ResetRadiateAnimation();
        }

        #endregion

        private void PopLeft_Opened(object sender, object e)
        {
            this.LegalPixelSense.SetOpacity(1);
        }

        private void PopLeft_Closed(object sender, object e)
        {
            this.LegalPixelSense.SetOpacity(0);
        }

        private void PopRight_Opened(object sender, object e)
        {
         
        }

        private void PopRight_Closed(object sender, object e)
        {
         
        }
    }
}
