using SDX.Toolkit.Helpers;
using SurfaceHeadphoneDemo.ViewModels;
using System;
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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SurfaceHeadphoneDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HowToPage : Page, INavigate
    {
        #region Private Members

        // for the entrance timing of the overview lines and text
        private double EntranceTime = 1250;
        private double EntranceStagger = 500;

        // used to keep track of selected list item
        private int PreviousSelected = -1;

        private HowToViewModel ViewModel
        {
            get { return DataContext as HowToViewModel; }
        }

        #endregion

        #region Public Static Properties

        public RoutedEventHandler CloseButton_Clicked;

        public static HowToPage Current { get; private set; }

        #endregion

        #region Construction

        public HowToPage()
        {
            this.InitializeComponent();
            HowToPage.Current = this;
            this.HowToExtraCoverBg.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Dark);
            this.HowToBg.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Lighter);
            this.Loaded += HowToPage_Loaded;
        }

        #endregion

        #region Public Methods

        public void PerformOverViewEntrance()
        {
            AnimationHelper.PerformFadeIn(this.Overview1, EntranceTime, EntranceStagger);
            AnimationHelper.PerformFadeIn(this.OverviewCaption1, EntranceTime, EntranceStagger + 250);

            AnimationHelper.PerformFadeIn(this.Overview2, EntranceTime, EntranceStagger + 500);
            AnimationHelper.PerformFadeIn(this.OverviewCaption2, EntranceTime, EntranceStagger + 500);

            AnimationHelper.PerformFadeIn(this.Overview3, EntranceTime, EntranceStagger + 750);
            AnimationHelper.PerformFadeIn(this.OverviewCaption3, EntranceTime, EntranceStagger + 750);

            AnimationHelper.PerformFadeIn(this.Overview4, EntranceTime, EntranceStagger + 1000);
            AnimationHelper.PerformFadeIn(this.OverviewCaption4, EntranceTime, EntranceStagger + 1000);

            AnimationHelper.PerformFadeIn(this.Overview5, EntranceTime, EntranceStagger + 1250);
            AnimationHelper.PerformFadeIn(this.OverviewCaption5, EntranceTime, EntranceStagger + 1250);

            AnimationHelper.PerformFadeIn(this.Overview6, EntranceTime, EntranceStagger + 1500);
            AnimationHelper.PerformFadeIn(this.OverviewCaption6, EntranceTime, EntranceStagger + 1500);
        }

        #endregion

        #region Private Methods

        private void HowToPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (null != itemListView)
            {
                itemListView.SelectedIndex = 0;
            }
        }

        private void PopClose_Click(object sender, PointerRoutedEventArgs e)
        {
            CloseButton_Clicked(sender, new RoutedEventArgs());
        }

        private void ShowIllustration(int id)
        {
            HideIllustration(PreviousSelected);
            switch (id)
            {
                case 0: //overview
                    this.Illustration1.Opacity = 1.0d;
                    this.OverviewLineCanvas.Opacity = 1.0d;
                    break;

                case 1: //play pause
                    this.Illustration2.Opacity = 1.0d;
                    this.Caption2.Opacity = 1.0d;
                    break;

                case 2: //skip
                    this.Illustration3.Opacity = 1.0d;
                    this.Caption3.Opacity = 1.0d;
                    break;

                case 3: //volume
                    this.Illustration4.Opacity = 1.0d;
                    this.Caption4.Opacity = 1.0d;
                    break;

                case 4: //noise
                    this.Illustration5.Opacity = 1.0d;
                    this.Caption5.Opacity = 1.0d;
                    break;

                case 5: //noise
                    this.Caption6.Opacity = 1.0d;
                    break;

                default:
                    break;
            }
            PreviousSelected = id;
        }

        private void HideIllustration(int id)
        {
            switch (id)
            {
                case 0: //overview
                    this.Illustration1.Opacity = 0.0d;
                    this.OverviewLineCanvas.Opacity = 0.0d;
                    break;

                case 1: //play pause
                    this.Illustration2.Opacity = 0.0d;
                    this.Caption2.Opacity = 0.0d;
                    break;

                case 2: //skip
                    this.Illustration3.Opacity = 0.0d;
                    this.Caption3.Opacity = 0.0d;
                    break;

                case 3: //volume
                    this.Illustration4.Opacity = 0.0d;
                    this.Caption4.Opacity = 0.0d;
                    break;

                case 4: //noise
                    this.Illustration5.Opacity = 0.0d;
                    this.Caption5.Opacity = 0.0d;
                    break;

                case 5: //noise
                    this.Caption6.Opacity = 0.0d;
                    break;

                default:
                    break;
            }
        }

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                this.AppSelectorImageKB.SetSelectedID(listView.SelectedIndex);
                this.ShowIllustration(listView.SelectedIndex);
                //this.SetIllustrationHeightandWidth(listView.SelectedIndex);
               // hack to force the controltemplates to change to use the selected icon and foreground
               // dont judge me
               //this.HowToString = 

                foreach (var item in e.AddedItems)
                {
                    ListViewItem listViewItem = (ListViewItem)listView.ContainerFromItem(item);

                    listViewItem.ContentTemplate = (DataTemplate)this.Resources["SelectedListViewItem"];

                    // change background to color specced in design
                    listViewItem.Background = ViewModel.SeletedBGColor;
                }
                foreach (var item in e.RemovedItems)
                {
                    ListViewItem listViewItem = (ListViewItem)listView.ContainerFromItem(item);

                    listViewItem.ContentTemplate = (DataTemplate)this.Resources["UnselectedListViewItem"];

                    // switch background color back to original based on position in items list (alternating color rows)
                    if (listView.Items.IndexOf(item) % 2 == 0)
                    {
                        listViewItem.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Light);
                    }
                    else
                    {
                        listViewItem.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Gray);
                    }
                }
            }
        }

        #endregion

        #region INavigate Interface

        public void NavigateToPage(INavigateMoveDirection moveDirection)
        {
            AnimationHelper.PerformPageEntranceAnimation(this);
        }

        public void NavigateFromPage()
        {
            AnimationHelper.PerformPageExitAnimation(this);
        }
        #endregion

    }
}
