using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


using Jacumba.Core;
using Jacumba.Core.Services;

using SurfaceHeadphoneDemo.ViewModels;
using SurfaceHeadphoneDemo.Services;


namespace SurfaceHeadphoneDemo.Views
{
    public sealed partial class FirmwareUpdatePage : Page, IVMUpgradeStatus
    {
        #region Private Constants

        private const string URI_FIRMWARE_FEATURE = @"ms-appx:///Assets/shop_mode_feature.ota";
        private const string URI_FIRMWARE_EQ = @"ms-appx:///Assets/shop_mode_equalizer.ota";

        private const string FIRMWARE_VERSION_EQ = "0.2018.41.10.4";

        private const int UPGRADE_RETRIES = 0;

        #endregion


        #region Private Members

        private FirmwareUpdateViewModel ViewModel
        {
            get { return DataContext as FirmwareUpdateViewModel; }
        }

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

        // page move timer
        DispatcherTimer _timeOutTimer = null;

        #endregion


        #region Construction

        public FirmwareUpdatePage()
        {
            InitializeComponent();

            // configure for joplin updates
            _loggerService = new Logger("FG", "JLog.txt", null);
            _hidDeviceManager = new HidDeviceManager(_loggerService, HidDevice.GetDeviceSelector(0xFF01, 0x0000, 0x045E, 0x0A1B));
            _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            // is joplin enabled?
            _isJoplinEnabled = Services.ConfigurationService.Current.GetIsJoplinUpdateEnabled();

            // event handlers
            this.Loaded += this.FirmwareUpdatePage_Loaded;
        }

        #endregion


        #region Event Handlers

        private async void FirmwareUpdatePage_Loaded(object sender, RoutedEventArgs e)
        {
            UInt32 vid = 0x045E;
            UInt32 pid = 0x0A1B;

            // if joplin is not enabled, then just go to the next page
            if (!_isJoplinEnabled)
            {
                NavigateToNext();
            }

            // set up a timer; if the jacumba status changed event doesn't fire
            // in 5 seconds, we'll just move to the next page
            _timeOutTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(2000) };
            _timeOutTimer.Tick += this._timeOutTimer_Tick;
            _timeOutTimer.Start();
        }

        private void _timeOutTimer_Tick(object sender, object e)
        {
            // stop the timer
            _timeOutTimer.Stop();

            // move to the next page
            NavigateToNext();
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


        #region Joplin Upgrade Event Handlers

        private async void HidDeviceManager_HidConnectedDeviceStatusChanged(object sender, HidDeviceStatusChangedEventArgs e)
        {
            if (_isJoplinEnabled)
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    // if joplin is connected
                    if (e.HidDeviceConnectionStatus == HidDeviceConnectionStatus.Connected)
                    {
                        // set up for update
                        _numberOfRetries = 0;
                        _btDeviceId = e.BTDeviceId;
                        _hidRequestManager = HidDeviceManager.GetConnectedDevicesRequestManager(_btDeviceId);

                        // if we didn't get what we needed, return
                        if ((null == _hidRequestManager)
                            || (null == _hidRequestManager.SoftwareVersion)
                            || (_hidRequestManager.SoftwareVersion.ToString() != FIRMWARE_VERSION_EQ))
                        {
                            // no upgrade possible or needed, so navigate to the next page
                            NavigateToNext();

                            return;
                        }

                        // show the update panel
                        ShowUpdatePanel();

                        // get the firmware file
                        StorageFile file = null;
                        try
                        {
                            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(URI_FIRMWARE_EQ));
                        }
                        catch { }

                        if (file == null)
                        {
                            // can't load firmware file, so go next
                            NavigateToNext();

                            return;
                        }

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

                        await StartUpgrade();
                    }
                    else
                    {
                        // joplin is not connected, so can't do update
                        NavigateToNext();
                    }
                });
            }
            else
            {
                // if we're here and the joplin firmware update is not enabled
                // then we need to navigate to the flipviewpage
                NavigateToNext();
            }
        }

        private async Task StartUpgrade()
        {
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
            if (_vmUpgrader != null)
            {
                await _vmUpgrader.PrepareForDisposalAsync();

                VMUpgrade toDispose = _vmUpgrader;
                _vmUpgrader = null;
                toDispose.Dispose();
            }
        }

        #endregion

        #region IVMUpgradeStatus

        public async void UpgradeError(VMUpgradeError error)
        {
            bool failed = false;

            if (VMUpgradeErrorStatus.Disconnected == error.Status)
            {
                failed = true;
            }
            else
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    // ignore the disconnect message when the update is completed
                    if (_isUpdateCompleted)
                        return;

                    await EndUpgrade();

                    if (_numberOfRetries < UPGRADE_RETRIES)
                    {
                        _numberOfRetries++;
                        await StartUpgrade();
                    }
                    else
                    {
                        failed = true;
                    }
                });
            }

            // if the upgrade exhausted retries and failed, go to the next page
            if (failed)
            {
                NavigateToNext();
            }
        }

        public async void UpgradeProgress(uint percentComplete)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.ProgressBar.Value = percentComplete;
            });
        }

        public async void UpgradeDownloadCompleted()
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                _isUpdateCompleted = true;
                await _vmUpgrader.CommitUpdateAsync();
                await EndUpgrade();
            });
        }

        public async void UpgradeInstallationCompleted()
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                _isUpdateCompleted = true;
                await _vmUpgrader.CommitUpdateAsync();
                await EndUpgrade();
            });
        }

        #endregion

        // this method makes the page content visible
        public async void ShowUpdatePanel()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                // hide the search panel
                this.SearchPanel.Visibility = Visibility.Collapsed;

                // show the update panel
                this.UpdatePanel.Visibility = Visibility.Visible;
            });
        }

        // this moves to the flipviewpage
        public void NavigateToNext()
        {
            // get the locator from app.xaml
            ViewModels.ViewModelLocator Locator = Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

            // use it to get the navigation service
            NavigationServiceEx NavigationService = Locator?.NavigationService;

            // navigate to the flipview page
            NavigationService?.Navigate(typeof(FlipViewViewModel).FullName);
        }
    }
}
