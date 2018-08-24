using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using SDX.Toolkit.Helpers;

namespace SDX.Toolkit.Controls
{
    public sealed class SurfaceDial : UserControl
    {
        #region Constants

        private readonly string URI_COLOR_WHEEL = "ms-appx:///Assets/custom_visual_color_wheel.png";
        private readonly string TEST_STRING = "TESTING PAINT ON SCREEN";

        #endregion

        #region Private Members

        private byte[] bmpBytes = null;
        private BitmapDecoder dec;
        private Grid _layoutRoot;
        private Canvas _dialCanvas;
        private Grid _dialGrid;
        private Image _dialImage;
        private TextBlock _testString;

        #endregion

        #region Private Methods

        private async void UpdateColorBrush()
        {
            int x = 0, y = 0;

            double angle = (360 - Rotation) * Math.PI / 180;
            double radius = 295;
            //get the co ords of the pixel based on rotation
            //x = radius cos t;
            //y = radius sin t;
            x = 350 + (int)(radius * Math.Cos(angle));
            y = 353 + (int)(radius * Math.Sin(angle));

            Color color = await GetSelectedColor(x, y);

            Debug.WriteLine($"X: {x}, Y: {y} rotation: {Rotation} Color: {color.R}:{color.G}:{color.B}");

            ColorBrush.Color = color;
        }

        private async Task<Color> GetSelectedColor(int x, int y)
        {
            Color color = new Color();
            if (null == bmpBytes)
            {
                string imagelocation = @"Assets\custom_visual_color_wheel.png";
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile file = await InstallationFolder.GetFileAsync(imagelocation);
                Stream imagestream = await file.OpenStreamForReadAsync();

                dec = await BitmapDecoder.CreateAsync(imagestream.AsRandomAccessStream());

                var data = await dec.GetPixelDataAsync();

                bmpBytes = data.DetachPixelData();
            }
            color = GetPixel(bmpBytes, x, y, dec.PixelWidth, dec.PixelHeight);

            return color;
        }

        #endregion

        #region Public Methods

        public Color GetPixel(byte[] pixels, int x, int y, uint width, uint height)
        {
            int i = y;
            int j = x;
            int k = (i * (int)width + j) * 4;
            var b = pixels[k + 0];
            var g = pixels[k + 1];
            var r = pixels[k + 2];
            return Color.FromArgb(255, r, g, b);
        }

        #endregion

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for ColorBrush
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(SurfaceDial), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        // Using a DependencyProperty as the backing store for Rotation
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(SurfaceDial), new PropertyMetadata(0.0));
        
        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); UpdateColorBrush(); }
        }

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush)GetValue(ColorBrushProperty); }
            set { SetValue(ColorBrushProperty, value); }
        }

        #endregion

        #region Constructor

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Event Handlers



        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layout base (a grid here)
            if (null == _dialCanvas) { _dialCanvas = (Canvas)this.GetTemplateChild("DialCanvas"); }

            // if we can't get the layout root, we can't do anything
            if (null == _dialCanvas) { return; }

            // add test string to screen
            _testString = new TextBlock()
            {
                Text = this.TEST_STRING
            };

            // create the image
            _dialImage = new Image()
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/custom_visual_color_wheel.png")),
                Name = "ColorRingImage",
                Width = 500,
                Height = 500
            };

            _dialCanvas.Children.Add(_testString);
        }
        
        #endregion
    }
}
