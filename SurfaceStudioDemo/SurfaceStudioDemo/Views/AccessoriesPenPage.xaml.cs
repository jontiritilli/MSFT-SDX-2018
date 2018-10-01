using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using SDX.Toolkit.Controls;
using SDX.Toolkit.Helpers;

using SurfaceStudioDemo.ViewModels;


namespace SurfaceStudioDemo.Views
{
    public sealed partial class AccessoriesPenPage : Page, INavigate
    {

        public Popup ReadyScreen;

        public RoutedEventHandler DialColorIdChanged;

        public RoutedEventHandler BookColorIdChanged;

        public RoutedEventHandler OnDialScreenContactStarted;

        public bool Visited = false;

        #region Private Members

        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }

        #endregion

        #region Construction

        public AccessoriesPenPage()
        {
            InitializeComponent();

            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {// well this works? but ew
                timer.Stop();
                ReadyScreen = FlipViewPage.Current.GetAccessoriesPenPopup();
                AccessoriesPenPopupPage.Current.CloseButton_Clicked += CloseButton_Clicked;                
            };

            this.rBtnPen.Clicked += OnPenTryItClicked;

            this.ColoringBook.ColorIDChanged += BookColorIDChanged;
            this.ColoringBook.OnPenScreenContacted += OnPenScreenContacted;

            this.SurfaceDial.ColorIDChanged += DialColorIDChanged;
            this.SurfaceDial.OnDialScreenContactStarted += OnDialScreenContacted;
        }

        #endregion

        #region Private Methods

        private void OnPenTryItClicked(object sender, EventArgs e)
        {
            this.ColoringBook.FadeInColoringImage();
        }

        private void DialColorIDChanged(object sender, EventArgs e)
        {
            if(SurfaceDial.ColorID != this.ColoringBook.ColorID)
            {
                this.ColoringBook.ForceColorChange(SurfaceDial.ColorID);
            }
        }

        private void BookColorIDChanged(object sender, EventArgs e)
        {
            if (ColoringBook.ColorID != this.SurfaceDial.ColorID)
            {
                this.SurfaceDial.ForceRotation(ColoringBook.ColorID);
            }
        }

        private void OnDialScreenContacted(object sender, EventArgs e)
        {
            this.rBtnDial.Visibility = Visibility.Collapsed;
        }

        private void OnPenScreenContacted(object sender, EventArgs e)
        {
            this.rBtnPen.Visibility = Visibility.Collapsed;
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            HidePopup();
            AnimatePageOnNavigate();
            Visited = true;
        }

        private void ShowPopup()
        {
            if (!Visited)
            {
                if(null != ReadyScreen)
                {
                    ReadyScreen.IsOpen = true;
                }
            }
        }

        private void HidePopup()
        {
            if (null != ReadyScreen)
            {
                if (ReadyScreen.IsOpen)
                {
                    ReadyScreen.IsOpen = false;
                }
            }
        }

        private void AnimatePageOnNavigate()
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
            rBtnDial.StartEntranceAnimation();
            rBtnDial.StartRadiateAnimation();

            rBtnPen.StartEntranceAnimation();
            rBtnPen.StartRadiateAnimation();
            SurfaceDial.ActivateOnNavigate();
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            // animations in
            if (Visited || null != ReadyScreen)// do regular entrance if theyve been here and did stuff already
            {
                AnimatePageOnNavigate();
            }
            ShowPopup();
            
                    
        }

        public void NavigateFromPage()
        {
            // animations out
            AnimationHelper.PerformPageExitAnimation(this);
            HidePopup();
            rBtnDial.ResetEntranceAnimation();
            rBtnDial.ResetRadiateAnimation();

            rBtnPen.ResetEntranceAnimation();
            rBtnPen.ResetRadiateAnimation();
        }

        #endregion
    }
}
