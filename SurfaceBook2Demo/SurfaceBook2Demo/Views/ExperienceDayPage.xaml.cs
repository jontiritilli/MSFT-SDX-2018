using System;

using Windows.UI.Xaml.Controls;

using SurfaceBook2Demo.ViewModels;
using SDX.Toolkit.Controls;


namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayPage : Page, INavigate
    {
        #region Private Members

        private ExperienceDayViewModel ViewModel
        {
            get { return DataContext as ExperienceDayViewModel; }
        }

        private INavigate _previousPage = null;

        #endregion


        #region Construction

        public ExperienceDayPage()
        {
            InitializeComponent();

            this.Loaded += this.ExperienceDayPage_Loaded;
        }

        private void ExperienceDayPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // set the current page
            this.DayFlipView.SelectedIndex = 0;

            // save the current page so we can navigate from it
            _previousPage = (INavigate)((FlipViewItemEx)this.DayFlipView.SelectedItem).GetChildViewAsObject();

            // navigate to it
            _previousPage.NavigateToPage();
        }

        #endregion


        #region Event Handlers

        private void DayFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((null != this.DayFlipView) && (null != this.DaySlider))
            {
                // navigate from the previous page
                if (null != _previousPage)
                {
                    _previousPage.NavigateFromPage();
                }

                // navigate to the current page
                if (null != this.DayFlipView.SelectedItem)
                {
                    // save the current page so we can navigate from it
                    _previousPage = (INavigate)((FlipViewItemEx)DayFlipView.SelectedItem).GetChildViewAsObject();

                    // navigate to it
                    _previousPage.NavigateToPage();
                }

                // update the slider
                switch (this.DayFlipView.SelectedIndex)
                {
                    case 0:
                        if (DaySliderSnapPositions.Morning != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Morning);
                        }
                        break;

                    case 1:
                        if (DaySliderSnapPositions.Afternoon != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Afternoon);
                        }
                        break;

                    case 2:
                        if (DaySliderSnapPositions.Evening != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Evening);
                        }
                        break;

                    case 3:
                        if (DaySliderSnapPositions.Night != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Night);
                        }
                        break;
                }
            }
        }

        private void DaySlider_Snapped(object sender, DaySliderSnappedEventArgs e)
        {
            if ((null != this.DayFlipView) && (null != this.DaySlider))
            {
                switch (e.SnapPosition)
                {
                    case DaySliderSnapPositions.Morning:
                        if (0 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 0;
                        }
                        break;

                    case DaySliderSnapPositions.Afternoon:
                        if (1 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 1;
                        }
                        break;

                    case DaySliderSnapPositions.Evening:
                        if (2 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 2;
                        }
                        break;

                    case DaySliderSnapPositions.Night:
                        if (3 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 3;
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
