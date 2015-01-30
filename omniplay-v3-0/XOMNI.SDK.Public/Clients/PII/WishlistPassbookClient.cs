using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class WishlistPassbookClient : BaseClient
	{
		public WishlistPassbookClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<byte[]>> GetAsync(Guid wishlistUniqueKey, string templateName)
		{
			string path = string.Format("/pii/wishlist/{0}/passbook?templateName={1}", wishlistUniqueKey, templateName);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<byte[]>>().ConfigureAwait(false);
			}
		}
	}
}