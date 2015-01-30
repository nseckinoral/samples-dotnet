using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class CategoryMetadataClient : BaseClient
	{
		public CategoryMetadataClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<Metadata>> GetAsync(int categoryId)
		{
			string path = string.Format("/catalog/categorymetadata?categoryId={0}", categoryId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<Metadata>>().ConfigureAwait(false);
			}
		}
	}
}