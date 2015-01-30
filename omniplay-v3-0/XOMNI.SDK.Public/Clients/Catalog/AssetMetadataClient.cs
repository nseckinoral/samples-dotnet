using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class AssetMetadataClient : BaseClient
	{
		public AssetMetadataClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<Metadata>> GetImageMetadataAsync(int assetId)
		{
			string path = string.Format("/catalog/imagemetadata?assetId={0}", assetId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<Metadata>>().ConfigureAwait(false);
			}
		}

		public async Task<ApiResponse<Metadata>> GetVideoMetadataAsync(int assetId)
		{
			string path = string.Format("/catalog/videometadata?assetId={0}", assetId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<Metadata>>().ConfigureAwait(false);
			}
		}

		public async Task<ApiResponse<Metadata>> GetDocumentMetadataAsync(int assetId)
		{
			string path = string.Format("/catalog/documentmetadata?assetId={0}", assetId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<Metadata>>().ConfigureAwait(false);
			}
		}
	}
}