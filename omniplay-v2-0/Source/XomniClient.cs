using Newtonsoft.Json;
using Source.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public class XomniClient
    {
        private static readonly string ApiEndpointUri = ConfigurationManager.AppSettings["XOMNI:ApiEndpointUri"];
        private static readonly string ApiClientAccessLicenceName = ConfigurationManager.AppSettings["XOMNI:ApiClientAccessLicenceName"];
        private static readonly string ApiClientAccessLicencePass = ConfigurationManager.AppSettings["XOMNI:ApiClientAccessLicencePass"];
        private const string ApiAuthorizationHeaderName = "Basic";
        private const string ApiAuthorizationHeaderFormat = "{0}:{1}";
        private const string PiiTokenHeaderName = "PIIToken";
        private const string PiiHeaderFormat = "username:{0};password:{1}";

        private static readonly Lazy<HttpClient> Client = new Lazy<HttpClient>(() =>
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(ApiEndpointUri)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.DefaultRequestHeaders.Accept.ParseAdd(string.Format("application/vnd.xomni.api-{0}", "v2_0"));

            string encodedHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(ApiAuthorizationHeaderFormat, ApiClientAccessLicenceName, ApiClientAccessLicencePass)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiAuthorizationHeaderName, encodedHeaderValue);

            return client;
        });

        public async Task<bool> SubscribeQueueAsync(string piiUser, string piiPassword, string deviceId)
        {
            string path = string.Format("omniplay/devices/{0}", deviceId);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, path);
            string encodedHeaderPiiValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(PiiHeaderFormat, piiUser, piiPassword)));
            requestMessage.Headers.Add(PiiTokenHeaderName, encodedHeaderPiiValue);
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
            {
                return response.StatusCode == HttpStatusCode.Accepted;
            }
        }

        public async Task<PII_ResponseObject> RequestAnonymousPiiAsync(PII_RequestObject piiRequestObject)
        {
            PII_ResponseObject result = null;
            using (HttpResponseMessage response = await Client.Value.PostAsJsonAsync("pii/anonymous", piiRequestObject))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<PII_ResponseObject>();
                }
            }

            return result;
        }

        public async Task<DiscoveryResponseObject> GetDiscoverableDevicesAsync()
        {
            DiscoveryResponseObject pollingResult;
            using (HttpResponseMessage response = await Client.Value.GetAsync("omniplay/store/devices"))
            {
                if (response.IsSuccessStatusCode)
                {
                    pollingResult = await response.Content.ReadAsAsync<DiscoveryResponseObject>();
                }
                else
                {
                    pollingResult = new DiscoveryResponseObject();
                }

                pollingResult.IsSuccess = response.IsSuccessStatusCode;
                pollingResult.HttpStatusCode = response.StatusCode;
            }

            return pollingResult;
        }

        public async Task<CreateWishlistResponseObject> CreateWishlistAsync(Wishlist wishlist, string piiUser, string piiPassword)
        {
            CreateWishlistResponseObject result;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "pii/wishlist");
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(wishlist), Encoding.UTF8, "application/json");
            string encodedHeaderPiiValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(PiiHeaderFormat, piiUser, piiPassword)));
            requestMessage.Headers.Add(PiiTokenHeaderName, encodedHeaderPiiValue);
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<CreateWishlistResponseObject>();
                }
                else
                {
                    result = new CreateWishlistResponseObject();
                }
                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }
            return result;
        }

        public async Task<AddWishlistItemResponse> AddWishlistItemAsync(WishlistItem wishlistItem, string wishlistUniqueKey, string piiUser, string piiPassword)
        {
            AddWishlistItemResponse result;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, string.Format("pii/wishlistitem?wishlistUniqueKey={0}", wishlistUniqueKey));
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(wishlistItem), Encoding.UTF8, "application/json");
            string encodedHeaderPiiValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(PiiHeaderFormat, piiUser, piiPassword)));
            requestMessage.Headers.Add(PiiTokenHeaderName, encodedHeaderPiiValue);
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<AddWishlistItemResponse>();
                }
                else
                {
                    result = new AddWishlistItemResponse();
                }
                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }
            return result;
        }
    }
}
