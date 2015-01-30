using System;
using System.Net.Http;

namespace XOMNI.SDK.Public.Clients
{
    public abstract class BaseClient
    {
        protected readonly HttpClient Client;

        protected BaseClient(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException("httpClient");
            }

            Client = httpClient;
        }
    }
}
