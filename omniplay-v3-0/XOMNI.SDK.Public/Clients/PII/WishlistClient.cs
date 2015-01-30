using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class WishlistClient : BaseClient
	{
		public WishlistClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task DeleteAsync()
        {
            string path = "/pii/wishlists";
            await Client.DeleteAsync(path).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid wishlistUniqueKey)
		{
			string path = string.Format("/pii/wishlist?wishlistUniqueKey={0}", wishlistUniqueKey);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<WishlistWithItems>> GetAsync(Guid wishlistUniqueKey, int longitude, int latitude, bool includeItemStaticProperties, bool includeItemDynamicProperties, bool includeCategoryMetadata, AssetDetailType imageAssetDetail, AssetDetailType videoAssetDetail, AssetDetailType documentAssetDetail, string metadataKey, string metadataValue)
		{
            string path = string.Format("/pii/wishlist?wishlistUniqueKey={0}&longitude={1}&latitude={2}&includeItemStaticProperties={3}&includeItemDynamicProperties={4}&includeCategoryMetadata={5}&imageAssetDetail={6}&videoAssetDetail={7}&documentAssetDetail={8}&metadataKey={9}&metadataValue={10}", wishlistUniqueKey, longitude, latitude, includeItemStaticProperties, includeItemDynamicProperties, includeCategoryMetadata, imageAssetDetail, videoAssetDetail, documentAssetDetail, metadataKey, metadataValue);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<WishlistWithItems>>().ConfigureAwait(false);
			}
		}

        public async Task PostAccessAsync(Guid wishlistUniqueKey, bool isPublic)
        {
            string path = string.Format("/pii/wishlistaccess?wishlistUniqueKey={0}&isPublic={1}", wishlistUniqueKey, isPublic);
            await Client.PostAsJsonAsync(path, string.Empty).ConfigureAwait(false);
        }

        public async Task<ApiResponse<List<Guid>>> GetAsync()
		{
			string path = "/pii/wishlists";

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Guid>>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<Wishlist>> PostWishlistAsync(WishlistWithItems wishlistWithItems)
		{
			string path = "/pii/wishlist";

            using (var response = await Client.PostAsJsonAsync(path, wishlistWithItems).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<Wishlist>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<Wishlist>> PutAsync(Wishlist wishlist)
		{
			string path = "/pii/wishlist";

			using (var response = await Client.PutAsJsonAsync(path, wishlist).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<Wishlist>>().ConfigureAwait(false);
			}
		}

        public async Task PostMailAsync(MailSendRequest mailSendRequest)
		{
			string path = "/pii/wishlist/mail";
            await Client.PostAsJsonAsync(path, mailSendRequest).ConfigureAwait(false);
		}
	}
}