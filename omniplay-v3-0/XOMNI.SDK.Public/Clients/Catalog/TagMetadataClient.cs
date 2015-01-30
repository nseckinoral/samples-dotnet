using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class TagMetadataClient : BaseClient
	{
		public TagMetadataClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<Metadata>> GetAsync(int tagId)
		{
			string path = string.Format("/catalog/tagmetadata?tagId={0}", tagId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<Metadata>>().ConfigureAwait(false);
			}
		}
	}
}