using System.Net.Http;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Exceptions;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Infrastructure
{
    public class XOMNIPublicApiErrorHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            ExceptionResult exceptionResult = null;
            var responseMessage = await base.SendAsync(request, cancellationToken);

            try
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    exceptionResult = responseMessage.Content.ReadAsAsync<ExceptionResult>().Result;
                    if (exceptionResult == null)
                    {
                        exceptionResult = new ExceptionResult();
                    }
                    exceptionResult.HttpStatusCode = responseMessage.StatusCode;
                    responseMessage = responseMessage.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException hrEx)
            {
                throw new XOMNIPublicAPIException(exceptionResult, hrEx);
            }

            return responseMessage;
        }
    }
}
