using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public.Clients.PII
{
	public class ShoppingCartClient : BaseClient
	{
		public ShoppingCartClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

        public async Task DeleteAsync(Guid shoppingCartUniqueKey)
		{
			string path = string.Format("/pii/shoppingcart?shoppingCartUniqueKey={0}", shoppingCartUniqueKey);
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task PostAccessAsync(Guid shoppingCartUniqueKey, bool isPublic)
        {
            string path = string.Format("/pii/shoppingcartaccess?shoppingCartUniqueKey={0}&isPublic={1}", shoppingCartUniqueKey, isPublic);
            await Client.PostAsJsonAsync(path, string.Empty).ConfigureAwait(false);
        }

        public async Task<ApiResponse<List<Guid>>> GetAsync()
		{
			string path = "/pii/shoppingcarts";

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<Guid>>>().ConfigureAwait(false);
			}
		}

		public async Task DeleteAsync()
		{
			string path = "/pii/shoppingcarts";
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<ShoppingCartWithItems>> GetAsync(Guid shoppingCartUniqueKey, int longitude, int latitude, bool IncludeItemStaticProperties, bool IncludeItemDynamicProperties, bool IncludeCategoryMetadata, int ImageAssetDetail, int VideoAssetDetail, int DocumentAssetDetail, string MetadataKey, string MetadataValue)
		{
			string path = string.Format("/pii/shoppingcart?shoppingCartUniqueKey={0}&longitude={1}&latitude={2}&includeItemStaticProperties={3}&includeItemDynamicProperties={4}&includeCategoryMetadata={5}&imageAssetDetail={6}&videoAssetDetail={7}&documentAssetDetail={8}&metadataKey={9}&metadataValue={10}", shoppingCartUniqueKey, longitude, latitude, IncludeItemStaticProperties, IncludeItemDynamicProperties, IncludeCategoryMetadata, ImageAssetDetail, VideoAssetDetail, DocumentAssetDetail, MetadataKey, MetadataValue);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<ShoppingCartWithItems>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<ShoppingCart>> PostShoppingcartAsync(ShoppingCart shoppingCart)
		{
			string path = "/pii/shoppingcart";

            using (var response = await Client.PostAsJsonAsync(path, shoppingCart).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<ShoppingCart>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<ShoppingCart>> PutAsync(ShoppingCart shoppingCart)
		{
			string path = "/pii/shoppingcart";

            using (var response = await Client.PutAsJsonAsync(path, shoppingCart).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<ShoppingCart>>().ConfigureAwait(false);
			}
		}

        public async Task PostMailAsync(MailSendRequest mailSendRequest)
		{
			string path = "/pii/shoppingcart/mail";
            await Client.PostAsJsonAsync(path, mailSendRequest).ConfigureAwait(false);
		}
	}
}