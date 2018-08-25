using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System.Profile;

namespace SDX.Telemetry.Models
{
    //since the telemetry project cannot implement everything for every platform , we need to implement the platform dependent parts
    //based on the ITelemetryPlatformDependents interface so we can get the required information for completing the telemetry calls
    public class UWPTelemetryDependent : ITelemetryPlatformDependents
    {
        private string _sessionId = Guid.NewGuid().ToString();

        private Stream _backupStream = null;

        public async Task<Stream> GetBackupFileStream()
        {
            bool state = false;
            if (null ==_backupStream)
            {
                state = false;
            }
            else
            {
                try
                {
                    if (_backupStream.CanWrite)
                    {
                        _backupStream.Write(new byte[1], 0, 0);
                        state = true;
                    }
                    else
                    {
                        state = false;
                    }
                }
                catch (Exception ex)
                {
                    state = false;
                }
            }

            if (state)
                return _backupStream;
            else
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile backupFile = await storageFolder.CreateFileAsync("TelemetryBackup.json", CreationCollisionOption.OpenIfExists);
                return _backupStream = await backupFile.OpenStreamForWriteAsync();
            }
        }

        public string GetMachineId()
        {
            if (RetailInfo.IsDemoModeEnabled)
                return RetailInfo.Properties["MachineId"].ToString();
            else
                return "TestMachineId";
        }

        public string GetMachineName()
        {
            var hostNames = NetworkInformation.GetHostNames();
            var hostName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "Unknown Machine";
            return hostName;
        }

        public string GetManufacturer()
        {
            if (RetailInfo.IsDemoModeEnabled)
                return RetailInfo.Properties[KnownRetailInfoProperties.ManufacturerName].ToString();
            else
                return "TestManufacturer";
        }

        public string GetModel()
        {
            if (RetailInfo.IsDemoModeEnabled)
                return RetailInfo.Properties[KnownRetailInfoProperties.ModelName].ToString();
            else
                return "TestModel";
        }

        public string GetOfficeInfo()
        {
            if (RetailInfo.IsDemoModeEnabled)
                return RetailInfo.Properties["ProductReleaseIds"].ToString();
            else
                return "";
        }

        public string GetSessionId()
        {
            return _sessionId;
        }

        public Object GetSetting(string name)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values[name];
        }

        public void SetSetting(string name, object value)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[name] = value;
        }

        public void RaiseError(string message, Exception ex)
        {
            //TODO
            //App.LogManager.Error(message, ex);
        }

        public void RaiseDebug(string message)
        {
            //TODO
            //App.LogManager.Debug(message);
        }
    }
}
