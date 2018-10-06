using System;

using Windows.UI.Xaml.Controls;

using SDX.Telemetry.Services;
using SDX.Toolkit.Controls;

using SurfaceProDemo.ViewModels;


namespace SurfaceProDemo.Views
{
    public sealed partial class ExperienceFlipViewPage : Page, INavigate
    {
        #region Static

        public static ExperienceFlipViewPage Current = null;

        public static FlipViewEx GetDeviceModeFlipView()
        {
            FlipViewEx flipView = null;

            if (null != ExperienceFlipViewPage.Current)
            {
                flipView = ExperienceFlipViewPage.Current.DeviceModeFlipView;
            }

            return flipView;
        }

        #endregion

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
            ExperienceFlipViewPage.Current = this;

            InitializeComponent();            

            this.Loaded += this.ExperienceFlipViewPage_Loaded;
        }

        private void ExperienceFlipViewPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // set the current page
            this.DeviceModeFlipView.SelectedIndex = 0;


        }

        #endregion

        #region Public Methods
        public int GetFlipViewSelectedIndex()
        {
            return this.DeviceModeFlipView.SelectedIndex;
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
                    INavigateMoveDirection moveDirection = INavigateMoveDirection.Unknown;

                    // get the pageIndex of the new page
                    int nextPageIndex = this.DeviceModeFlipView.SelectedIndex;
                    
                    
                    // find the index of the previous page
                    int previousPageIndex = this.DeviceModeFlipView.Items.IndexOf(_previousPage);
                    
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
                    _previousPage = (INavigate)((FlipViewItemEx)this.DeviceModeFlipView.SelectedItem).GetChildViewAsObject();

                    // navigate to it
                    _previousPage.NavigateToPage(moveDirection);

                    // telemetry
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewExperience);
                }

                // update slider
                switch (this.DeviceModeFlipView.SelectedIndex)
                {
                    case 0:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (DeviceModeSliderSnapPositions.Studio != this.DeviceModeSlider.Position)
                        {
                            this.DeviceModeSlider.SnapTo(DeviceModeSliderSnapPositions.Studio);
                        }
                        break;

                    case 1:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (DeviceModeSliderSnapPositions.Laptop != this.DeviceModeSlider.Position)
                        {
                            this.DeviceModeSlider.SnapTo(DeviceModeSliderSnapPositions.Laptop);
                        }
                        break;

                    case 2:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (DeviceModeSliderSnapPositions.Tablet != this.DeviceModeSlider.Position)
                        {
                            this.DeviceModeSlider.SnapTo(DeviceModeSliderSnapPositions.Tablet);
                        }
                        break;

                }

                this.RaiseSelectionChangedEvent(this);
            }
        }

        private void DeviceModeSlider_Snapped(object sender, SDX.Toolkit.Controls.DeviceModeSliderSnappedEventArgs e)
        {
            if ((null != this.DeviceModeFlipView) && (null != this.DeviceModeSlider))
            {
                switch (e.SnapPosition)
                {
                    case DeviceModeSliderSnapPositions.Studio:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (0 != this.DeviceModeFlipView.SelectedIndex)
                        {
                            this.DeviceModeFlipView.SelectedIndex = 0;
                        }
                        break;

                    case DeviceModeSliderSnapPositions.Laptop:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (1 != this.DeviceModeFlipView.SelectedIndex)
                        {
                            this.DeviceModeFlipView.SelectedIndex = 1;
                        }
                        break;

                    case DeviceModeSliderSnapPositions.Tablet:
                        // to avoid infinite event loops, only update if it's not already set to the new value 
                        if (2 != this.DeviceModeFlipView.SelectedIndex)
                        {
                            this.DeviceModeFlipView.SelectedIndex = 2;
                        }
                        break;
                }
            }
        }

        #endregion

        #region Custom Event Handlers        
        public delegate void OnSelectionChangedEvent(object sender, EventArgs e);

        public event OnSelectionChangedEvent SelectionChanged;

        private void RaiseSelectionChangedEvent(ExperienceFlipViewPage sender, EventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        private void RaiseSelectionChangedEvent(ExperienceFlipViewPage sender)
        {
            this.RaiseSelectionChangedEvent(sender, new EventArgs());
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
                    this.DeviceModeFlipView.SelectedIndex = 2;
                    break;

                case INavigateMoveDirection.Forward:
                    // moving forwards, so set to first
                    this.DeviceModeFlipView.SelectedIndex = 0;
                    break;

                case INavigateMoveDirection.Unknown:
                default:
                    // don't do anything for Unknown
                    break;
            }

            // animations in
            // propagate navigation to child flipview pages on first navigate from outer FV page
            INavigate navigatePage = (INavigate)((FlipViewItemEx)this.DeviceModeFlipView.SelectedItem).GetChildViewAsObject();            
            navigatePage.NavigateToPage(moveDirection);

        }

        public void NavigateFromPage()
        {
            // animations out
            INavigate navigatePage = (INavigate)((FlipViewItemEx)this.DeviceModeFlipView.SelectedItem).GetChildViewAsObject();
            navigatePage.NavigateFromPage();            
        }

        #endregion

    }
}
