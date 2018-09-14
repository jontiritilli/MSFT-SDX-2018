using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Helpers;



// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{

    public sealed class ImageEx : Control
    {
        #region Constants

        private const string URI_BASE_APPDATA = "ms-appdata:///local/IdleContent/{0}";

        #endregion


        #region Private Members

        private Border _layoutRoot;
        private Image _image;

        #endregion


        #region Construction

        public ImageEx()
        {
            this.DefaultStyleKey = typeof(ImageEx);

            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion


        #region Public Static Methods

        #endregion


        #region Dependency Properties

        // ImageSource
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageEx), new PropertyMetadata(null, OnImageSourceChanged));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // BitmapImage
        public static readonly DependencyProperty BitmapImageProperty =
        DependencyProperty.Register("BitmapImage", typeof(BitmapImage), typeof(ImageEx), new PropertyMetadata(null, OnBitmapImageChanged));

        public BitmapImage BitmapImage
        {
            get { return (BitmapImage)GetValue(BitmapImageProperty); }
            set { SetValue(BitmapImageProperty, value); }
        }

        // ImageWidth
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageEx), new PropertyMetadata(0d, OnImageWidthChanged));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // TranslateDirection
        public static readonly DependencyProperty TranslateDirectionProperty =
        DependencyProperty.Register("TranslateDirection", typeof(TranslateDirection), typeof(ImageEx), new PropertyMetadata(TranslateDirection.Left, OnTranslateDirectionChanged));

        public TranslateDirection TranslateDirection
        {
            get { return (TranslateDirection)GetValue(TranslateDirectionProperty); }
            set { SetValue(TranslateDirectionProperty, value); }
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEx imageEx)
            {
                imageEx.RenderUI();
            }
        }

        private static void OnBitmapImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEx imageEx)
            {
                imageEx.RenderUI();
            }
        }

        private static void OnImageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEx imageEx)
            {
                imageEx.RenderUI();
            }
        }

        private static void OnTranslateDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout root (a border here)
            _layoutRoot = (Border)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // clear the layout children
            _layoutRoot.Child = null;

            // create the image
            _image = new Image()
            {
                Width = this.ImageWidth,
                HorizontalAlignment = this.HorizontalContentAlignment,
                VerticalAlignment = this.VerticalContentAlignment,
                Opacity = 1.0d,
            };

            // TEST ONLY -- add failed error handler
            _image.ImageFailed += Image_ImageFailed;

            // add the image to the layout root
            _layoutRoot.Child = _image;

            // load the image source
            // ---------------------------------------------
            // do we have a bitmapimage?
            if (null != this.BitmapImage)
            {
                // yes, so just load it
                _image.Source = this.BitmapImage;
            }
            else
            {
                // no bitmapimage provided, so try to use source
                if (!String.IsNullOrWhiteSpace(this.ImageSource))
                {
                    // here are the rules for how this works. you must either provide
                    // 1) a full URI starting with ms-appx:/// or ms-appdata:///, 
                    // OR
                    // 2) a bare filename representing a file in LocalState/IdleContent

                    // if we don't start with one of the required prefixes
                    if (!this.ImageSource.StartsWith(@"ms-appx:") && !this.ImageSource.StartsWith(@"ms-appdata:"))
                    {
                        // convert this to a ms-appdata: uri
                        this.ImageSource = String.Format(URI_BASE_APPDATA, this.ImageSource);
                    }

                    // now we should start with one of the correct prefixes in all cases
                    if (this.ImageSource.StartsWith(@"ms-appx:") || this.ImageSource.StartsWith(@"ms-appdata:"))
                    {
                        // is this an SVG or raster image?
                        if (this.ImageSource.EndsWith(@".svg"))
                        {
                            // this is an SVG image
                            _image.Source = new SvgImageSource() { UriSource = new Uri(this.ImageSource), RasterizePixelWidth = this.ImageWidth };
                        }
                        else
                        {
                            // assume this is a bmp, png, jpg, gif or other supported raster image

                            // Image understands ms-appx: and ms-appdata:, so just set the image source
                            _image.Source = new BitmapImage() { UriSource = new Uri(this.ImageSource), DecodePixelWidth = (int)this.ImageWidth };
                        }
                    }
                }
            }
            // ---------------------------------------------
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // TEST ONLY
            //throw new BadImageFormatException(e.ErrorMessage);
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
