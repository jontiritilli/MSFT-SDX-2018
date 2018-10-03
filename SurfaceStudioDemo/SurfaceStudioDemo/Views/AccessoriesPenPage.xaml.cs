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

        #region Private Members

        private AccessoriesPenViewModel ViewModel
        {
            get { return DataContext as AccessoriesPenViewModel; }
        }

        private bool HasInteracted = false;
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        #endregion

        #region Public Members

        public static AccessoriesPenPage Current { get; private set; }

        #endregion

        #region Construction

        public AccessoriesPenPage()
        {
            InitializeComponent();

            var timer = new Windows.UI.Xaml.DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(500)
            };

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

            this.Loaded += AccessoriesPenPage_Loaded;
        }

        #endregion

        #region Private Methods

        private void OnPenTryItClicked(object sender, EventArgs e)
        {
            this.rBtnPen.FadeOutButton();
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
            this.rBtnDial.FadeOutButton();
        }

        private void OnPenScreenContacted(object sender, EventArgs e)
        {
            this.rBtnPen.FadeOutButton();
            this.ColoringBook.FadeInColoringImage();
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            HidePopup();
            AnimatePageEntrance();
            HasInteracted = true;
        }

        private void ShowPopup()
        {
            if (null != ReadyScreen && !HasInteracted)
            {
                ReadyScreen.IsOpen = true;
            }
        }

        private void HidePopup()
        {
            if (null != ReadyScreen && ReadyScreen.IsOpen)
            {
                ReadyScreen.IsOpen = false;
            }
        }

        private void AccessoriesPenPage_Loaded(object sender, RoutedEventArgs e)
        {
            AnimationHelper.PerformPageExitAnimation(this);

            this.HasLoaded = true;

            if (this.HasNavigatedTo)
            {
                AnimatePageEntrance();
            }
        }

        private void AnimatePageEntrance()
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
            ShowPopup();
            if (this.HasLoaded)
            {
                AnimatePageEntrance();
            }
            else
            {
                this.HasNavigatedTo = true;
            }
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
