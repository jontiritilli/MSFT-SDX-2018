﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using SDX.Toolkit.Helpers;


namespace SDX.Toolkit.Controls
{
    public sealed partial class TextBlockEx : UserControl
    {
        #region Constructor

        public TextBlockEx()
        {
            this.InitializeComponent();

            this.Loaded += OnLoaded;

            // inherited dependency property
            new PropertyChangeEventSource<double>(
                this, "Opacity", BindingMode.OneWay).ValueChanged +=
                OnOpacityChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion


        #region Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            // if our textblock child exists
            if (null != this.TheText)
            {
                // tell it to measure
                this.TheText.Measure(availableSize);

                // return its desired size
                return this.TheText.DesiredSize;
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
            if (null != this.TheText)
            {
                this.TheText.Inlines.Add(inline);
            }
        }

        public void SetOpacity(double opacity)
        {
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }
            this.Opacity = opacity;
            if (null != this.LayoutRoot)
            {
                this.LayoutRoot.Opacity = opacity;
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
            DependencyProperty.Register("TextStyle", typeof(TextStyles), typeof(TextBlockEx), new PropertyMetadata(null)); //, OnTextStyleChanged));

        public TextStyles TextStyle
        {
            get { return (TextStyles)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        // TextAlignment
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(TextBlockEx), new PropertyMetadata(TextAlignment.Left)); //, OnTextAlignmentChanged));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        // TextWrapping
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBlockEx), new PropertyMetadata(TextWrapping.WrapWholeWords)); //, OnTextWrappingChanged));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // LineStackingStrategy
        public static readonly DependencyProperty LineStackingStrategyProperty =
            DependencyProperty.Register("LineStackingStrategy", typeof(LineStackingStrategy), typeof(TextBlockEx), new PropertyMetadata(LineStackingStrategy.BlockLineHeight)); //, OnLineStackingStrategyChanged));

        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy)GetValue(LineStackingStrategyProperty); }
            set { SetValue(LineStackingStrategyProperty, value); }
        }

        // LineHeight
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(double), typeof(TextBlockEx), new PropertyMetadata(null)); //, OnLineHeightChanged));

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
            if (d is TextBlockEx textBlockEx)
            {
                textBlockEx.TheText.Text = (string)e.NewValue;
            }
        }

        private void OnOpacityChanged(object sender, double e)
        {
            double opacity = e;

            if (null != this.LayoutRoot)
            {
                // correct opacity range
                opacity = Math.Max(0.0, opacity);
                opacity = Math.Min(1.0, opacity);

                // set opacity
                this.LayoutRoot.Opacity = opacity;
            }
        }

        #endregion
    }
}