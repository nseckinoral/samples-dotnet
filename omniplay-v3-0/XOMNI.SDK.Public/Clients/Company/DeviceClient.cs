using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Company;

namespace XOMNI.SDK.Public.Clients.Company
{
	public class DeviceClient : BaseClient
	{
		public DeviceClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task DeleteAsync(string deviceId)
		{
			string path = string.Format("/company/devices/{0}", deviceId);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<List<Device>>> GetAsync(int searchDistance, string GPSLocation, int deviceTypeId, string metadataKey, string metadataValue, bool includeMetadata)
		{
			string path = string.Format("/company/stores/{0}/devices?searchDistance={0}&deviceTypeId={1}&metadataKey={2}&metadataValue={3}&includeMetadata={4}", searchDistance, GPSLocation, deviceTypeId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Device>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<List<Device>>> GetAsync(int deviceTypeId, string metadataKey, string metadataValue, bool includeMetadata)
		{
			string path = string.Format("/company/stores/devices?deviceTypeId={0}&metadataKey={1}&metadataValue={2}&includeMetadata={3}", deviceTypeId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Device>>>().ConfigureAwait(false);
			}
		}

        //TODO:Implement HTTP Patch
        //public async Task<ApiResponse<Device>> PatchAsync(string deviceId, Device device)
        //{
        //    string path = string.Format("/company/devices/{deviceId}", deviceId);

        //    using (var response = await Client.PutAsJsonAsync(path, device).ConfigureAwait(false))
        //    {
        //        return await response.Content.ReadAsAsync<ApiResponse<Device>>().ConfigureAwait(false);
        //    }
        //}

        public async Task<ApiResponse<Device>> PostAsync(Device device)
		{
			string path = "/company/devices";

            using (var response = await Client.PostAsJsonAsync(path, device).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<Device>>().ConfigureAwait(false);
			}
		}
	}
}