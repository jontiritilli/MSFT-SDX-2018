using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;


namespace SDX.Toolkit.Controls
{
    public sealed partial class AnimatedIcon : UserControl
    {
        #region Private Constants

        #endregion

        #region Private Members

        Image _iconImage = null;
        List<BitmapImage> _bitmapImages = new List<BitmapImage>();
        Storyboard _storyboard;

        #endregion

        #region Construction/Initialization

        public AnimatedIcon()
        {
            this.InitializeComponent();

            // no focus visual indications
            this.UseSystemFocusVisuals = false;

            // event handlers
            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion


        #region Public Properties

        public GridLength IconWidthGL { get { return new GridLength(this.IconWidth); } }

        #endregion


        #region Public Methods

        public void Start()
        {
            if (null != _storyboard)
            {
                _storyboard.Begin();
            }
        }

        public void Pause()
        {
            if (null != _storyboard)
            {
                _storyboard.Pause();
            }
        }

        public void Stop()
        {
            if (null != _storyboard)
            {
                _storyboard.Stop();
            }
        }

        #endregion


        #region Dependency Properties

        // IconUriStrings
        public static readonly DependencyProperty IconUriStringsProperty =
            DependencyProperty.Register("IconUriStrings", typeof(List<string>), typeof(AnimatedIcon), new PropertyMetadata(null, OnIconUriStringsChanged));

        public List<string> IconUriStrings
        {
            get { return (List<string>)GetValue(IconUriStringsProperty); }
            set { SetValue(IconUriStringsProperty, value); }
        }

        // IconWidth
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(AnimatedIcon), new PropertyMetadata(20, OnIconWidthChanged));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        // IconIndex
        public static readonly DependencyProperty IconIndexProperty =
            DependencyProperty.Register("IconIndex", typeof(int), typeof(AnimatedIcon), new PropertyMetadata(0, OnIconIndexChanged));

        public int IconIndex
        {
            get { return (int)GetValue(IconIndexProperty); }
            set { SetValue(IconIndexProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(AnimatedIcon), new PropertyMetadata(false));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        // FrameDurationInMilliseconds
        public static readonly DependencyProperty FrameDurationInMillisecondsProperty =
            DependencyProperty.Register("FrameDurationInMilliseconds", typeof(double), typeof(AnimatedIcon), new PropertyMetadata(30, OnFrameDurationInMillisecondsChanged));

        public double FrameDurationInMilliseconds
        {
            get { return (double)GetValue(FrameDurationInMillisecondsProperty); }
            set { SetValue(FrameDurationInMillisecondsProperty, value); }
        }

        #endregion


        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnIconUriStringsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedIcon ai)
            {
                ai.RenderUI();
            }
        }

        private static void OnIconWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedIcon ai)
            {
                ai.RenderUI();
            }
        }

        private static void OnIconIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedIcon ai)
            {
                ai.UpdateUI();
            }
        }

        private static void OnFrameDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedIcon ai)
            {
                ai.RenderUI();
            }
        }

        #endregion


        #region UI Methods

        private void RenderUI()
        {
            // can't render without icon uris
            if ((null == this.IconUriStrings) || (0 == this.IconUriStrings.Count)) { return; }

            // do we have a storyboard?
            if (null != _storyboard)
            {
                // stop it before we create a new one
                _storyboard.Stop();
            }

            // clear the bitmapimages list
            _bitmapImages = new List<BitmapImage>();

            // do we have a layout root?
            if (null != this.LayoutRoot)
            {
                // clear its contents
                this.LayoutRoot.Children.Clear();

                // set its properties
                this.LayoutRoot.BorderThickness = new Thickness(0);
                this.LayoutRoot.Margin = new Thickness(0);
                this.LayoutRoot.Padding = new Thickness(0);
                this.LayoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                this.LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(this.IconWidth) });

                // create our image control
                _iconImage = new Image()
                {
                    Width = this.IconWidth,
                    Margin = new Thickness(0),
                };
                Grid.SetRow(_iconImage, 0);
                Grid.SetColumn(_iconImage, 0);
                this.LayoutRoot.Children.Add(_iconImage);

                // loop through our image URIs
                foreach (string iconUriString in this.IconUriStrings)
                {
                    // if it's not null
                    if (!String.IsNullOrWhiteSpace(iconUriString))
                    {
                        // create a BitmapImage
                        BitmapImage bitmapImage = new BitmapImage() { UriSource = new Uri(iconUriString), DecodePixelWidth = (int)this.IconWidth };

                        // save it in the list
                        _bitmapImages.Add(bitmapImage);
                    }
                }

                // set up the ImageEx control
                if (_bitmapImages.Count > 0)
                {
                    // set first image
                    _iconImage.Source = _bitmapImages[0];

                    // create the storyboard
                    _storyboard = this.CreateStoryboard(_bitmapImages.Count, this.FrameDurationInMilliseconds);

                    // if we're on autoplay, start it
                    if (this.AutoStart)
                    {
                        _storyboard.Begin();
                    }
                }
            }
        }

        private void UpdateUI()
        {
            // do we have all the things we need?
            if ((null != _bitmapImages) && (_bitmapImages.Count > 0) && (null != _iconImage))
            {
                // verify that the index is valid
                if ((0 <= this.IconIndex) && (this.IconIndex < _bitmapImages.Count))
                {
                    // update the bitmap that IconImage is using
                    _iconImage.Source = _bitmapImages[this.IconIndex];
                }
            }
        }

        private Storyboard CreateStoryboard(int frameCount, double frameDuration)
        {
            // set up the Storyboard
            Storyboard storyboard = new Storyboard()
            {
                AutoReverse = false,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // set the target of the storyboard
            Storyboard.SetTarget(storyboard, this);
            Storyboard.SetTargetProperty(storyboard, "IconIndex");

            // create the key frames collection
            ObjectAnimationUsingKeyFrames frames = new ObjectAnimationUsingKeyFrames()
            {
                Duration = TimeSpan.FromMilliseconds(frameCount * frameDuration),
                RepeatBehavior = RepeatBehavior.Forever,
            };

            // add it to the storyboard
            storyboard.Children.Add(frames);

            // add the children to the frames collection
            for (int i = 0; i< frameCount; i++)
            {
                frames.KeyFrames.Add(new DiscreteObjectKeyFrame()
                {
                    KeyTime = TimeSpan.FromMilliseconds(i * frameDuration),
                    Value = i,
                });
            }

            return storyboard;
        }

        #endregion
    }
}
