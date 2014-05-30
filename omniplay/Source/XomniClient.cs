using System;
using System.Collections.Generic;
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

        public async Task<bool> SubscribeQueue(string piiUser, string piiPassword, string deviceId)
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

        public async Task<PII_ResponseObject> RequestAnonymousPii(PII_RequestObject piiRequestObject)
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

        public async Task<DiscoveryResponseObject> GetDiscoverableDevices()
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
    }

    public class PII_RequestObject
    {
        public string UserName { get; set; }
        public string Name { get; set; }
    }
    public class PII_ResponseObject
    {
        public Data Data { get; set; }
    }
    public class Data
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public object Name { get; set; }
        public string PIIUserType { get; set; }
    }

    public class Device
    {
        public string DeviceId { get; set; }
        public string Description { get; set; }
    }

    public class DiscoveryResponseObject
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<Device> Data { get; set; }
    }
}
