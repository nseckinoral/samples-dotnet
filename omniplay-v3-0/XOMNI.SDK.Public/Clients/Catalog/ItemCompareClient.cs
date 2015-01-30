using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class ItemCompareClient : BaseClient
	{
		public ItemCompareClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        //TODO: Implement this methods return type when Item Models ready.
        public async Task<ApiResponse<MultipleItemSearchResult<Item>>> CompareAsync(List<int> itemIds)
		{
			string path = "/catalog/itemcompare";

			using (var response = await Client.PostAsJsonAsync(path, itemIds).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<MultipleItemSearchResult<Item>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<ItemCompareResult>> CompareMatrixAsync(List<int> itemIds)
		{
			string path = "/catalog/itemcomparematrix";

            using (var response = await Client.PostAsJsonAsync(path, itemIds).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<ItemCompareResult>>().ConfigureAwait(false);
			}
		}
	}
}