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
    public sealed class Hero : Control
    {
        #region Private Members

        private StackPanel _rowMaster = null;
        private List<StackPanel> _rows = new List<StackPanel>();
        private List<TextBlockEx> _wordTextBlocks = new List<TextBlockEx>();

        private List<Storyboard> _storyboards = new List<Storyboard>();

        #endregion

        #region Constructor

        public Hero()
        {
            this.DefaultStyleKey = typeof(Hero);
            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Methods


        public void StartAnimation()
        {
            if (null != _storyboards)
            {
                foreach (Storyboard storyboard in _storyboards)
                {
                    storyboard.Begin();
                }
            }
        }

        public void ResetAnimation()
        {
            if ((null != _storyboards) && (null != _wordTextBlocks))
            {
                foreach (Storyboard storyboard in _storyboards)
                {
                    storyboard.Stop();
                }

                foreach (TextBlockEx word in _wordTextBlocks)
                {
                    word.Opacity = 0d;
                }
            }
        }

        #endregion

        #region Dependency Properties

        // Words
        public static readonly DependencyProperty WordsProperty =
            DependencyProperty.Register("Words", typeof(string), typeof(Hero), new PropertyMetadata(null, OnWordsChanged));

        public string Words
        {
            get { return (string)GetValue(WordsProperty); }
            set
            {
                // if it's valid
                if (String.IsNullOrWhiteSpace(value))
                { return; }

                // save it
                SetValue(WordsProperty, value);
            }
        }

        // WordRows
        public static readonly DependencyProperty WordRowsProperty =
            DependencyProperty.Register("WordRows", typeof(int), typeof(Hero), new PropertyMetadata(1, OnWordRowsChanged));

        public int WordRows
        {
            get { return (int)GetValue(WordRowsProperty); }
            set { SetValue(WordRowsProperty, (value < 1) ? 1 : value); }
        }

        // DurationPerWordInMilliseconds
        public static readonly DependencyProperty DurationPerWordInMillisecondsProperty =
            DependencyProperty.Register("DurationPerWordInMilliseconds", typeof(double), typeof(Hero), new PropertyMetadata(400d, OnDurationPerWordInMillisecondsChanged));

        public double DurationPerWordInMilliseconds
        {
            get { return (double)GetValue(DurationPerWordInMillisecondsProperty); }
            set { SetValue(DurationPerWordInMillisecondsProperty, (value < 0) ? 0 : value); }
        }

        // StaggerDelayMilliseconds
        public static readonly DependencyProperty StaggerDelayMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayMilliseconds", typeof(double), typeof(Hero), new PropertyMetadata(0d, OnStaggerDelayMillisecondsChanged));

        public double StaggerDelayMilliseconds
        {
            get { return (double)GetValue(StaggerDelayMillisecondsProperty); }
            set { SetValue(StaggerDelayMillisecondsProperty, (value < 0) ? 0 : value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(Hero), new PropertyMetadata(false, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        // TextStyle

        public static readonly DependencyProperty TextStyleProperty =
        DependencyProperty.Register("TextStyle", typeof(TextStyles), typeof(Hero), new PropertyMetadata(TextStyles.Hero));

        public TextStyles TextStyle
        {
            get { return (TextStyles)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
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

        private static void OnWordsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Hero hero)
            {
                hero.RenderUI();
            }
        }

        private static void OnWordRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Hero hero)
            {
                hero.RenderUI();
            }
        }

        private static void OnStaggerDelayMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Hero hero)
            {
                hero.RenderUI();
            }
        }

        private static void OnDurationPerWordInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Hero hero)
            {
                hero.RenderUI();
            }
        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Hero hero)
            {
                hero.RenderUI();
            }
        }

        #endregion

        #region UI Methods

        private void RenderUI()
        {
            // get the master row container
            _rowMaster = (StackPanel)this.GetTemplateChild("RowMaster");

            // exit if we can't find it
            if (null == _rowMaster) { return; }

            // clear the children
            _rowMaster.Children.Clear();

            // create rows
            for (int row = 0; row < this.WordRows; row++)
            {
                // create row stackpanel and add to the master
                StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                _rowMaster.Children.Add(stackPanel);
            }

            // break the words into text blocks
            List<string> words = this.Words.Split(" ").ToList<string>();

            // calculate word positions;
            int wordsPerLine = words.Count / this.WordRows;
            int wordsLeftover = words.Count - (wordsPerLine * this.WordRows);

            int wordIndex = 0;

            // loop through the rows
            foreach (StackPanel row in _rowMaster.Children)
            {
                int wordsThisLine = wordsPerLine;
                if (wordsLeftover > 0)
                {
                    wordsThisLine++;
                    wordsLeftover--;
                }

                for (int i = 0; i < wordsThisLine; i++)
                {
                    // get the word
                    string word = words.ElementAt(wordIndex);

                    // create the name
                    string name = String.Format("Word_{0}", wordIndex);

                    // create the textblock
                    TextBlockEx textBlockWord = CreateWord(name, word);

                    // save it to reset opacity for animation
                    _wordTextBlocks.Add(textBlockWord);

                    // add it to the row
                    row.Children.Add(textBlockWord);

                    // increment the word index
                    wordIndex++;
                }
            }

            // animation index
            int animationIndex = 0;

            // loop through the words to animate them
            foreach (TextBlockEx textBlockEx in _wordTextBlocks)
            {
                double staggerDelay = this.StaggerDelayMilliseconds + (animationIndex * this.DurationPerWordInMilliseconds);

                _storyboards.Add(
                    AnimationHelper.CreateStandardAnimation(textBlockEx, "(TextBlockEx.Opacity)", 0.0, 0.0, 1.0,
                                    this.DurationPerWordInMilliseconds, staggerDelay, false, false, new RepeatBehavior(1d)));

                animationIndex++;
            }
        }

        #endregion

        #region UI Helpers

        private TextBlockEx CreateWord(string name, string text)
        {
            TextBlockEx _textBlock = new TextBlockEx()
            {
                Name = name,
                Text = text,
                TextStyle = this.TextStyle,
                Margin = StyleHelper.GetApplicationThickness(LayoutThicknesses.HeroMargin),
                Opacity = 0d,
            };

            return _textBlock;
        }

        #endregion
    }
}
