using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XOMNI.SDK.Public.Models;

namespace XOMNI.SDK.Public.Exceptions
{
    public class XOMNIPublicAPIException : System.Exception
    {
        public ExceptionResult ApiExceptionResult { get; private set; }
        public XOMNIPublicAPIException(ExceptionResult apiExceptionResult, HttpRequestException httpRequestException)
            : base(apiExceptionResult.FriendlyDescription, httpRequestException)
        {
            this.ApiExceptionResult = apiExceptionResult;
        }
    }
}


