using System;

using Windows.UI.Xaml.Controls;

using SurfaceProDemo.ViewModels;
using SDX.Toolkit.Controls;


namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceFlipViewPage : Page, INavigate
    {
        #region Private Members

        private ExperienceFlipViewViewModel ViewModel
        {
            get { return DataContext as ExperienceFlipViewViewModel; }
        }

        private INavigate _previousPage = null;

        #endregion


        #region Construction

        public ExperienceFlipViewPage()
        {
            InitializeComponent();

            this.Loaded += this.ExperienceFlipViewPage_Loaded;
        }

        private void ExperienceFlipViewPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // set the current page
            this.DeviceModeFlipView.SelectedIndex = 0;

            // save the current page so we can navigate from it
            _previousPage = (INavigate)((FlipViewItemEx)this.DeviceModeFlipView.SelectedItem).GetChildViewAsObject();

            // navigate to it
            _previousPage.NavigateToPage();
        }

        #endregion


        #region Event Handlers

        private void DeviceModeFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((null != this.DeviceModeFlipView) && (null != this.DeviceModeSlider))
            {
                // navigate from the previous page
                if (null != _previousPage)
                {
                    _previousPage.NavigateFromPage();
                }

                // navigate to the current page
                if (null != this.DeviceModeFlipView.SelectedItem)
                {
                    // save the current page so we can navigate from it
                    _previousPage = (INavigate)((FlipViewItemEx)this.DeviceModeFlipView.SelectedItem).GetChildViewAsObject();

                    // navigate to it
                    _previousPage.NavigateToPage();
                }

                // update slider
                switch (this.DeviceModeFlipView.SelectedIndex)
                {
                    case 0:
                        if (DeviceModeSliderSnapPositions.Studio != this.DeviceModeSlider.Position)
                        {
                            this.DeviceModeSlider.SnapTo(DeviceModeSliderSnapPositions.Studio);
                        }
                        break;

                    case 1:
                        if (DeviceModeSliderSnapPositions.Laptop != this.DeviceModeSlider.Position)
                        {
                            this.DeviceModeSlider.SnapTo(DeviceModeSliderSnapPositions.Laptop);
                        }
                        break;

                    case 2:
                        if (DeviceModeSliderSnapPositions.Tablet != this.DeviceModeSlider.Position)
                        {
                            this.DeviceModeSlider.SnapTo(DeviceModeSliderSnapPositions.Tablet);
                        }
                        break;

                }
            }
        }

        private void DeviceModeSlider_Snapped(object sender, SDX.Toolkit.Controls.DeviceModeSliderSnappedEventArgs e)
        {
            if ((null != this.DeviceModeFlipView) && (null != this.DeviceModeSlider))
            {
                switch (e.SnapPosition)
                {
                    case DeviceModeSliderSnapPositions.Studio:
                        if (0 != this.DeviceModeFlipView.SelectedIndex)
                        {
                            this.DeviceModeFlipView.SelectedIndex = 0;
                        }
                        break;

                    case DeviceModeSliderSnapPositions.Laptop:
                        if (1 != this.DeviceModeFlipView.SelectedIndex)
                        {
                            this.DeviceModeFlipView.SelectedIndex = 1;
                        }
                        break;

                    case DeviceModeSliderSnapPositions.Tablet:
                        if (2 != this.DeviceModeFlipView.SelectedIndex)
                        {
                            this.DeviceModeFlipView.SelectedIndex = 2;
                        }
                        break;
                }
            }
        }

        #endregion


        #region INavigate Interface

        public void NavigateToPage()
        {
            // animations in
        }

        public void NavigateFromPage()
        {
            // animations out
        }

        #endregion

    }
}
