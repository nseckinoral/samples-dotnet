using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Target.Models
{
    public class Wishlist
    {
        public List<WishlistItem> WishlistItems { get; set; }
        public Guid UniqueKey { get; set; }
        public string Name { get; set; }
        public Location LastSeenLocation { get; set; }
        public bool IsPublic { get; set; }
    }
}
