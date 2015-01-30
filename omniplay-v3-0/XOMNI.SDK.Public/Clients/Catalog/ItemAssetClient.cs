using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class ItemAssetClient : BaseClient
	{
		public ItemAssetClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<List<ImageAsset>>> GetImagesAsync(int itemId, string metadataKey = null, string metadataValue = null, bool includeMetadata = false)
		{
			string path = string.Format("/catalog/items/{0}/images?metadataKey={1}&metadataValue={2}&includeMetadata={3}", itemId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<ImageAsset>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<List<Asset>>> GetVideosAsync(int itemId, string metadataKey = null, string metadataValue = null, bool includeMetadata = false)
		{
            string path = string.Format("/catalog/items/{0}/images?metadataKey={1}&metadataValue={2}&includeMetadata={3}", itemId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Asset>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<List<Asset>>> GetDocumentrelationAsync(int itemId, string metadataKey = null, string metadataValue = null, bool includeMetadata = false)
		{
			string path = string.Format("/catalog/items/{0}/images?metadataKey={1}&metadataValue={2}&includeMetadata={3}", itemId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Asset>>>().ConfigureAwait(false);
			}
		}
	}
}