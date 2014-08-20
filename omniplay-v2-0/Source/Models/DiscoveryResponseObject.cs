using System.Collections.Generic;
using System.Net;

namespace Source
{
    public class DiscoveryResponseObject
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<Device> Data { get; set; }
    }
}