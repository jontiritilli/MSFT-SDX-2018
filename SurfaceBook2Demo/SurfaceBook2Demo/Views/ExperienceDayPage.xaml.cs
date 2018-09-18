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

        #endregion


        #region Construction

        public ExperienceDayPage()
        {
            InitializeComponent();
        }

        #endregion


        #region Event Handlers

        private void DayFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((null != this.DayFlipView) && (null != this.DaySlider))
            {
                switch (this.DayFlipView.SelectedIndex)
                {
                    case 0:
                        if (SnapPositions.Morning != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(SnapPositions.Morning);
                        }
                        break;

                    case 1:
                        if (SnapPositions.Afternoon != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(SnapPositions.Afternoon);
                        }
                        break;

                    case 2:
                        if (SnapPositions.Evening != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(SnapPositions.Evening);
                        }
                        break;

                    case 3:
                        if (SnapPositions.Night != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(SnapPositions.Night);
                        }
                        break;
                }
            }
        }

        private void DaySlider_Snapped(object sender, SDX.Toolkit.Controls.SnappedEventArgs e)
        {
            if ((null != this.DayFlipView) && (null != this.DaySlider))
            {
                switch (e.SnapPosition)
                {
                    case SnapPositions.Morning:
                        if (0 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 0;
                        }
                        break;

                    case SnapPositions.Afternoon:
                        if (1 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 1;
                        }
                        break;

                    case SnapPositions.Evening:
                        if (2 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 2;
                        }
                        break;

                    case SnapPositions.Night:
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
