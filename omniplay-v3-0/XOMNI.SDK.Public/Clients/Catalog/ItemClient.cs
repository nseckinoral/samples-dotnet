using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class ItemClient : BaseClient
	{
		public ItemClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<SingleItemSearchResult<Item>>> GetAsync(int id, bool includeItemStaticProperties, bool includeItemDynamicProperties, int imageAssetDetail, int videoAssetDetail, int documentAssetDetail)
		{
			string path = string.Format("/catalog/item/{0}?includeItemMetadata={1}&includeItemStaticProperties={2}&includeItemDynamicProperties={3}&imageAssetDetail={4}&videoAssetDetail={5}&documentAssetDetail={6}", id, includeItemStaticProperties, includeItemDynamicProperties, imageAssetDetail, videoAssetDetail, documentAssetDetail);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<SingleItemSearchResult<Item>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<Navigation>> GetSearchOptions(ItemSearchOptionsRequest itemSearchOptionsRequest)
		{
			string path = "/catalog/itemsearchoptions";

            using (var response = await Client.PostAsJsonAsync(path, itemSearchOptionsRequest).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<Navigation>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<MultipleItemSearchResult<Item>>> Search(bool IncludeItemMetadata, ItemSearchRequest itemSearchRequest)
		{
			string path = string.Format("/catalog/items?includeItemMetadata={0}", IncludeItemMetadata);

            using (var response = await Client.PostAsJsonAsync(path, itemSearchRequest).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<MultipleItemSearchResult<Item>>>().ConfigureAwait(false);
			}
		}
	}
}