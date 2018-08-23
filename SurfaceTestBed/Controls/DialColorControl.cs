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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SurfaceTestBed.Helpers;
using SurfaceTestBed.Services;

namespace SurfaceTestBed.Controls
{
    public sealed partial class DialColorControl : UserControl
    {
        #region Private Members

        private byte[] bmpBytes = null;
        private BitmapDecoder dec;
        private Border _layoutRoot;
        private Canvas _dialRoot;
        private Grid _dialGrid;
        private Image _dialImage;

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
                string imagelocation = @"Assets\custom_visual_colour_wheel.png";
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

        // Using a DependencyProperty as the backing store for ColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(DialColorControl), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(DialColorControl), new PropertyMetadata(0.0));

        // ImageSource
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(DialColorControl), new PropertyMetadata(null, OnImageSourceChanged));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // ImageWidth
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(DialColorControl), new PropertyMetadata(0d, OnImageWidthChanged));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        #endregion

        #region UI Methods

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
                _dialImage = new Image()
                {
                    Source = bitmapImage,
                    Name = "ColorRingImage",
                    Width = this.ImageWidth,
                    HorizontalAlignment = this.HorizontalContentAlignment,
                    VerticalAlignment = this.VerticalContentAlignment,
                    Opacity = 0.0d
                };


                // add the image to the layout root
                _layoutRoot.Child = _dialImage;
            }
            //    <Grid>
            //        <Image x:Name="ColorRingImage"  Source="ms-appx:///Assets/custom_visual_colour_wheel.png" RenderTransformOrigin="0.5,0.5" >
            //            <Image.RenderTransform>
            //                <CompositeTransform Rotation = "{Binding Rotation, ElementName=colorControl}" />
            //            </ Image.RenderTransform >
            //        </ Image >
            //    </ Grid >
            #endregion
        }
}
