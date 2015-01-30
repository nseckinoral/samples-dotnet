using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class AnonymousClient : BaseClient
	{
		public AnonymousClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<AnonymousUser>> PostAsync(AnonymousUserRequest anonymousUser)
		{
			string path = "/pii/anonymous";

            if (string.IsNullOrEmpty(anonymousUser.UserName))
            {
                throw new ArgumentNullException("userName");
            }

            using (var response = await Client.PostAsJsonAsync(path, anonymousUser).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<AnonymousUser>>().ConfigureAwait(false);
			}
		}
	}
}