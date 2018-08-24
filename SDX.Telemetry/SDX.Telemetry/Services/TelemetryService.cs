using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using SDX.Telemetry.Models;

// This code is taken from the BTS project. Minimal refactoring and commenting were done.
// comment

namespace SDX.Telemetry.Services
{
    [DataContract]
    public class TelemetryObject
    {
        [DataMember]
        public DateTime TimeRaised;
        [DataMember]
        public string Event;
        [DataMember]
        public string Parameters;
        [DataMember]
        public bool? IsResponseSuccess;
        [DataMember]
        public string Response;

        public TelemetryObject(string _event, string _params)
        {
            Event = _event;
            Parameters = _params;
            TimeRaised = DateTime.UtcNow;
            IsResponseSuccess = null;
            Response = null;
        }
    }

    public class TelemetryService
    {
        public const string TELEMETRY_STARTAPPLICATION = "StartApplication";
        public const string TELEMETRY_ENDAPPLICATION = "EndApplication";
        public const string TELEMETRY_STARTSAPPLICATIONTYPE = "StartsApplicationType";
        public const string TELEMETRY_IMPLEMENTDOWN = "ImplementDown";
        public const string TELEMETRY_NAVEXPERIENCE = "NavExperience";
        public const string TELEMETRY_NAVACCESSORIES = "NavAccessories";
        public const string TELEMETRY_NAVBESTOF = "NavBestOf";
        public const string TELEMETRY_NAVCOMPARISON = "NavComparison";
        public const string TELEMETRY_FIRSTSWIPE = "FirstSwipe";
        public const string TELEMETRY_PORTABLEVIEWPIXELSENSE = "PortableViewOne";
        public const string TELEMETRY_PORTABLEVIEWLIGHT = "PortableViewTwo";
        public const string TELEMETRY_PORTABLEVIEWTHIN = "PortableViewThree";
        public const string TELEMETRY_CREATEFRESHPAINT = "CreateFreshPaint";
        public const string TELEMETRY_PRODUCTROTATION = "ProductRotation";
        public const string TELEMETRY_WATCHVIEWGALLERY = "WatchViewOne";
        public const string TELEMETRY_WATCHVIEWFASTCHARGE = "WatchViewTwo";
        public const string TELEMETRY_WORKONENOTE = "WorkOneNote";
        public const string TELEMETRY_STARTPEN = "StartPen";
        public const string TELEMETRY_ENDPEN = "EndPen";
        public const string TELEMETRY_KEYBOARDVIEWCOLOR = "KeyboardViewColor";
        public const string TELEMETRY_KEYBOARDVIEWPEN = "KeyboardViewOne";
        public const string TELEMETRY_KEYBOARDVIEWALCANTARA = "KeyboardViewTwo";
        public const string TELEMETRY_KEYBOARDVIEWANGLE = "KeyboardViewThree";
        public const string TELEMETRY_MOUSEVIEWCOLOR = "MouseViewColor";
        public const string TELEMETRY_MOUSEVIEWTRACKPAD = "MouseViewOne";
        public const string TELEMETRY_MOUSEVIEWMOUSE = "MouseViewTwo";
        public const string TELEMETRY_BESTOFCTAWINDOWS = "BestRDAWindows";
        public const string TELEMETRY_BESTOFCTAHELLO = "BestHello";
        public const string TELEMETRY_BESTOFCTAOFFICE = "BestRDAOffice";
        public const string TELEMETRY_BESTOFCTABULLET5 = "Best5";
        public const string TELEMETRY_COMPARESEEOTHERS = "ComparisonViewOne";
        public const string TELEMETRY_COMPARESWIPE = "ComparisonSwipe";


        public static TelemetryService Current { get; private set; }

        public TelemetryService()
        {
            // save ourself
            TelemetryService.Current = this;
        }

        private string _baseUrl;
        private Stream _backupStream;
        private string _telemetryCode;
        private bool _isAcceptNewEntries = false;
        private object _lockObject = new object();
        private ITelemetryPlatformDependents _platformDependents;
        private bool _isWorkerAllowedToRun = true;
        private string _prevSessionId = null;

        private List<TelemetryObject> _backupQueue = new List<TelemetryObject>();
        BackgroundWorker _telemetryWorker = null;


        public async Task<bool> Initialize(string telemetryBaseUrl, string telemetryId, ITelemetryPlatformDependents platformProperties)
        {
            _isAcceptNewEntries = false;
            if (platformProperties == null)
            {
                return false;
            }
            _platformDependents = platformProperties;
            _baseUrl = telemetryBaseUrl;
            //if (!_baseUrl.EndsWith("?"))
            //    _baseUrl += "?";
            //.net standard cannot access the file system on UWP, 
            //we need to open a StorageFile from the UWP app and pass the stream so we can write backup to disk
            _backupStream = await platformProperties.GetBackupFileStream();
            _telemetryCode = telemetryId;

            LoadSavedTelemetry();

            _telemetryWorker = new BackgroundWorker();
            _telemetryWorker.DoWork += Telemetry_OnDoWork;
            _telemetryWorker.RunWorkerCompleted += OnRunWorkerCompleted;
            _telemetryWorker.WorkerSupportsCancellation = true;
            _isAcceptNewEntries = true;

            string prevSId = (string)_platformDependents.GetSetting("SessionId");
            if (!string.IsNullOrWhiteSpace(prevSId))
            {
                if (prevSId != _platformDependents.GetSessionId())
                {
                    _prevSessionId = prevSId;
                    SendTelemetry("SessionEnd", System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture), false);
                    _prevSessionId = null;
                }
            }
            _platformDependents.SetSetting("SessionId", _platformDependents.GetSessionId());

            _isWorkerAllowedToRun = true;
            if (!_telemetryWorker.IsBusy && _backupQueue.Count > 0)
                _telemetryWorker.RunWorkerAsync();

            return true;
        }

        bool noNewEntries = false;

        private async void Telemetry_OnDoWork(object o, DoWorkEventArgs args)
        {
            noNewEntries = false;
            while (_backupQueue.Count > 0 && _isWorkerAllowedToRun && !noNewEntries)
            {
                TelemetryObject currentObject = null;
                lock (_lockObject)
                {
                    foreach (var item in _backupQueue)
                    {
                        if (item.IsResponseSuccess == null)
                        {
                            currentObject = item;
                            break;
                        }
                    }
                }

                if (currentObject != null)
                {
                    //do http calls
                    HttpResponseMessage response = await GetUrl(_baseUrl + currentObject.Parameters);

                    //check result and update the backup collection with results
                    if (response != null)
                    {
                        currentObject.Response = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            _platformDependents.RaiseError(currentObject.Response, null);
                            currentObject.IsResponseSuccess = false;
                        }
                        else
                        {
                            currentObject.IsResponseSuccess = true;
                            _backupQueue.Remove(currentObject);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    //only failed items left
                    noNewEntries = true;
                }

                if (_telemetryWorker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }

                if (_isWorkerAllowedToRun)
                    System.Threading.Thread.Sleep(1000);
            }
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            //if (_backupStream.Length > 0)
            //    _backupStream.SetLength(0);
            if (!_isWorkerAllowedToRun)
            {
                _platformDependents.RaiseDebug("TelemetryWorker Finished after cancellation");
            }
        }

        private async Task<HttpResponseMessage> GetUrl(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(new Uri(url));
                //response.EnsureSuccessStatusCode();
                //responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                response = null;
#if (DEBUG)
                _platformDependents.RaiseError(ex.Message, ex);
#endif
            }
            //-------------------------------------------------------------------------------------------------
            Debug.WriteLine(url + "   " + System.DateTime.Now.ToString("o") + " >>>>>");
            return response;
        }

        private void LoadSavedTelemetry()
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<TelemetryObject>));

            //load previous collection from disk if any
            _backupStream.Seek(0, SeekOrigin.Begin);
            if (_backupStream.Length > 0)
            {
                List<TelemetryObject> pastTelemetries = (List<TelemetryObject>)ser.ReadObject(_backupStream);

                lock (_lockObject)
                {
                    if (pastTelemetries?.Count > 0)
                    {
                        foreach (var item in pastTelemetries)
                        {
                            item.IsResponseSuccess = null;
                            _backupQueue.Add(item);
                        }
                    }
                }
            }
        }


        public void SendTelemetry(string eventName, string value, bool startWorker = true, int duration = 0)
        {
            // if we have no telemetry key (for example, we have no test key), just return
            if (String.IsNullOrEmpty(_telemetryCode)) { return; }

            // if we aren't accepting new entries, return
            if (!_isAcceptNewEntries) { return; }

            //put together the params and all the bacground information and add to collection
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("code", _telemetryCode);
            parameters.Add("device", _platformDependents.GetMachineId());
            parameters.Add("name", _platformDependents.GetMachineName());
            if (string.IsNullOrWhiteSpace(_prevSessionId))
                parameters.Add("sid", _platformDependents.GetSessionId());
            else
                parameters.Add("sid", _prevSessionId);
            parameters.Add("model", _platformDependents.GetModel());
            parameters.Add("manufacturer", _platformDependents.GetManufacturer());
            parameters.Add("datetime", System.DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
            parameters.Add("event", eventName);
            parameters.Add("duration", duration.ToString());
            parameters.Add("value", value);

            TelemetryObject tObj = new TelemetryObject(eventName, ToQueryString(parameters));

            lock (_lockObject)
            {
                _backupQueue.Add(tObj);
            }

            //check if background worker running and start if not
            if (!_telemetryWorker.IsBusy && startWorker)
            {
                _telemetryWorker.RunWorkerAsync();
            }
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();
            return "?" + string.Join("&", array);
        }

        public async Task CloseTelemetry()
        {
            _isAcceptNewEntries = false;
            _isWorkerAllowedToRun = false;
            //todo: close worker and save to file

            if ((null != _telemetryWorker) && (_telemetryWorker.IsBusy))
            {
                _telemetryWorker.CancelAsync();
            }

            if (null != _backupStream)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<TelemetryObject>));

                using (_backupStream)
                {
                    _backupStream.SetLength(0);
                    lock (_lockObject)
                    {
                        if (_backupQueue?.Count > 0)
                        {
                            ser.WriteObject(_backupStream, _backupQueue);
                        }
                    }
                    await _backupStream.FlushAsync();
                }
                _backupStream = null;
            }
        }
    }
}
