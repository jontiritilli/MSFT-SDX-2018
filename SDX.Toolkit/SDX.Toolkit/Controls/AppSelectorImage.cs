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

    public sealed class AppSelectorImage : Control
    {
        #region Private Constants

        private static readonly Size BOUNDS = WindowHelper.GetViewSizeInfo();

        private static readonly double WIDTH_ORIGINAL = BOUNDS.Width;
        private static readonly double HEIGHT_ORIGINAL = BOUNDS.Height;


        #endregion


        #region Private Members

        public Grid _layoutRoot = null;
        private List<Image> Images;

        private AppSelector _previousAppSelector = null;

        //private Storyboard _storyboardFadeIn = null;
        //private Storyboard _storyboardFadeOut = null;

        #endregion


        #region Construction

        public AppSelectorImage()
        {
            this.DefaultStyleKey = typeof(AppSelectorImage);

            this.Loaded += OnLoaded;
            this.Images = new List<Image>();
            this.Opacity = 0;
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

        // ImageStyle
        public static readonly DependencyProperty SelectedIDProperty =
            DependencyProperty.Register("SelectedID", typeof(int), typeof(AppSelectorImage), new PropertyMetadata(0, OnSelectedIDChanged));


        public int SelectedID
        {
            get { return ( (int)GetValue(SelectedIDProperty) ); }
            set { SetValue(SelectedIDProperty, value); }
        }

        public static readonly DependencyProperty URIsProperty =
        DependencyProperty.Register("URIs", typeof(List<AppSelectorImageURI>), typeof(AppSelectorImage), new PropertyMetadata(new List<AppSelectorImageURI>(), OnSelectedIDChanged));


        public List<AppSelectorImageURI> URIs
        {
            get { return ((List<AppSelectorImageURI>)GetValue(URIsProperty)); }
            set { SetValue(URIsProperty, value); }
        }


        public static readonly DependencyProperty ImageWidthProperty =
        DependencyProperty.Register("ImageWidth", typeof(double), typeof(AppSelectorImage), new PropertyMetadata(0d, OnSelectedIDChanged));


        public double ImageWidth
        {
            get { return ((double)GetValue(ImageWidthProperty)); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
        DependencyProperty.Register("ImageHeight", typeof(double), typeof(AppSelectorImage), new PropertyMetadata(0d, OnSelectedIDChanged));


        public double ImageHeight
        {
            get { return ((double)GetValue(ImageHeightProperty)); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty imageHorizontalAlignmentProperty =
        DependencyProperty.Register("imageHorizontalAlignment", typeof(HorizontalAlignment), typeof(AppSelectorImage), new PropertyMetadata(HorizontalAlignment.Center, OnSelectedIDChanged));


        public HorizontalAlignment imageHorizontalAlignment
        {
            get { return ((HorizontalAlignment)GetValue(imageHorizontalAlignmentProperty)); }
            set { SetValue(imageHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty BitmapImagesProperty =
        DependencyProperty.Register("BitmapImages", typeof(List<BitmapImage>), typeof(AppSelectorImage), new PropertyMetadata(new List<BitmapImage>()));


        public List<BitmapImage> BitmapImages
        {
            get { return ((List<BitmapImage>)GetValue(BitmapImagesProperty)); }
            set { SetValue(BitmapImagesProperty, value); }
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
                        this.Images[i].Opacity = 1;
                    }
                    
                }
                for (int i = 0; i < this.Images.Count; i++)
                {
                    if (i != selectedID)
                    {
                        this.Images[i].Opacity = 0;
                    }
                }
                
                //ColorSelectorColors newColor = (ColorSelectorColors.None == forcedColor) ? this.ColorSelector.SelectedColor : forcedColor;

                //// change images in two steps to avoid flashing

                //// make the new color visible
                //_imageBurgundy.Opacity = (ColorSelectorColors.Burgundy == newColor) ? 1.0 : _imageBurgundy.Opacity;
                //_imageCobalt.Opacity = (ColorSelectorColors.Cobalt == newColor) ? 1.0 : _imageCobalt.Opacity;
                //_imageBlack.Opacity = (ColorSelectorColors.Black == newColor) ? 1.0 : _imageBlack.Opacity;
                //_imageSilver.Opacity = (ColorSelectorColors.Silver == newColor) ? 1.0 : _imageSilver.Opacity;

                //// hide the old color
                //_imageBurgundy.Opacity = (ColorSelectorColors.Burgundy != newColor) ? 0.0 : _imageBurgundy.Opacity;
                //_imageCobalt.Opacity = (ColorSelectorColors.Cobalt != newColor) ? 0.0 : _imageCobalt.Opacity;
                //_imageBlack.Opacity = (ColorSelectorColors.Black != newColor) ? 0.0 : _imageBlack.Opacity;
                //_imageSilver.Opacity = (ColorSelectorColors.Silver != newColor) ? 0.0 : _imageSilver.Opacity;
            }
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
