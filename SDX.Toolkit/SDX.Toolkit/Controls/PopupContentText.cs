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


// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class PopupContentText : Control
    {
        #region Private Members

        // ui elements to track
        Grid _layoutRoot = null;
        TextBlock _block = null;

        Storyboard _storyboard = null;

        #endregion

        #region Construction/Destruction

        public PopupContentText()
        {
            this.DefaultStyleKey = typeof(PopupContentText);

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

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(RadiatingButton), new PropertyMetadata(String.Empty, OnTextChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(RadiatingButton), new PropertyMetadata(true, OnAutoStartChanged));

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
            ((PopupContentText)sender).UpdateUI();
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

            // set the background of the grid
            _layoutRoot.Background = new SolidColorBrush(Colors.LightGray) { Opacity = StyleHelper.PopupBackgroundOpacity };
            _layoutRoot.BorderBrush = new SolidColorBrush(Colors.White);
            _layoutRoot.BorderThickness = new Thickness(0, 3, 0, 0);
            if (this.AutoStart) { _layoutRoot.Opacity = 0.0; }

            // set the width of the grid
            if (!Double.IsInfinity(this.Width) && !Double.IsNaN(this.Width))
            {
                _layoutRoot.Width = this.Width;
                //_layoutRoot.MaxWidth = this.Width;
            }

            // create the textblock
            _block = new TextBlock()
            {
                TextWrapping = TextWrapping.WrapWholeWords,
                Margin = new Thickness(30),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            StyleHelper.SetFontCharacteristics(_block, ControlStyles.PopupText);
            _block.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("Text"), Mode = BindingMode.OneWay });

            // add it to the layout
            _layoutRoot.Children.Add(_block);

            // set up the entrance animation
            _storyboard = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 0.0, 0.0, 1.0, 200d, 0d, false, false, new RepeatBehavior(1d));
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
