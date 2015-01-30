using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
	public class TrendingItemClient : BaseClient
	{
		public TrendingItemClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<List<TrendingItem>> GetAsync(int top, bool includeActionDetails)
		{
			string path = string.Format("/catalog/trendingitems?top={0}&includeActionDetails={1}", top, includeActionDetails);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<List<TrendingItem>>().ConfigureAwait(false);
			}
		}

        public async Task<List<TrendingItem>> GetByStoreAsync(int top, int storeId, bool includeActionDetails)
		{
			string path = string.Format("/catalog/trendingitems?top={0}&storeId={1}&includeActionDetails={2}", top, storeId, includeActionDetails);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<List<TrendingItem>>().ConfigureAwait(false);
			}
		}

        public async Task<List<TrendingItem>> GetByBrandAsync(int top, int brandId, bool includeActionDetails)
        {
            string path = string.Format("/catalog/trendingitems?top={0}&brandId={1}&includeActionDetails={2}", top, brandId, includeActionDetails);

            using (var response = await Client.GetAsync(path).ConfigureAwait(false))
            {
                return await response.Content.ReadAsAsync<List<TrendingItem>>().ConfigureAwait(false);
            }
        }

        public async Task<List<TrendingItem>> GetByBrandAndStoreAsync(int top, int brandId, int storeId, bool includeActionDetails)
		{
			string path = string.Format("/catalog/trendingitems?top={0}&brandId={1}&storeId={2}&includeActionDetails={3}", top, brandId, storeId, includeActionDetails);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<List<TrendingItem>>().ConfigureAwait(false);
			}
		}
	}
}