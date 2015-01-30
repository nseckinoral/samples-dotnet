using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using XOMNI.SDK.Public.Infrastructure;
using XOMNI.SDK.Public.Models.OmniPlay;
using XOMNI.SDK.Public.Models.PII;

namespace XOMNI.SDK.Public
{
    public class ClientContext : IDisposable
    {
        private const string sessionHeaderFormat = "sessionGuid:{0}";
        private const string piiHeaderFormat = "username:{0};password:{1}";
        private const string AuthenticationSchema = "Basic";
        public HttpClient HttpClient { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public ConcurrentDictionary<Type, object> Clients { get; private set; }

        private User piiUser { get; set; }
        public User PIIUser
        {
            get { return piiUser; }
            set
            {
                InitalizePIIToken(value);
                piiUser = value;
            }
        }

        private OmniSession omniSession;
        public OmniSession OmniSession
        {
            get { return omniSession; }
            set
            {
                InitalizePIIToken(value);
                omniSession = value;
            }
        }

        public ClientContext(string userName, string password, string serviceUri, HttpClient httpClient = null)
        {
            omniSession = new OmniSession();
            piiUser = new User();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(serviceUri))
            {
                throw new ArgumentNullException("username, password or service uri");
            }

            var apiErrorHandler = new XOMNIPublicApiErrorHandler();
            HttpMessageHandler innerHandler = HttpClientFactory.CreatePipeline(new HttpClientHandler(), new[] { apiErrorHandler });
            this.UserName = userName;
            this.Password = password;
            this.Clients = new ConcurrentDictionary<Type, object>();
            
            if (httpClient == null)
            {
                this.HttpClient = new HttpClient(innerHandler);
            }
            else
            {
                this.HttpClient = httpClient;
            }
            
            this.HttpClient.BaseAddress = new Uri(serviceUri);
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationSchema,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password))));

            this.HttpClient.DefaultRequestHeaders.Accept.ParseAdd(string.Format("application/vnd.xomni.api-{0}", "v3_0"));

        }

        private void InitalizePIIToken(OmniSession value)
        {
            lock (piiUser)
            {
                if (value == null)
                {
                    this.HttpClient.DefaultRequestHeaders.Remove("PIIToken");
                }
                else
                {
                    var sessionHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(sessionHeaderFormat,value.SessionGuid)));
                    this.HttpClient.DefaultRequestHeaders.Add("PIIToken", sessionHeader);
                    piiUser = null;
                }
            }
        }
        private void InitalizePIIToken(User value)
        {
            lock (omniSession)
            {
                if (value == null)
                {
                    this.HttpClient.DefaultRequestHeaders.Remove("PIIToken");
                }
                else
                {
                    var piiHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format(piiHeaderFormat, value.UserName, value.Password)));
                    this.HttpClient.DefaultRequestHeaders.Add("PIIToken", piiHeader);
                    omniSession = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (HttpClient != null)
                {
                    HttpClient.Dispose();
                    HttpClient = null;
                }
            }
        }
    }
}
