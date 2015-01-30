using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class ShoppingCartPassbookClient : BaseClient
	{
		public ShoppingCartPassbookClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<byte[]>> GetAsync(Guid shoppingCartUniqueKey, string templateName)
		{
			string path = string.Format("/pii/shoppingcart/{0}/passbook?templateName={1}", shoppingCartUniqueKey, templateName );

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<dynamic>().ConfigureAwait(false);
			}
		}
	}
}