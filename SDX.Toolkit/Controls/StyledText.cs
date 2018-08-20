using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using SDX.Toolkit.Helpers;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class StyledText : Control
    {

        #region Private Members

        private Grid _layoutRoot = null;
        private TextBlock _text = null;

        #endregion

        #region Constructor

        public StyledText()
        {
            this.DefaultStyleKey = typeof(StyledText);
            this.Loaded += OnLoaded;

            // inherited dependency property
            new PropertyChangeEventSource<double>(
                this, "Opacity", BindingMode.OneWay).ValueChanged +=
                OnOpacityChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Methods

        public bool IsVisible()
        {
            bool visible = false;

            if (null != _text)
            {
                visible = (_text.Opacity > 0.0);
            }

            return visible;
        }

        public void SetOpacity(double opacity)
        {
            opacity = Math.Max(0.0, opacity);
            opacity = Math.Min(1.0, opacity);

            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = opacity;
            }
        }

        #endregion

        #region Dependency Properties

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FadeInText), new PropertyMetadata(null, OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // ControlStyle
        public static readonly DependencyProperty ControlStyleProperty =
            DependencyProperty.Register("ControlStyle", typeof(ControlStyles), typeof(FadeInText), new PropertyMetadata(null, OnControlStyleChanged));

        public ControlStyles ControlStyle
        {
            get { return (ControlStyles)GetValue(ControlStyleProperty); }
            set { SetValue(ControlStyleProperty, value); }
        }

        // TextAlignment
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(FadeInText), new PropertyMetadata(null, OnTextAlignmentChanged));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnTextAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnControlStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnOpacityChanged(object sender, double e)
        {
            double opacity = e;

            if (null != _layoutRoot)
            {
                // correct opacity range
                opacity = Math.Max(0.0, opacity);
                opacity = Math.Min(1.0, opacity);

                // set opacity
                _layoutRoot.Opacity = opacity;
            }
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the layoutroot
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");
            if (null == _layoutRoot) { return; }

            // set up grid
            _layoutRoot.Width = this.Width;

            // create textblock
            _text = new TextBlock()
            {
                Name = "Text",
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.WrapWholeWords,
                Width = this.Width
            };
            StyleHelper.SetFontCharacteristics(_text, this.ControlStyle);
            Grid.SetRow(_text, 0);
            Grid.SetColumn(_text, 0);
            _layoutRoot.Children.Add(_text);

            // set headline binding
            _text.SetBinding(TextBlock.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath("Text"), Mode = BindingMode.OneWay });
        }

        #endregion

        #region UI Helpers

        #endregion
    }
}

