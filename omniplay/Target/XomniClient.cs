using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
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

        public async Task<PollingResponseObject> GetIncomingDevices(string currentTargetDeviceId)
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

        public async Task<PIISession> ExchangeOmniTokenWithPiiSession(string incommingOmniTicket)
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

        public async Task<bool> RegisterDevice(RegisterRequestObject registerRequestObject)
        {
            using (HttpResponseMessage response = await Client.Value.PostAsJsonAsync("device/register", registerRequestObject))
            {
                return response.IsSuccessStatusCode;
            }
        }
    }

    public class Datum
    {
        public string OmniTicket { get; set; }
        public string PIIDisplayName { get; set; }
    }

    public class PollingResponseObject
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<Datum> Data { get; set; }
    }
    public class OmniTicket
    {
        public string Ticket { get; set; }
    }

    public class PIISessionData
    {
        public string SessionGuid { get; set; }
    }

    public class PIISession
    {
        public PIISessionData Data { get; set; }
    }

    public class RegisterRequestObject
    {
        public string DeviceId { get; set; }
        public string Description { get; set; }
    }
}