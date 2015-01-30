using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class DynamicAttributeClient : BaseClient
	{
		public DynamicAttributeClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<PaginatedContainer<DynamicAttributeType>>> GetDynamicattributeTypesAsync(int skip, int take)
		{
			string path = string.Format("/catalog/dynamicattributetypes?skip={0}&take={1}", skip, take);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<PaginatedContainer<DynamicAttributeType>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<PaginatedContainer<DynamicAttribute>>> GetDynamicAttributesAsync(int skip, int take)
		{
			string path = string.Format("/catalog/dynamicattributes?skip={0}&take={1}", skip, take);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<PaginatedContainer<DynamicAttribute>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<List<DynamicAttribute>>> GetAsync(int dynamicAttributeTypeId)
		{
			string path = string.Format("/catalog/dynamicattributes/{0}", dynamicAttributeTypeId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<DynamicAttribute>>>().ConfigureAwait(false);
			}
		}
	}
}