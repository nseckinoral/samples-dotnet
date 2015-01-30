using System;
using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Models;
using XOMNI.SDK.Public.Models.Social;

namespace XOMNI.SDK.Public.Clients.Social
{
	public class PostClient : BaseClient
	{
		public PostClient(HttpClient httpClient)
			: base(httpClient)
		{

		}

		//TODO: Requires PII
		public async Task<ApiResponse<SocialPost>> GetAsync(int relatedItemId)
		{
			string path = string.Format("/social/post?relatedItemId={0}", relatedItemId);

			using (var response = await Client.GetAsync(path).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<SocialPost>>().ConfigureAwait(false);
			}
		}

		//TODO: Requires PII
		public async Task DeleteAsync(int socialPostId)
		{
			string path = string.Format("/social/post/{0}", socialPostId);
			await Client.DeleteAsync(path).ConfigureAwait(false);
		}

		//TODO: Requires PII

		public async Task<ApiResponse<SocialPost>> PostAsync(SocialPostRequest socialPostRequest)
		{
			string path = "/social/post";

			using (var response = await Client.PostAsJsonAsync(path, socialPostRequest).ConfigureAwait(false))
			{
				return await response.Content.ReadAsAsync<ApiResponse<SocialPost>>().ConfigureAwait(false);
			}
		}

        public async Task<ApiResponse<SocialPolicy>> GetPoliciesAsync()
        {
            string path = "/social/post/policies";

            using (var response = await Client.GetAsync(path).ConfigureAwait(false))
            {
                return await response.Content.ReadAsAsync<ApiResponse<SocialPolicy>>().ConfigureAwait(false);
            }
        }
	}
}