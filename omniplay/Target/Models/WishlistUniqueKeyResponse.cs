using System;
using System.Collections.Generic;
using System.Net;

namespace Target.Models
{
    public class WishlistUniqueKeyResponse
    {
        public List<Guid> Data { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
