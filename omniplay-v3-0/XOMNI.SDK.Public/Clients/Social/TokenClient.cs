using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models.Social;

namespace XOMNI.SDK.Public.Clients.Social
{
	public class TokenClient : BaseClient
	{
		public TokenClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		//TODO: Requires PII
		public async Task<string> GetAsync(SocialPlatformType socialPlatform)
		{
			string path = string.Format("/social/token/{0}", socialPlatform.ToString().ToLowerInvariant());

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<string>().ConfigureAwait(false);
			}
		}

		//TODO: Requires PII
		public async Task<string> PostAsync(SocialPlatformType socialPlatform, string token)
		{
			string path = string.Format("/social/token/{0}", socialPlatform.ToString().ToLowerInvariant());

			using (var response = await Client.PostAsJsonAsync(path, token).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<string>().ConfigureAwait(false);
			}
		}
	}
}