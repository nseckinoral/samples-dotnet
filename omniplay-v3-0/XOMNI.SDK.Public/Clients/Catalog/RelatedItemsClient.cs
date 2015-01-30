using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class RelatedItemsClient : BaseClient
	{
		public RelatedItemsClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<List<int>>> GetAsync(int itemId)
		{
			string path = string.Format("/catalog/relatedItems?itemId={0}", itemId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<int>>>().ConfigureAwait(false);
			}
		}
	}
}