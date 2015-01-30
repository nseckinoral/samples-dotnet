using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.OmniPlay;

namespace XOMNI.SDK.Public.Clients.OmniPlay
{
	public class DeviceClient : BaseClient
	{
		public DeviceClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<List<OmniTicketDetail>>> GetIncomingsAsync(string deviceId)
		{
			string path = string.Format("/omniplay/devices/{0}/incoming", deviceId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<OmniTicketDetail>>>().ConfigureAwait(false);
			}
		}

        //TODO: Requires PIIToken
        public async Task SubscribeToDevice(string deviceId)
        {
            string path = string.Format("/omniplay/devices/{0}", deviceId);
            await Client.PostAsync(path, null).ConfigureAwait(false);
        }

        //public async Task<ApiResponse<List<Device>>> GetAsync()
        //{
        //    string path = "/omniplay/store/devices";

        //    using (var response = await Client.GetAsync(path).ConfigureAwait(false))
        //    {
        //        return await response.Content.ReadAsAsync<ApiResponse<List<Device>>>().ConfigureAwait(false);
        //    }
        //}

        //public async Task<ApiResponse<List<Device>>> GetAsync(int searchDistance, string GPSLocation)
        //{
        //    string path = string.Format("/omniplay/stores/{GPSLocation}/Devices?searchDistance={0}", searchDistance, GPSLocation);

        //    using (var response = await Client.GetAsync(path).ConfigureAwait(false))
        //    {
        //        return await response.Content.ReadAsAsync<ApiResponse<List<Device>>>().ConfigureAwait(false);
        //    }
        //}

        //public async Task<ApiResponse<Device>> RegisterAsync(Device device)
        //{
        //    string path = "/device/register";

        //    using (var response = await Client.PostAsJsonAsync(path, device).ConfigureAwait(false))
        //    {
        //        return await response.Content.ReadAsAsync<ApiResponse<Device>>().ConfigureAwait(false);
        //    }
        //}
	}
}