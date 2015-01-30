using System;
using XOMNI.SDK.Public.Clients;
using XOMNI.SDK.Public.Clients.PII;
using XOMNI.SDK.Public.Clients.Utility;
using XOMNI.SDK.Public.Clients.Catalog;
using XOMNI.SDK.Public.Clients.OmniPlay;
using XOMNI.SDK.Public.Clients.Social;

namespace XOMNI.SDK.Public
{
    public static class ClientContextExtensions
    {
        public static TClient Of<TClient>(this ClientContext apiClientContext) where TClient : BaseClient
        {
            return (TClient)apiClientContext.Clients.GetOrAdd(typeof(TClient), 
                (k) => (TClient)Activator.CreateInstance(typeof(TClient), apiClientContext.HttpClient));
        }
    }
}
