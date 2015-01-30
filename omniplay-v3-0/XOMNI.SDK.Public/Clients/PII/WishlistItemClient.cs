using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class WishlistItemClient : BaseClient
	{
		public WishlistItemClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task DeleteWishlistitemAsync(Guid wishlistItemUniqueKey, int Longitude, int Latitude)
		{
			string path = string.Format("/pii/wishlistitem?wishlistItemUniqueKey={0}&longitude={1}&latitude={2}", wishlistItemUniqueKey, Longitude, Latitude);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task DeleteWishlistitemsAsync(Guid wishlistUniqueKey, int Longitude, int Latitude)
		{
			string path = string.Format("/pii/wishlistitems?wishlistUniqueKey={0}&longitude={1}&latitude={2}", wishlistUniqueKey, Longitude, Latitude);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<WishlistItem>> PostAsync(Guid wishlistUniqueKey, WishlistItem wishlistItem)
		{
			string path = string.Format("/pii/wishlistitem?wishlistUniqueKey={0}", wishlistUniqueKey);

            using (var response = await Client.PostAsJsonAsync(path, wishlistItem).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<WishlistItem>>().ConfigureAwait(false);
			}
		}
	}
}