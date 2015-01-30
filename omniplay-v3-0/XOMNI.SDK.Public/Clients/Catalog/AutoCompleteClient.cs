using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class AutoCompleteClient : BaseClient
	{
		public AutoCompleteClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<AutoCompleteResult>> GetAsync(AutoCompleteSearchType searchType, string searchTerm, int skip, int take)
		{
            if(string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException("searchTerm");
            }
            if(skip<=0)
            {
                throw new ArgumentException("skip");
            }
            if(take<=0)
            {
                throw new ArgumentException("take");
            }

			string path = string.Format("/catalog/autocomplete/{0}?searchTerm={1}&skip={2}&take={3}", searchType.ToString(), searchTerm, skip, take);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<AutoCompleteResult>>().ConfigureAwait(false);
			}
		}
	}
}