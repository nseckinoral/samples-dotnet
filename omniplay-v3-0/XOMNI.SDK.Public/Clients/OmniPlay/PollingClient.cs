using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.OmniPlay
{
	public class PollingClient : BaseClient
	{
		public PollingClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<WishlistWithItems>> GetWishlistAsync(Guid wishlistUniqueKey, int longitude, bool includeItemStaticProperties, bool includeItemDynamicProperties, bool includeCategoryMetadata, int imageAssetDetail, int videoAssetDetail, int documentAssetDetail, string metadataKey, string metadataValue, int latitude)
		{
			string path = string.Format("/omniplay/wishlist?wishlistUniqueKey={0}&longitude={1}&latitude={2}&includeItemStaticProperties={3}&includeItemDynamicProperties={4}&includeCategoryMetadata={5}&imageAssetDetail={6}&videoAssetDetail={7}&documentAssetDetail={8}&metadataKey={9}&metadataValue={10}", wishlistUniqueKey, longitude, includeItemStaticProperties, includeItemDynamicProperties, includeCategoryMetadata, imageAssetDetail, videoAssetDetail, documentAssetDetail, metadataKey, metadataValue, latitude);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<WishlistWithItems>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<ShoppingCartWithItems>> GetShoppingcartAsync(Guid shoppingcartUniqueKey, int longitude, bool includeItemStaticProperties, bool includeItemDynamicProperties, bool includeCategoryMetadata, int imageAssetDetail, int videoAssetDetail, int documentAssetDetail, string metadataKey, string metadataValue, int latitude)
		{
			string path = string.Format("/omniplay/shoppingcart?shoppingCartUniqueKey={0}&longitude={1}&latitude={2}&includeItemStaticProperties={3}&includeItemDynamicProperties={4}&includeCategoryMetadata={5}&imageAssetDetail={6}&videoAssetDetail={7}&documentAssetDetail={8}&metadataKey={9}&metadataValue={10}", shoppingcartUniqueKey, longitude, includeItemStaticProperties, includeItemDynamicProperties, includeCategoryMetadata, imageAssetDetail, videoAssetDetail, documentAssetDetail, metadataKey, metadataValue, latitude);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<ShoppingCartWithItems>>().ConfigureAwait(false);
			}
		}
	}
}