﻿
using SDX.Toolkit.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.Media;
using Windows.UI.Xaml.Input;

namespace SDX.Toolkit.Controls
{
    // determines if the image pair rendering code will fire or if the line drawing code will fire
    public enum SelectorMode
    {
        Color,
        App
    }


    #region classes
    public class AppSelectorButton : Button
    {
        public int ID = 0;
        public Brush NormalBackground;
    }
    public class AppSelectorData: DependencyObject
    {
        public string SourceSVG_SelectedImage = "";
        public string SourceSVG_NotSelectedImage = "";
        //public string Source_NotSelectedImage = "";

        public static readonly DependencyProperty Source_SelectedImageProperty =
        DependencyProperty.Register("Source_SelectedImage", typeof(string), typeof(AppSelectorData), new PropertyMetadata(""));

        public string Source_SelectedImage
        {
            get { return (string)GetValue(Source_SelectedImageProperty); }
            set { SetValue(Source_SelectedImageProperty, value); }
        }
        public static readonly DependencyProperty Source_NotSelectedImageProperty =
        DependencyProperty.Register("Source_NotSelectedImage", typeof(string), typeof(AppSelectorData), new PropertyMetadata(""));

        public string Source_NotSelectedImage
        {
            get { return (string)GetValue(Source_NotSelectedImageProperty); }
            set { SetValue(Source_NotSelectedImageProperty, value); }
        }
        //public string Message;
        public static readonly DependencyProperty MessageProperty =
    DependencyProperty.Register("Message", typeof(string), typeof(AppSelectorData), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public bool IsClearButton = false;
    }
    public class ImagePair
    {
        public int ID = 0;
        public Image Selected = new Image();
        public Image NotSelected = new Image();
        public bool IsClearButton = false;
        public string Message;

    }
    #endregion

    public sealed class AppSelector : Control, IAnimate
    {
        #region Private Constants

        private const double WIDTH_GRID = 340d;
        private const double WIDTH_GRID_COLUMNSPACING = 10d;
        private const double WIDTH_GRID_ROWSPACING = 10d;
        private const double HORIZONTAL_LINE_OFFSET = 3d;
        private const double VERTICAL_LINE_OFFSET = 2d;
        private Style _buttonStyle;
  
        #endregion

        #region Private Members

        private Grid _layoutRoot = null;
        private Grid btnGrid = null;
        private List<AppSelectorButton> _buttonList = new List<AppSelectorButton>();
        private Storyboard _storyboardFadeIn = null;
        private Storyboard _storyboardFadeOut = null;
        private int _syncID = -1; // cant be 0 b/c 0 is first part of index in list
        private List<AppSelectorButton> Buttons;
        private Line selectedLine = new Line();
        private AppSelectorButton SelectedButton;
        private TranslateTransform translateTransform = new TranslateTransform();
        private Storyboard storyboard = new Storyboard();
        private DoubleAnimation daAnimation = new DoubleAnimation();

        #endregion

        #region Construction

        /*  NOTE FOR DEVELOPER USING THIS*** 
        // up to the consumer (u) to feed the data for messages and images shown
        // set orientation to vertical to put buttons on top of each other
        // set ShowMessages to true to show messages
        // width/height is also up to the consumer to handle b/c # of buttons 
        // can cause the h/w to vary. 
        // use AppSelectorData class to house selected and not selected images
        //        TelemetryId = TelemetryService.TELEMETRY_KEYBOARDVIEWCOLOR,
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        VerticalAlignment = VerticalAlignment.Top,
        //        DurationInMilliseconds = 200d,
        //        StaggerDelayInMilliseconds = 200d,
        //        AutoStart = false,
        //        Orientation = Orientation.Vertical,
        //        ShowMessages = true,
        //        URIs = new List<AppSelector.AppSelectorData>        
        //        {
        //            new AppSelector.AppSelectorData
        //            {
        //                URI_NotSelectedImage="ms-appx:///Assets/Selector/silver.png",
        //                URI_SelectedImage ="ms-appx:///Assets/Selector/silver-selected.png",
        //                Message= "first"
        //            },
        //            new AppSelector.AppSelectorData
        //            {
        //                URI_NotSelectedImage="ms-appx:///Assets/Selector/silver.png",
        //                URI_SelectedImage ="ms-appx:///Assets/Selector/silver-selected.png",
        //                Message= "second"
        //            },
        //            new AppSelector.AppSelectorData
        //            {
        //                URI_NotSelectedImage="ms-appx:///Assets/Selector/silver.png",
        //                URI_SelectedImage ="ms-appx:///Assets/Selector/silver-selected.png",
        //                Message= "third"
        //            },
        //            new AppSelector.AppSelectorData
        //            {
        //                URI_NotSelectedImage="ms-appx:///Assets/Selector/silver.png",
        //                URI_SelectedImage ="ms-appx:///Assets/Selector/silver-selected.png",
        //                Message= "fourth"
        //            },
        //            new AppSelector.AppSelectorData
        //            {
        //                URI_NotSelectedImage="ms-appx:///Assets/Selector/silver.png",
        //                URI_SelectedImage ="ms-appx:///Assets/Selector/silver-selected.png",
        //                Message= "fif"
        //            }

        //        }
        */
        public AppSelector()
        {
            this.DefaultStyleKey = typeof(AppSelector);
            this.Loaded += OnLoaded;
            this.ImagePairs = new Dictionary<int, ImagePair>();
            this.Buttons = new List<AppSelectorButton>();
            this.Opacity = 0;

            // inherited dependency property
            //new PropertyChangeEventSource<double>(
            //    this, "Opacity", BindingMode.OneWay).ValueChanged +=
            //    OnOpacityChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.RenderUI();
        }

        #endregion

        #region Public Methods

        public void StartFadeIn()
        {
            // start the textblock
            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Begin();
            }
        }

        public void StartFadeOut()
        {
            // start the textblock
            if (null != _storyboardFadeOut)
            {
                _storyboardFadeOut.Begin();
            }
        }

        public void ResetAnimation()
        {
            // reset the textblock
            if (null != _storyboardFadeIn)
            {
                _storyboardFadeIn.Stop();
            }
            if (null != _storyboardFadeOut)
            {
                _storyboardFadeOut.Stop();
            }

            // reset opacity to starting point
            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = 0.0;
            }
        }

        public void SetOpacity(double opacity)
        {
            if ((opacity < 0.0) || (opacity > 1.0)) { return; }

            if (null != _layoutRoot)
            {
                _layoutRoot.Opacity = opacity;
            }
        }

        public void SyncSelectedID(int ID)
        {
            // save this color so the handler will know we changed it
            _syncID = ID;

            this.SelectedID = ID;
        }

        #endregion

        #region Public Properties
        // pass me in at init to define me pls
        public string TelemetryId { get; set; }
        #endregion

        #region Dependency Properties

        // IsPenOnly
        public static readonly DependencyProperty IsPenOnlyProperty =
            DependencyProperty.Register("IsPenOnly", typeof(bool), typeof(AppSelector), new PropertyMetadata(false));

        public bool IsPenOnly
        {
            get { return (bool)GetValue(IsPenOnlyProperty); }
            set { SetValue(IsPenOnlyProperty, value); }
        }

        // SelectedID
        public static readonly DependencyProperty SelectedIDProperty =
            DependencyProperty.Register("SelectedID", typeof(int), typeof(AppSelector), new PropertyMetadata(0, OnSelectedIDChanged));

        public int SelectedID
        {
            get { return (int)GetValue(SelectedIDProperty); }
            set { SetValue(SelectedIDProperty, value); }
        }

        // DurationInMilliseconds
        public static readonly DependencyProperty DurationInMillisecondsProperty =
            DependencyProperty.Register("DurationInMilliseconds", typeof(double), typeof(AppSelector), new PropertyMetadata(200d, OnDurationInMillisecondsChanged));

        public double DurationInMilliseconds
        {
            get { return (double)GetValue(DurationInMillisecondsProperty); }
            set { SetValue(DurationInMillisecondsProperty, value); }
        }

        // FadeInCompletedHandler
        public static readonly DependencyProperty FadeInCompletedHandlerProperty =
            DependencyProperty.Register("FadeInCompletedHandler", typeof(EventHandler<object>), typeof(AppSelector), new PropertyMetadata(null));

        public EventHandler<object> FadeInCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeInCompletedHandlerProperty); }
            set { SetValue(FadeInCompletedHandlerProperty, value); }
        }

        // FadeOutCompletedHandler
        public static readonly DependencyProperty FadeOutCompletedHandlerProperty =
            DependencyProperty.Register("FadeOutCompletedHandler", typeof(EventHandler<object>), typeof(AppSelector), new PropertyMetadata(null));

        public EventHandler<object> FadeOutCompletedHandler
        {
            get { return (EventHandler<object>)GetValue(FadeOutCompletedHandlerProperty); }
            set { SetValue(FadeOutCompletedHandlerProperty, value); }
        }

        // StaggerDelayInMilliseconds
        public static readonly DependencyProperty StaggerDelayInMillisecondsProperty =
            DependencyProperty.Register("StaggerDelayInMilliseconds", typeof(double), typeof(AppSelector), new PropertyMetadata(0d, OnStaggerDelayInMillisecondsChanged));

        public double StaggerDelayInMilliseconds
        {
            get { return (double)GetValue(StaggerDelayInMillisecondsProperty); }
            set { SetValue(StaggerDelayInMillisecondsProperty, value); }
        }

        // AutoStart
        public static readonly DependencyProperty AutoStartProperty =
        DependencyProperty.Register("AutoStart", typeof(bool), typeof(AppSelector), new PropertyMetadata(true, OnAutoStartChanged));

        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        //List<AppSelectorData> URIs        
        public static readonly DependencyProperty URIsProperty =
        DependencyProperty.Register("URIs", typeof(List<AppSelectorData>), typeof(AppSelector), new PropertyMetadata(new List<AppSelectorData>()));

        public List<AppSelectorData> URIs
        {
            get { return (List<AppSelectorData>)GetValue(URIsProperty); }
            set { SetValue(URIsProperty, value); }
        }

        public static readonly DependencyProperty ClearButtonDataProperty =
        DependencyProperty.Register("ClearButtonData", typeof(AppSelectorData), typeof(AppSelector), new PropertyMetadata(null));

        public AppSelectorData ClearButtonData
        {
            get { return (AppSelectorData)GetValue(ClearButtonDataProperty); }
            set { SetValue(ClearButtonDataProperty, value); }
        }

        public static readonly DependencyProperty MainOrientationProperty =
        DependencyProperty.Register("MainOrientation", typeof(Orientation), typeof(AppSelector), new PropertyMetadata(Orientation.Horizontal));

        public Orientation MainOrientation
        {
            get { return (Orientation)GetValue(MainOrientationProperty); }
            set { SetValue(MainOrientationProperty, value); }
        }

        public static readonly DependencyProperty ShowMessagesProperty =
        DependencyProperty.Register("ShowMessages", typeof(bool), typeof(AppSelector), new PropertyMetadata(false));

        public bool ShowMessages
        {
            get { return (bool)GetValue(ShowMessagesProperty); }
            set { SetValue(ShowMessagesProperty, value); }
        }

        public static readonly DependencyProperty AppSelectorModeProperty =
        DependencyProperty.Register("AppSelectorMode", typeof(SelectorMode), typeof(AppSelector), new PropertyMetadata(SelectorMode.Color));

        public SelectorMode AppSelectorMode
        {
            get { return (SelectorMode)GetValue(AppSelectorModeProperty); }
            set { SetValue(AppSelectorModeProperty, value); }
        }

        public static readonly DependencyProperty ButtonHeightProperty =
        DependencyProperty.Register("ButtonHeight", typeof(Double), typeof(AppSelector), new PropertyMetadata(0d));

        public Double ButtonHeight
        {
            get { return (Double)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly DependencyProperty ButtonWidthProperty =
        DependencyProperty.Register("ButtonWidth", typeof(Double), typeof(AppSelector), new PropertyMetadata(0d));

        public Double ButtonWidth
        {
            get { return (Double)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty ShowSelectedLineProperty =
        DependencyProperty.Register("ShowSelectedLine", typeof(bool), typeof(AppSelector), new PropertyMetadata(false));

        public bool ShowSelectedLine
        {
            get { return (bool)GetValue(ShowSelectedLineProperty); }
            set { SetValue(ShowSelectedLineProperty, value); }
        }

        public static readonly DependencyProperty ImagePairsProperty =
        DependencyProperty.Register("ImagePairs", typeof(Dictionary<int, ImagePair>), typeof(AppSelector), new PropertyMetadata(new Dictionary<int,ImagePair>()));

        public Dictionary<int,ImagePair> ImagePairs
        {
            get { return (Dictionary<int,ImagePair>)GetValue(ImagePairsProperty); }
            set { SetValue(ImagePairsProperty, value); }
        }

        public static readonly DependencyProperty ClearButtonImagePairProperty =
        DependencyProperty.Register("ClearButtonImagePair", typeof(ImagePair), typeof(AppSelector), new PropertyMetadata(null));

        public ImagePair ClearButtonImagePair
        {
            get { return (ImagePair)GetValue(ClearButtonImagePairProperty); }
            set { SetValue(ClearButtonImagePairProperty, value); }
        }

        // AnimationDirection
        public static readonly DependencyProperty PageEntranceDirectionProperty =
        DependencyProperty.Register("PageEntranceDirection", typeof(AnimationDirection), typeof(Header), new PropertyMetadata(AnimationDirection.Left));

        public AnimationDirection PageEntranceDirection
        {
            get { return (AnimationDirection)GetValue(PageEntranceDirectionProperty); }
            set { SetValue(PageEntranceDirectionProperty, value); }
        }

        //// IsListView
        //public static readonly DependencyProperty IsListViewProperty =
        //DependencyProperty.Register("IsListView", typeof(bool), typeof(AppSelector), new PropertyMetadata(false));

        //public bool IsListView
        //{
        //    get { return (bool)GetValue(IsListViewProperty); }
        //    set { SetValue(IsListViewProperty, value); }
        //}
        #endregion

        #region Custom Events

        public delegate void OnSelectedIDChangedEvent(object sender, EventArgs e);

        public event OnSelectedIDChangedEvent SelectedIDChanged;

        private void RaiseSelectedIDChangedEvent(AppSelector Selector, EventArgs e)
        {
            SelectedIDChanged?.Invoke(Selector, e);
        }

        private void RaiseSelectedIDChangedEvent(AppSelector Selector)
        {
            this.RaiseSelectedIDChangedEvent(Selector, new EventArgs());
        }


        public delegate void OnClearClickedEvent(object sender, EventArgs e);

        public event OnClearClickedEvent OnClearClicked;

        private void RaiseClearClickedEvent(AppSelector Selector, EventArgs e)
        {
            OnClearClicked?.Invoke(Selector, e);
        }

        private void RaiseClearClickedEvent(AppSelector Selector)
        {
            this.RaiseClearClickedEvent(Selector, new EventArgs());
        }


        #endregion

        #region Event Handlers

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoStart)
            {
                this.StartFadeIn();
            }
        }

        private static void OnSelectedIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AppSelector selector)
            {
                // update the UI with the new color
                selector.UpdateUI();
                // only raise the event if this was NOT a sync change
                if (selector.SelectedID != selector._syncID)
                {
                    // reset the sync ID
                    selector._syncID = -1;

                    // raise the selected color changed event
                    selector.RaiseSelectedIDChangedEvent(selector);
                }
            }
        }

        //private void OnOpacityChanged(object sender, double e)
        //{
        //    double opacity = e;

        //    if (null != _layoutRoot)
        //    {
        //        // correct opacity range
        //        opacity = Math.Max(0.0, opacity);
        //        opacity = Math.Min(1.0, opacity);

        //        // set opacity
        //        _layoutRoot.Opacity = opacity;
        //    }
        //}

        private static void OnDurationInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnStaggerDelayInMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnAutoStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Render UI

        private void RenderUI()
        {
            // get the layout base (a canvas here)
            _layoutRoot = (Grid)this.GetTemplateChild("LayoutRoot");


            // if we can't get the layout root, we can't do anything
            if (null == _layoutRoot) { return; }

            // update the grid
            _layoutRoot.Name = "AppSelectorGrid";



            // must construct additional columns or rows based on orientation and number
            // keep 1.0 so it creates a ratio (double) for the width/height definitions
            // coloring book will pass in image pairs, otherwise its a regular URI count from a normal app selector
            int iButtonCount = this.ImagePairs.Count > 0 ? this.ImagePairs.Count + 1: URIs.Count;

            double ratio = 1.0 / iButtonCount;
            if (this.MainOrientation == Orientation.Horizontal)
            {
                _layoutRoot.ColumnSpacing = WIDTH_GRID_COLUMNSPACING;
                _layoutRoot.RowDefinitions.Add(new RowDefinition());
                for (int i = 0; i < iButtonCount; i++)
                {
                    _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ratio, GridUnitType.Star) });
                }
            }
            else
            {
                //_layoutRoot.RowSpacing = (!IsListView ? WIDTH_GRID_ROWSPACING: 0);
                _layoutRoot.RowSpacing = WIDTH_GRID_ROWSPACING;
                _layoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
                for (int i = 0; i < iButtonCount; i++)
                {
                    _layoutRoot.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ratio, GridUnitType.Star) });
                }
            }

            // create the button style
            //if (IsListView)
            //{
            //    _buttonStyle = StyleHelper.GetApplicationStyle("ListViewButton");
            //}
            //else
            //{
                _buttonStyle = StyleHelper.GetApplicationStyle("AppSelectorButton");
            //}
            
            // JN loop this area to create images and buttons based on list            
            int index = 0;
            if (this.ImagePairs.Count != 0)
            {// hubris! dont add clearbutton image pair unless it has a value. should only have a value on colorbook
                if (this.ClearButtonImagePair != null)
                {
                    GenerateButton(this.ClearButtonImagePair, 0, 0);
                    index = 1;
                }
                else
                {
                    index = 0;
                }

                for (int i = 0; i < this.ImagePairs.Count; i++, index++)
                {
                    GenerateButton(this.ImagePairs[i], i, index);
                }
            }
            else
            {
                for (int i = 0; i < this.URIs.Count; i++)
                {
                    GenerateButton(this.URIs[i], i);
                }
            }
            

            if (this.ShowSelectedLine)
            {
                selectedLine.StrokeThickness = 5;
                double LineRightMargin = StyleHelper.GetApplicationDouble("AppSelectorLineBottomMargin");
                double LineBottomMargin = StyleHelper.GetApplicationDouble("AppSelectorLineBottomMargin");

                if (this.MainOrientation == Orientation.Vertical)
                {
                    selectedLine.X1 = -LineRightMargin;
                    selectedLine.Y1 = VERTICAL_LINE_OFFSET;

                    selectedLine.X2 = -LineRightMargin;
                    selectedLine.Y2 = this.ButtonHeight + VERTICAL_LINE_OFFSET;
                }
                if (this.MainOrientation == Orientation.Horizontal)
                {
                    selectedLine.X1 = 3;
                    selectedLine.Y1 = this.ButtonHeight + 5 + LineBottomMargin;

                    selectedLine.X2 = this.ButtonWidth + 4;// border thickness for some reason 2 on both sides
                    selectedLine.Y2 = this.ButtonHeight + 5 + LineBottomMargin;
                }

                SolidColorBrush SelectedLineColor = RadiatingButton.GetSolidColorBrush("#FF0078D4");

                selectedLine.Stroke = SelectedLineColor;
                selectedLine.Fill = SelectedLineColor;
                _layoutRoot.Children.Add(selectedLine);

                this.SelectedButton = this.Buttons[0];// set the first button as the selected button for line moving

                // need sine easing
                SineEase sineEaseIn = new SineEase()
                {
                    EasingMode = EasingMode.EaseIn
                };
                // set this once here and forget it. 
                this.selectedLine.RenderTransform = this.translateTransform;
                Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 400));
                daAnimation = new DoubleAnimation()
                {
                    Duration = duration,
                    EasingFunction = sineEaseIn
                };

                this.storyboard.Children.Add(daAnimation);
                Storyboard.SetTarget(daAnimation, this.translateTransform);
                if (this.MainOrientation == Orientation.Horizontal)
                {
                    Storyboard.SetTargetProperty(daAnimation, "X");
                }
                if (this.MainOrientation == Orientation.Vertical)
                {
                    Storyboard.SetTargetProperty(daAnimation, "Y");
                }

            }

            // set up animations
            _storyboardFadeIn = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 0.0, 0.0, 1.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));
            _storyboardFadeOut = AnimationHelper.CreateEasingAnimation(_layoutRoot, "Opacity", 1.0, 1.0, 0.0, this.DurationInMilliseconds, this.StaggerDelayInMilliseconds, false, false, new RepeatBehavior(1d));

            this.UpdateUI();
        }

        private void GenerateButton(AppSelectorData AppSelectorData, int index)
        {
            btnGrid = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            if (this.ShowMessages && this.MainOrientation == Orientation.Vertical)
            {
                btnGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.5, GridUnitType.Star) });
                btnGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.5, GridUnitType.Star) });
                btnGrid.ColumnSpacing = WIDTH_GRID_COLUMNSPACING;
            }

            ImagePair images = new ImagePair()
            {
                //ID = i,
                Selected = new Image()
                {                    
                    Width = this.ButtonWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                },
                NotSelected = new Image()
                {                    
                    Width = this.ButtonWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 1.0
                }
            };
            if (!string.IsNullOrEmpty(AppSelectorData.Source_NotSelectedImage))
            {
                images.NotSelected.Source = new BitmapImage() { UriSource = new Uri(AppSelectorData.Source_NotSelectedImage), DecodePixelWidth = (int)this.ButtonWidth };
            }
            else if (!string.IsNullOrEmpty(AppSelectorData.SourceSVG_NotSelectedImage))
            {
                images.NotSelected.Source = new SvgImageSource(new Uri(AppSelectorData.SourceSVG_NotSelectedImage));
            }

            if (!string.IsNullOrEmpty(AppSelectorData.Source_SelectedImage))
            {
                images.Selected.Source = new BitmapImage() { UriSource = new Uri(AppSelectorData.Source_SelectedImage), DecodePixelWidth = (int)this.ButtonWidth };
            }
            else if (!string.IsNullOrEmpty(AppSelectorData.SourceSVG_SelectedImage))
            {
                images.Selected.Source = new SvgImageSource(new Uri(AppSelectorData.SourceSVG_SelectedImage));
            }
            //images.Selected.Source
            Grid.SetRow(images.NotSelected, 0);
            Grid.SetColumn(images.NotSelected, 0);
            btnGrid.Children.Add(images.NotSelected);
            Grid.SetRow(images.Selected, 0);
            Grid.SetColumn(images.Selected, 0);
            btnGrid.Children.Add(images.Selected);

            if (this.MainOrientation == Orientation.Vertical && this.ShowMessages)
            {
                TextBlockEx tbMessage = new TextBlockEx()
                {
                    Name = "Text",
                    Text = AppSelectorData.Message,
                    TextStyle = TextStyles.AppSelectorText,
                    FontSize = 20,
                    Opacity = 1,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextStyleBold = TextStyles.AppSelectorTextBold
                    //TextStyleBold = (IsListView ? TextStyles.AppSelectorTextDarkBold: TextStyles.AppSelectorTextBold)
                };

                if (index == 0)
                {
                    tbMessage.ShowBoldText(true);
                }

                Grid.SetRow(tbMessage, 0);
                Grid.SetColumn(tbMessage, 1);// kk this isnt working just yet. 
                btnGrid.Children.Add(tbMessage);
            }
            if (!AppSelectorData.IsClearButton) {
                this.ImagePairs.Add(index, images);
            }
                
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
            // if we need to show messages then align left 
            if (this.MainOrientation == Orientation.Vertical && this.ShowMessages)
            {
                horizontalAlignment = HorizontalAlignment.Left;
            }

            AppSelectorButton sbButton = new AppSelectorButton()
            {
                ID = index,
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center,
                Content = btnGrid
            };

            //if (!IsListView)
            //{
            //    sbButton.Background = new SolidColorBrush(Colors.Transparent);
            //}
            //else
            //{
            //    sbButton.NormalBackground = (index % 2 == 0 ? BackGroundWhiteAcrylic : BackGroundGrayAcrylic);
            //    if (index == 0)
            //    {
            //        sbButton.Background = new SolidColorBrush(Colors.Blue);
            //    }
            //    else
            //    {
            //        sbButton.Background = sbButton.NormalBackground;
            //    }

            //}

            //only set the dimensions of the button if the control variables are passed in
            // and the orientation is correct
            if (this.ButtonHeight > 0)
            {
                sbButton.Height = this.ButtonHeight;
            }

            // if u need to show messages, dont set the width b/c theres no way to figure out the width 
            // if there is text
            if (this.ButtonWidth > 0 && (!this.ShowMessages && !(this.MainOrientation == Orientation.Vertical)))
            {
                sbButton.Width = this.ButtonWidth;
            }
            if (null != _buttonStyle) { sbButton.Style = _buttonStyle; };

            if (AppSelectorData.IsClearButton)
            {// these buttons get their own handler and dont change the selection of the app selector
                sbButton.AddHandler(PointerReleasedEvent, new PointerEventHandler(Selector_ClearButtonClick), true);
            }
            else
            {
                sbButton.AddHandler(PointerReleasedEvent, new PointerEventHandler(Selector_ButtonClick), true);
            }
            btnGrid.PointerEntered += pointerEntered;


            if (this.MainOrientation == Orientation.Horizontal)
            {
                Grid.SetRow(sbButton, 0);
                Grid.SetColumn(sbButton, index);
            }
            else // vertical
            {
                Grid.SetColumn(sbButton, 0);
                Grid.SetRow(sbButton, index);
            }
            if (!AppSelectorData.IsClearButton)
            {// clear button is not a part of the regular buttons 
                this.Buttons.Add(sbButton);
            }            
            _layoutRoot.Children.Add(sbButton);
            _buttonList.Add(sbButton);
        }

        private void GenerateButton(ImagePair imagePair, int i, int position)
        {
            Grid grid = new Grid()
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            if (this.ShowMessages && this.MainOrientation == Orientation.Vertical)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.5, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.5, GridUnitType.Star) });
                grid.ColumnSpacing = WIDTH_GRID_COLUMNSPACING;
            }

            Grid.SetRow(imagePair.NotSelected, 0);
            Grid.SetColumn(imagePair.NotSelected, 0);
            grid.Children.Add(imagePair.NotSelected);
            Grid.SetRow(imagePair.Selected, 0);
            Grid.SetColumn(imagePair.Selected, 0);
            grid.Children.Add(imagePair.Selected);

            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
            // if we need to show messages then align left 
            if (this.MainOrientation == Orientation.Vertical && this.ShowMessages && !string.IsNullOrWhiteSpace(imagePair.Message))
            {
                horizontalAlignment = HorizontalAlignment.Left;
                TextBlockEx tbMessage = new TextBlockEx()
                {
                    Name = "TheText",
                    Text = imagePair.Message,
                    TextStyle= TextStyles.AppSelectorText,
                    FontSize = 20,
                    Opacity = 1,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextStyleBold = TextStyles.AppSelectorTextBold
                };

                if (i == 0)
                {
                    tbMessage.ShowBoldText(true);
                }

                Grid.SetRow(tbMessage, 0);
                Grid.SetColumn(tbMessage, 1);// kk this isnt working just yet. 
                grid.Children.Add(tbMessage);

            }

            AppSelectorButton sbButton = new AppSelectorButton()
            {
                ID = i,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center,
                Content = grid
            };

            //if (!IsListView)
            //{
            //    sbButton.Background = new SolidColorBrush(Colors.Transparent);
            //}
            //else
            //{
            //    sbButton.NormalBackground = (i % 2 == 0 ? BackGroundWhiteAcrylic : BackGroundGrayAcrylic);
            //    if (i == 0)
            //    {
            //        sbButton.Background = new SolidColorBrush(Colors.Blue);
            //    }
            //    else
            //    {
            //        sbButton.Background = sbButton.NormalBackground;
            //    }

            //}

            //only set the dimensions of the button if the control variables are passed in
            // and the orientation is correct
            if (this.ButtonHeight > 0)
            {
                sbButton.Height = this.ButtonHeight;
            }

            // if u need to show messages, dont set the width b/c theres no way to figure out the width 
            // if there is text
            if (this.ButtonWidth > 0 && (!this.ShowMessages && !(this.MainOrientation == Orientation.Vertical)))
            {
                sbButton.Width = this.ButtonWidth;
            }
            if (null != _buttonStyle) { sbButton.Style = _buttonStyle; };

            if (imagePair.IsClearButton)
            {// these buttons get their own handler and dont change the selection of the app selector
                sbButton.AddHandler(PointerReleasedEvent, new PointerEventHandler(Selector_ClearButtonClick), true);
            }
            else
            {
                sbButton.AddHandler(PointerReleasedEvent, new PointerEventHandler(Selector_ButtonClick), true);
            }
            grid.PointerEntered += pointerEntered;


            if (this.MainOrientation == Orientation.Horizontal)
            {
                Grid.SetRow(sbButton, 0);
                Grid.SetColumn(sbButton, position);
            }
            else // vertical
            {
                Grid.SetColumn(sbButton, 0);
                Grid.SetRow(sbButton, position);
            }
            if (!imagePair.IsClearButton)
            {// clear button is not a part of the regular buttons 
                this.Buttons.Add(sbButton);
            }
            _layoutRoot.Children.Add(sbButton);
            _buttonList.Add(sbButton);

        }

        private void pointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            // you can see the 2images in there. which is which?
            // and should you refactor the render ui so it doesnt put the 
            // clear button into the Imagepairs collection? or where it shouldnt
            // be referred to?
        }

        private void Selector_ButtonClick(object sender, PointerRoutedEventArgs e)
        {
            if (IsPenOnly && e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Pen)
            {
                return;
            }

            AppSelectorButton sbButton = (AppSelectorButton)sender;
            if (this.ShowSelectedLine && this.SelectedButton.ID != sbButton.ID)
            {// transform the line so it moves to underneath the sender
                this.Selector_SlideLine(sbButton);
                this.Selector_UpdateSelectedState(sbButton);
            }
            this.SelectedID = sbButton.ID;
            this.SelectedButton = sbButton;
            this.Selector_UpdateSelectedState(sbButton);

            // telemetry
            //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);
        }

        private void Selector_UpdateSelectedState(AppSelectorButton sbButton)
        {
            foreach(AppSelectorButton button in _buttonList)
            {
                if(((Panel)button.Content).Children[2] is TextBlockEx)
                {
                    TextBlockEx text = (TextBlockEx)((Panel)button.Content).Children[2];
                    
                    if(button.ID == sbButton.ID)
                    {
                        text.ShowBoldText(true);
                        //if (IsListView)
                        //{
                        //    button.Background = new SolidColorBrush(Colors.Blue);
                        //}
                    }
                    else
                    {
                        text.ShowBoldText(false);
                        //if (IsListView)
                        //{
                        //    button.Background = button.NormalBackground;
                        //}
                    }                    
                }
            }
        }

        private void Selector_ClearButtonClick(object sender, PointerRoutedEventArgs e)
        {
            if(IsPenOnly && e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Pen)
            {
                return;
            }
            AppSelectorButton sbButton = (AppSelectorButton)sender;
            // raise event clear clicked
            // raise the selected color changed event
            this.RaiseClearClickedEvent(this);
            // telemetry
            //TelemetryService.Current?.SendTelemetry(this.TelemetryId, System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), true, 0);

        }

        private void Selector_SlideLine(AppSelectorButton End)
        {
            /** how this place works
                // transform the line so it moves to underneath the sender
                // make sure it cant go less than the first button and past the last button
                //calculate move distance based on orientation and how many button widths away to move
                *** SPECIAL NOTE if u storyboard a  translatetransform with a double animation, the
                the translatetransform gets no value and the animation gets it instead in the To prop
            */
            double iDistanceInButtons = End.ID;
            double iDistance = 0;
            double VerticalLineOffset = End.ID == 0 ? 0 : VERTICAL_LINE_OFFSET;
            double HorizontalLineOffset = End.ID == 0 ? 0 : HORIZONTAL_LINE_OFFSET;


            // move left or right
            if (this.MainOrientation == Orientation.Horizontal)
            {
                iDistance = iDistanceInButtons * (this.ButtonWidth) + (iDistanceInButtons * (WIDTH_GRID_COLUMNSPACING) + HorizontalLineOffset);
            }

            // move up or down
            else // orientation must be vertical
            {
                iDistance = iDistanceInButtons * (this.ButtonHeight) + (iDistanceInButtons * WIDTH_GRID_ROWSPACING) + VerticalLineOffset;
            }
            daAnimation.To = iDistance;
            this.storyboard.Begin();
        }

        public void UpdateUI()
        {
            // test the first image and return if it hasn't been created
            if (null == this.ImagePairs) { return; }
            // if there are image pairs and the setting has image pairs, then do image pairs. 
            // otherwise keep all opaque and move the line and update the button text to be bold
            if (this.AppSelectorMode == SelectorMode.Color)
            {
                for (int i = 0; i < this.ImagePairs.Count; i++)
                {
                    if (this.SelectedID == i)
                    { // selected change opacity to 1 and unselected to 0
                        this.ImagePairs[i].Selected.Opacity = 1;
                        this.ImagePairs[i].NotSelected.Opacity = 0;
                    }
                }

                for (int i = 0; i < this.ImagePairs.Count; i++)
                {
                    if (this.SelectedID != i)
                    { // selected change opacity to 1 and unselected to 0
                        this.ImagePairs[i].Selected.Opacity = 0;
                        this.ImagePairs[i].NotSelected.Opacity = 1;
                    }
                }
            }
            else
            {   // app mode so no flipping opacity and move line and change textblock bold and fontsize 
                //on button 

            }
        }

        public bool HasAnimateChildren()
        {
            return false;
        }

        public bool HasPageEntranceAnimation()
        {
            return true;
        }

        public bool HasPageEntranceTranslation()
        {
            return false;
        }

        public AnimationDirection Direction()
        {
            return PageEntranceDirection;
        }

        public List<UIElement> AnimatableChildren()
        {
            return new List<UIElement>();
        }

        #endregion

        #region UI Helpers

        #endregion

        #region Code Helpers

        #endregion
    }
}
