﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

using Jacumba.Core;
using Jacumba.Core.Services;

using SDX.Toolkit.Controls;
using SDX.Toolkit.Models;
using SDX.Telemetry.Services;
using SDX.Toolkit.Helpers;

using SurfaceHeadphoneDemo.ViewModels;


namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class FlipViewPage : Page, IVMUpgradeStatus
    {
        #region Private Constants

        private const double PAGE_TIMER_DURATION = 5000d;

        private const string URI_FIRMWARE_FEATURE = @"ms-appx:///Assets/shop_mode_feature.ota";
        private const string URI_FIRMWARE_EQ = @"ms-appx:///Assets/shop_mode_equalizer.ota";

        private const string FIRMWARE_VERSION_EQ = "0.2018.41.10.4";

        #endregion

        #region Private Members

        private FlipViewViewModel ViewModel
        {
            get { return DataContext as FlipViewViewModel; }
        }

        private DispatcherTimer _pageMoveTimer = null;

        private INavigate _previousPage = null;

        // headphone updates
        private HidDeviceManager _hidDeviceManager;
        private HidRequestManager _hidRequestManager;
        private int _numberOfRetries;
        private VMUpgrade _vmUpgrader;
        private ILoggerService _loggerService;
        private string _btDeviceId;
        private byte[] _fileBytes;
        private CoreDispatcher _dispatcher;
        private bool _isUpdateCompleted;
        private bool _isJoplinEnabled;

        #endregion

        #region Public Static Properties

        public static FlipViewPage Current { get; private set; }

        #endregion

        #region Construction

        public FlipViewPage()
        {
            // save a pointer to ourself
            FlipViewPage.Current = this;

            // We use NavigationCacheMode.Required to keep track the selected item on navigation. For further information see the following links.
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.controls.page.navigationcachemode.aspx
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh771188.aspx
            NavigationCacheMode = NavigationCacheMode.Required;
            InitializeComponent();

            // disappear the title bar
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            // configure focus
            this.FocusVisualMargin = new Thickness(0);
            this.FocusVisualPrimaryBrush = new SolidColorBrush(Colors.Transparent);
            this.FocusVisualPrimaryThickness = new Thickness(0);
            this.FocusVisualSecondaryBrush = new SolidColorBrush(Colors.Transparent);
            this.FocusVisualSecondaryThickness = new Thickness(0);

            // initialize the navigation bar
            foreach (NavigationSection section in ViewModel.Sections)
            {
                this.BottomNavBar.NavigationSections.Add(section);
            }
            this.BottomNavBar.IsHomeEnabled = true;

            // initialize the navigation bar root
            this.BottomNavBar.Root = ViewModel.Root;

            // initialize the music bar
            this.MusicBar.Volume = ViewModel.Volume;
            this.MusicBar.PlayerPlaylist = ViewModel.Playlist;
            this.MusicBar.Background = StyleHelper.GetAcrylicBrush(AcrylicColors.Light);

            // configure for joplin updates
            _loggerService = new Logger("FG", "JLog.txt", null);
            _hidDeviceManager = new HidDeviceManager(_loggerService, HidDevice.GetDeviceSelector(0xFF01, 0x0000, 0x045E, 0x0A1B));
            _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            this._isJoplinEnabled = Services.ConfigurationService.Current.GetIsJoplinUpdateEnabled();



            // configure our page move timer
            _pageMoveTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(PAGE_TIMER_DURATION)
            };
            _pageMoveTimer.Tick += PageMoveTimer_Tick;
            _pageMoveTimer.Start();
        }

        private void PageMoveTimer_Tick(object sender, object e)
        {
            // stop the timer
            if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // move to the next page
            if (null != this.BottomNavBar)
            {
              //  this.BottomNavBar.MoveToNextPage();
            }
        }

        #endregion


        #region Overrides

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // add joplin firmware update handler
            HidDeviceManager.HidConnectedDeviceStatusChanged += HidDeviceManager_HidConnectedDeviceStatusChanged;

            // start joplin watcher
            if (null != _hidDeviceManager)
            {
                _hidDeviceManager.StartWatcher();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // remove joplin firmware update handler
            HidDeviceManager.HidConnectedDeviceStatusChanged -= HidDeviceManager_HidConnectedDeviceStatusChanged;

            // stop joplin watcher
            if (null != _hidDeviceManager)
            {
                _hidDeviceManager.StopWatcherAsync();
            }

            base.OnNavigatedFrom(e);
        }

        #endregion


        #region Event Handlers

        private void FlipViewEx_Loaded(object sender, RoutedEventArgs e)
        {
            // set the current page
            this.ContentFlipView.SelectedIndex = 0;

            // save the current page so we can navigate from it
            _previousPage = (INavigate)((FlipViewItemEx)this.ContentFlipView.SelectedItem).GetChildViewAsObject();

            // navigate to it
            _previousPage.NavigateToPage(INavigateMoveDirection.Forward);

        }

        private void FlipViewEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // stop the page timer
            if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // if we have a bottom nav bar
            if (null != this.BottomNavBar)
            {
                // get the sender
                if (sender is FlipViewEx flipView)
                {
                    // navigate from the previous page
                    if (null != _previousPage)
                    {
                        _previousPage.NavigateFromPage();
                    }

                    // navigate to the new page
                    if (null != flipView.SelectedItem)
                    {
                        INavigateMoveDirection moveDirection = INavigateMoveDirection.Unknown;

                        // get the pageIndex of the new page
                        int nextPageIndex = flipView.SelectedIndex;

                        // find the index of the previous page
                        int previousPageIndex = flipView.GetIndexOfChildView(_previousPage);

                        // if we got it
                        if (-1 != previousPageIndex)
                        {
                            // are we moving forward or backward to get to the new page?
                            if (previousPageIndex < nextPageIndex)
                            {
                                moveDirection = INavigateMoveDirection.Forward;
                            }
                            else if (nextPageIndex < previousPageIndex)
                            {
                                moveDirection = INavigateMoveDirection.Backward;
                            }
                        }

                        // save the current page so we can navigate away from it later
                        _previousPage = (INavigate)((FlipViewItemEx)flipView.SelectedItem).GetChildViewAsObject();

                        // navigate to it
                        _previousPage.NavigateToPage(moveDirection);

                        // tell the navbar to move to it
                        this.BottomNavBar.MoveToPageIndex(nextPageIndex, (INavigateMoveDirection.Forward == moveDirection));
                    }
                }
            }
        }

        private void BottomNavBar_OnNavigation(object sender, NavigateEventArgs e)
        {
            // stop the page timer
            if (null != _pageMoveTimer) { _pageMoveTimer.Stop(); }

            // if we have a flipview
            if (null != this.ContentFlipView)
            {
                // get the sender
                if (sender is NavigationBar navBar)
                {
                    // get the page index
                    int pageIndex = navBar.Root.SelectedIndex;

                    // move the flipview to that index
                    if (pageIndex != this.ContentFlipView.SelectedIndex)
                    {
                        this.ContentFlipView.SelectedIndex = pageIndex;
                    }
                }
            }

            // telemetry - log nav sections
            if (NavigationActions.Section == e.NavAction)
            {
                // we've gone to a section, so log it
                switch (e.NavSection.Name)
                {
                    case "Experience":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavExperience);
                        break;

                    case "Accessories":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavAccessories);
                        break;

                    case "BestOfMicrosoft":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavBestOf);
                        break;

                    case "Compare":
                        TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.NavComparison);
                        break;
                }
            }

            // telemetry - log page view
            switch (e.NavItem.Section.Name)
            {
                case "Experience":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewExperience);
                    break;

                case "Accessories":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewAccessories);
                    break;

                case "BestOfMicrosoft":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewBestOf);
                    break;

                case "Compare":
                    TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.ViewComparison);
                    break;
            }
        }

        private void AppClose_Click(object sender, RoutedEventArgs e)
        {
            // log application exit
            TelemetryService.Current?.LogTelemetryEvent(TelemetryEvents.EndApplication);

            // this is not kosher by guidelines, but no other way to do this
            Application.Current.Exit();
        }

        #endregion

        #region Public Methods

        public void ShowAppClose()
        {
            if (null != this.AppClose)
            {
                this.AppClose.Visibility = Visibility.Visible;
            }
        }

        public void HideAppClose()
        {
            if (null != this.AppClose)
            {
                this.AppClose.Visibility = Visibility.Collapsed;
            }
        }

        public void SelectTrack(int Index)
        {
            if (null != this.MusicBar)// check for correct index?
            {
                this.MusicBar.MoveToTrack(Index);
            }
        }

        public Popup GetHowToPagePopup()
        {
            return this.HowToPagePopup;
        }

        public Popup GetAudioListenPopup()
        {
            return this.AudioListenPopup;
        }

        public MusicBar GetMusicBar()
        {
            return this.MusicBar;
        }

        public void EnablePageNavigation(object sender, object e)
        {
            this.BottomNavBar.IsNavigationEnabled = true;
        }

        public void DisablePageNavigation(object sender, object e)
        {
            this.BottomNavBar.IsNavigationEnabled = false;
        }

        #endregion

        #region Private Methods

        private void MusicBar_SelectionChanged(object sender, EventArgs e)
        {
            if (sender is MusicBar musicBar)
            {
                AudioListenPage.Current.ChangeSelectedTrack(musicBar.PlayerPlaylist.SelectedIndex);
            }
        }

        #endregion


        #region Joplin Upgrade Event Handlers

        private async void HidDeviceManager_HidConnectedDeviceStatusChanged(object sender, HidDeviceStatusChangedEventArgs e)
        {
            if (_isJoplinEnabled)
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    //// firmware status
                    //SetFirmwareStatus("Checking for Surface Headphones...", false);

                    // if joplin is connected
                    if (e.HidDeviceConnectionStatus == HidDeviceConnectionStatus.Connected)
                    {
                        //// firmware status
                        //SetFirmwareStatus("\rChecking for Surface Headphones...connected.");

                        // set up for update
                        _numberOfRetries = 0;
                        _btDeviceId = e.BTDeviceId;
                        _hidRequestManager = HidDeviceManager.GetConnectedDevicesRequestManager(_btDeviceId);

                        // if we didn't get what we needed, return
                        if ((null == _hidRequestManager)
                            || (null == _hidRequestManager.SoftwareVersion)
                            || (_hidRequestManager.SoftwareVersion.ToString() == FIRMWARE_VERSION_EQ))
                        {
                            // firmware status
                            string firmwareVersion = _hidRequestManager.SoftwareVersion.ToString();
                            //SetFirmwareStatus($"No update needed. Firmware version: {firmwareVersion}");
                            //ClearFirmwareStatus();
                            return;
                        }

                        // get the firmware file
                        StorageFile file = null;
                        try
                        {
                            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(URI_FIRMWARE_EQ));
                        }
                        catch { }

                        if (file == null)
                            return;

                        // get its contents as a byte array
                        _fileBytes = null;
                        using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
                        {
                            _fileBytes = new byte[stream.Size];
                            using (DataReader reader = new DataReader(stream))
                            {
                                await reader.LoadAsync((uint)stream.Size);
                                reader.ReadBytes(_fileBytes);
                            }
                        }

                        //SetFirmwareStatus("Loaded firmware file.");

                        await StartUpgrade();
                    }
                    //else
                    //{
                    //    SetFirmwareStatus("\rChecking for Surface Headphones...not found.");
                    //    ClearFirmwareStatus();
                    //}
                    return;
                });
            }
        }

        private async Task StartUpgrade()
        {
            // firmware status
            SetFirmwareStatus("Starting firmware upgrade for Surface Headphones.");

            // create the update object
            _vmUpgrader = new VMUpgrade(_loggerService,
                                        _btDeviceId,
                                        _fileBytes,
                                        new VMHidTransport(_loggerService, _hidRequestManager),
                                        this);

            _isUpdateCompleted = false;

            // call the update method
            await _vmUpgrader.BeginUpdateAsync();
        }

        private async Task EndUpgrade()
        {
            SetFirmwareStatus("Finishing upgrade.");

            if (_vmUpgrader != null)
            {
                await _vmUpgrader.PrepareForDisposalAsync();

                VMUpgrade toDispose = _vmUpgrader;
                _vmUpgrader = null;
                toDispose.Dispose();
            }

            ClearFirmwareStatus();
        }

        #endregion

        #region IVMUpgradeStatus

        public async void UpgradeError(VMUpgradeError error)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                // ignore the disconnect message when the update is completed
                if (_isUpdateCompleted)
                    return;

                await EndUpgrade();

                if (_numberOfRetries < 2)
                {
                    _numberOfRetries++;
                    SetFirmwareStatus($"Upgrade failed. Retrying upgrade, retry #{_numberOfRetries}.");
                    await StartUpgrade();
                }
                else
                {
                    SetFirmwareStatus("Upgrade failed.");
                    ClearFirmwareStatus();
                }
            });
        }

        public async void UpgradeProgress(uint percentComplete)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //updateStatusTextBlock.Text = $"{percentComplete}%";
                SetFirmwareStatus($"\rFirmware upgrade is {percentComplete}% complete.", false);
            });
        }

        public async void UpgradeDownloadCompleted()
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                _isUpdateCompleted = true;
                //updateStatusTextBlock.Text = "Completed";
                SetFirmwareStatus("Firmware upgrade completed.");
                ClearFirmwareStatus();
                await _vmUpgrader.CommitUpdateAsync();
                await EndUpgrade();
            });
        }

        public async void UpgradeInstallationCompleted()
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                _isUpdateCompleted = true;
                //updateStatusTextBlock.Text = "Completed";
                SetFirmwareStatus("Firmware upgrade completed.");
                ClearFirmwareStatus();
                await _vmUpgrader.CommitUpdateAsync();
                await EndUpgrade();
            });

        }

        #endregion

        private async void SetFirmwareStatus(string message, bool includeReturn = true)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (Visibility.Collapsed == this.FirmwareStatusBorder.Visibility)
                {
                    this.FirmwareStatusBorder.Visibility = Visibility.Visible;
                }

                // firmware status
                this.FirmwareStatus.Text = message;
                if (includeReturn)
                {
                    Debug.WriteLine(message);
                }
                else
                {
                    Debug.Write(message);
                }
            });
        }

        private async void ClearFirmwareStatus()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                this.FirmwareStatusBorder.Visibility = Visibility.Collapsed;
            });
        }
    }
}
