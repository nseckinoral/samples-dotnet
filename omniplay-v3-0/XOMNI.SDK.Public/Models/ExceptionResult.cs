using System;
using System.Net;

namespace XOMNI.SDK.Public.Models
{
    public class ExceptionResult
    {
        public Guid IdentifierGuid { get; set; }
        public long IdentifierTick { get; set; }
        public string FriendlyDescription { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
