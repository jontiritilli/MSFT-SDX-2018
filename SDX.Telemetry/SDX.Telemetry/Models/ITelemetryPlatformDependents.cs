using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDX.Telemetry.Models
{
    public interface ITelemetryPlatformDependents
    {
        string GetMachineName();
        string GetMachineId();
        string GetModel();
        string GetManufacturer();
        string GetSessionId();
        string GetOfficeInfo();
        Object GetSetting(string name);
        void SetSetting(string name, object value);
        Task<Stream> GetBackupFileStream();
        void RaiseError(string message, Exception ex);
        void RaiseDebug(string message);
    }
}
