using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Helpers
{
    public class DeviceIdentity
    {
        //Official documentation; https://msdn.microsoft.com/en-us/library/windows/apps/jj553431.aspx
        public static string GetASHWID()
        {
            var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
            string installationId = Windows.Security.Cryptography.CryptographicBuffer.EncodeToBase64String(token.Id);
            return installationId;
        }

        public static string GetFriendlyName()
        {
            string deviceFriendlyName = "";
            var hostNamesList = Windows.Networking.Connectivity.NetworkInformation
                .GetHostNames();

            foreach (var entry in hostNamesList)
            {
                if (entry.Type == Windows.Networking.HostNameType.DomainName)
                {
                    deviceFriendlyName = entry.CanonicalName;
                }
            }

            //Outputting a friendly name in case device name is not found.
            if (string.IsNullOrEmpty(deviceFriendlyName))
            {
                deviceFriendlyName = "Device " + DateTime.Now.TimeOfDay;
            }

            return deviceFriendlyName;
        }
    }
}
