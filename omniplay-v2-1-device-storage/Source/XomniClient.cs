using Newtonsoft.Json;
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
            client.DefaultRequestHeaders.Accept.ParseAdd(string.Format("application/vnd.xomni.api-{0}", "v2_1"));

            string encodedHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(ApiAuthorizationHeaderFormat, ApiClientAccessLicenceName, ApiClientAccessLicencePass)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiAuthorizationHeaderName, encodedHeaderValue);

            return client;
        });

        public async Task<DiscoveryResponseObject> GetDiscoverableDevicesAsync()
        {
            DiscoveryResponseObject pollingResult;
            using (HttpResponseMessage response = await Client.Value.GetAsync("company/stores/devices"))
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

        public async Task<CreateDeviceStorageItemResponseObject> CreateDeviceStorageItemAsync(string deviceId, DeviceStorageItem deviceStorageItem)
        {
            CreateDeviceStorageItemResponseObject result;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, string.Format("company/devices/{0}/storage", deviceId));
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(deviceStorageItem), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await Client.Value.SendAsync(requestMessage))
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<CreateDeviceStorageItemResponseObject>();
                }
                else
                {
                    result = new CreateDeviceStorageItemResponseObject();
                }
                result.IsSuccess = response.IsSuccessStatusCode;
                result.HttpStatusCode = response.StatusCode;
            }
            return result;
        }
    }
}
