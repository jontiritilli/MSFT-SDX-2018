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
using Windows.Foundation;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace SDX.Toolkit.Controls
{
    public sealed class TextBlockEx : Control
    {
        #region Private Members

        private Grid _layoutRoot = null;
        private TextBlock _text = new TextBlock();

        #endregion

        #region Constructor

        public TextBlockEx()
        {
            this.DefaultStyleKey = typeof(TextBlockEx);
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


        #region Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            // if our textblock child exists
            if (null != _text)
            {
                // tell it to measure
                _text.Measure(availableSize);

                // return its desired size
                return _text.DesiredSize;
            }
            else
            {
                // our textblock doesn't exist, so just return our size
                return base.MeasureOverride(availableSize);
            }
        }

        #endregion


        #region Public Methods

        public void AddInline(Inline inline)
        {
            if ((null != _text) && (null != inline))
            {
                _text.Inlines.Add(inline);
            }
        }

        public bool IsVisible()
        {
            bool visible = false;

            if (null != _layoutRoot)
            {
                visible = (_layoutRoot.Opacity > 0.0);
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
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlockEx), new PropertyMetadata(null, OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // TextStyle
        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register("TextStyle", typeof(TextStyles), typeof(TextBlockEx), new PropertyMetadata(null, OnTextStyleChanged));

        public TextStyles TextStyle
        {
            get { return (TextStyles)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        // TextAlignment
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(TextBlockEx), new PropertyMetadata(TextAlignment.Left, OnTextAlignmentChanged));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        // TextWrapping
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBlockEx), new PropertyMetadata(TextWrapping.WrapWholeWords, OnTextWrappingChanged));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // LineStackingStrategy
        public static readonly DependencyProperty LineStackingStrategyProperty =
            DependencyProperty.Register("LineStackingStrategy", typeof(LineStackingStrategy), typeof(TextBlockEx), new PropertyMetadata(LineStackingStrategy.BlockLineHeight, OnLineStackingStrategyChanged));

        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy)GetValue(LineStackingStrategyProperty); }
            set { SetValue(LineStackingStrategyProperty, value); }
        }

        // LineHeight
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double), typeof(TextBlockEx), new PropertyMetadata(null, OnLineHeightChanged));

        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBlockEx textLine) && (null != textLine._text))
            {
                if (e.NewValue is string newValue)
                {
                    textLine._text.Text = newValue;
                    textLine._text.InvalidateMeasure();
                }
            }
        }

        private static void OnTextAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBlockEx textLine) && (null != textLine._text))
            {
                if (e.NewValue is TextAlignment newValue)
                {
                    textLine._text.TextAlignment = newValue;
                }
            }
        }

        private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBlockEx textLine) && (null != textLine._text))
            {
                if (e.NewValue is TextWrapping newValue)
                {
                    textLine._text.TextWrapping = newValue;
                }
            }
        }

        private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBlockEx textLine) && (null != textLine._text))
            {
                if (e.NewValue is TextStyles newValue)
                {
                    Style style = StyleHelper.GetApplicationStyle(newValue);
                    if (null != style)
                    {
                        textLine._text.Style = style;
                    }
                }
            }
        }

        private static void OnLineStackingStrategyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBlockEx textLine) && (null != textLine._text))
            {
                if (e.NewValue is LineStackingStrategy newValue)
                {
                    textLine._text.LineStackingStrategy = newValue;
                }
            }
        }

        private static void OnLineHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBlockEx textLine) && (null != textLine._text))
            {
                if (e.NewValue is double newValue)
                {
                    textLine._text.LineHeight = newValue;
                }
            }
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

            //// set up grid
            //_layoutRoot.Width = this.Width;

            // create textblock, if needed
            if (null == _text)
            {
                _text = new TextBlock();
            }

            // set properties
            _text.Name = "Text";
            _text.Text = this.Text ?? String.Empty;
            _text.TextAlignment = this.TextAlignment;
            _text.TextWrapping = this.TextWrapping;
            _text.LineStackingStrategy = this.LineStackingStrategy;
            _text.LineHeight = this.LineHeight;
            _text.VerticalAlignment = this.VerticalAlignment;
            _text.HorizontalAlignment = this.HorizontalAlignment;
            //_text.Width = this.Width;
            
            // set its style
            Style style = StyleHelper.GetApplicationStyle(this.TextStyle);
            if (null != style)
            {
                _text.Style = style;
            }

            // add to grid
            Grid.SetRow(_text, 0);
            Grid.SetColumn(_text, 0);
            _layoutRoot.Children.Add(_text);

        }

        #endregion

        #region UI Helpers

        #endregion
    }
}

