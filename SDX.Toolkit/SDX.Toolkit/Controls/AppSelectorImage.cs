using SDX.Toolkit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace SDX.Toolkit.Controls
{
    #region classes
    public class AppSelectorImageURI
    {
        public string URI = "";
        public double Width = 0;
    }
    #endregion

    public sealed class AppSelectorImage : Control, IAnimate
    {
        #region Private Constants

        private static readonly Size BOUNDS = WindowHelper.GetViewSizeInfo();

        private static readonly double WIDTH_ORIGINAL = BOUNDS.Width;
        private static readonly double HEIGHT_ORIGINAL = BOUNDS.Height;

        #endregion

        #region Private Members

        public Grid _layoutRoot = null;
        private List<Image> Images;
        private int PreviousSelectedID = -1;
        private AppSelector _previousAppSelector = null;

        #endregion

        #region Construction

        public AppSelectorImage()
        {
            this.DefaultStyleKey = typeof(AppSelectorImage);

            this.Loaded += OnLoaded;
            this.Images = new List<Image>();            
            // inherited dependency property
            //new PropertyChangeEventSource<double>(
            //    this, "Opacity", BindingMode.OneWay).ValueChanged +=
            //    OnOpacityChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Members

        //
        public enum TransitionType
        {
            Fade,
            TranslateLeft,
            TranslateRight
        }

        // pass me in on init pls
        //public List<AppSelectorImageURI> URIs;
        //public HorizontalAlignment imageHorizontalAlignment = HorizontalAlignment.Left;
        //public double Width_Image = 200;// BOUNDS.Width;
        //public double Height_Image = 200;// BOUNDS.Height;

        #endregion

        #region Public Methods

        //public void StartFadeIn()
        //{
        //    // start the textblock
        //    if (null != _storyboardFadeIn)
        //    {
        //        _storyboardFadeIn.Begin();
        //    }
        //}

        //public void StartFadeOut()
        //{
        //    // start the textblock
        //    if (null != _storyboardFadeOut)
        //    {
        //        _storyboardFadeOut.Begin();
        //    }
        //}

        //public void ResetAnimation()
        //{
        //    // reset the textblock
        //    if (null != _storyboardFadeIn)
        //    {
        //        _storyboardFadeIn.Stop();
        //    }
        //    if (null != _storyboardFadeOut)
        //    {
        //        _storyboardFadeOut.Stop();
        //    }

        //    // reset opacity to starting point
        //    if (null != _layoutRoot)
        //    {
        //        _layoutRoot.Opacity = 0.0;
        //    }
        //}

        //public void SetOpacity(double opacity)
        //{
        //    if ((opacity < 0.0) || (opacity > 1.0)) { return; }

        //    if (null != _layoutRoot)
        //    {
        //        _layoutRoot.Opacity = opacity;
        //    }
        //}

        public void ForceID(int SelectedID)
        {
            this.UpdateUI(SelectedID);
        }

        #endregion

        #region Dependency Properties

        // ColorSelector
        public static readonly DependencyProperty AppSelectorProperty =
            DependencyProperty.Register("AppSelector", typeof(AppSelector), typeof(AppSelectorImage), new PropertyMetadata(null, OnAppSelectorChanged));

        public AppSelector AppSelector
        {
            get { return (AppSelector)GetValue(AppSelectorProperty); }
            set { SetValue(AppSelectorProperty, value); }
        }

        // SelectedID
        public static readonly DependencyProperty SelectedIDProperty =
            DependencyProperty.Register("SelectedID", typeof(int), typeof(AppSelectorImage), new PropertyMetadata(0, OnSelectedIDChanged));

        public int SelectedID
        {
            get { return ( (int)GetValue(SelectedIDProperty) ); }
            set { SetValue(SelectedIDProperty, value); }
        }

        // URI's (for images)
        public static readonly DependencyProperty URIsProperty =
        DependencyProperty.Register("URIs", typeof(List<AppSelectorImageURI>), typeof(AppSelectorImage), new PropertyMetadata(new List<AppSelectorImageURI>(), OnSelectedIDChanged));

        public List<AppSelectorImageURI> URIs
        {
            get { return ((List<AppSelectorImageURI>)GetValue(URIsProperty)); }
            set { SetValue(URIsProperty, value); }
        }

        // ImageWidth
        public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register("ImageWidth", typeof(double), typeof(AppSelectorImage), new PropertyMetadata(0d, OnSelectedIDChanged));

        public double ImageWidth
        {
            get { return ((double)GetValue(ImageWidthProperty)); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // ImageHeight
        public static readonly DependencyProperty ImageHeightProperty =
        DependencyProperty.Register("ImageHeight", typeof(double), typeof(AppSelectorImage), new PropertyMetadata(0d, OnSelectedIDChanged));

        public double ImageHeight
        {
            get { return ((double)GetValue(ImageHeightProperty)); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // imageHorizontalAlignment
        public static readonly DependencyProperty imageHorizontalAlignmentProperty =
        DependencyProperty.Register("imageHorizontalAlignment", typeof(HorizontalAlignment), typeof(AppSelectorImage), new PropertyMetadata(HorizontalAlignment.Center, OnSelectedIDChanged));


        public HorizontalAlignment imageHorizontalAlignment
        {
            get { return ((HorizontalAlignment)GetValue(imageHorizontalAlignmentProperty)); }
            set { SetValue(imageHorizontalAlignmentProperty, value); }
        }

        // BitmapImages
        public static readonly DependencyProperty BitmapImagesProperty =
        DependencyProperty.Register("BitmapImages", typeof(List<BitmapImage>), typeof(AppSelectorImage), new PropertyMetadata(new List<BitmapImage>()));

        public List<BitmapImage> BitmapImages
        {
            get { return ((List<BitmapImage>)GetValue(BitmapImagesProperty)); }
            set { SetValue(BitmapImagesProperty, value); }
        }

        // TranslateDirection
        public static readonly DependencyProperty PageEntranceDirectionProperty =
        DependencyProperty.Register("PageEntranceDirection", typeof(AnimationDirection), typeof(AppSelectorImage), new PropertyMetadata(AnimationDirection.Left));

        public AnimationDirection PageEntranceDirection
        {
            get { return (AnimationDirection)GetValue(PageEntranceDirectionProperty); }
            set { SetValue(PageEntranceDirectionProperty, value); }
        }

        // HasPageEntranceAnimation
        public static readonly DependencyProperty HasPageEntranceAnimationEnabledProperty =
        DependencyProperty.Register("HasPageEntranceAnimationEnabled", typeof(bool), typeof(AppSelectorImage), new PropertyMetadata(true));

        public bool HasPageEntranceAnimationEnabled
        {
            get { return (bool)GetValue(HasPageEntranceAnimationEnabledProperty); }
            set { SetValue(HasPageEntranceAnimationEnabledProperty, value); }
        }

        // HasPageEntranceTranslation
        public static readonly DependencyProperty HasEntranceTranslationProperty =
        DependencyProperty.Register("HasEntranceTranslation", typeof(bool), typeof(AppSelectorImage), new PropertyMetadata(true));

        public bool HasEntranceTranslation
        {
            get { return (bool)GetValue(HasEntranceTranslationProperty); }
            set { SetValue(HasEntranceTranslationProperty, value); }
        }

        // HasTransitionAnimation
        public static readonly DependencyProperty HasTransitionAnimationProperty =
        DependencyProperty.Register("HasTransitionAnimation", typeof(bool), typeof(AppSelectorImage), new PropertyMetadata(false));

        public bool HasTransitionAnimation
        {
            get { return (bool)GetValue(HasTransitionAnimationProperty); }
            set { SetValue(HasTransitionAnimationProperty, value); }
        }

        // TransitionAnimationStyle
        public static readonly DependencyProperty TransitionAnimationStyleProperty =
        DependencyProperty.Register("TransitionAnimationStyle", typeof(TransitionType), typeof(AppSelectorImage), new PropertyMetadata(TransitionType.Fade));

        public TransitionType TransitionAnimationStyle
        {
            get { return (TransitionType)GetValue(TransitionAnimationStyleProperty); }
            set { SetValue(TransitionAnimationStyleProperty, value); }
        }

        // TransitionTranslateDistance
        public static readonly DependencyProperty TransitionTranslateDistanceProperty =
        DependencyProperty.Register("TransitionTranslateDistance", typeof(double), typeof(AppSelectorImage), new PropertyMetadata(50d));

        public double TransitionTranslateDistance
        {
            get { return ((double)GetValue(TransitionTranslateDistanceProperty)); }
            set { SetValue(TransitionTranslateDistanceProperty, value); }
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (this.AutoStart)
            //{
            //    this.StartFadeIn();
            //}
        }

        private static void OnAppSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AppSelectorImage AppSelectorImage)
            {
                // clean up event handler from previous colorselector
                if (null != AppSelectorImage._previousAppSelector)
                {
                    AppSelectorImage._previousAppSelector.SelectedIDChanged -= AppSelectorImage.AppSelector_OnSelectionChanged;
                }

                // save this as the previous
                AppSelectorImage._previousAppSelector = AppSelectorImage.AppSelector;

                // add new event handler
                if (null != AppSelectorImage.AppSelector)
                {
                    AppSelectorImage.AppSelector.SelectedIDChanged += AppSelectorImage.AppSelector_OnSelectionChanged;
                }
            }
        }

        //private void OnOpacityChanged(object sender, double e)
        //{
        //    double opacity = e;

        //    if (null != _layoutRoot)
        //    {
        //        // correct opacity range
        //        opacity = Math.Max(0.0, opacity);
        //        opacity = Math.Min(1.0, opacity);

        //        // set opacity
        //        _layoutRoot.Opacity = opacity;
        //    }
        //}


        private void AppSelector_OnSelectionChanged(object sender, EventArgs e)
        {
            if (sender is AppSelector selector)
            {
                AppSelector appSelector = (AppSelector)sender;
                this.UpdateUI(appSelector.SelectedID);
            }
        }

        private static void OnSelectedIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a border here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // configure grid
            _layoutRoot.Margin = new Thickness(0);
            _layoutRoot.Padding = new Thickness(0);            
            // generate images
            // create the burgundy image

            Image image = new Image();
            if (this.URIs.Count > 0)
            {
                for (int i = 0; i < this.URIs.Count; i++)
                {

                    image = new Image()
                    {
                        Source = new BitmapImage() { UriSource = new Uri(this.URIs[i].URI), DecodePixelWidth = (int)ImageWidth, DecodePixelHeight = (int)ImageHeight },
                        Width = ImageWidth,
                        Height = ImageHeight,
                        HorizontalAlignment = imageHorizontalAlignment,
                        VerticalAlignment = VerticalAlignment.Top,
                        Opacity = 0.0
                    };
                    Grid.SetRow(image, 0);
                    Grid.SetColumn(image, 0);
                    _layoutRoot.Children.Add(image);
                    this.Images.Add(image);
                }
            }
            else if (this.BitmapImages.Count > 0)
            {
                for (int i = 0; i < this.BitmapImages.Count; i++)
                {

                    image = new Image()
                    {
                        Source = BitmapImages[i],
                        Width = BitmapImages[i].DecodePixelWidth,
                        Height = BitmapImages[i].DecodePixelHeight,
                        HorizontalAlignment = imageHorizontalAlignment,
                        VerticalAlignment = VerticalAlignment.Top,
                        Opacity = 0.0
                    };
                    Grid.SetRow(image, 0);
                    Grid.SetColumn(image, 0);
                    _layoutRoot.Children.Add(image);
                    this.Images.Add(image);
                }
            }
            
            UpdateUI();
        }

        private void UpdateUI(int SelectedID = 0)
        {
            int selectedID = SelectedID;
            if (null != this.AppSelector)
            {
                for (int i = 0; i < this.Images.Count; i++) {
                    if(i == selectedID)
                    {
                        if (HasTransitionAnimation)
                        {
                            TranslateTransition(this.Images[i]);
                        }
                        else
                        {
                            this.Images[i].Opacity = 1;
                        }
                    }
                    
                }
                for (int i = 0; i < this.Images.Count; i++)
                {
                    if (i == this.PreviousSelectedID)
                    {
                        if (HasTransitionAnimation)
                        {
                            AnimationHelper.PerformFadeOut(this.Images[i], 300d);
                        }
                        else
                        {
                            this.Images[i].Opacity = 0;
                        }
                    }
                }

                this.PreviousSelectedID = SelectedID;
            }
        }
        private void TranslateTransition(Image image)
        {

            TranslateDirection TD = TranslateDirection.Left;
            switch (this.TransitionAnimationStyle)
            {
                case TransitionType.TranslateLeft:
                    TD = TranslateDirection.Left;
                    break;
                case TransitionType.TranslateRight:
                    TD = TranslateDirection.Right;
                    break;
            }

            AnimationHelper.PerformFadeIn(image, 500d, 300d);
            AnimationHelper.PerformTranslateIn(image, TD, this.TransitionTranslateDistance, 300d, 300d);
        }
        public bool HasAnimateChildren()
        {
            return false;
        }

        public bool HasPageEntranceAnimation()
        {
            return HasPageEntranceAnimationEnabled;
        }

        public AnimationDirection Direction()
        {
            return PageEntranceDirection;
        }

        public List<UIElement> AnimatableChildren()
        {
            return new List<UIElement>();
        }

        public bool HasPageEntranceTranslation()
        {
            return HasEntranceTranslation;
        }
        #endregion

        #region UI Helpers

        #endregion

        #region Code Helpers

        #endregion
    }
}
