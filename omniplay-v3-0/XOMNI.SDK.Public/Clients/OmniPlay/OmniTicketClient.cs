using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.OmniPlay;

namespace XOMNI.SDK.Public.Clients.OmniPlay
{
	public class OmniTicketClient : BaseClient
	{
		public OmniTicketClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<OmniTicketDetail>> GetTicketAsync()
		{
			string path = "/omniplay/pii/ticket";

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<OmniTicketDetail>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<OmniSession>> GetSessionAsync()
		{
			string path = "/omniplay/pii/session";

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<OmniSession>>().ConfigureAwait(false);
			}
		}

		public async Task DeleteAsync()
		{
			string path = "/omniplay/pii/session";
            await Client.DeleteAsync(path).ConfigureAwait(false);
		}

        public async Task<ApiResponse<OmniTicketDetail>> GetAsync(Guid wishlistUniqueKey)
		{
			string path = string.Format("/omniplay/wishlist/ticket?wishlistUniqueKey={0}", wishlistUniqueKey);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<OmniTicketDetail>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<OmniSession>> PostSessionAsync(OmniTicket omniTicket)
		{
			string path = "/omniplay/pii/session";

			using (var response = await Client.PostAsJsonAsync(path, omniTicket).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<OmniSession>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<WishlistImportResponse>> PostImportAsync(WishlistImportRequest wishlistImportRequest)
		{
			string path = "/pii/wishlist/import";

            using (var response = await Client.PostAsJsonAsync(path, wishlistImportRequest).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<WishlistImportResponse>>().ConfigureAwait(false);
			}
		}
	}
}