using System.Net;

namespace Target.Models
{
    public class WishlistResponse
    {
        public Wishlist Data { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
