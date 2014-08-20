using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Target.Models;

namespace Target
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
        private const string PiiHeaderSessionGuidFormat = "sessionGuid:{0}";


        private static readonly Lazy<HttpClient> Client = new Lazy<HttpClient>(() =>
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(ApiEndpointUri)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.DefaultRequestHeaders.Accept.ParseAdd(string.Format("application/vnd.xomni.api-{0}", "v2_1"));

            string encodedHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(ApiAuthorizationHeaderFormat, ApiClientAccessLicenceName, ApiClientAccessLicencePass)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiAuthorizationHeaderName, encodedHeaderValue);

            return client;
        });

        public async Task<PollingResponseObject> GetIncomingDevicesAsync(string currentTargetDeviceId)
        {
            PollingResponseObject result;
            using (HttpResponseMessage response = await Client.Value.GetAsync(string.Format("omniplay/devices/{0}/incoming", currentTargetDeviceId)))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<PollingResponseObject>();
                }
                else
                {
                    result = new PollingResponseObject();
                }

                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }

            return result;
        }

        public async Task<PIISession> ExchangeOmniTokenWithPiiSessionAsync(string incommingOmniTicket)
        {
            PIISession result = null;
            using (HttpResponseMessage responseForExchange = await Client.Value.PostAsJsonAsync("omniplay/pii/session", new OmniTicket() { Ticket = incommingOmniTicket }))
            {
                if (responseForExchange.IsSuccessStatusCode)
                {
                    result = await responseForExchange.Content.ReadAsAsync<PIISession>();
                }
            }

            return result;
        }

        public async Task<bool> RegisterDeviceAsync(RegisterRequestObject registerRequestObject)
        {
            using (HttpResponseMessage response = await Client.Value.PostAsJsonAsync("company/devices", registerRequestObject))
            {
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<WishlistUniqueKeyResponse> GetWishlistUniqueKeysAsync(string sessionGuid)
        {
            WishlistUniqueKeyResponse result;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "pii/wishlists");
            string encodedHeaderPiiValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(PiiHeaderSessionGuidFormat, sessionGuid)));
            requestMessage.Headers.Add(PiiTokenHeaderName, encodedHeaderPiiValue);
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<WishlistUniqueKeyResponse>();
                }
                else
                {
                    result = new WishlistUniqueKeyResponse();
                }
                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }
            return result;
        }

        public async Task<WishlistResponse> GetWishlistAsync(string uniqueKey, string sessionGuid)
        {
            WishlistResponse result;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, string.Format("pii/wishlist?wishlistUniqueKey={0}&includeItemStaticProperties=true&includeItemDynamicProperties=true&includeCategoryMetadata=true&imageAssetDetail=4&videoAssetDetail=4&documentAssetDetail=4", uniqueKey));
            string encodedHeaderPiiValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(PiiHeaderSessionGuidFormat, sessionGuid)));
            requestMessage.Headers.Add(PiiTokenHeaderName, encodedHeaderPiiValue);
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<WishlistResponse>();
                }
                else
                {
                    result = new WishlistResponse();
                }
                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }
            return result;
        }
    }
}