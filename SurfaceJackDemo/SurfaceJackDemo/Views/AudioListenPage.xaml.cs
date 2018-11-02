using System;

using Windows.UI.Xaml.Controls;

using SurfaceJackDemo.ViewModels;
using SDX.Toolkit.Helpers;
using SDX.Toolkit.Controls;
using Jacumba.Core;
using Jacumba.Core.Services;
using System.Threading.Tasks;
using Windows.Devices.HumanInterfaceDevice;
using Windows.ApplicationModel.Core;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;

namespace SurfaceJackDemo.Views
{
    public sealed partial class AudioListenPage : Page, INavigate, IVMUpgradeStatus
    {
        #region Private Members

        private AudioListenViewModel ViewModel
        {
            get { return DataContext as AudioListenViewModel; }
        }
        private bool HasInteracted = false;
        private bool HasLoaded = false;
        private bool HasNavigatedTo = false;

        // headphone updates
        private ListView PlayerListView;
        private HidDeviceManager _hidDeviceManager;
        private HidRequestManager _hidRequestManager;
        private int _numberOfRetries;
        private VMUpgrade _vmUpgrader;
        private ILoggerService _loggerService;
        private string _btDeviceId;
        private byte[] _fileBytes;
        private CoreDispatcher _dispatcher;
        private bool _isUpdateCompleted;

    #endregion

    #region public members

    public Popup ReadyScreen;
    public Popup HowToScreen;
    public MusicBar MusicBar;

    #endregion

    #region Public Static Members

    public static AudioListenPage Current { get; private set; }

    #endregion

    #region Construction

    public AudioListenPage()
    {
        InitializeComponent();

        AudioListenPage.Current = this;

        this.PlayerListView = this.itemListView;

        //setup updater services
        _loggerService = new Logger("FG", "JLog.txt", null);
        _hidDeviceManager = new HidDeviceManager(_loggerService, HidDevice.GetDeviceSelector(0xFF01, 0x0000, 0x045E, 0x0A1B));
        _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

        this.Loaded += AudioListenPage_Loaded;
    }

    #endregion

    #region Public Methods

    public void ChangeSelectedTrack(int Index)
    {
        if (null != this.PlayerListView)
        {
            this.PlayerListView.SelectedIndex = Index;
        }            
    }

    public void AnimatePageEntrance()
    {
        ShowPopup();
        AnimationHelper.PerformPageEntranceAnimation(this);
        rBtnLeft.StartEntranceAnimation();
        rBtnLeft.StartRadiateAnimation();
    }

    #endregion

    #region Private Methods

    private void AudioListenPage_Loaded(object sender, RoutedEventArgs e)
    {
        NavigateFromPage();

        // get the initial screen cover popup
        this.ReadyScreen = FlipViewPage.Current.GetAudioListenPopup();
        AudioListenPopupPage.Current.CloseButton_Clicked += AudioTryItClose_Button_Clicked;

        // get the howto screen popup
        this.HowToScreen = FlipViewPage.Current.GetHowToPagePopup();
        this.rBtnLeft.PopupChild = HowToScreen;
        HowToScreen.Opened += PerformHowToEntranceAnimation;
        HowToPage.Current.CloseButton_Clicked += HowToCloseButton_Clicked;

        // get the music bar
        this.MusicBar = FlipViewPage.Current.GetMusicBar();

        if (this.HasNavigatedTo)
        {
            AnimatePageEntrance();
        }

        if (null != itemListView)
        {
            this.itemListView.Background = new SolidColorBrush(Colors.Black);

            this.itemListView.SelectedIndex = 0;
        }

        this.HasLoaded = true;
    }

    private void PerformHowToEntranceAnimation(object sender, object e)
    {
        HowToPage.Current?.PerformOverViewEntrance();
    }

    private void HowToCloseButton_Clicked(object sender, RoutedEventArgs e)
    {
        this.rBtnLeft.HandleClick();
    }

    private void AudioTryItClose_Button_Clicked(object sender, RoutedEventArgs e)
    {
        HasInteracted = true;
        HidePopup();
        AnimatePageEntrance();
        MusicBar.Play();
    }

    private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListView itemListView)
        {
            FlipViewPage.Current.SelectTrack(itemListView.SelectedIndex);
            // hack to force the controltemplates to change to use the selected icon and foreground
            // dont judge me
            this.itemListView.ScrollIntoView(this.itemListView.SelectedItem);
            foreach (var item in e.AddedItems)
            {
                ListViewItem listViewItem = (ListViewItem)itemListView.ContainerFromItem(item);

                listViewItem.ContentTemplate = (DataTemplate)this.Resources["SelectedPlayListViewItem"];
            }
            foreach (var item in e.RemovedItems)
            {
                ListViewItem listViewItem = (ListViewItem)itemListView.ContainerFromItem(item);
                listViewItem.ContentTemplate = (DataTemplate)this.Resources["PlayListViewItem"];
            }
        }
    }

    private void ClosePopupsOnExit()
    {
        if (null != rBtnLeft.PopupChild && rBtnLeft.PopupChild.IsOpen)
        {
            rBtnLeft.PopupChild.IsOpen = false;
        }
        HidePopup();
    }

    private void ShowPopup()
    {
        if (null != ReadyScreen && !HasInteracted)
        {
            ReadyScreen.IsOpen = true;
        }
    }

    private void HidePopup()
    {
        if (null != ReadyScreen && ReadyScreen.IsOpen)
        {
            ReadyScreen.IsOpen = false;
        }
    }
    private async void HidDeviceManager_HidConnectedDeviceStatusChanged(object sender, HidDeviceStatusChangedEventArgs e)
    {
        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        {
            if (e.HidDeviceConnectionStatus == HidDeviceConnectionStatus.Connected)
            {
                _numberOfRetries = 0;
                _btDeviceId = e.BTDeviceId;
                connectionStatusTextBlock.Text = "Connected";
                _hidRequestManager = HidDeviceManager.GetConnectedDevicesRequestManager(_btDeviceId);

                if (_hidRequestManager == null ||
                    _hidRequestManager.SoftwareVersion == null ||
                    _hidRequestManager.SoftwareVersion.ToString() == "1.0.3.337.21")
                    return;

                StorageFile file = null;
                try
                {
                    file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/1_0_3_337.ota"));
                }
                catch { }

                if (file == null)
                    return;

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
            return;
        });
    }

    private async Task StartUpgrade()
    {
        _vmUpgrader = new VMUpgrade(_loggerService,
                                    _btDeviceId,
                                    _fileBytes,
                                    new VMHidTransport(_loggerService, _hidRequestManager),
                                    this);

        _isUpdateCompleted = false;
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
        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        {
            // ignore the disconnect message when the upgrade is completed
            if (_isUpdateCompleted)
                return;

            updateStatusTextBlock.Text = $"Error {error.Status} {error.FWErrorCode}";
            await EndUpgrade();

            if (_numberOfRetries < 2)
            {
                _numberOfRetries++;
                await StartUpgrade();
            }
        });
    }

    public async void UpgradeProgress(uint percentComplete)
    {
        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        {
            updateStatusTextBlock.Text = $"{percentComplete}%";
        });
    }

    public async void UpgradeDownloadCompleted()
    {
        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        {
            _isUpdateCompleted = true;
            updateStatusTextBlock.Text = "Completed";
            await _vmUpgrader.CommitUpdateAsync();
            await EndUpgrade();
        });
    }

    public async void UpgradeInstallationCompleted()
    {
        await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        {
            _isUpdateCompleted = true;
            updateStatusTextBlock.Text = "Completed";
            await _vmUpgrader.CommitUpdateAsync();
            await EndUpgrade();
        });
    }

    #endregion
    #region INavigate Interface

    public void NavigateToPage(INavigateMoveDirection moveDirection)
    {
        HidDeviceManager.HidConnectedDeviceStatusChanged += HidDeviceManager_HidConnectedDeviceStatusChanged;
        _hidDeviceManager.StartWatcher();
        // animations in
        if (AudioListenPage.Current.HasLoaded)
        {
            AnimatePageEntrance();
        }
        else
        {
            this.HasNavigatedTo = true;
        }
    }

    public void NavigateFromPage()
    {
        HidDeviceManager.HidConnectedDeviceStatusChanged -= HidDeviceManager_HidConnectedDeviceStatusChanged;
        _hidDeviceManager.StopWatcherAsync();

        // animations out
        AnimationHelper.PerformPageExitAnimation(this);

            ClosePopupsOnExit();
        }

        #endregion
    }
}
