using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Company;

namespace XOMNI.SDK.Public.Clients.Company
{
	public class DeviceTypesClient : BaseClient
	{
		public DeviceTypesClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<PaginatedContainer<DeviceType>>> GetAsync(int skip, int take)
		{
			string path = string.Format("/company/devicetypes?skip={0}&take={1}", skip, take);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<PaginatedContainer<DeviceType>>>().ConfigureAwait(false);
			}
		}
	}
}