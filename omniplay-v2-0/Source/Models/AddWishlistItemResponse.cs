using System.Net;

namespace Source.Models
{
    public class AddWishlistItemResponse
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public WishlistItem Data { get; set; }
    }
}
