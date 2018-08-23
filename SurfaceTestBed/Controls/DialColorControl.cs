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
using Windows.UI.Xaml.Navigation;

namespace SurfaceTestBed.Controls
{
    public sealed partial class DialColorControl : UserControl
    {
        #region Private Members
        private bool rightHanded;
        private byte[] bmpBytes = null;
        private BitmapDecoder dec;
        private Border _layoutRoot = null;
        private Grid _layoutGrid = null;
        private UserControl _userControl = null;

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

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(DialColorControl), new PropertyMetadata(0.0));

        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush)GetValue(ColorBrushProperty); }
            set { SetValue(ColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(DialColorControl), new PropertyMetadata(new SolidColorBrush(Colors.White)));

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

        //public DialColorControl()
        //{
        //    //this.InitializeComponent();
        //    //handedness for on-screen UI
        //    Windows.UI.ViewManagement.UISettings settings = new Windows.UI.ViewManagement.UISettings();
        //    rightHanded = (settings.HandPreference == Windows.UI.ViewManagement.HandPreference.RightHanded);
        //    //if left handed then rotate the entire control 180
        //    if (!rightHanded)
        //    {
        //        RotateTransform trans = new RotateTransform();
        //        trans.Angle = 180;
        //        colorControl.RenderTransform = trans;
        //    }
        //}

        #endregion

        #region UI Methods

        public RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Canvas)this.GetTemplateChild("dialCanvas");
            if (null == _layoutRoot) { return; }

            UserControl dialDisplay = new UserControl();
            _layoutRoot.Children.Add(UserControl dialDisplay)
        }
        <UserControl x:Name="colorControl"
            x:Class="DialDemo.Controls.DialColorControl"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
            mc:Ignorable="d"
            d:DesignHeight="300"
            d:DesignWidth="300"
            RenderTransformOrigin="0.5,0.5">

            <Grid>
                <Image x:Name="ColorRingImage"  Source="ms-appx:///Assets/custom_visual_colour_wheel.png" RenderTransformOrigin="0.5,0.5" >
                    <Image.RenderTransform>
                        <CompositeTransform Rotation = "{Binding Rotation, ElementName=colorControl}" />
                    </ Image.RenderTransform >
                </ Image >
            </ Grid >
        </ UserControl >
        #endregion
    }
}
