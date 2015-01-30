using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Company;

namespace XOMNI.SDK.Public.Clients.Company
{
	public class DeviceStorageClient : BaseClient
	{
		public DeviceStorageClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<List<DeviceStorageItem>>> GetAsync(string deviceId, string Key, bool Delete)
		{
			string path = string.Format("/company/devices/{0}/storage?key={1}&delete={2}", deviceId, Key, Delete);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<DeviceStorageItem>>>().ConfigureAwait(false);
			}
		}

		public async Task DeleteAsync(string deviceId, string Key)
		{
			string path = string.Format("/company/devices/{0}/storage?key={1}", deviceId, Key);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<DeviceStorageItem>> PostAsync(string deviceId, DeviceStorageItem storageItem)
		{
			string path = string.Format("/company/devices/{0}/storage", deviceId);

            using (var response = await Client.PostAsJsonAsync(path, storageItem).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<dynamic>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<DeviceStorageItem>> PutAsync(string deviceId, DeviceStorageItem storageItem)
		{
			string path = string.Format("/company/devices/{0}/storage", deviceId);

            using (var response = await Client.PutAsJsonAsync(path, storageItem).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<DeviceStorageItem>>().ConfigureAwait(false);
			}
		}
	}
}