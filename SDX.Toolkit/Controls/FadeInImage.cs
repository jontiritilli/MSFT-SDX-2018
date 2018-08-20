using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

using SDX.Toolkit.Helpers;
using SDX.Toolkit.Services;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.Storage.Streams;



// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class FadeInImage : Control
    {
        #region Constants

        #endregion


        #region Private Members

        private Border _layoutRoot;
        private Image _image;
        private Storyboard _storyboardFadeIn;
        private Storyboard _storyboardFadeOut;

        private DispatcherTimer _timer = null;
        private int _dispatchCount = 0;

        #endregion


        #region Construction

        public FadeInImage()
        {
            this.DefaultStyleKey = typeof(FadeInImage);

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
            DependencyProperty.Register("ImageSource", typeof(string), typeof(FadeInImage), new PropertyMetadata(null, OnImageSourceChanged));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // ImageWidth
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(FadeInImage), new PropertyMetadata(0d, OnImageWidthChanged));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // FadeInCompletedHandler
        public static readonly DependencyProperty FadeInCompletedHandlerProperty =
            DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(FadeInImage), new PropertyMetadata(null));

        public EventHandler<object> FadeInCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
            set { SetValue(FadeInCompletedHandlerProperty, value); }
        }

        // FadeOutCompletedHandler
        public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
            DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(FadeInImage), new PropertyMetadata(null));

        public EventHandler<object> FadeOutCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
            set { SetValue(FadeOutCompletedHandlerProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(FadeInImage), new PropertyMetadata(2000d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(FadeInImage), new PropertyMetadata(0d, OnDurationInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(FadeInImage), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Public Methods

        public void StartFadeIn()
        {
            // if we're not rendered yet
            if (null == _storyboardFadeIn)
            {
                // inc the counter
                _dispatchCount++;

                // limit the number of times we do this
                if (_dispatchCount < 10)
                {
                    // create a timer
                    if (null == _timer)
                    {
                        _timer = new DispatcherTimer()
                        {
                            Interval = TimeSpan.FromMilliseconds(500d)
                        };
                        _timer.Tick += DispatcherTimer_Tick;
                    }

                    // start it
                    _timer.Start();
                }

                // return
                return;
            }

            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Begin();
            }
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _timer) { _timer.Stop(); }

            // call the method that sets up the timer
            this.StartFadeIn();
        }

        public void StartFadeOut()
        {
            if (null != _storyboardFadeOut)
            {
                _storyboardFadeOut.Begin();
            }
        }

        public void ResetAnimation()
        {
            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Stop();
                _storyboardFadeOut.Stop();
                _image.Opacity = 0d;
            }
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartFadeIn();
            }
        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnImageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion


        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a canvas here)
            _layoutRoot = (Border)this.GetTemplateChild("LayoutRoot");

            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot)
            {
                return;
            }

            // if we have a valid image source and width
            if (!String.IsNullOrWhiteSpace(this.ImageSource))
            {
                BitmapImage bitmapImage = null;

                // if the image source doesn't start with "ms-appx:", then we need to look for it in the
                // local folder and load it from there.
                // set image source; if the image is an app asset
                if (this.ImageSource.StartsWith(@"ms-appx:"))
                {
                    // just set the image source
                    bitmapImage = new BitmapImage() { UriSource = new Uri(this.ImageSource), DecodePixelWidth = (int)this.ImageWidth };
                }
                else
                {
                    // get a reference to the file from local storage (IdleContent)
                    StorageFile bitmapFile = ConfigurationService.Current.GetFileFromLocalStorage(this.ImageSource);

                    // if we got it
                    if (null != bitmapFile)
                    {
                        bitmapImage = new BitmapImage() { DecodePixelWidth = (int)this.ImageWidth };

                        // otherwise we need to load from the filesystem
                        AsyncHelper.RunSync(() => LoadBitmapFromFileAsync(bitmapImage, bitmapFile));
                    }
                }

                // create the image
                _image = new Image()
                {
                    Source = bitmapImage,
                    Width = this.ImageWidth,
                    HorizontalAlignment = this.HorizontalContentAlignment,
                    VerticalAlignment = this.VerticalContentAlignment,
                    Opacity = 0.0d
                };
                //_image.ImageFailed += this.Image_ImageFailed;

                // add the image to the layout root
                _layoutRoot.Child = _image;

                // set up animation
                _storyboardFadeIn = AnimationHelper.CreateEasingAnimationWithNotify(_image, this.FadeInCompletedHandler,
                                                                "Opacity", 0.0, 0.0, 1.0, null, null, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds,
                                                                false, false, new RepeatBehavior(1d));

                _storyboardFadeOut = AnimationHelper.CreateEasingAnimationWithNotify(_image, this.FadeOutCompletedHandler,
                                                "Opacity", 1.0, 1.0, 0.0, null, null, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds,
                                                false, false, new RepeatBehavior(1d));

            }
        }

        private async Task LoadBitmapFromFileAsync(BitmapImage bitmapImage, StorageFile imageFile)
        {
            if ((null == bitmapImage) || (null == imageFile))
            {
                return;
            }

            // set the bitmap source on the UI thread
            bitmapImage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                // create a stream for the image file
                using (IRandomAccessStream fileStream = await imageFile.OpenReadAsync())
                {
                    // set the image source
                    bitmapImage.SetSourceAsync(fileStream);
                }
            });
        }

        //private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
