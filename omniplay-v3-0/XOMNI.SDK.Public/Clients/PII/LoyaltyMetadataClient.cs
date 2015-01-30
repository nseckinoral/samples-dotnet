using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class LoyaltyMetadataClient : BaseClient
	{
		public LoyaltyMetadataClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<List<Metadata>>> GetAsync()
		{
			string path = "/pii/loyaltymetadata";

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Metadata>>>().ConfigureAwait(false);
			}
		}
	}
}