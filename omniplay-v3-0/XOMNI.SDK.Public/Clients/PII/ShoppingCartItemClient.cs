using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class ShoppingCartItemClient : BaseClient
	{
		public ShoppingCartItemClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task<ApiResponse<ShoppingCartItem>> PutAsync(Guid shoppingCartItemUniqueKey, int quantity, int latitude, int longitude)
        {
            string path = string.Format("/pii/shoppingcartitem?shoppingCartItemUniqueKey={0}&quantity={1}&longitude={2}&latitude={3}", shoppingCartItemUniqueKey, quantity, latitude, longitude);

            using (var response = await Client.PutAsJsonAsync(path, string.Empty).ConfigureAwait(false))
            {
                return await response.Content.ReadAsAsync<ApiResponse<ShoppingCartItem>>().ConfigureAwait(false);
            }
        }

        public async Task DeleteShoppingcartitemAsync(Guid shoppingCartItemUniqueKey, int longitude, int latitude)
		{
			string path = string.Format("/pii/shoppingcartitem?shoppingCartItemUniqueKey={0}&longitude={1}&latitude={2}", shoppingCartItemUniqueKey, longitude, latitude);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task DeleteShoppingcartitemsAsync(Guid shoppingCartUniqueKey, int longitude, int latitude)
		{
			string path = string.Format("/pii/shoppingcartitems?shoppingCartUniqueKey={0}&longitude={1}&latitude={2}", shoppingCartUniqueKey, longitude, latitude);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<ShoppingCartItem>> PostAsync(Guid shoppingCartUniqueKey, object body)
		{
			string path = string.Format("/pii/shoppingcartitem?shoppingCartUniqueKey={0}", shoppingCartUniqueKey);

			using (var response = await Client.PostAsJsonAsync(path, body).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<ShoppingCartItem>>().ConfigureAwait(false);
			}
		}
	}
}