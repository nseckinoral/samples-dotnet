using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.Social
{
	public class AuthorizationURLClient : BaseClient
	{
		public AuthorizationURLClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<string> GetAsync(string socialPlatformName)
		{
			string path = string.Format("/social/authurl/{0}", socialPlatformName);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<string>().ConfigureAwait(false);
			}
		}
	}
}