using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<RegisterDeviceResponseObject> RegisterDeviceAsync(Device device)
        {
            RegisterDeviceResponseObject result;
            using (HttpResponseMessage response = await Client.Value.PostAsJsonAsync("company/devices", device))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<RegisterDeviceResponseObject>();
                }
                else
                {
                    result = new RegisterDeviceResponseObject();
                }
                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }
            return result;
        }

        public async Task<PollingResponseObject> GetDeviceStorageItemsAsync(string deviceId, bool deleteAfterFetching)
        {
            PollingResponseObject result;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, string.Format("company/devices/{0}/storage?delete={1}", deviceId, deleteAfterFetching));
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
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
    }
}