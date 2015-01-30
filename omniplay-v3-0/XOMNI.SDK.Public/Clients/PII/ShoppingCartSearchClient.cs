using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class ShoppingCartSearchClient : BaseClient
	{
		public ShoppingCartSearchClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<List<Guid>>> PostAsync(ShoppingCartSearchRequest searchRequest)
		{
			string path = "/pii/shoppingcartsearch";

            using (var response = await Client.PostAsJsonAsync(path, searchRequest).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Guid>>>().ConfigureAwait(false);
			}
		}
	}
}