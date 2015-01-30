using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Clients.PII
{
    public class WishlistMetadataClient : BaseClient
    {
        public WishlistMetadataClient(HttpClient httpClient)
            : base(httpClient)
        {

        }

        public async Task<List<Metadata>> GetAsync(Guid wishlistUniqueKey)
        {
            string path = string.Format("/pii/wishlistmetadata?wishlistUniqueKey={0}", wishlistUniqueKey);

            using (var response = await Client.GetAsync(path).ConfigureAwait(false))
            {
                return await response.Content.ReadAsAsync<List<Metadata>>().ConfigureAwait(false);
            }
        }
    }
}
