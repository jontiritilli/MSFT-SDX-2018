using System;

using SurfaceLaptopDemo.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;


namespace SurfaceLaptopDemo.Views
{
    public sealed partial class ExperienceInnovationPage : Page, INavigate
    {
        #region Private Members

        private ExperienceInnovationViewModel ViewModel
        {
            get { return DataContext as ExperienceInnovationViewModel; }
        }

        #endregion


        #region Construction

        public ExperienceInnovationPage()
        {
            InitializeComponent();

            Popup popup = new Popup()
            {
                IsOpen = false,
                IsLightDismissEnabled = true
            };

            TextBlock _block = new TextBlock()
            {
                Text = "Testing, Testing, Testing",
                TextWrapping = TextWrapping.WrapWholeWords,
                Margin = new Thickness(30),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            popup.Child =_block;
            this.rButtonOne.PopupChild = popup;
        }

        #endregion


        #region INavigate Interface


        public void NavigateToPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageEntranceAnimation(this);
            rButtonOne.StartEntranceAnimation();
            rButtonOne.StartRadiateAnimation();
        }

        public void NavigateFromPage()
        {
            SDX.Toolkit.Helpers.AnimationHelper.PerformPageExitAnimation(this);
        }

        #endregion
    }
}
