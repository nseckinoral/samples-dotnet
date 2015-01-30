using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Social;

namespace XOMNI.SDK.Public.Clients.Social
{
	public class ProfileClient : BaseClient
	{
		public ProfileClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		//TODO: Requires PII
		public async Task<ApiResponse<List<SocialProfile>>> GetAsync()
		{
			string path = "/social/profiles";

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<List<SocialProfile>>>().ConfigureAwait(false);
			}
		}
	}
}