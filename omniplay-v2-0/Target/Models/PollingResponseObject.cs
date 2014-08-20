using System.Collections.Generic;
using System.Net;

namespace Target
{
    public class PollingResponseObject
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<Datum> Data { get; set; }
    }
}