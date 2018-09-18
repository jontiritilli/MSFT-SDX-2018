using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using SDX.Toolkit.Helpers;
using Windows.Foundation;
using Windows.Storage;


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public enum PopupTypes
    {
        None,
        Text,
        Image,
        Video
    }

    public sealed class PopupMedia : Control
    {
        #region Private Members

        // ui elements to track
        Grid _layoutRoot = null;
        Header _header = null;
        ImageEx _image = null;
        LoopPlayer _player = null;

        Storyboard _storyboard = null;

        #endregion

        #region Construction/Destruction

        public PopupMedia()
        {
            this.DefaultStyleKey = typeof(PopupMedia);

            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Dependency Properties

        // Headline
        public static readonly DependencyProperty HeadlineProperty =
            DependencyProperty.Register("Headline", typeof(string), typeof(RadiatingButton), new PropertyMetadata(String.Empty, OnHeadlineChanged));

        public string Headline
        {
            get => (string)GetValue(HeadlineProperty);
            set => SetValue(HeadlineProperty, value);
        }

        // Lede
        public static readonly DependencyProperty LedeProperty =
            DependencyProperty.Register("Lede", typeof(string), typeof(RadiatingButton), new PropertyMetadata(String.Empty, OnLedeChanged));

        public string Lede
        {
            get => (string)GetValue(LedeProperty);
            set => SetValue(LedeProperty, value);
        }

        // PopupType
        public static readonly DependencyProperty PopupTypeProperty =
            DependencyProperty.Register("PopupType", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true, OnPopupTypeChanged));

        public PopupTypes PopupType
        {
            get => (PopupTypes)GetValue(PopupTypeProperty);
            set => SetValue(PopupTypeProperty, value);
        }

        //// MediaSourceStorageFile
        //public static readonly DependencyProperty MediaSourceStorageFileProperty =
        //    DependencyProperty.Register("MediaSourceStorageFile", typeof(StorageFile), typeof(RadiatingButton), new PropertyMetadata(new StorageFile, OnMediaSourceStorageFileChanged));


        //public StorageFile MediaSourceStorageFile
        //{
        //    get => (StorageFile)GetValue(MediaSourceStorageFileProperty);
        //    set => SetValue(MediaSourceStorageFileProperty, value);
        //}

        //// MediaSourceUri
        public static readonly DependencyProperty MediaSourceUriProperty =
            DependencyProperty.Register("MediaSourceUri", typeof(Uri), typeof(RadiatingButton), new PropertyMetadata(null, OnMediaSourceUriChanged));

        public Uri MediaSourceUri
        {
            get => (Uri)GetValue(MediaSourceUriProperty);
            set => SetValue(MediaSourceUriProperty, value);
        }

        public static readonly DependencyProperty MediaHeightProperty =
    DependencyProperty.Register("MediaHeight", typeof(double), typeof(RadiatingButton), new PropertyMetadata(0d));

        public double MediaHeight
        {
            get => (double)GetValue(MediaHeightProperty);
            set => SetValue(MediaHeightProperty, value);
        }


        public static readonly DependencyProperty MediaWidthProperty =
    DependencyProperty.Register("MediaWidth", typeof(double), typeof(RadiatingButton), new PropertyMetadata(0d));

        public double MediaWidth
        {
            get => (double)GetValue(MediaWidthProperty);
            set => SetValue(MediaWidthProperty, value);
        }


        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(false, OnAutoStartChanged));

        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
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


        #region Custom Events


        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartAnimation();
            }
        }

        private void OnSizeChanged(object sender, RoutedEventArgs e)
        {
            ((PopupMedia)sender).UpdateUI();
        }

        private static void OnHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is PopupMedia popup) && (null != popup._header))
            {
                if (e.NewValue is string newValue)
                {
                    popup._header.Headline = newValue;
                }
            }
        }

        private static void OnLedeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is PopupMedia popup) && (null != popup._header))
            {
                if (e.NewValue is string newValue)
                {
                    popup._header.Lede = newValue;
                }
            }
        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnPopupTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnMediaSourceStorageFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnMediaSourceUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");

            // we can't work without it, so return if that failed
            if (null == _layoutRoot) { return; }

            // set the background/border of the grid
            _layoutRoot.Background = StyleHelper.GetAcrylicBrush();
            _layoutRoot.BorderBrush = new SolidColorBrush(Colors.White);
            _layoutRoot.BorderThickness = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupBorder);

            // add rows to the popup; only need to do this if we have an image or video
            if ((PopupTypes.Image == this.PopupType) || (PopupTypes.Video == this.PopupType))
            {
                // header
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // spacer
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(StyleHelper.GetApplicationDouble(LayoutSizes.PopupSpacer))});

                // image/video
                _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // set our Padding
            _layoutRoot.Padding = StyleHelper.GetApplicationThickness(LayoutThicknesses.PopupPadding);

            // figure out our width
            // if the control width is infinity or not a number
            if (Double.IsInfinity(this.Width) || Double.IsNaN(this.Width))
            {
                // use the default popup width
                this.Width = StyleHelper.GetApplicationDouble(LayoutSizes.PopupDefaultWidth);
            }

            // set our width
            _layoutRoot.Width = this.Width;

            // if we autostart, set our opacity to 0 to prepare for that
            if (this.AutoStart) { _layoutRoot.Opacity = 0.0; }

            // create the header
            _header = new Header()
            {
                Headline = this.Headline,
                Lede = this.Lede,
                HeadlineStyle = TextStyles.PopupHeadline,
                LedeStyle = TextStyles.PopupLede,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = this.Width - _layoutRoot.Padding.Left - _layoutRoot.Padding.Right,
            };

            // add it to the layout
            Grid.SetRow(_header, 0);
            _layoutRoot.Children.Add(_header);

            // if this is a popup with an image
            if (this.PopupType == PopupTypes.Image)
            {
                // create the image
                _image = new ImageEx()
                {
                    ImageSource = this.MediaSourceUri.OriginalString,
                    ImageWidth = this.MediaWidth
                };

                // add to the grid
                Grid.SetRow(_image, 2);
                _layoutRoot.Children.Add(_image);
            }
            // if this is a poup with a video
            else if (this.PopupType == PopupTypes.Video)
            {
                // create the loop player and add it to the grid
                _player = new LoopPlayer()
                {
                    MediaSourceUri = this.MediaSourceUri,
                    VideoWidth = this.MediaWidth,
                    VideoHeight = this.MediaHeight
                };

                // add to the grid
                Grid.SetRow(_player, 2);
                _layoutRoot.Children.Add(_player);
            }

        }

        private void UpdateUI()
        {

        }

        #endregion

        #region Code Helpers

        #endregion

        #region UI Helpers



        #endregion
    }
}
