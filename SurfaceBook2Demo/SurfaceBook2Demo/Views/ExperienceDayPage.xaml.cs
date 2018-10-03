using System;

using Windows.UI.Xaml.Controls;

using SurfaceBook2Demo.ViewModels;
using SDX.Toolkit.Controls;
using SDX.Telemetry.Services;


namespace SurfaceBook2Demo.Views
{
    public sealed partial class ExperienceDayPage : Page, INavigate
    {
        #region Static

        public static ExperienceDayPage Current = null;

        public static FlipViewEx GetDayFlipView()
        {
            FlipViewEx dayFlipView = null;

            if (null != Current)
            {
                dayFlipView = Current.DayFlipView;
            }

            return dayFlipView;
        }

        #endregion


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
            ExperienceDayPage.Current = this;

            InitializeComponent();

            this.Loaded += this.ExperienceDayPage_Loaded;
        }

        private void ExperienceDayPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // set the current page
            this.DayFlipView.SelectedIndex = 0;
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
                    INavigateMoveDirection moveDirection = INavigateMoveDirection.Unknown;

                    // get the pageIndex of the new page
                    int nextPageIndex = this.DayFlipView.SelectedIndex;

                    // find the index of the previous page
                    int previousPageIndex = this.DayFlipView.Items.IndexOf(_previousPage);

                    // if we got it
                    if (-1 != previousPageIndex)
                    {
                        // are we moving forward or backward?
                        if (previousPageIndex < nextPageIndex)
                        {
                            moveDirection = INavigateMoveDirection.Forward;
                        }
                        else if (nextPageIndex < previousPageIndex)
                        {
                            moveDirection = INavigateMoveDirection.Backward;
                        }
                    }

                    // save the current page so we can navigate from it
                    _previousPage = (INavigate)((FlipViewItemEx)this.DayFlipView.SelectedItem).GetChildViewAsObject();

                    // navigate to it
                    _previousPage.NavigateToPage(moveDirection);

                    // telemetry
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewExperience);
                }

                // update the slider
                switch (this.DayFlipView.SelectedIndex)
                {
                    case 0:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (DaySliderSnapPositions.Morning != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Morning);
                        }
                        break;

                    case 1:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (DaySliderSnapPositions.Afternoon != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Afternoon);
                        }
                        break;

                    case 2:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (DaySliderSnapPositions.Evening != this.DaySlider.Position)
                        {
                            this.DaySlider.SnapTo(DaySliderSnapPositions.Evening);
                        }
                        break;

                    case 3:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
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
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (0 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 0;
                        }
                        break;

                    case DaySliderSnapPositions.Afternoon:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (1 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 1;
                        }
                        break;

                    case DaySliderSnapPositions.Evening:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (2 != this.DayFlipView.SelectedIndex)
                        {
                            this.DayFlipView.SelectedIndex = 2;
                        }
                        break;

                    case DaySliderSnapPositions.Night:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
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

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // if we're navigating to this flipview page,
            // we need to know which direction we're coming from
            // so that we know which page in the flipview to show
            switch (moveDirection)
            {
                case INavigateMoveDirection.Backward:
                    // moving backwards to get here, so set to the last page in the flipview
                    this.DayFlipView.SelectedIndex = 3;
                    break;

                case INavigateMoveDirection.Forward:
                    // moving forwards, so set to first
                    this.DayFlipView.SelectedIndex = 0;
                    break;

                case INavigateMoveDirection.Unknown:
                default:
                    // don't do anything for Unknown
                    break;
            }

            // animations in
            // propagate navigation to child flipview pages on first navigate from outer FV page
            INavigate navigatePage = (INavigate)((FlipViewItemEx)this.DayFlipView.SelectedItem).GetChildViewAsObject();            
            navigatePage.NavigateToPage(moveDirection);

        }

        public void NavigateFromPage()
        {
            // animations out
            INavigate navigatePage = (INavigate)((FlipViewItemEx)this.DayFlipView.SelectedItem).GetChildViewAsObject();
            navigatePage.NavigateFromPage();
        }

        #endregion

    }
}
