using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class BrandAssetClient : BaseClient
	{
		public BrandAssetClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<List<ImageAsset>>> GetImagesAsync(int brandId, string metadataKey = null, string metadataValue = null, bool includeMetadata = false)
		{
			string path = string.Format("/catalog/brands/{0}/images?metadataKey={1}&metadataValue={2}&includeMetadata={3}", brandId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<ImageAsset>>>().ConfigureAwait(false);
			}
		}

        public async Task<List<Asset>> GetVideosAsync(int brandId, string metadataKey = null, string metadataValue = null, bool includeMetadata = false)
		{
			string path = string.Format("/catalog/brands/{0}/videos?metadataKey={1}&metadataValue={2}&includeMetadata={3}", brandId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<List<Asset>>().ConfigureAwait(false);
			}
		}

        public async Task<List<Asset>> GetDocumentsAsync(int brandId, string metadataKey = null, string metadataValue = null, bool includeMetadata = false)
		{
			string path = string.Format("/catalog/brands/{0}/documents?metadataKey={1}&metadataValue={2}&includeMetadata={3}", brandId, metadataKey, metadataValue, includeMetadata);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<List<Asset>>().ConfigureAwait(false);
			}
		}
	}
}