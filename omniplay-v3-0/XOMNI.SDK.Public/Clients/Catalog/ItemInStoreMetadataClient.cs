using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Catalog;

namespace XOMNI.SDK.Public.Clients.Catalog
{
    public class ItemInStoreMetadataClient : BaseClient
    {
        public ItemInStoreMetadataClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		public async Task<ApiResponse<List<InStoreMetadata>>> GetAsync(int id, string key, string value)
		{
			string path = string.Format("/catalog/items/{id}/storemetadata?key={0}&value={1}&skip={2}&take={3}", id, key, value);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
                return await response.Content.ReadAsAsync<ApiResponse<List<InStoreMetadata>>>().ConfigureAwait(false);
			}
		}
    }
}
