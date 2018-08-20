using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using SDX.Toolkit.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum FloatDirections
    {
        Up,
        Down
    }

    public enum FloatParallax
    {
        Near,
        Middle,
        Far
    }

    public enum FRImageTypes
    {
        Tablet,
        FeatherBottomRight,
        FeatherBottomLeft,
        FeatherTopRight,
        FeatherLeftMost,
        FeatherTopLeft,
        DustMote1,
        DustMote2,
        DustMote3,
        DustMote4,
        DustMote5,
        DustMote6,
        DustMote7,
        DustMote8,
        DustMote9,
        DustMote10,
        DustMote11,
        DustMote12,
        DustMote13,
        DustMote14,
        DustMote15,
        DustMote16,
        DustMote17
    }

    public sealed class FloatingReactiveImage : Control
    {
        #region Constants

        private const string URI_TABLET = @"ms-appx:///Assets/FloatingReactiveImage/floating-tablet.png";

        private const string URI_FEATHER_BOTTOMRIGHT = @"ms-appx:///Assets/FloatingReactiveImage/feather-1.png"; // bottom right
        private const string URI_FEATHER_BOTTOMLEFT = @"ms-appx:///Assets/FloatingReactiveImage/feather-2.png"; // bottom left
        private const string URI_FEATHER_TOPRIGHT = @"ms-appx:///Assets/FloatingReactiveImage/feather-3.png";
        private const string URI_FEATHER_LEFTMOST = @"ms-appx:///Assets/FloatingReactiveImage/feather-4.png";
        private const string URI_FEATHER_TOPLEFT = @"ms-appx:///Assets/FloatingReactiveImage/feather-5.png";

        private const string URI_DUST_1 = @"ms-appx:///Assets/FloatingReactiveImage/dust-1.png";
        private const string URI_DUST_2 = @"ms-appx:///Assets/FloatingReactiveImage/dust-2.png";
        private const string URI_DUST_3 = @"ms-appx:///Assets/FloatingReactiveImage/dust-3.png";
        private const string URI_DUST_4 = @"ms-appx:///Assets/FloatingReactiveImage/dust-4.png";
        private const string URI_DUST_5 = @"ms-appx:///Assets/FloatingReactiveImage/dust-5.png";
        private const string URI_DUST_6 = @"ms-appx:///Assets/FloatingReactiveImage/dust-6.png";
        private const string URI_DUST_7 = @"ms-appx:///Assets/FloatingReactiveImage/dust-7.png";
        private const string URI_DUST_8 = @"ms-appx:///Assets/FloatingReactiveImage/dust-8.png";
        private const string URI_DUST_9 = @"ms-appx:///Assets/FloatingReactiveImage/dust-9.png";
        private const string URI_DUST_10 = @"ms-appx:///Assets/FloatingReactiveImage/dust-10.png";
        private const string URI_DUST_11 = @"ms-appx:///Assets/FloatingReactiveImage/dust-11.png";
        private const string URI_DUST_12 = @"ms-appx:///Assets/FloatingReactiveImage/dust-12.png";
        private const string URI_DUST_13 = @"ms-appx:///Assets/FloatingReactiveImage/dust-13.png";
        private const string URI_DUST_14 = @"ms-appx:///Assets/FloatingReactiveImage/dust-14.png";
        private const string URI_DUST_15 = @"ms-appx:///Assets/FloatingReactiveImage/dust-15.png";
        private const string URI_DUST_16 = @"ms-appx:///Assets/FloatingReactiveImage/dust-16.png";
        private const string URI_DUST_17 = @"ms-appx:///Assets/FloatingReactiveImage/dust-17.png";

        public static readonly int Z_CONTROLS = 100;
        public static readonly int Z_TABLET = 90;
        public static readonly int Z_FEATHER = 20;
        public static readonly int Z_DUST = 10;
        public static readonly int Z_SHADOW = 0;


        #endregion


        #region Private Members

        private Border _layoutRoot;
        private Image _image;
        private Storyboard _storyboard;

        #endregion


        #region Construction

        public FloatingReactiveImage()
        {
            this.DefaultStyleKey = typeof(FloatingReactiveImage);

            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Static Methods

        public static FloatingReactiveImage CreateRandomDust(double minX, double minY, double maxX, double maxY, bool autoStart)
        {
            Random random = new Random();

            //FloatDirections floatDirection = (FloatDirections)(Math.Round(random.NextDouble()));
            //double floatDepth = Math.Round(random.NextDouble()*10, 0);
            FRImageTypes fRImageType = (FRImageTypes)(random.Next(16) + (int)FRImageTypes.DustMote1);

            double x = ((maxX - minX) * random.NextDouble()) + minX;
            double y = ((maxY - minY) * random.NextDouble()) + minY;

            FloatingReactiveImage dust = new FloatingReactiveImage()
            {
                FRImageType = fRImageType,
                AutoStart = autoStart,
                DurationInMilliseconds = 1000 + (random.NextDouble() * 1000),
                StaggerDelayInMilliseconds = (random.NextDouble() * 900)
            };
            Canvas.SetLeft(dust, x);
            Canvas.SetTop(dust, y);
            Canvas.SetZIndex(dust, FloatingReactiveImage.Z_DUST);

            return dust;
        }

        #endregion

        #region Dependency Properties


        // FRImageType
        public static readonly DependencyProperty FRImageTypeProperty =
        DependencyProperty.Register(nameof(FRImageType), typeof(FRImageTypes), typeof(FloatingReactiveImage), new PropertyMetadata(FRImageTypes.FeatherBottomRight, OnFRImageTypeChanged));

        public FRImageTypes FRImageType
        {
            get { return (FRImageTypes)GetValue(FRImageTypeProperty); }
            set { SetValue(FRImageTypeProperty, value); }
        }

        // FloatDepth
        public static readonly DependencyProperty FloatDepthProperty =
            DependencyProperty.Register("FloatDepth", typeof(double), typeof(FloatingReactiveImage), new PropertyMetadata(10, OnFloatDepthChanged));

        public double FloatDepth
        {
            get { return (double)GetValue(FloatDepthProperty); }
            set { SetValue(FloatDepthProperty, value); }
        }

        // FloatDirection
        public static readonly DependencyProperty FloatDirectionProperty =
            DependencyProperty.Register("FloatDirection", typeof(FloatDirections), typeof(FloatingReactiveImage), new PropertyMetadata(FloatDirections.Down, OnFloatDirectionChanged));

        public FloatDirections FloatDirection
        {
            get { return (FloatDirections)GetValue(FloatDirectionProperty); }
            set { SetValue(FloatDirectionProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(FloatingReactiveImage), new PropertyMetadata(2000d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(FloatingReactiveImage), new PropertyMetadata(0d, OnDurationInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(FloatingReactiveImage), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        #endregion

        #region Public Methods

        public void StartAnimation()
        {
            if (null != _storyboard)
            {
                _storyboard.Begin();
            }
        }

        public void ResetAnimation()
        {
            if (null != _storyboard)
            {
                _storyboard.Stop();
            }
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartAnimation();
            }
        }

        private static void OnFRImageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FloatingReactiveImage)d).RenderUI();
        }

        private static void OnFloatDepthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnFloatDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            string sourcePath = String.Empty;
            double width = 0d;

            // which source image are we using?
            switch (this.FRImageType)
            {
                case FRImageTypes.Tablet:
                    sourcePath = URI_TABLET;
                    width = 750d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 4;
                    break;

                case FRImageTypes.FeatherBottomRight:
                    sourcePath = URI_FEATHER_BOTTOMRIGHT;
                    width = 125d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 7;
                    break;

                case FRImageTypes.FeatherBottomLeft:
                    sourcePath = URI_FEATHER_BOTTOMLEFT;
                    width = 150d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 9;
                    break;

                case FRImageTypes.FeatherTopRight:
                    sourcePath = URI_FEATHER_TOPRIGHT;
                    width = 200d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 2;
                    break;

                case FRImageTypes.FeatherLeftMost:
                    sourcePath = URI_FEATHER_LEFTMOST;
                    width = 100d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 3;
                    break;

                case FRImageTypes.FeatherTopLeft:
                    sourcePath = URI_FEATHER_TOPLEFT;
                    width = 150d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 3;
                    break;

                case FRImageTypes.DustMote1:
                    sourcePath = URI_DUST_1;
                    width = 10d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote2:
                    sourcePath = URI_DUST_2;
                    width = 10d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 10;
                    break;

                case FRImageTypes.DustMote3:
                    sourcePath = URI_DUST_3;
                    width = 5d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote4:
                    sourcePath = URI_DUST_4;
                    width = 10d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 10;
                    break;

                case FRImageTypes.DustMote5:
                    sourcePath = URI_DUST_5;
                    width = 10d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 8;
                    break;

                case FRImageTypes.DustMote6:
                    sourcePath = URI_DUST_6;
                    width = 15d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote7:
                    sourcePath = URI_DUST_7;
                    width = 45d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 10;
                    break;

                case FRImageTypes.DustMote8:
                    sourcePath = URI_DUST_8;
                    width = 25d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 8;
                    break;

                case FRImageTypes.DustMote9:
                    sourcePath = URI_DUST_9;
                    width = 15d;
                    this.FloatDirection = FloatDirections.Up;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote10:
                    sourcePath = URI_DUST_10;
                    width = 5d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote11:
                    sourcePath = URI_DUST_11;
                    width = 10d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote12:
                    sourcePath = URI_DUST_12;
                    width = 15d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 12;
                    break;

                case FRImageTypes.DustMote13:
                    sourcePath = URI_DUST_13;
                    width = 5d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 5;
                    break;

                case FRImageTypes.DustMote14:
                    sourcePath = URI_DUST_14;
                    width = 10d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 9;
                    break;

                case FRImageTypes.DustMote15:
                    sourcePath = URI_DUST_15;
                    width = 15d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 7;
                    break;

                case FRImageTypes.DustMote16:
                    sourcePath = URI_DUST_16;
                    width = 5d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 9;
                    break;

                case FRImageTypes.DustMote17:
                    sourcePath = URI_DUST_17;
                    width = 15d;
                    this.FloatDirection = FloatDirections.Down;
                    this.FloatDepth = 5;
                    break;

                default:
                    sourcePath = String.Empty;
                    width = 0d;
                    break;
            }

            // if we have a valid image
            if ((!String.IsNullOrWhiteSpace(sourcePath)) && (width > 0))
            {
                // create the image
                _image = new Image()
                {
                    Width = width,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Source = new BitmapImage(new Uri(sourcePath))
                };

                // Create the translatetransform and attach to the image
                TranslateTransform _imageTranslate = new TranslateTransform
                {
                    X = 0,
                    Y = 0
                };
                _image.RenderTransform = _imageTranslate;

                // set up the animation
                //_storyboard = AnimationHelper.CreateStandardAnimation(_image, "(Image.RenderTransform).(TranslateTransform.Y)", 0.0, 0.0, this.FloatDepth * ((FloatDirections.Down == this.FloatDirection) ? -1 : 1), this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, true, true, new RepeatBehavior(1d));
                _storyboard = AnimationHelper.CreateEasingAnimation(_image, "(Image.RenderTransform).(TranslateTransform.Y)", 0.0, 0.0, this.FloatDepth * ((FloatDirections.Down == this.FloatDirection) ? -1 : 1), new SineEase() { EasingMode = EasingMode.EaseIn }, new SineEase() { EasingMode = EasingMode.EaseOut }, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, true, true, new RepeatBehavior(1d));

                // add the image to the layout root
                _layoutRoot.Child = _image;
            }
        }

        #endregion


        #region UI Helpers

        #endregion


        #region Code Helpers

        #endregion
    }
}
